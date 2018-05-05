import { Injectable } from "@angular/core";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Injectable()
export class CoreToasterService {
    constructor(public toastr: ToastsManager) {
    }

    showSuccess(content?: string, title?: string) {
        this.toastr.success(content, title, { showCloseButton: true, });
    }
    showError(content?: string, title?: string) {
        this.toastr.error(content, title, { showCloseButton: true, });
    }
    showWarning(content?: string, title?: string) {
        this.toastr.warning(content, title, { showCloseButton: true, });
    }
    showInfo(content?: string, title?: string) {
        this.toastr.info(content, title, { showCloseButton: true, });
    }
    showCustom() {
        this.toastr.custom('<span style="color: red">Message in red.</span>', null, { enableHTML: true });
    }
}