import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { FormGroup, Validators, FormBuilder, FormArray, FormControl, Validator, AbstractControl } from "@angular/forms";
import { LoaderService } from "../../../core/loader.service";
import { DataBaseNameFilterServiceModel, IdNameServiceModel, TableDetailServiceModel, DropdownModel, TableConfigurationDetails, FieldConfigurtionDetails, MappedTableConfigurationDetails, TableConfigAndFieldConfigurationsDetails } from "../table-configuration.class";
import { CoreService } from "../../../core/core.service";
import { CoreToasterService } from "../../../core/core-toaster.service";
import * as _ from 'lodash';
import { validateConfig } from "@angular/router/src/config";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { AlertDialogComponent } from "../../../shared/component/dialoges/alerts-dialog/alert-dialog.component";
import { TableConfigurationApiService } from "../table-configuration-api.service";
import { CoreFormValidation } from "../../../core/core-form-validation.service";
import { TableConfigurationService } from "../table-configuration.service";



@Component({
    moduleId: module.id,
    selector: 'table-configuration-create',
    templateUrl: 'configuration-create.component.html',
    styleUrls: ['configuration-create.component.scss']
})

export class TableConfigurationCreateComponent implements OnInit {

    tableConfigCreateForm: FormGroup;
    tableLists: Array<string> = [];
    connectionId: number;
    tableDetailsList: Array<TableDetailServiceModel> = [];


    primaryKeyColumnLists: Array<string> = [];
    dropdownSettings: Object;

    mappedColumnNames: any = [];
    selectedItems: any = [];
    refTableListsArray: any = [];
    refTableColumnsList: any = [];
    isCheckedAll: boolean = false;

    constructor(
        private route: ActivatedRoute,
        private fBuilder: FormBuilder,
        private tableConfigurationApiService: TableConfigurationApiService,
        private loderService: LoaderService,
        private coreService: CoreService,
        private coreToasterService: CoreToasterService,
        private modalService: BsModalService,
        private coreToasetrService: CoreToasterService,
        private router: Router,
        private coreFormValidation: CoreFormValidation,
        private tableConfigurationService: TableConfigurationService
    ) {
    }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            let dataToFilter = new DataBaseNameFilterServiceModel();
            this.connectionId = Number(params.id);
            dataToFilter = { connectionId: this.connectionId, isTable: true, isView: false };
            this.getTableLists(dataToFilter);
        });
        this.tableConfigCreateForm = this.tableConfigurationService.initializeTableConfigurationForm();
        this.dropdownSettings = {
            text: "Select Display Columns",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            classes: "my-class",
            maxHeight: 160,
            badgeShowLimit: 2
        };
    }
    checkBoxEvent(whichOne: string) {
        this.tableDetailsList = [];
        this.tableConfigurationService.checkBoxSettings(whichOne.toLowerCase(), this.tableConfigCreateForm, this.connectionId);
        let dataToFilter = new DataBaseNameFilterServiceModel();
        dataToFilter.isTable = this.tableConfigCreateForm.controls['isTable'].value;
        dataToFilter.isView = this.tableConfigCreateForm.controls['isView'].value;
        dataToFilter.connectionId = this.connectionId;
        this.getTableLists(dataToFilter);
    }

    getTableLists = (dataToFilter: DataBaseNameFilterServiceModel) => {
        this.tableConfigurationApiService.getTableNamesFromDatabase(dataToFilter)
            .subscribe(data => {
                if (this.coreService.isNullOrUndefined(data) || data.length === 0) {
                    let message = (dataToFilter.isTable) ? 'There are no table' : 'There are no view';
                    this.coreService.alertDialog(message);
                }
                else {
                    this.tableLists = data;
                }
            });
    }

    getDetailsOfSelectedTabels = (tableName: string) => {
        if (tableName === "" || this.coreService.isNullOrUndefined(tableName)) {
            this.coreToasterService.showError("Something are going Wrong Table name is not find");
        } else {
            let tablefilter = new IdNameServiceModel();
            tablefilter = { id: this.connectionId, name: tableName };
            this.tableConfigurationApiService.getTableDetails(tablefilter)
                .subscribe(data => {
                    this.tableDetailsList = data;
                    if (this.tableDetailsList.length > 0) {
                        this.initializeArrays();
                        this.patchValueOfFormData(this.tableConfigCreateForm, this.tableDetailsList);
                    } else {
                        this.coreService.alertDialog('No columns found....');
                    }
                });
        }
    }
    getRefTableDetailsIndexWise = (i: number) => {
        this.refTableListsArray[i] = this.tableLists;
    }

    getPrimaryKeycolumnsName = (relationShipTableName: string, i: number) => {
        if (relationShipTableName === '' || relationShipTableName === null || relationShipTableName === undefined) {
            this.refTableColumnsList[i] = [];
            this.primaryKeyColumnLists = [];
        } else {
            (<FormGroup>(<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls[i]).controls['primaryTableColumnName'].patchValue('');
            (<FormGroup>(<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls[i]).controls['mappedColumns'].patchValue([]);
            
            let columnFilter = new IdNameServiceModel();
            columnFilter = { id: this.connectionId, name: relationShipTableName };
            this.tableConfigurationApiService.getPrimaryKeyTableColumnName(columnFilter)
                .subscribe(data => {
                    this.primaryKeyColumnLists = data;
                    this.refTableColumnsList[i] = this.primaryKeyColumnLists;
                });
        }
    }

    selectMappedColumnsAfterRefselect = (columnToMap: string, j: number) => {
        this.mappedColumnNames[j] = [];
        this.selectedItems[j] = [];
        let i = 0;
        _.each(this.refTableColumnsList[j], data => {
            let dataToPush = { "itemName": data, "id": i++ };
            if (columnToMap === dataToPush.itemName)
                this.selectedItems[j].push(dataToPush);
            this.mappedColumnNames[j].push(dataToPush);
        });
    }
    checkAll = () => {
        this.tableConfigurationService.checkAll(this.tableConfigCreateForm, this.isCheckedAll);
    }
    checkOne = (isChecked: boolean) => {
        this.isCheckedAll = this.tableConfigurationService.checkOne(this.tableConfigCreateForm, isChecked);
    }
    goToHome = () => {
        this.router.navigate(['/table-configuration']);
    }
    saveTableConfigurations = () => {
        if (this.tableConfigCreateForm.invalid) {
            this.coreFormValidation.formValidate(this.tableConfigCreateForm, false);
        } else {
            // validate FormArry ,If relationship table is selected then others are required
            let error: Array<string> = this.tableConfigurationService.additionalValidationOfFormSaveTime(this.tableConfigCreateForm);
            if (error.length !== 0) {
                let modelRef: BsModalRef = this.modalService.show(AlertDialogComponent)
                modelRef.content.errorMessages = error;
            }
            else {
                //get tableconfiguration Details from form
                let tableConfigurationDetails = this.tableConfigurationService.getTableConfigurationDetailsFromForm(this.tableConfigCreateForm, this.connectionId);

                //Get Details of Fields 
                let fieldConfigurationDetails: FieldConfigurtionDetails[] = [];
                (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls.forEach(ctrl => {
                    let dataToPush = new FieldConfigurtionDetails();
                    if ((<FormGroup>ctrl).controls['isChecked'].value) {
                        dataToPush.sourceTableName = this.tableConfigCreateForm.controls['masterTableName'].value;
                        dataToPush.sourceColumnName = (<FormGroup>ctrl).controls['columnName'].value;
                        dataToPush.referenceTableName = (<FormGroup>ctrl).controls['relationShipTableName'].value;
                        dataToPush.referenceColumnName = (<FormGroup>ctrl).controls['primaryTableColumnName'].value;
                        if (!this.coreService.isNullOrUndefined((<FormGroup>ctrl).controls['mappedColumns'].value) && (<FormGroup>ctrl).controls['mappedColumns'].value !== []) {
                            _.each((<FormGroup>ctrl).controls['mappedColumns'].value, val => {
                                dataToPush.mappedCoumns.push(val.itemName)
                            });
                        }
                        fieldConfigurationDetails.push(dataToPush);
                    }
                });

                if (fieldConfigurationDetails.length === 0) {
                    this.coreService.alertDialog("Please Select atleast one field..");
                } else {
                    let tableAndFieldConfigurations = new TableConfigAndFieldConfigurationsDetails();
                    tableAndFieldConfigurations.tableConfiguration = tableConfigurationDetails;
                    tableAndFieldConfigurations.fieldConfiguration = fieldConfigurationDetails;
                    this.tableConfigurationApiService.saveTableConfigurationDetails(tableAndFieldConfigurations)
                        .subscribe(data => {
                            this.coreToasetrService.showSuccess(data);
                            this.router.navigate(['/table-configuration']);
                        });
                }
            }
        }
    }

    //Initializing Arrays
    private initializeArrays = () => {
        for (let i = 0; i < this.tableDetailsList.length; i++) {
            this.refTableListsArray[i] = [];
            this.refTableColumnsList[i] = [];
            this.mappedColumnNames[i] = [];
            this.selectedItems[i] = [];
        }
    }

    private patchValueOfFormData = (formToPatch: FormGroup, tableDetailsList: Array<TableDetailServiceModel>) => {
        (<FormArray>formToPatch.controls['tableDetailsArray']) = this.fBuilder.array([]);
        let counter = 0;
        tableDetailsList.forEach(table => {
            const fb = this.tableConfigurationService.initializeFormArray();
            fb.controls['columnName'].patchValue(table.columnName);
            fb.controls['relationShipTableName'].patchValue(table.relationShipTableName);
            fb.controls['constraintsType'].patchValue(table.constraintsType);
            fb.controls['primaryTableColumnName'].patchValue(table.primaryTableColumnName);
            fb.controls['isChecked'].setValue(false);
            if (!this.coreService.isNullOrUndefined(table.referenceTableColumns) && table.referenceTableColumns.length > 0) {
                this.refTableColumnsList[counter] = table.referenceTableColumns;
                let i = 0;
                _.each(table.referenceTableColumns, column => {
                    let dataToPush = { "itemName": column, "id": i++ };
                    if (dataToPush.itemName === table.primaryTableColumnName) { this.selectedItems[counter].push(dataToPush); }
                    this.mappedColumnNames[counter].push(dataToPush);
                });

            }
            if (!this.coreService.isNullOrUndefined(table.relationShipTableName) && table.relationShipTableName !== '') {
                this.refTableListsArray[counter] = this.tableLists;

            }
            (<FormArray>formToPatch.controls['tableDetailsArray']).push(fb);
            counter = counter + 1;
        });
    }

    
    deleteConfiguration = (i: number) => {
        this.refTableColumnsList[i] = [];
        this.refTableListsArray[i] = [];
        this.mappedColumnNames[i] = [];
        this.selectedItems[i] = [];

        (<FormGroup>(<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls[i]).controls['relationShipTableName'].patchValue('');
        (<FormGroup>(<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls[i]).controls['primaryTableColumnName'].patchValue('');
        (<FormGroup>(<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls[i]).controls['mappedColumns'].patchValue([]);

    }


}

