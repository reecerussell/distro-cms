export default class User {
    id?: string;
    firstname: string;
    lastname: string;
    email: string;
    dateCreated: Date;
    dateUpdated?: Date;

    constructor() {
        this.id = "";
        this.firstname = "";
        this.lastname = "";
        this.email = "";
        this.dateCreated = new Date();
        this.dateUpdated = null;
    }
}

export class UserCreate {
    firstname: string;
    lastname: string;
    email: string;
}

export class NewlyCreatedUser extends User {
    password: string;
}

export class UserUpdate {
    id: string;
    firstname: string;
    lastname: string;
    email: string;
}
