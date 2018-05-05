import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'exception-dialog-single',
    templateUrl: 'exception-catch.component.html',
    styleUrls: ['exception-catch.component.scss']
})
export class ServerSideExceptionCatchComponent {
    constructor(
        private bsModalRef: BsModalRef
    ) {
    }
    colseModel() {
        this.bsModalRef.hide();
    }
}
