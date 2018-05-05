import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DataBrowserMainComponent } from './data-browser.component';
import { DatabrowserFilterResolver } from './data-browser-main-api.service';


const dataBrowserConfigRoute: Routes = [
    {
        path: 'data-browser',
        component: DataBrowserMainComponent,
        resolve: {
            dataBrowserFilters: DatabrowserFilterResolver
        }
    },
]

@NgModule({
    imports: [
        RouterModule.forChild(dataBrowserConfigRoute),
        CommonModule
    ],
    exports:[RouterModule]
})
export class DatabrowserMainRoutingModule {

}