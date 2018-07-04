export class UserModel {
    userId?: number;
    name?: string;
    userName?: string;
    email?: string;
    password?: string;
}

export class Login {
    userName?: string;
    password?: string;
    grant_type?: string;
}