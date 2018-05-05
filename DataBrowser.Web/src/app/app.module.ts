import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
import { DataBrowserModule } from './DataBrowser/databrowser.module';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { SharedModule } from './shared/shared.module';
import { DatabaseConfigurationModule } from './DataBrowser/database-configuration/database-configuration.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([]),
    ReactiveFormsModule,
    ToastModule,
    Ng4LoadingSpinnerModule,
    CoreModule,
    DataBrowserModule,
    SharedModule,
    
    
  ],
  providers: [Ng4LoadingSpinnerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
