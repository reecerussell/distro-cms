import { RoleState } from "./role.state";
import Role from "../../models/role.model";
import { Action } from "@ngrx/store";
import { AddError } from "../errors/error.action";

export const GET_ROLES = "[Role] GET ROLES";
export const GET_ROLES_SUCCESS = "[Role] GET ROLES SUCCESS";
export const GET_ROLES_ERROR = "[Role] GET ROLES ERROR";

export class GetRoles implements Action {
    readonly type = GET_ROLES;
}

export class GetRolesSuccess implements Action {
    readonly type = GET_ROLES_SUCCESS;

    constructor(public payload: RoleState[]) {}
}

export class GetRolesError implements Action {
    readonly type = GET_ROLES_ERROR;
}

export const GET_ROLE = "[Role] GET ROLE";
export const GET_ROLE_SUCCESS = "[Role] GET ROLE SUCCESS";
export const GET_ROLE_ERROR = "[Role] GET ROLE ERROR";

export class GetRole implements Action {
    readonly type = GET_ROLE;

    constructor(public payload: string) {}
}

export class GetRoleSuccess implements Action {
    readonly type = GET_ROLE_SUCCESS;

    constructor(public payload: Role) {}
}

export class GetRoleError implements Action {
    readonly type = GET_ROLE_ERROR;
}

export const CREATE_ROLE = "[Role] CREATE ROLE";
export const CREATE_ROLE_SUCCESS = "[Role] CREATE ROLE SUCCESS";
export const CREATE_ROLE_ERROR = "[Role] CREATE ROLE ERROR";

export class CreateRole implements Action {
    readonly type = CREATE_ROLE;

    constructor(public payload: Role) {}
}

export class CreateRoleSuccess implements Action {
    readonly type = CREATE_ROLE_SUCCESS;

    constructor(public payload: Role) {}
}

export class CreateRoleError implements Action {
    readonly type = CREATE_ROLE_ERROR;
}

export const UPDATE_ROLE = "[Role] UPDATE ROLE";
export const UPDATE_ROLE_SUCCESS = "[Role] UPDATE ROLE SUCCESS";
export const UPDATE_ROLE_ERROR = "[Role] UPDATE ROLE ERROR";

export class UpdateRole implements Action {
    readonly type = UPDATE_ROLE;

    constructor(public payload: Role) {}
}

export class UpdateRoleSuccess implements Action {
    readonly type = UPDATE_ROLE_SUCCESS;

    constructor(public payload: Role) {}
}

export class UpdateRoleError implements Action {
    readonly type = UPDATE_ROLE_ERROR;
}

export const DELETE_ROLE = "[Role] DELETE ROLE";
export const DELETE_ROLE_SUCCESS = "[Role] DELETE ROLE SUCCESS";
export const DELETE_ROLE_ERROR = "[Role] DELETE ROLE ERROR";

export class DeleteRole implements Action {
    readonly type = DELETE_ROLE;

    /**
     *
     * @param payload The id of the role to be deleted.
     */
    constructor(public payload: string) {}
}

export class DeleteRoleSuccess implements Action {
    readonly type = DELETE_ROLE_SUCCESS;

    /**
     *
     * @param payload The id of the role which has been deleted.
     */
    constructor(public payload: string) {}
}

export class DeleteRoleError implements Action {
    readonly type = DELETE_ROLE_ERROR;
}

export type All =
    | GetRole
    | GetRoleSuccess
    | GetRoleError
    | GetRoles
    | GetRolesSuccess
    | GetRolesError
    | CreateRole
    | CreateRoleSuccess
    | CreateRoleError
    | UpdateRole
    | UpdateRoleSuccess
    | UpdateRoleError
    | DeleteRole
    | DeleteRoleSuccess
    | DeleteRoleError;
