import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'app-header',
    templateUrl: 'header.component.html',
    styleUrls: ['header.component.scss']
})
export class HeaderComponent {

    constructor(private router: Router) {

    }

    Logout = () => {
        localStorage.clear();
        this.router.navigate(["/login"]);
    }
}
