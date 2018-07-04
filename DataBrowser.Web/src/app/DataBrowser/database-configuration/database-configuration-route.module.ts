import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DataBaseConfigurationComponent } from './database-configuration-list/database-configuration.component';
import { CommonModule } from '@angular/common';
import { DataBaseConnectionResolver } from './database-configuration-api.service';
import { AuthGuard } from '../../authentication/auth/auth-guard.service';

const databaseConfigRoute: Routes = [
    {
        path: 'database-configuration',
        component: DataBaseConfigurationComponent,
        resolve: {
            databaseConnection: DataBaseConnectionResolver
        },
        canActivate:[AuthGuard]
    }]

@NgModule({
    imports: [
        RouterModule.forChild(databaseConfigRoute),
        CommonModule  
    ],
    exports: [RouterModule]
})
export class DatabaseConfigurationRouteModule {
}