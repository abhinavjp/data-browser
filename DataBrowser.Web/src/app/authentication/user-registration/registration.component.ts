import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { CoreFormValidation } from '../../core/core-form-validation.service';
import { UserModel } from '../authentication.class';
import { AuthenticationApiService } from '../authentication-api.service';
import { CoreToasterService } from '../../core/core-toaster.service';


@Component({
    moduleId: module.id,
    selector: 'user-registration',
    templateUrl: 'registration.component.html',
    styleUrls: ['registration.component.scss']
})

export class RegistrationComponent implements OnInit {

    registrationForm: FormGroup;
    passwordType: string = "password";
    constructor(
        private activateRoute: Router,
        private authenticationService: AuthenticationService,
        private coreFormValidation: CoreFormValidation,
        private authenticationApiService: AuthenticationApiService,
        private coreToasterService: CoreToasterService, ) {


    }
    ngOnInit(): void {
        this.registrationForm = this.authenticationService.initializeRegistrationForm();
    }

    changePasswordToggle = () => {
        this.passwordType = (this.passwordType === "password") ? "text" : "password";
    }
    gotoLogin() {
        this.activateRoute.navigate(['/login']);
    }
    registerDetail = () => {
        if (this.registrationForm.invalid) {
            this.coreFormValidation.formValidate(this.registrationForm, false, 'toast-top-left');
        }
        else {
            let registerUser = new UserModel();
            Object.assign(registerUser, this.registrationForm.value);
            console.log(registerUser);
            this.authenticationApiService.registerUser(registerUser)
                .subscribe(data => {
                    this.coreToasterService.showSuccess(data, "User Create");
                    this.gotoLogin();
                });
        }
    }
}