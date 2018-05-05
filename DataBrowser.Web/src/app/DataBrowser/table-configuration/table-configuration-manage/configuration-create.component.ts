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
        private coreFormValidation: CoreFormValidation
    ) {
    }


    //logic
    ngOnInit(): void {
        this.route.params.subscribe(params => {
            let dataToFilter = new DataBaseNameFilterServiceModel();
            this.connectionId = Number(params.id);
            dataToFilter.connectionId = this.connectionId;
            dataToFilter.isTable = true;
            dataToFilter.isView = false;
            this.getTableLists(dataToFilter);
        });

        this.initializeForm();
        this.dropdownSettings = {
            text: "Select Display Columns",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            classes: "my-class",
            maxHeight: 160,
            badgeShowLimit: 2
        };

        // this.tableConfigCreateForm.controls['tableDetailsArray'].valueChanges.subscribe(
        //     (selectedValue) => {
        //         console.log(selectedValue);
        //     }
        // );
    }

    //logic
    initializeForm = () => {
        this.tableConfigCreateForm = this.fBuilder.group({
            name: ['', Validators.required,],
            dataKey: ['', Validators.required],
            masterTableName: ['', Validators.required],
            isTable: [true],
            isView: [],
            tableDetailsArray: this.fBuilder.array([]),

        });
    }

    //logic
    initializeFormArray = () => {
        return this.fBuilder.group({
            columnName: [''],
            alias: [''],
            constraintsType: [''],
            relationShipTableName: [''],
            primaryTableColumnName: [''],
            isSaved: [''],
            mappedColumns: [[]],
            isChecked: [],
        });
    }

    //logic
    checkBoxEventTable() {
        this.tableDetailsList = [];
        this.tableConfigCreateForm.controls['masterTableName'].patchValue('');
        if (this.tableConfigCreateForm.controls['isTable'].value) {
            this.tableConfigCreateForm.controls['isView'].setValue(false);
            this.tableConfigCreateForm.controls['isTable'].setValue(true);
        } else {
            this.tableConfigCreateForm.controls['isView'].setValue(true);
            this.tableConfigCreateForm.controls['isTable'].setValue(false);
        }

        let dataToFilter = new DataBaseNameFilterServiceModel();
        dataToFilter.isTable = this.tableConfigCreateForm.controls['isTable'].value;
        dataToFilter.isView = this.tableConfigCreateForm.controls['isView'].value;
        dataToFilter.connectionId = this.connectionId;

        this.getTableLists(dataToFilter);
    }

    //logic
    checkBoxEventView() {
        this.tableDetailsList = [];
        this.tableConfigCreateForm.controls['masterTableName'].patchValue('');
        if (this.tableConfigCreateForm.controls['isView'].value) {
            this.tableConfigCreateForm.controls['isView'].setValue(true);
            this.tableConfigCreateForm.controls['isTable'].setValue(false);
        } else {
            this.tableConfigCreateForm.controls['isView'].setValue(false);
            this.tableConfigCreateForm.controls['isTable'].setValue(true);
        }
        let dataToFilter = new DataBaseNameFilterServiceModel();
        dataToFilter.isTable = this.tableConfigCreateForm.controls['isTable'].value;
        dataToFilter.isView = this.tableConfigCreateForm.controls['isView'].value;
        dataToFilter.connectionId = this.connectionId;
        this.getTableLists(dataToFilter);
    }

    getTableLists = (dataToFilter: DataBaseNameFilterServiceModel) => {
        this.tableConfigurationApiService.getTableNamesFromDatabase(dataToFilter)
            .subscribe(data => {
                this.tableLists = data;
                if (this.coreService.isNullOrUndefined(data) || data.length === 0) {
                    let message = (dataToFilter.isTable) ? 'There are no table' : 'There are no view';
                    this.coreService.alertDialog(message);
                }
            });
    }

    getDetailsOfSelecredTabels = (tableName: string) => {
        if (tableName === "" || this.coreService.isNullOrUndefined(tableName)) {
            this.coreToasterService.showError("Something are going Wrong Table name is not find");
        } else {
            let tablefilter = new IdNameServiceModel();
            tablefilter = {
                id: this.connectionId,
                name: tableName
            };
            this.tableConfigurationApiService.getTableDetails(tablefilter)
                .subscribe(data => {
                    this.tableDetailsList = data;

                    if (this.tableDetailsList.length > 0) {
                        (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']) = this.fBuilder.array([]);
                        let counter = 0;
                        this.tableDetailsList.forEach(table => {
                            this.refTableListsArray[counter] = [];
                            this.refTableColumnsList[counter] = [];
                            this.mappedColumnNames[counter] = [];
                            this.selectedItems[counter] = [];

                            const fb = this.initializeFormArray();
                            fb.controls['columnName'].patchValue(table.columnName);
                            fb.controls['relationShipTableName'].patchValue(table.relationShipTableName);
                            fb.controls['constraintsType'].patchValue(table.constraintsType);
                            fb.controls['primaryTableColumnName'].patchValue(table.primaryTableColumnName);
                            fb.controls['isChecked'].setValue(false);

                            if (table.primaryTableColumnName !== '' && table.primaryTableColumnName !== null && table.primaryTableColumnName !== undefined)
                                fb.controls['isSaved'].setValue(true);
                            else
                                fb.controls['isSaved'].setValue(false);
                            (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).push(fb);
                            counter++;
                        });

                        //assign data which have allready relationship created for mappdColumnDropdown
                        for (let i = 0; i < this.tableDetailsList.length; i++) {
                            if (this.tableDetailsList[i].relationShipTableName !== '') {
                                let columnFilter = new IdNameServiceModel();
                                columnFilter = {
                                    id: this.connectionId,
                                    name: this.tableDetailsList[i].relationShipTableName
                                };
                                this.tableConfigurationApiService.getPrimaryKeyTableColumnName(columnFilter)
                                    .subscribe(data => {
                                        this.primaryKeyColumnLists = data;
                                        this.refTableColumnsList[i] = this.primaryKeyColumnLists;
                                        if (this.tableDetailsList[i].primaryTableColumnName !== '' && !this.coreService.isNullOrUndefined(this.tableDetailsList[i].primaryTableColumnName)) {
                                            this.selectMappedColumnsAfterRefselect(this.tableDetailsList[i].primaryTableColumnName, i);
                                        }
                                    });
                            }
                        }

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
        (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls.forEach(ctrl => {
            if (this.isCheckedAll)
                (<FormGroup>ctrl).controls['isChecked'].setValue(true);
            else
                (<FormGroup>ctrl).controls['isChecked'].setValue(false);
        });
    }
    checkOne = (isChecked: boolean) => {
        if (isChecked) {
            let lengthOfArry = (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).length;
            let isCheckedLength = (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls.filter(ctrl => { return (<FormGroup>ctrl).controls['isChecked'].value == true; }).length;
            this.isCheckedAll = (lengthOfArry === isCheckedLength);
        } else {
            this.isCheckedAll = false;
        }
    }
    goToHome = () => {
        this.router.navigate(['/table-configuration']);
    }
    saveTableConfigurations = () => {
        debugger;
        if (this.tableConfigCreateForm.invalid) {
            this.coreFormValidation.formValidate(this.tableConfigCreateForm, false);
        } else {
            // validate FormArry for
            let error: Array<string> = [];
            let counter = 0;
            (<FormArray>this.tableConfigCreateForm.controls['tableDetailsArray']).controls.forEach(cntrl => {
                counter = counter + 1;
                if (cntrl instanceof FormGroup) {
                    if (cntrl.controls['relationShipTableName'] instanceof FormControl) {
                        if (!this.coreService.isNullOrUndefined(cntrl.controls['relationShipTableName'].value) && cntrl.controls['relationShipTableName'].value !== '') {
                            if (this.coreService.isNullOrUndefined(cntrl.controls['primaryTableColumnName'].value) || cntrl.controls['primaryTableColumnName'].value === '') {
                                let err = "Reference Column Name " + counter + " is required";
                                error.push(err);
                            }
                        }
                    }
                }
            });
            if (error.length !== 0) {
                let modelRef: BsModalRef = this.modalService.show(AlertDialogComponent)
                modelRef.content.errorMessages = error;
            }
            else {
                //get tableconfiguration Details
                let tableConfigurationDetails = new TableConfigurationDetails();
                tableConfigurationDetails = {
                    name: this.tableConfigCreateForm.controls['name'].value,
                    dataKey: this.tableConfigCreateForm.controls['dataKey'].value,
                    masterTableName: this.tableConfigCreateForm.controls['masterTableName'].value,
                    isTable: this.tableConfigCreateForm.controls['isTable'].value,
                    isView: this.tableConfigCreateForm.controls['isView'].value,
                    connectionId: this.connectionId
                }
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
}

