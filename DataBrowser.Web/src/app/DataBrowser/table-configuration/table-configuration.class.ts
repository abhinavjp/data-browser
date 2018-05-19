import { FormControl } from "@angular/forms";

export class IdNameServiceModel {
    id?: number;
    name?: string;
}
export class TableListsServiceModel {
    idAndName: Array<IdNameServiceModel>;
    tableConfigList: Array<TableConfigurationDetails>;
}
export class DropdownModel {
    label?: string;
    value: Object;
}


export class DataBaseNameFilterServiceModel {
    connectionId?: number;
    isTable?: boolean;
    isView?: boolean;
}

export class TableDetailServiceModel {
    columnName: string;
    constraintsType: string;
    relationShipTableName: string;
    primaryTableColumnName: string;
    referenceTableColumns?:Array<string>;
}

// Save Details 
export class TableConfigurationDetails {
    id?:number;
    name?: string;
    dataKey?: string;
    isTable?: boolean;
    isView?: boolean;
    masterTableName?: string;
    connectionId?: number;
}

export class FieldConfigurtionDetails {
    id?: number;
    tableConfigId?: number;
    sourceColumnName?: string;
    sourceTableName?: string;
    referenceColumnName?: string;
    referenceTableName?: string;
    constraintsType?: string;
    mappedCoumns?: Array<string> = [];
    referenceTableColumns?:Array<string>;
    isDisplay?:boolean;
}

export class TableConfigAndFieldConfigurationsDetails {
    tableConfiguration: TableConfigurationDetails;
    fieldConfiguration: Array<FieldConfigurtionDetails>;
}

export class MappedTableConfigurationDetails {
    mappedColumns?: Array<string>;
    mappedTable?: string;
}
