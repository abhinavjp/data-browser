import { Injectable } from "@angular/core";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Injectable()
export class CoreToasterService {
    constructor(public toastr: ToastsManager) {
    }

    showSuccess(content?: string, title?: string, pos?: string) {

        this.toastr.success(content, title,
            {
                showCloseButton: true,
                positionClass: (pos !== '' && pos !== null && pos !== undefined) ? pos : 'toast-top-right'
            });
    }
    showError(content?: string, title?: string, pos?: string) {
        this.toastr.error(content, title,
            {
                showCloseButton: true, positionClass: (pos !== '' && pos !== null && pos !== undefined) ? pos : 'toast-top-right'
            });
    }
    showWarning(content?: string, title?: string, pos?: string) {
        this.toastr.warning(content, title,
            {
                showCloseButton: true, positionClass: (pos !== '' && pos !== null && pos !== undefined) ? pos : 'toast-top-right'
            });
    }
    showInfo(content?: string, title?: string, pos?: string) {
        this.toastr.info(content, title, { showCloseButton: true, positionClass: (pos !== '' && pos !== null && pos !== undefined) ? pos : 'toast-top-right' });
    }
    showCustom() {
        this.toastr.custom('<span style="color: red">Message in red.</span>', null, { enableHTML: true });
    }
}