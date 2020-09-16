import { Action } from "@ngrx/store";

export const ADD_ERROR = "[Error] ADD ERROR";
export const CLEAR_ERRORS = "[Error] CLEAR ERRORS";

export class AddError implements Action {
    readonly type = ADD_ERROR;

    constructor(public error: string) {}
}

export class ClearErrors implements Action {
    readonly type = CLEAR_ERRORS;
}

export type All = AddError | ClearErrors;
