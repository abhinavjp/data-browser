import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpHandler, HttpEvent, HttpRequest } from "@angular/common/http";
import { Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/do";
@Injectable()
export class AuthIntersapor implements HttpInterceptor {

    constructor(private router: Router) {

    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
        if (this.router.url === "/login") {
            return next.handle(request.clone());
        }

        if (request.headers.get('No-Auth') == "true") {
            return next.handle(request.clone());
        }


        if (localStorage.getItem("token") !== null && localStorage.getItem("token") !== undefined) {
            // code for request with authorization token
            const colneReq = request.clone({
                headers: request.headers.set("Authorization", "bearer " + localStorage.getItem("token"))
            });

            return next.handle(colneReq)
                .do(
                    success => {

                    },
                    err => {
                        if (err.status === 401) {
                            this.router.navigateByUrl("/login");
                        }
                    }
                )
        }
        else {
            this.router.navigateByUrl("/login");
        }
    }
}
