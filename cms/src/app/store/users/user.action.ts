import User from "../../models/user.model";
import { Action } from "@ngrx/store";

export const GET_USERS = "[User] GET USERS";
export const GET_USERS_SUCCESS = "[User] GET USERS SUCCESS";
export const GET_USERS_ERROR = "[User] GET USERS ERROR";
export const GET_USER = "[User] GET USER";
export const GET_USER_SUCCESS = "[User] GET USER SUCCESS";
export const GET_USER_ERROR = "[User] GET USER ERROR";

export class GetUsers implements Action {
    readonly type = GET_USERS;

    constructor(public searchTerm?: string, public roleId?: string) {}
}

export class GetUsersSuccess implements Action {
    readonly type = GET_USERS_SUCCESS;

    constructor(public users: User[]) {}
}

export class GetUsersError implements Action {
    readonly type = GET_USERS_ERROR;

    constructor(public error: string) {}
}

export class GetUser implements Action {
    readonly type = GET_USER;

    constructor(public id: string) {}
}

export class GetUserSuccess implements Action {
    readonly type = GET_USER_SUCCESS;

    constructor(public user: User) {}
}

export class GetUserError implements Action {
    readonly type = GET_USER_ERROR;

    constructor(public error: string) {}
}

export type All =
    | GetUsers
    | GetUsersSuccess
    | GetUsersError
    | GetUser
    | GetUserSuccess
    | GetUserError;
