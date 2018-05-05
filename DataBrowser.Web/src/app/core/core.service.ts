import { Injectable } from "@angular/core";
import { FormGroup, FormControl, FormArray } from "@angular/forms";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { CoreToasterService } from "./core-toaster.service";
import { SingleAlertDialogComponent } from "../shared/component/dialoges/alert-single/alert-single.component";
import { ServerSideExceptionCatchComponent } from "../shared/component/dialoges/exception-catch-server/exception-catch.component";
import { AlertDialogComponent } from "../shared/component/dialoges/alerts-dialog/alert-dialog.component";

@Injectable()
export class CoreService {
    constructor(private modalService: BsModalService,
        private coreToasterService: CoreToasterService) {
    }
    alertDialog = (error: string) => {
        let modelRef: BsModalRef = this.modalService.show(SingleAlertDialogComponent)
        modelRef.content.errorMessages = error;
    }

    exceptionDialog = (title: string, exceptionMessage: string, error1: string, error2: string) => {
        let modelRef: BsModalRef = this.modalService.show(ServerSideExceptionCatchComponent)
        modelRef.content.title = title;
        modelRef.content.exceptionMessage = exceptionMessage;
        modelRef.content.errorDetail1 = error1;
        modelRef.content.errorDetail2 = error2;
    }

    isNullOrUndefined = <T>(tObj: T): boolean => {
        return (tObj == null || tObj == undefined) ? true : false;
    }
}