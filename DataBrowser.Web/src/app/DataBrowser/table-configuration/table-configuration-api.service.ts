import { Injectable } from "@angular/core";
import { CoreHttpService } from "../../core/core-http.service";
import { LoaderService } from "../../core/loader.service";
import { Observable } from "rxjs/Observable";
import { TableListsServiceModel, DataBaseNameFilterServiceModel, IdNameServiceModel, TableConfigAndFieldConfigurationsDetails, TableDetailServiceModel } from "./table-configuration.class";
import { Resolve, ActivatedRoute, ActivatedRouteSnapshot } from "@angular/router";

@Injectable()
export class TableConfigurationApiService {
    apiUrl: string;

    constructor(
        private coreHttpService: CoreHttpService,
        private loderservice: LoaderService) {
        this.apiUrl = "http://localhost:4478/api/";

    }

    getDatabaseIdName(): Observable<TableListsServiceModel> {
        return this.coreHttpService.httpGetRequest(this.apiUrl + 'TableConfiguration/GetDatabaseConnectionName');
    }
    getTableNamesFromDatabase(dataToFilter: DataBaseNameFilterServiceModel): Observable<Array<string>> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + "TableConfiguration/GetTablesFromDatabase", dataToFilter);
    }
    getTableDetails(tableFilter: IdNameServiceModel): Observable<Array<TableDetailServiceModel>> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + 'TableConfiguration/GetTablesDetails', tableFilter);
    }
    getPrimaryKeyTableColumnName(columnFilter: IdNameServiceModel): Observable<Array<string>> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + 'TableConfiguration/GetPrimaryKeyTableColumnsName', columnFilter);
    }
    saveTableConfigurationDetails(tableAndFieldConfiguration: TableConfigAndFieldConfigurationsDetails): Observable<string> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + "TableConfiguration/SaveFieldConfiguration", tableAndFieldConfiguration);
    }
    getDetailsOfTableAndFieldConfigurationToedit(id: number): Observable<TableConfigAndFieldConfigurationsDetails> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + "TableConfiguration/GetDetailstableAndFieldsById", { id: id, name: '' });
    }
    UpdateTableConfigurationDetails(tableAndFieldConfiguration: TableConfigAndFieldConfigurationsDetails): Observable<string> {
        return this.coreHttpService.httpPostRequest(this.apiUrl + "TableConfiguration/UpdateTableAndFieldMappingConfiguration", tableAndFieldConfiguration);
    }
}

@Injectable()
export class DataBaseConnectionNameResolver implements Resolve<TableListsServiceModel> {
    constructor(private tableConfigurationApiService: TableConfigurationApiService,
        private loaderService: LoaderService) {
    }
    resolve() {
        return this.tableConfigurationApiService.getDatabaseIdName();
    }
}

@Injectable()
export class TableConfigurationEditResolver implements Resolve<TableConfigAndFieldConfigurationsDetails> {
    constructor(
        private tableConfigurationApiService: TableConfigurationApiService,
        private loaderService: LoaderService,
    ) {
    }
    resolve(route: ActivatedRouteSnapshot) {
        let id = route.paramMap.get('id');
        return this.tableConfigurationApiService.getDetailsOfTableAndFieldConfigurationToedit(Number(id));
    }
}
