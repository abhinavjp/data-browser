import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'alert-dialog-single',
    templateUrl: 'alert-single.component.html',
    styleUrls: ['alert-single.component.scss']
})
export class SingleAlertDialogComponent {
    constructor(private bsModalRef: BsModalRef) {
    }
    colseModel() {
        this.bsModalRef.hide();
    }
}
