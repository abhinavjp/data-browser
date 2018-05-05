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
import { DataBaseConnectionNameResolver, TableConfigurationApiService } from './table-configuration-api.service';


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
        TableConfigurationComponent
    ],
    exports: [
        TableConfigurationCreateComponent,
        TableConfigurationComponent
    ],
    entryComponents: [],
    providers: [
        TableConfigurationApiService,
        DataBaseConnectionNameResolver
    ],
})
export class TableConfigurationModule {

}