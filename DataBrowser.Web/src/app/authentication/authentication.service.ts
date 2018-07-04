import { Injectable } from "@angular/core";
import { FormBuilder, Validators, FormControl, AbstractControl } from "@angular/forms";


@Injectable()
export class AuthenticationService {
    constructor(private fBuilder: FormBuilder) {

    }

    initializeRegistrationForm = () => {
        return this.fBuilder.group({
            name: ['', Validators.required],
            userName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength]],
            confirmPassword: ['', [Validators.required, Validators.minLength,
            this.checkDublicatePassword]]
        })
    }

    initializeLoginForm = () => {
        return this.fBuilder.group({
            userName: ['', Validators.required],
            password: ['', Validators.required]
        });
    }


    checkDublicatePassword = (input: AbstractControl | any): { [key: string]: boolean } | null => {
        if (!input.root || !input.root.controls || !input.root.controls.password) {
            return null;
        }
        const exactMatch = input.root.controls.password.value === input.value;
        return exactMatch ? null : { mismatchedPassword: true };
    }

}