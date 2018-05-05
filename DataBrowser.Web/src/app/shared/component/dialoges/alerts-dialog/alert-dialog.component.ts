import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'alert-dialog',
    templateUrl: 'alert-dialog.component.html',
    styleUrls: ['alert-dialog.component.scss']
})
export class AlertDialogComponent {
    constructor(private bsModalRef: BsModalRef) {
    }

    colseModel() {
        this.bsModalRef.hide();
    }
}
