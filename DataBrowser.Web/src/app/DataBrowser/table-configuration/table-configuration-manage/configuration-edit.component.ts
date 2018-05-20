import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { TableConfigAndFieldConfigurationsDetails, FieldConfigurtionDetails, IdNameServiceModel, DataBaseNameFilterServiceModel, TableDetailServiceModel } from "../table-configuration.class";
import { FormGroup, FormArray, FormControl, FormBuilder } from "@angular/forms";
import { TableConfigurationService } from "../table-configuration.service";
import * as _ from 'lodash';
import { CoreService } from "../../../core/core.service";
import { TableConfigurationApiService } from "../table-configuration-api.service";
import { CoreToasterService } from "../../../core/core-toaster.service";
import { CoreFormValidation } from "../../../core/core-form-validation.service";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { AlertDialogComponent } from "../../../shared/component/dialoges/alerts-dialog/alert-dialog.component";

@Component({
    moduleId: module.id,
    selector: 'table-configuration-edit',
    templateUrl: 'configuration-create.component.html',
    styleUrls: ['configuration-create.component.scss']
})

export class TableConfigurationEditComponent implements OnInit {

    tableConfigCreateForm: FormGroup;
    tableAndFieldConfiguration = new TableConfigAndFieldConfigurationsDetails();

    tableDetailsList: Array<FieldConfigurtionDetails> | Array<TableDetailServiceModel> = [];

    primaryKeyColumnLists: Array<string> = [];
    dropdownSettings: Object;
    tableLists: Array<string> = [];

    mappedColumnNames: any = [];
    selectedItems: any = [];
    refTableListsArray: any = [];
    refTableColumnsList: any = [];
    isCheckedAll: boolean = false;
    isEditScreen: boolean = true;

    constructor(
        private router: Router,
        private fBuilder: FormBuilder,
        private route: ActivatedRoute,
        private tableConfigurationService: TableConfigurationService,
        private coreService: CoreService,
        private tableConfigurationApiService: TableConfigurationApiService,
        private coreToasterService: CoreToasterService,
        private coreFormValidation: CoreFormValidation,
        private modalService: BsModalService, ) {

    }

    ngOnInit(): void {
        this.route.data.map(data => data.tableAndFieldDetailsToedit).subscribe((res) => {
            this.tableAndFieldConfiguration = res;
            this.tableAndFieldConfiguration.tableConfiguration.isTable = (this.coreService.isNullOrUndefined(this.tableAndFieldConfiguration.tableConfiguration.isTable)) ? true : this.tableAndFieldConfiguration.tableConfiguration.isTable;
            this.tableConfigCreateForm = this.tableConfigurationService.initializeTableConfigurationForm();
            this.tableConfigCreateForm.patchValue(this.tableAndFieldConfiguration.tableConfiguration);

            if (this.tableAndFieldConfiguration.fieldConfiguration.length > 0) {
                this.initializeArrays();
                this.tableDetailsList = this.tableAndFieldConfiguration.fieldConfiguration;

                let dataTofilter = new DataBaseNameFilterServiceModel();
                dataTofilter.connectionId = this.tableAndFieldConfiguration.tableConfiguration.connectionId;
                dataTofilter.isTable = this.tableAndFieldConfiguration.tableConfiguration.isTable;
                dataTofilter.isView = this.tableAndFieldConfiguration.tableConfiguration.isView;
                this.getTableLists(dataTofilter, true);
            }
        });

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
        this.tableConfigurationService.checkBoxSettings(whichOne.toLowerCase(), this.tableConfigCreateForm, this.tableAndFieldConfiguration.tableConfiguration.connectionId);
        let dataToFilter = new DataBaseNameFilterServiceModel();
        dataToFilter.isTable = this.tableConfigCreateForm.controls['isTable'].value;
        dataToFilter.isView = this.tableConfigCreateForm.controls['isView'].value;
        dataToFilter.connectionId = this.tableAndFieldConfiguration.tableConfiguration.connectionId;
        this.getTableLists(dataToFilter, false);
    }

    getTableLists = (dataToFilter: DataBaseNameFilterServiceModel, isCallMethod?: boolean) => {
        this.tableConfigurationApiService.getTableNamesFromDatabase(dataToFilter)
            .subscribe(data => {
                if (this.coreService.isNullOrUndefined(data) || data.length === 0) {
                    let message = (dataToFilter.isTable) ? 'There are no table' : 'There are no view';
                    this.coreService.alertDialog(message);
                }
                else {
                    this.tableLists = data;
                    //Patch value first time to enter in edit only calls one time
                    if (isCallMethod)
                        this.patchValueOfFormDataAtEditTime(this.tableConfigCreateForm, this.tableDetailsList);
                }
            });
    }

    getRefTableDetailsIndexWise = (i: number) => {
        this.refTableListsArray[i] = this.tableLists;;
    }
    getPrimaryKeycolumnsName = (relationShipTableName: string, i: number) => {
        if (relationShipTableName === '' || relationShipTableName === null || relationShipTableName === undefined) {
            this.refTableColumnsList[i] = [];
            this.primaryKeyColumnLists = [];
        } else {
            (<FormGroup>(<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls[i]).controls['primaryTableColumnName'].patchValue('');
            (<FormGroup>(<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls[i]).controls['mappedColumns'].patchValue([]);
            let columnFilter = new IdNameServiceModel();
            columnFilter = { id: this.tableAndFieldConfiguration.tableConfiguration.connectionId, name: relationShipTableName };
            this.tableConfigurationApiService.getPrimaryKeyTableColumnName(columnFilter)
                .subscribe(data => {
                    this.tableConfigCreateForm.controls['']
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

    //Initializing Arrays
    private initializeArrays = () => {
        for (let i = 0; i < this.tableAndFieldConfiguration.fieldConfiguration.length; i++) {
            this.refTableListsArray[i] = [];
            this.refTableColumnsList[i] = [];
            this.mappedColumnNames[i] = [];
            this.selectedItems[i] = [];
        }
    }

    private patchValueOfFormDataAtEditTime = (formToPatch: FormGroup, tableDetailLists: Array<FieldConfigurtionDetails>) => {
        (<FormArray>formToPatch.controls['tableDetailsArray']) = this.fBuilder.array([]);
        let counter = 0;
        tableDetailLists.forEach(table => {
            const fb = this.tableConfigurationService.initializeFormArray();
            fb.controls['id'].patchValue(table.id);
            fb.controls['columnName'].patchValue(table.sourceColumnName);
            fb.controls['relationShipTableName'].patchValue(table.referenceTableName);
            fb.controls['constraintsType'].patchValue(table.constraintsType);
            fb.controls['primaryTableColumnName'].patchValue(table.referenceColumnName);

            if (table.referenceTableName !== '' && !this.coreService.isNullOrUndefined(table.referenceTableName)) {
                this.refTableListsArray[counter] = this.tableLists;
            }
            if (!this.coreService.isNullOrUndefined(table.referenceTableColumns) && table.referenceTableColumns.length > 0) {
                this.refTableColumnsList[counter] = table.referenceTableColumns;
                let i = 0;
                _.each(table.referenceTableColumns, column => {
                    let dataToPush = { "itemName": column, "id": i++ };
                    let ifIts = _.includes(table.mappedCoumns, column);
                    if (ifIts) { this.selectedItems[counter].push(dataToPush); }
                    this.mappedColumnNames[counter].push(dataToPush);
                });
            }
            if (!table.isDisplay) {
                fb.controls['isChecked'].patchValue(false);
            } else {
                fb.controls['isChecked'].patchValue(true);
            }

            (<FormArray>formToPatch.controls['tableDetailsArray']).push(fb);
            counter = counter + 1;
        });
    }

    saveTableConfigurations = () => {
        if (this.tableConfigCreateForm.invalid) {
            this.coreFormValidation.formValidate(this.tableConfigCreateForm, false);
        } else {
            let error: Array<string> = this.tableConfigurationService.additionalValidationOfFormSaveTime(this.tableConfigCreateForm);
            if (error.length !== 0) {
                let modelRef: BsModalRef = this.modalService.show(AlertDialogComponent)
                modelRef.content.errorMessages = error;
            } else {
                //get tableconfiguration Details from form
                let tableConfigurationDetails = this.tableConfigurationService.getTableConfigurationDetailsFromForm(this.tableConfigCreateForm, this.tableAndFieldConfiguration.tableConfiguration.connectionId);
                //Get Details of Fields 
                let fieldConfigurationDetails: FieldConfigurtionDetails[] = [];

                (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls.forEach(ctrl => {
                    let dataToPush = new FieldConfigurtionDetails();
                    if ((<FormGroup>ctrl).controls['isChecked'].value) {
                        dataToPush.id = (<FormGroup>ctrl).controls['id'].value
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
                    let tableAndFieldConfigurationss = new TableConfigAndFieldConfigurationsDetails();
                    tableAndFieldConfigurationss.tableConfiguration = tableConfigurationDetails;
                    tableAndFieldConfigurationss.fieldConfiguration = fieldConfigurationDetails;

                    console.log(tableAndFieldConfigurationss);
                    this.tableConfigurationApiService.UpdateTableConfigurationDetails(tableAndFieldConfigurationss)
                        .subscribe(data => {
                            this.coreToasterService.showSuccess(data);
                            this.router.navigate(['/table-configuration']);
                        });
                }
            }
        }

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

    // getDetailsOfSelectedTabels = (tableName: string) => {
    //     if (tableName === "" || this.coreService.isNullOrUndefined(tableName)) {
    //         this.coreToasterService.showError("Something are going Wrong Table name is not find");
    //     } else {
    //         let tablefilter = new IdNameServiceModel();
    //         tablefilter = { id: this.tableAndFieldConfiguration.tableConfiguration.connectionId, name: tableName };
    //         this.tableConfigurationApiService.getTableDetails(tablefilter)
    //             .subscribe(data => {
    //                 this.tableDetailsList = data;
    //                 if (this.tableDetailsList.length > 0) {
    //                     this.initializeArrays();
    //                     this.patchValueOfFormDataAfterTableChange(this.tableConfigCreateForm, this.tableDetailsList);

    //                 } else {
    //                     this.coreService.alertDialog('No columns found....');
    //                 }
    //             });
    //     }
    // }

    // private patchValueOfFormDataAfterTableChange = (formToPatch: FormGroup, tableDetailsList: any) => {
    //     (<FormArray>formToPatch.controls['tableDetailsArray']) = this.fBuilder.array([]);
    //     let counter = 0;
    //     tableDetailsList.forEach(table => {
    //         const fb = this.tableConfigurationService.initializeFormArray();
    //         fb.controls['columnName'].patchValue(table.columnName);
    //         fb.controls['relationShipTableName'].patchValue(table.relationShipTableName);
    //         fb.controls['constraintsType'].patchValue(table.constraintsType);
    //         fb.controls['primaryTableColumnName'].patchValue(table.primaryTableColumnName);
    //         fb.controls['isChecked'].setValue(false);
    //         if (!this.coreService.isNullOrUndefined(table.referenceTableColumns) && table.referenceTableColumns.length > 0) {
    //             this.refTableColumnsList[counter] = table.referenceTableColumns;
    //             let i = 0;
    //             _.each(table.referenceTableColumns, column => {
    //                 let dataToPush = { "itemName": column, "id": i++ };
    //                 if (dataToPush.itemName === table.primaryTableColumnName) { this.selectedItems[counter].push(dataToPush); }
    //                 this.mappedColumnNames[counter].push(dataToPush);
    //             });

    //         }
    //         if (!this.coreService.isNullOrUndefined(table.relationShipTableName) && table.relationShipTableName !== '') {
    //             this.refTableListsArray[counter] = this.tableLists;

    //         }
    //         (<FormArray>formToPatch.controls['tableDetailsArray']).push(fb);
    //         counter = counter + 1;
    //     });
    // }
}
