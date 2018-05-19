import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'
import { PanelModule } from 'primeng/panel';
import { DataTableModule } from 'primeng/datatable';
import { PaginatorModule } from 'primeng/paginator';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TableCongigurationRoutingModule } from './table-configuration-routing.module';

import { TableConfigurationCreateComponent } from './table-configuration-manage/configuration-create.component';
import { TableConfigurationComponent } from './table-confiruration-list/table-configuration.component';
import { MultiSelectModule } from 'primeng/multiselect';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown/multiselect.component';
import { DataBaseConnectionNameResolver, TableConfigurationApiService, TableConfigurationEditResolver } from './table-configuration-api.service';
import { TableConfigurationService } from './table-configuration.service';
import { TableConfigurationEditComponent } from './table-configuration-manage/configuration-edit.component';


@NgModule({
    imports: [
        CommonModule,
        PanelModule,
        DataTableModule,
        PaginatorModule,
        ReactiveFormsModule,
        FormsModule,
        AngularMultiSelectModule,
        TableCongigurationRoutingModule
    ],
    declarations: [
        TableConfigurationCreateComponent,
        TableConfigurationComponent,
        TableConfigurationEditComponent
    ],
    exports: [
        TableConfigurationCreateComponent,
        TableConfigurationComponent,
        TableConfigurationEditComponent
    ],
    entryComponents: [],
    providers: [
        TableConfigurationApiService,
        DataBaseConnectionNameResolver,
        TableConfigurationService,
        TableConfigurationEditResolver
    ],
})
export class TableConfigurationModule {

}