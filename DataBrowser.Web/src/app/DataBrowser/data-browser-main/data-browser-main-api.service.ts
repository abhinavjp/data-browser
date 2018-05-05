import { Injectable } from "@angular/core";
import { CoreHttpService } from "../../core/core-http.service";
import { LoaderService } from "../../core/loader.service";
import { DatabrowserDropdownServiceModel } from "./data-browser.class";
import { Observable } from "rxjs/Observable";
import { Resolve } from "@angular/router";

@Injectable()
export class DatabrowserMainApiService {
    apiUrl: string;
    
    constructor(
        private coreHttpService: CoreHttpService,
        private loderservice: LoaderService) {
        this.apiUrl = "http://localhost:4478/api/";
    }
    getTableConfigurationsLists(): Observable<Array<DatabrowserDropdownServiceModel>> {
        return this.coreHttpService.httpGetRequest(this.apiUrl + "DataBrowser/GetTableConfigurations");
    }

    getFieldsDetails(fieldDetailsFilterModel: DatabrowserDropdownServiceModel): Observable<any> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + "DataBrowser/GetFieldsDetails", fieldDetailsFilterModel);
    }
}

@Injectable()
export class DatabrowserFilterResolver implements Resolve<Array<DatabrowserDropdownServiceModel>> {
    constructor(
        private databrowserMainApiService: DatabrowserMainApiService,
        private loaderService: LoaderService) {
    }
    resolve() {
        return this.databrowserMainApiService.getTableConfigurationsLists();
    }
}