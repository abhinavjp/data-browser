import { NgModule, ViewContainerRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

// Core Services
import { CoreService } from './core.service';
import { CoreHttpService } from './core-http.service';
import { CoreToasterService } from './core-toaster.service';
import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { LoaderService } from './loader.service';
import { CoreFormValidation } from './core-form-validation.service';

@NgModule({
    declarations: [

    ],
    imports: [
        CommonModule,
        HttpClientModule,
        ToastModule.forRoot()
    ],
    providers: [
        CoreService,
        CoreHttpService,
        CoreToasterService,
        CoreFormValidation,
        LoaderService
    ],

})
export class CoreModule { }
