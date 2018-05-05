import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';


const routes: Routes = [
    {
        path: 'database-configuration',
        loadChildren: 'app/DataBrowser/database-configuration/database-configuration.module#DatabaseConfigurationModule'
    },
    {
        path: 'table-configuration',
        loadChildren: 'app/DataBrowser/table-configuration/table-configuration.module#TableConfigurationModule'
    },
    {
        path: 'data-browser',
        loadChildren: 'app/DataBrowser/data-browser-main/data-browser-main.module#DatabrowserMainModule'
    },
    {
        path: '',
        redirectTo: '',
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes),
        CommonModule
    ],
    exports: [RouterModule]
})
export class DatabrowserRoutingModule {

}