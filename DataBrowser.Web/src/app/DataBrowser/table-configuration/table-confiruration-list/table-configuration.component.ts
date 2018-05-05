import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Dropdown } from "primeng/dropdown";
import * as _ from 'lodash';
import { IdNameServiceModel, DropdownModel, TableConfigurationDetails } from "../table-configuration.class";
import { CoreService } from "../../../core/core.service";


@Component({
    moduleId: module.id,
    selector: 'table-configuration',
    templateUrl: 'table-configuration.component.html',
    styleUrls: ['table-configuration.component.scss']
})

export class TableConfigurationComponent implements OnInit {

    dataBaseConnectionLists: Array<IdNameServiceModel> = [];
    dataBaseConnectionbindDropdown: Array<DropdownModel> = [{ label: 'Select Connection', value: null }];
    selectedDataBaseConnecton: any;

    tableconfigurationLists: Array<TableConfigurationDetails>
    constructor(
        private route: ActivatedRoute,
        private coreService: CoreService,
        private router: Router) {
    }
    ngOnInit(): void {
        //buisness logic
        this.route.data.map(data => data.databaseConnectionIdName).subscribe((res) => {
            this.dataBaseConnectionLists = res.idAndName;
            if (!this.coreService.isNullOrUndefined(res.idAndName)) {
                _.each(this.dataBaseConnectionLists, list => {
                    let data = new DropdownModel();
                    data.label = list.name;
                    data.value = { id: list.id, name: list.name };
                    this.dataBaseConnectionbindDropdown.push(data);
                });
                this.tableconfigurationLists = res.tableConfigList;
                _.each(this.tableconfigurationLists,list => {
                    list.isTable = (this.coreService.isNullOrUndefined(list.isTable)) ? false : list.isTable;
                    list.isView = (this.coreService.isNullOrUndefined(list.isView)) ? false : list.isView;
                });
            } else {
                this.dataBaseConnectionLists = res;
            }
        });
    }

    createTableConfiguration() {
        if (this.coreService.isNullOrUndefined(this.selectedDataBaseConnecton)) {
            this.coreService.alertDialog("Please select Database connection first");
        } else {
            let id = this.selectedDataBaseConnecton.id;
            this.router.navigate(['/table-configuration-create', id]);
        }
    }
}