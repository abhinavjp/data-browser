import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'
import { PanelModule } from 'primeng/panel';
import { DataTableModule } from 'primeng/datatable';
import { PaginatorModule } from 'primeng/paginator';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataBrowserMainComponent } from './data-browser.component';
import { DatabrowserMainRoutingModule } from './data-browser-main.routing.module';

import { DatabrowserMainApiService, DatabrowserFilterResolver } from './data-browser-main-api.service';


@NgModule({
    imports: [
        CommonModule,
        PanelModule,
        DataTableModule,
        PaginatorModule,
        ReactiveFormsModule,
        FormsModule,
        DatabrowserMainRoutingModule
    ],
    declarations: [
        DataBrowserMainComponent
    ],
    exports: [
        DataBrowserMainComponent
    ],
    entryComponents: [],
    providers: [
        DatabrowserMainApiService,
        DatabrowserFilterResolver
    ],
})
export class DatabrowserMainModule {

}