import { Component } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'confirm-dialog',
    templateUrl: 'confirm.component.html',
    styleUrls: ['confirm.component.scss']
})
export class ConfirmDialogComponent {
    constructor(private bsModalRef: BsModalRef,
        private modelService: BsModalService) {
    }
    colseModel() {
        this.bsModalRef.hide();
    }
    confirm(): void {
        this.modelService.setDismissReason('YES');
        this.bsModalRef.hide();
    }
    decline(): void {
        this.modelService.setDismissReason('NO');
        this.bsModalRef.hide();
    }
}
