import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

//ALL Components
import { AlertDialogComponent } from './component/dialoges/alerts-dialog/alert-dialog.component';
import { SingleAlertDialogComponent } from './component/dialoges/alert-single/alert-single.component';
import { ConfirmDialogComponent } from './component/dialoges/confirm-dialog/confirm.component';
import { ServerSideExceptionCatchComponent } from './component/dialoges/exception-catch-server/exception-catch.component';
import { SidebarComponent } from './component/sidebar/sidebar.component';
import { HeaderComponent } from './component/header/header.component';
import { DataBrowserMainComponent } from '../DataBrowser/data-browser-main/data-browser.component';
import { TableConfigurationCreateComponent } from '../DataBrowser/table-configuration/table-configuration-manage/configuration-create.component';

@NgModule({
    declarations: [
        SidebarComponent,
        HeaderComponent,
        AlertDialogComponent,
        SingleAlertDialogComponent,
        ConfirmDialogComponent,
        ServerSideExceptionCatchComponent
    ],
    exports: [
        SidebarComponent,
        HeaderComponent,

    ],
    entryComponents: [
        AlertDialogComponent,
        SingleAlertDialogComponent,
        ConfirmDialogComponent,
        ServerSideExceptionCatchComponent
    ],
    imports: [
        RouterModule,
        CommonModule
    ],
    providers: [],

})
export class SharedModule { }
