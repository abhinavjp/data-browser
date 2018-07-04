import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { Login } from '../authentication.class';
import { CoreFormValidation } from '../../core/core-form-validation.service';
import { AuthenticationApiService } from '../authentication-api.service';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'user-login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss']
})

export class LoginComponent implements OnInit {

    loginForm: FormGroup;
    constructor(
        private authenticationService: AuthenticationService,
        private coreFormValidation: CoreFormValidation,
        private authenticationApiService: AuthenticationApiService,
        private route:Router
    ) {
    }

    ngOnInit(): void {
        this.loginForm = this.authenticationService.initializeLoginForm();
    }

    login = () => {
        if (this.loginForm.invalid) {
            this.coreFormValidation.formValidate(this.loginForm, true);
        } else {
            let loginUser = new Login();
            Object.assign(loginUser, this.loginForm.value);
            this.authenticationApiService.login(loginUser)
                .subscribe(data => {
                    if (!!data) {
                        localStorage.setItem("token", data.access_token);
                        localStorage.setItem("token_type", data.token_type);

                        this.route.navigate(['/database-configuration']);
                    }
                });

        }
    }
}