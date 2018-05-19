import { Injectable } from "@angular/core";
import { HttpClient, HttpHandler, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { LoaderService } from "./loader.service";
import "rxjs/add/operator/catch";
import { CoreService } from "./core.service";

@Injectable()
export class CoreHttpService {

    constructor(
        private http: HttpClient,
        private loderService: LoaderService,
        private coreService: CoreService) {

    }

    httpGetRequest<TResponse>(url: string): Observable<TResponse> {
        this.loderService.display(true);
        return this.http.get(url)
            .map(res => {
                this.loderService.display(false);
                return <TResponse>res;
            }).catch(this.customErrorHandlors);
    }

    httpPostRequest<TRequest, TResponse>(url: string, data: TRequest): Observable<TResponse> {
        this.loderService.display(true);
        return this.http.post(url, data)
            .map(res => {
                this.loderService.display(false);
                return <TResponse>res
            }).catch(this.customErrorHandlors);

    }
    httpDeleteRequest<TRequest, TResponse>(url: string, id?: TRequest): Observable<TResponse> {
        this.loderService.display(true);
        return this.http.delete(url, id)
            .map(res => {
                this.loderService.display(false);
                return <TResponse>res
            }).catch(this.customErrorHandlors);
    }

    customErrorHandlors = (error: HttpErrorResponse) => {
        let errors = (!this.coreService.isNullOrUndefined(error.error.exceptionMessage)) ? error.error.exceptionMessage : error.error.message;
        let error1: string = '';
        let error2: string = '';
        let errorStatus = (error.statusText === "Unknown Error") ? "errors" : error.statusText;

        this.loderService.display(false);

        if (error.status === 0) {
            errors = "You are offline!";
        }
        else if (!this.coreService.isNullOrUndefined(error.error.innerException) 
        && error.error.innerException.exceptionMessage !== '') {
            error1 = error.error.innerException.exceptionMessage;

            if (!this.coreService.isNullOrUndefined(error.error.innerException.innerException) && error.error.innerException.innerException.exceptionMessage !== '') {
                error2 = error.error.innerException.innerException.exceptionMessage;
            }
        }
        debugger;
        this.coreService.exceptionDialog(errorStatus, errors, error1, error2);
        return Observable.throw(error.message);
    }
}