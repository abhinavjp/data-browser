import { Injectable } from "@angular/core";
import { CoreService } from "./core.service";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { CoreToasterService } from "./core-toaster.service";
import { FormGroup, FormControl, FormArray } from "@angular/forms";
import { AlertDialogComponent } from "../shared/component/dialoges/alerts-dialog/alert-dialog.component";

@Injectable()
export class CoreFormValidation {

    constructor(
        private coreService: CoreService,
        private coreToasterService: CoreToasterService,
        private modalService: BsModalService, ) {

    }

    formValidate = (validateForm: FormGroup | any, isShownInToaster?: boolean) => {
        let errors = this.getFormValidationErrors(validateForm);
        if (!this.coreService.isNullOrUndefined(errors)) {
            if (isShownInToaster) {
                errors.forEach(e => {
                    this.coreToasterService.showError(e, 'Error!');
                });
            } else {
                let modelRef: BsModalRef = this.modalService.show(AlertDialogComponent)
                modelRef.content.errorMessages = errors;
            }
        }
    }

    getFormValidationErrors = (validateForm: FormGroup | any) => {
        let errors: string[] = [];
        if (validateForm.invalid) {
            if (validateForm instanceof FormGroup) {
                for (let formControls in validateForm.controls) {
                    if (validateForm.controls[formControls] instanceof FormControl) {
                        if (validateForm.controls[formControls].invalid) {
                            errors.push(this.getErrorMessage(validateForm.controls[formControls].errors, formControls));
                        }
                    }
                    else if (validateForm.controls[formControls] instanceof FormArray) {
                        let counter = 0;
                        (<FormArray>validateForm.controls[formControls]).controls.forEach(element => {
                            counter++;
                            if (element instanceof FormGroup) {
                                for (let formControlsFronFormArray in element.controls) {
                                    if (element.controls[formControlsFronFormArray] instanceof FormControl) {
                                        if (element.controls[formControlsFronFormArray].invalid) {
                                            errors.push(this.getErrorMessage(element.controls[formControlsFronFormArray].errors, formControlsFronFormArray + ' ' + counter))
                                        }
                                    }
                                    else if (element.controls[formControlsFronFormArray] instanceof FormArray) {
                                        (<FormArray>element.controls[formControlsFronFormArray]).controls.forEach(ele => {
                                            if (ele instanceof FormGroup) {
                                                this.getFormValidationErrors(ele);
                                            }
                                        });
                                    }
                                }
                            }
                        });
                    }
                }
            }
        }
        return errors;
    }
    getErrorMessage = (error: any, formControlName: string) => {
        let errorMessage: string = "";
        switch (Object.getOwnPropertyNames(error)[0]) {
            case "required":
                errorMessage = formControlName + " is required";
                break;
            case "minlength":
                errorMessage = formControlName + " length is too small";
                break;
            case "maxlength":
                errorMessage = formControlName + " length is too large";
                break;

            default:
        }
        return errorMessage;
    }

}
