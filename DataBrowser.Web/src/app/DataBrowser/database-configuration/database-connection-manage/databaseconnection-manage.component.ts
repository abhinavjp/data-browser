import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CoreService } from '../../../core/core.service';
import { DataBaseNameListFilterServiceModel, DataBaseConnectionServiceModel } from '../database-configuration.class';
import { CoreToasterService } from '../../../core/core-toaster.service';
import { HttpErrorResponse } from '@angular/common/http';
import { LoaderService } from '../../../core/loader.service';
import { DatabaseConfigurationApiService } from '../database-configuration-api.service';
import { CoreFormValidation } from '../../../core/core-form-validation.service';
import { DatabaseConfigurationService } from '../database-configuration.service';

@Component({
    moduleId: module.id,
    selector: 'database-connection',
    templateUrl: 'databaseconnection-manage.component.html',

})
export class DataBaseConnectionManagecomponent implements OnInit, OnDestroy {

    dataBaseConnecationForm: FormGroup;
    private dataBaseNameLists: Array<string> = [];

    private alive: boolean = true;

    constructor(
        private modelClose: BsModalRef,
        private modelServices: BsModalService,
        private fBuilder: FormBuilder,
        private coreService: CoreService,
        private coreFormValidation: CoreFormValidation,
        private databaseConfigurationApiService: DatabaseConfigurationApiService,
        private coreToasterService: CoreToasterService,
        private loaderService: LoaderService,
        private databaseConfigurationService: DatabaseConfigurationService) {
    }

    ngOnInit(): void {
        this.dataBaseConnecationForm = this.databaseConfigurationService.initializeDatabaseConnectionFormControls();
    }

    getDataBaseLists = () => {
        if (this.dataBaseConnecationForm.invalid) {
            this.coreFormValidation.formValidate(this.dataBaseConnecationForm, true);
        } else {
            let dataBaseNamelistFilterServiceModel = new DataBaseNameListFilterServiceModel();
            Object.assign(dataBaseNamelistFilterServiceModel, this.dataBaseConnecationForm.value);
            this.databaseConfigurationApiService.getDataBaseNameLists(dataBaseNamelistFilterServiceModel)
                .takeWhile(() => this.alive)
                .subscribe(list => {
                    if (!this.coreService.isNullOrUndefined(list))
                        this.dataBaseNameLists = list;
                });
        }
    }
    saveDataBaseConnection() {
        if (this.dataBaseConnecationForm.controls['dataBaseName'].value === '' || this.dataBaseConnecationForm.controls['dataBaseName'].value === null || this.dataBaseConnecationForm.controls['dataBaseName'].value === undefined) {
            this.coreToasterService.showError('Database name is required', 'Database Connection');
        } else {
            if (this.dataBaseConnecationForm.invalid) {
                this.coreFormValidation.formValidate(this.dataBaseConnecationForm, true);
            } else {
                this.loaderService.display(true);
                let dataBaseConnection = new DataBaseConnectionServiceModel();
                Object.assign(dataBaseConnection, this.dataBaseConnecationForm.value);
                this.databaseConfigurationApiService.createDataBaseConnection(dataBaseConnection)
                    .takeWhile(() => this.alive)
                    .subscribe(data => {
                        this.coreToasterService.showSuccess('Database Connection Create Successfully', 'Database Connection');
                        this.modelServices.setDismissReason('Yes');
                        this.modelClose.hide();
                    });
            }
        }
    }
    closeModel() {
        this.modelServices.setDismissReason('No');
        this.modelClose.hide();
    }

    ngOnDestroy(): void {
        this.alive = false;
    }
}
