import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { HttpClientModule } from '@angular/common/http';
//PrimeNg
import { DropdownModule } from 'primeng/dropdown';
//NgX-Bootstrap
import { ModalModule } from 'ngx-bootstrap';
import { TooltipModule } from 'ngx-bootstrap';


import { CommonModule } from '@angular/common';
import { DataBrowserMainComponent } from './data-browser-main/data-browser.component';
import { DatabaseConfigurationModule } from './database-configuration/database-configuration.module';
import { DatabrowserRoutingModule } from './databrowser-routing.module';
import { TableConfigurationModule } from './table-configuration/table-configuration.module';
import { DatabrowserMainModule } from './data-browser-main/data-browser-main.module';




@NgModule({
    imports: [
        ModalModule.forRoot(),
        TooltipModule.forRoot(),
        CommonModule,
        DropdownModule,
        DatabaseConfigurationModule,
        TableConfigurationModule,
        DatabrowserMainModule
    ],
    declarations: [

    ],
    entryComponents: [],
    exports: [
        DatabrowserRoutingModule
    ],
    providers: [
    ],
})
export class DataBrowserModule { }
