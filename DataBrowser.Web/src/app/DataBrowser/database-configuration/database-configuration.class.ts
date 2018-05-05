
export class DataBaseNameListFilterServiceModel {
    serverInstanceName?: string;
    userName?: string = "";
    password?: string = "";
}

export class DataBaseConnectionServiceModel {
    name: string
    serverInstanceName: string
    userName: string
    password: string
    dataBaseName: string
}