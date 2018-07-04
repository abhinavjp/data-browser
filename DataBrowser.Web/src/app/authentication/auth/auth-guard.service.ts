
import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private route: Router) { }
    canActivate(next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean {
        if (localStorage.getItem("token") !== null) {
            return true;
        } else {
            this.route.navigate(['/login']);
            return false;
        }

    }
}