import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
import { DataBrowserModule } from './DataBrowser/databrowser.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { SharedModule } from './shared/shared.module';
import { AuthenticationModule } from './authentication/authentication.module';
import { routing } from './app-routing.module';
import { RouterModule, Routes } from '@angular/router';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthIntersapor } from './authentication/auth/auth-intersaptor.service';
import { CommonModule } from '@angular/common';



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
    FormsModule,
    CoreModule,
    DataBrowserModule,
    AuthenticationModule,
    SharedModule,
    routing,
    CommonModule

  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthIntersapor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
