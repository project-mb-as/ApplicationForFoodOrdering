export class User {
    userId: number;
    password: string;
    activated?: boolean;
    email: string;
    token?: string;
    roles?: string;
}
