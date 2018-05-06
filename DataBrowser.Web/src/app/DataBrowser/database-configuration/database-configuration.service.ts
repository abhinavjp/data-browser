import { Injectable } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";


@Injectable()
export class DatabaseConfigurationService {

    constructor(
        private fBuilder: FormBuilder,
    ) { }
 
    initializeDatabaseConnectionFormControls = () => {
        return this.fBuilder.group({
            name: ['', Validators.required],
            serverInstanceName: ['', Validators.required],
            userName: ['', [Validators.required]],
            password: ['', [Validators.required]],
            dataBaseName: ['',]
        })
    }
}