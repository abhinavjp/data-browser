import { Injectable } from "@angular/core";
import { CoreHttpService } from "../core/core-http.service";
import { UserModel, Login } from "./authentication.class";
import { Observable } from "rxjs/Observable";
import { HttpHandler, HttpHeaders, HttpClient, HttpErrorResponse } from "@angular/common/http";
import { CoreService } from "../core/core.service";
import { LoaderService } from "../core/loader.service";

@Injectable()
export class AuthenticationApiService {
    apiUrl: string;
    constructor(private coreHttpService: CoreHttpService,
        private http: HttpClient,
        private coreService: CoreService,
        private loderService: LoaderService) {
        this.apiUrl = "http://localhost:4478/api/";
    }


    registerUser = (userDetails: UserModel): Observable<string> => {
        let reqHeader = new HttpHeaders({ "No-Auth": "true" });
        return this.coreHttpService.httpPostRequest(this.apiUrl + "User/CreateUserRegistration", userDetails, reqHeader);
    }

    login = (login: Login): Observable<any> => {
        let url = "http://localhost:4478/token"
        var data = "userName=" + login.userName + ";password=" + login.password + ";grant_type=password;";
        login.grant_type = "password";
        let headers = new HttpHeaders({ 'Accept': 'application/json' });

        let urlSearchParams = new URLSearchParams();
        urlSearchParams.set('grant_type', 'password');
        urlSearchParams.set('username', login.userName);
        urlSearchParams.set('password', login.password);
        let body = urlSearchParams.toString();

        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        headers.append("No-Auth", "true");
        
        this.loderService.display(true);
        return this.http.post(url, body, { headers: headers })
            .map(res => {
                this.loderService.display(false);
                return <any>res
            }).catch(this.errorHandler);

    }

    errorHandler = (error: HttpErrorResponse) => {
        this.loderService.display(false);
        let errorStatus = error.statusText;
        let errors;
        if (error.status === 400) {
            errors = error.error.error_description;
        }
        this.coreService.exceptionDialog(errorStatus, errors);
        return Observable.throw(error.message);
    }



}