import { IdNameServiceModel, DataBaseNameFilterServiceModel, TableDetailServiceModel, TableConfigurationDetails } from "./table-configuration.class";
import { CoreService } from "../../core/core.service";
import * as _ from 'lodash';
import { TableConfigurationApiService } from "./table-configuration-api.service";
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from "@angular/forms";
import { Injectable } from "@angular/core";

@Injectable()
export class TableConfigurationService {
    constructor(
        private fBuilder: FormBuilder,
        private coreService: CoreService,
        private tableConfigurationApiService: TableConfigurationApiService
    ) {
    }

    /////  ============================================================== Create Table Configurations ===================================================================
    initializeTableConfigurationForm = () => {
        return this.fBuilder.group({
            name: ['', Validators.required,],
            dataKey: ['', Validators.required],
            masterTableName: ['', Validators.required],
            isTable: [true],
            isView: [],
            tableDetailsArray: this.fBuilder.array([]),

        });
    }
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

    checkBoxSettings = (from: string, formToSet: FormGroup, connectionId: number) => {
        switch (from) {
            case "table":
                formToSet.controls['masterTableName'].patchValue('');
                if (formToSet.controls['isTable'].value) {
                    formToSet.controls['isView'].setValue(false);
                    formToSet.controls['isTable'].setValue(true);
                } else {
                    formToSet.controls['isView'].setValue(true);
                    formToSet.controls['isTable'].setValue(false);
                }
                break;
            case "view":
                formToSet.controls['masterTableName'].patchValue('');
                if (formToSet.controls['isView'].value) {
                    formToSet.controls['isView'].setValue(true);
                    formToSet.controls['isTable'].setValue(false);
                } else {
                    formToSet.controls['isView'].setValue(false);
                    formToSet.controls['isTable'].setValue(true);
                }
                break;
            default:
                confirm("Sorry, hahahhahah!");
        }
    }
    checkAll = (tableConfigurationForm: FormGroup, isCheckedAll: boolean) => {
        (<FormArray>tableConfigurationForm.controls['tableDetailsArray']).controls.forEach(ctrl => {
            if (isCheckedAll)
                (<FormGroup>ctrl).controls['isChecked'].setValue(true);
            else
                (<FormGroup>ctrl).controls['isChecked'].setValue(false);
        });
    }
    checkOne = (tableConfigurationForm: FormGroup, isChecked: boolean): boolean => {
        if (isChecked) {
            let lengthOfArry = (<FormArray>tableConfigurationForm.controls['tableDetailsArray']).length;
            let isCheckedLength = (<FormArray>tableConfigurationForm.controls['tableDetailsArray']).controls.filter(ctrl => { return (<FormGroup>ctrl).controls['isChecked'].value == true; }).length;
            return (lengthOfArry === isCheckedLength);
        } else {
            return false;
        }
    }

    patchValueOfFormData = (formToPatch: FormGroup, tableDetailsList: Array<TableDetailServiceModel>) => {
        (<FormArray>formToPatch.controls['tableDetailsArray']) = this.fBuilder.array([]);
        tableDetailsList.forEach(table => {
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
            (<FormArray>formToPatch.controls['tableDetailsArray']).push(fb);
        });
    }

    additionalValidationOfFormSaveTime = (tableConfigCreateForm: FormGroup) => {
        let counter = 0;
        let error: string[] = [];
        (<FormArray>tableConfigCreateForm.controls['tableDetailsArray']).controls.forEach(cntrl => {
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
        return (error.length > 0) ? error : [];
    }

    getTableConfigurationDetailsFromForm = (tableConfigCreateForm: FormGroup, connectionId: number): TableConfigurationDetails => {
        let tableConfigDetails: TableConfigurationDetails = {
            name: tableConfigCreateForm.controls['name'].value,
            dataKey: tableConfigCreateForm.controls['dataKey'].value,
            masterTableName: tableConfigCreateForm.controls['masterTableName'].value,
            isTable: tableConfigCreateForm.controls['isTable'].value,
            isView: this.coreService.isNullOrUndefined(tableConfigCreateForm.controls['isView'].value) ? false : tableConfigCreateForm.controls['isView'].value,
            connectionId: connectionId
        };
        return tableConfigDetails;
    }

    //=================================================================================================================================================


}