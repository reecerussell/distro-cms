import DictionaryItem from "../../models/dictionary-item.model";
import { Action } from "@ngrx/store";

export const GET_ITEMS = "[Dictionary] GET ITEMS";
export const GET_ITEMS_SUCCESS = "[Dictionary] GET ITEMS SUCCESS";
export const GET_ITEMS_ERROR = "[Dictionary] GET ITEMS ERROR";

export class GetItems implements Action {
    readonly type = GET_ITEMS;

    constructor(public culture: string) {}
}

export class GetItemsSuccess implements Action {
    readonly type = GET_ITEMS_SUCCESS;

    constructor(public items: DictionaryItem[]) {}
}

export class GetItemsError implements Action {
    readonly type = GET_ITEMS_ERROR;

    constructor(public error: string) {}
}

export const CREATE_ITEM = "[Dictionary] CREATE ITEM";
export const CREATE_ITEM_SUCCESS = "[Dictionary] CREATE ITEM SUCCESS";
export const CREATE_ITEM_ERROR = "[Dictionary] CREATE ITEM ERROR";

export class CreateItem implements Action {
    readonly type = CREATE_ITEM;

    constructor(public item: DictionaryItem, public culture: string) {}
}

export class CreateItemSuccess implements Action {
    readonly type = CREATE_ITEM_SUCCESS;

    constructor(public item: DictionaryItem) {}
}

export class CreateItemError implements Action {
    readonly type = CREATE_ITEM_ERROR;

    constructor(public error: string) {}
}

export const UPDATE_ITEM = "[Dictionary] UPDATE ITEM";
export const UPDATE_ITEM_SUCCESS = "[Dictionary] UPDATE ITEM SUCCESS";
export const UPDATE_ITEM_ERROR = "[Dictionary] UPDATE ITEM ERROR";

export class UpdateItem implements Action {
    readonly type = UPDATE_ITEM;

    constructor(public item: DictionaryItem) {}
}

export class UpdateItemSuccess implements Action {
    readonly type = UPDATE_ITEM_SUCCESS;

    constructor(public item: DictionaryItem) {}
}

export class UpdateItemError implements Action {
    readonly type = UPDATE_ITEM_ERROR;

    constructor(public error: string) {}
}

export const DELETE_ITEM = "[Dictionary] DELETE ITEM";
export const DELETE_ITEM_SUCCESS = "[Dictionary] DELETE ITEM SUCCESS";
export const DELETE_ITEM_ERROR = "[Dictionary] DELETE ITEM ERROR";

export class DeleteItem implements Action {
    readonly type = DELETE_ITEM;

    constructor(public id: string) {}
}

export class DeleteItemSuccess implements Action {
    readonly type = DELETE_ITEM_SUCCESS;

    constructor(public id: string) {}
}

export class DeleteItemError implements Action {
    readonly type = DELETE_ITEM_ERROR;

    constructor(public error: string) {}
}

export const SET_EDITING = "[Dictionary] SET EDITING";

export class SetEditing implements Action {
    readonly type = SET_EDITING;

    constructor(public item: DictionaryItem, public editing: boolean) {}
}

export const SET_CULTURE = "[Dictionary] SET CULTURE";

export class SetCulture implements Action {
    readonly type = SET_CULTURE;

    constructor(public culture: string) {}
}

export type All =
    | GetItems
    | GetItemsSuccess
    | GetItemsError
    | CreateItem
    | CreateItemSuccess
    | CreateItemError
    | UpdateItem
    | UpdateItemSuccess
    | UpdateItemError
    | DeleteItem
    | DeleteItemSuccess
    | DeleteItemError
    | SetEditing
    | SetCulture;
