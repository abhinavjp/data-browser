import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { CoreService } from "../../core/core.service";
import { DropdownModel } from "../table-configuration/table-configuration.class";
import { DatabrowserDropdownServiceModel } from "./data-browser.class";
import * as _ from 'lodash';
import { Paginator } from "primeng/paginator";
import { DatabrowserMainApiService } from "./data-browser-main-api.service";


@Component({
    moduleId: module.id,
    selector: 'data-browser-main',
    templateUrl: 'data-browser.component.html',
    styleUrls: ['data-browser.component.scss']
})

export class DataBrowserMainComponent implements OnInit {


    @ViewChild('p') paginator: Paginator;
    
    dataBrowserFilterDropDown: Array<DropdownModel> = [{ label: 'Select Table Configuration', value: null }];
    dataBrowserFilterLists: Array<DatabrowserDropdownServiceModel> = [];
    selectedTableConfiguration: any;

    columns: Array<Object> = [];
    fieldValues: Array<Object> = [];

    pageSize: number = 25;
    totalRecords: number;
    constructor(
        private route: ActivatedRoute,
        private coreService: CoreService,
        private databrowserMainApiService: DatabrowserMainApiService) {

    }

    ngOnInit(): void {
        this.route.data.map(data => data.dataBrowserFilters).subscribe((res) => {
            this.dataBrowserFilterLists = res;
            if (!this.coreService.isNullOrUndefined(this.dataBrowserFilterLists)) {
                _.each(this.dataBrowserFilterLists, list => {
                    let data = new DropdownModel();
                    data.label = list.name;
                    data.value = { id: list.id, name: list.name };
                    this.dataBrowserFilterDropDown.push(data);
                });
            }
        });
    }

    paginate = (event,$event) => {
        let pageNumber = event.page + 1;
        this.pageSize = event.rows;
        this.getDetailsOfFields(pageNumber,$event);
    }

    getDetailsOfFields = (pageNumber: number,$event) => {
        this.fieldValues = [];
        this.columns = [];
        this.totalRecords = 0;
        let fieldDetailsFilterModel = new DatabrowserDropdownServiceModel();
        if (this.coreService.isNullOrUndefined(this.selectedTableConfiguration)) {

        } else {
            fieldDetailsFilterModel = _.find(this.dataBrowserFilterLists, list => { return list.id === this.selectedTableConfiguration.id });
            if (this.coreService.isNullOrUndefined(fieldDetailsFilterModel)) {
            } else {
                fieldDetailsFilterModel.pageNumber = pageNumber;
                fieldDetailsFilterModel.pageSize = this.pageSize;
                this.databrowserMainApiService.getFieldsDetails(fieldDetailsFilterModel)
                    .subscribe(data => {
                        this.fieldValues = data.table;
                        this.totalRecords = data.table1[0].totalCount;
                        let key = Object.keys(this.fieldValues[0]);
                        if (!this.coreService.isNullOrUndefined(key)) {
                            _.each(key, d => {
                                let obj: Object;
                                obj = {
                                    field: d,
                                    header: d.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); })
                                }
                                this.columns.push(obj);
                            });
                        }
                    });
                        if(pageNumber == 1){
                            this.reset($event);
                        }
                    
            }
        }
    }


    reset($event) {
        this.paginator.changePageToFirst($event);
    }
}


















//OFFSET @PageSize * (@PageNumber - 1) ROWS
//FETCH NEXT @PageSize ROWS ONLY 

