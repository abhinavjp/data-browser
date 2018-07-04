import { Injectable } from "@angular/core";
import { Resolve } from "@angular/router";
import { CoreHttpService } from "../../core/core-http.service";
import { LoaderService } from "../../core/loader.service";
import { Observable } from "rxjs/Observable";
import { DataBaseConnectionServiceModel, DataBaseNameListFilterServiceModel } from "./database-configuration.class";
import { HttpHeaders } from "@angular/common/http";

@Injectable()
export class DatabaseConfigurationApiService {
    apiUrl: string;
    constructor(
        private coreHttpService: CoreHttpService,
        private loderservice: LoaderService) {
        this.apiUrl = "http://localhost:4478/api/";
    }
    getDataBaseConnection(): Observable<any> {
        return this.coreHttpService.httpGetRequest(this.apiUrl + 'DataBaseConnection/GetDataBaseConnection');
    }
    getDataBaseNameLists(databaseFilterServiceModel: DataBaseNameListFilterServiceModel): Observable<Array<string>> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + 'DataBaseConnection/GetDatabaseName', databaseFilterServiceModel)
    }
    createDataBaseConnection(dataBaseConnection: DataBaseConnectionServiceModel): Observable<any> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + 'DataBaseConnection/CreateDataBaseConnection', dataBaseConnection);
    }
    deleteDatabaseConnection(id): Observable<string> {
        return this.coreHttpService.httpDeleteRequest(this.apiUrl + 'DataBaseConnection/DeleteDatabaseConnection?id=' + id);
    }
}
@Injectable()
export class DataBaseConnectionResolver implements Resolve<any> {
    constructor(
        private databaseConfigurationApiService: DatabaseConfigurationApiService,
        private loaderService: LoaderService) {
    }
    resolve() {
        return this.databaseConfigurationApiService.getDataBaseConnection();
    }
}
