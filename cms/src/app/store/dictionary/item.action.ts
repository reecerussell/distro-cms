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

export const SET_CULTURE = "[Dictionary] SET CULTURE";

export class SetCulture implements Action {
    readonly type = SET_CULTURE;

    constructor(public culture: string) {}
}

export type All =
    | GetItems
    | GetItemsSuccess
    | GetItemsError
    | UpdateItem
    | UpdateItemSuccess
    | UpdateItemError
    | SetCulture;
