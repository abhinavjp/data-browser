import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ActivatedRoute } from '@angular/router';
import { filter } from 'rxjs/operator/filter';
import { LoaderService } from '../../../core/loader.service';
import { CoreToasterService } from '../../../core/core-toaster.service';
import { DataBaseConnectionManagecomponent } from '../database-connection-manage/databaseconnection-manage.component';
import { ConfirmDialogComponent } from '../../../shared/component/dialoges/confirm-dialog/confirm.component';
import { DatabaseConfigurationApiService } from '../database-configuration-api.service';
import { ConfirmationService } from 'primeng/api';


@Component({
    moduleId: module.id,
    selector: 'database-configuration',
    templateUrl: 'database-configuration.component.html',
    styleUrls: ['database-configuration.component.scss']
})
export class DataBaseConfigurationComponent implements OnInit {
    private columnDefs;
    private rowData;

    constructor(
        private modalService: BsModalService,
        private databaseConfigurationApiService: DatabaseConfigurationApiService,
        private route: ActivatedRoute,
        private loaderService: LoaderService,
        private coreToasterService: CoreToasterService,
        private confirmationService: ConfirmationService) {
    }

    ngOnInit(): void {
        this.route.data.map(data => data.databaseConnection).subscribe((res) => {
            this.rowData = res.result;
        });
    }
    createDataBaseConnection() {
        let modelRef: BsModalRef = this.modalService.show(DataBaseConnectionManagecomponent, { class: 'modal-md' })
        this.modalService.onHide.subscribe(data => {
            if (data === 'Yes') {
                this.getAllDatabaseConnections();
            } else { }
        });
    }
    getAllDatabaseConnections = () => {
        this.databaseConfigurationApiService.getDataBaseConnection()
            .subscribe(data => {
                this.rowData = data;
            });
    }
    // deleteDatabaseConnection(id: number, name: string) {
    //     let modelRef: BsModalRef = this.modalService.show(ConfirmDialogComponent, { class: 'modal-sm' });
    //     modelRef.content.name = name;
    //     this.modalService.onHide.subscribe(data => {
    //         if (data === 'YES') {
    //             debugger;
    //             this.databaseConfigurationApiService.deleteDatabaseConnection(id)
    //                 .subscribe(data => {
    //                     this.coreToasterService.showSuccess(data);
    //                     this.getAllDatabaseConnections();
    //                 });
    //         } else {
    //             console.log(data);
    //         }
    //     });
    // }


    deleteDatabaseConnection = (id: number, name: string) => {
        this.confirmationService.confirm({
            message: 'Do you want to delete ' + name + ' ?',
            header: 'Delete Confirmation',
            icon: 'fa fa-trash',
            accept: () => {
                this.databaseConfigurationApiService.deleteDatabaseConnection(id)
                    .subscribe(data => {
                        this.coreToasterService.showSuccess(data);
                        this.getAllDatabaseConnections();
                    });
            },
            reject: () => {
                //this.msgs = [{severity:'info', summary:'Rejected', detail:'You have rejected'}];
            }
        });
    }


}
