import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'
import { DataBaseConfigurationComponent } from './database-configuration-list/database-configuration.component';
import { DataBaseConnectionManagecomponent } from './database-connection-manage/databaseconnection-manage.component';

import { PanelModule } from 'primeng/panel';
import { DatabaseConfigurationRouteModule } from './database-configuration-route.module';
import { DataTableModule } from 'primeng/datatable';
import { PaginatorModule } from 'primeng/paginator';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DatabaseConfigurationApiService, DataBaseConnectionResolver } from './database-configuration-api.service';
import { DatabaseConfigurationService } from './database-configuration.service';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';

@NgModule({
    imports: [
        CommonModule,
        PanelModule,
        DataTableModule,
        PaginatorModule,
        ReactiveFormsModule,
        FormsModule,
        DatabaseConfigurationRouteModule,
        ConfirmDialogModule
    ],
    declarations: [
        DataBaseConfigurationComponent,
        DataBaseConnectionManagecomponent
    ],
    exports: [
        DataBaseConfigurationComponent,
    ],
    entryComponents: [DataBaseConnectionManagecomponent],
    providers: [
        DatabaseConfigurationApiService,
        DataBaseConnectionResolver,
        DatabaseConfigurationService,
        ConfirmationService
    ],
})
export class DatabaseConfigurationModule {

}