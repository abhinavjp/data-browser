import { NgModule } from '@angular/core';
import { RegistrationComponent } from './user-registration/registration.component';
import { LoginComponent } from './user-login/login.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthenticationService } from './authentication.service';
import { CommonModule } from '@angular/common';
import { AuthenticationApiService } from './authentication-api.service';
import { AuthGuard } from './auth/auth-guard.service';


@NgModule({
    imports: [RouterModule, ReactiveFormsModule, CommonModule],
    declarations: [RegistrationComponent, LoginComponent],
    exports: [RegistrationComponent, LoginComponent],
    providers: [AuthenticationService, AuthenticationApiService, AuthGuard]
})

export class AuthenticationModule {

}