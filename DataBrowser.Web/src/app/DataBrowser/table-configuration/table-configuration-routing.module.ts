import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CommonModule } from '@angular/common';
import { TableConfigurationComponent } from './table-confiruration-list/table-configuration.component';
import { TableConfigurationCreateComponent } from './table-configuration-manage/configuration-create.component';
import { DataBaseConnectionNameResolver } from './table-configuration-api.service';


const tableConfigRoute: Routes = [
    {
        path: 'table-configuration',
        component: TableConfigurationComponent,
        resolve: {
            databaseConnectionIdName: DataBaseConnectionNameResolver

        },
    },
    {
        path: 'table-configuration-create/:id',
        component: TableConfigurationCreateComponent,
        // resolve: {
        //     databaseConnectionIdName: DataBaseConnectionNameResolver
        // }
    }];

@NgModule({
    imports: [
        RouterModule.forChild(tableConfigRoute),
        CommonModule
    ],
    exports: [RouterModule]
})
export class TableCongigurationRoutingModule {

}