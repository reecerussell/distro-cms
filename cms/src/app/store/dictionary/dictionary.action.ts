import {
    DictionaryItem,
    SupportedCulture,
    SupportedCultureCreate,
} from "src/app/models";
import { Action } from "@ngrx/store";

export const GET_ITEMS = "[Dictionary] GET ITEMS";
export const GET_ITEMS_SUCCESS = "[Dictionary] GET ITEMS SUCCESS";
export const GET_ITEMS_ERROR = "[Dictionary] GET ITEMS ERROR";
export const CREATE_ITEM = "[Dictionary] CREATE ITEM";
export const CREATE_ITEM_SUCCESS = "[Dictionary] CREATE ITEM SUCCESS";
export const CREATE_ITEM_ERROR = "[Dictionary] CREATE ITEM ERROR";
export const UPDATE_ITEM = "[Dictionary] UPDATE ITEM";
export const UPDATE_ITEM_SUCCESS = "[Dictionary] UPDATE ITEM SUCCESS";
export const UPDATE_ITEM_ERROR = "[Dictionary] UPDATE ITEM ERROR";
export const DELETE_ITEM = "[Dictionary] DELETE ITEM";
export const DELETE_ITEM_SUCCESS = "[Dictionary] DELETE ITEM SUCCESS";
export const DELETE_ITEM_ERROR = "[Dictionary] DELETE ITEM ERROR";
export const SET_EDITING = "[Dictionary] SET EDITING";
export const SET_CULTURE = "[Dictionary] SET CULTURE";
export const GET_DROPDOWN_ITEMS = "[Supported Cultures] GET DROPDOWN ITEMS";
export const GET_DROPDOWN_ITEMS_SUCCESS =
    "[Supported Cultures] GET DROPDOWN ITEMS SUCCESS";
export const GET_DROPDOWN_ITEMS_ERROR =
    "[Supported Cultures] GET DROPDOWN ITEMS ERROR";
export const CREATE_CULTURE = "[Supported Cultures] CREATE CULTURE";
export const CREATE_CULTURE_SUCCESS =
    "[Supported Cultures] CREATE CULTURE SUCCESS";
export const CREATE_CULTURE_ERROR = "[Supported Cultures] CREATE CULTURE ERROR";
export const GET_CULTURES = "[Supported Cultures] GET CULTURES";
export const GET_CULTURES_SUCCESS = "[Supported Cultures] GET CULTURES SUCCESS";
export const GET_CULTURES_ERROR = "[Supported Cultures] GET CULTURES ERROR";
export const GET_CULTURE = "[Supported Cultures] GET CULTURE";
export const GET_CULTURE_SUCCESS = "[Supported Cultures] GET CULTURE SUCCESS";
export const GET_CULTURE_ERROR = "[Supported Cultures] GET CULTURE ERROR";
export const UPDATE_CULTURE = "[Supported Cultures] UPDATE CULTURE";
export const UPDATE_CULTURE_SUCCESS =
    "[Supported Cultures] UPDATE CULTURE SUCCESS";
export const UPDATE_CULTURE_ERROR = "[Supported Cultures] UPDATE CULTURE ERROR";
export const DELETE_CULTURE = "[Supported Cultures] DELETE CULTURE";
export const DELETE_CULTURE_SUCCESS =
    "[Supported Cultures] DELETE CULTURE SUCCESS";
export const DELETE_CULTURE_ERROR = "[Supported Cultures] DELETE CULTURE ERROR";
export const SET_AS_DEFAULT = "[Supported Cultures] SET AS DEFAULT";
export const SET_AS_DEFAULT_SUCCESS =
    "[Supported Cultures] SET AS DEFAULT SUCCESS";
export const SET_AS_DEFAULT_ERROR = "[Supported Cultures] SET AS DEFAULT ERROR";

/*
    GET DICTIONARY ITEMS
*/

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

/*
    CREATE DICTIONARY ITEM
*/

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

/*
    UPDATE DICTIONARY ITEM
*/

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

/*
    DELETE DICTIONARY ITEM
*/

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

/*
    SET DICTIONARY ITEM AS EDITING
*/

export class SetEditing implements Action {
    readonly type = SET_EDITING;

    constructor(public item: DictionaryItem, public editing: boolean) {}
}

/*
    SET UI CULTURE
*/

export class SetCulture implements Action {
    readonly type = SET_CULTURE;

    constructor(public culture: string) {}
}

/*
    GET CULTURE DROPDOWN ITEMS
*/

export class GetDropdownItems implements Action {
    readonly type = GET_DROPDOWN_ITEMS;

    constructor(public forceRefresh?: boolean) {}
}

export class GetDropdownItemsSuccess implements Action {
    readonly type = GET_DROPDOWN_ITEMS_SUCCESS;

    constructor(public items: SupportedCulture[]) {}
}

export class GetDropdownItemsError implements Action {
    readonly type = GET_DROPDOWN_ITEMS_ERROR;

    constructor(public error: string) {}
}

/*
    CREATE SUPPORTED CULTURE
*/

export class CreateCulture implements Action {
    readonly type = CREATE_CULTURE;

    constructor(public culture: SupportedCultureCreate) {}
}

export class CreateCultureSuccess implements Action {
    readonly type = CREATE_CULTURE_SUCCESS;

    constructor(public culture: SupportedCulture) {}
}

export class CreateCultureError implements Action {
    readonly type = CREATE_CULTURE_ERROR;

    constructor(public error: string) {}
}

/*
    GET SUPPORTED CULTURES
*/

export class GetCultures implements Action {
    readonly type = GET_CULTURES;

    constructor(public term: string) {}
}

export class GetCulturesSuccess implements Action {
    readonly type = GET_CULTURES_SUCCESS;

    constructor(public cultures: SupportedCulture[]) {}
}

export class GetCulturesError implements Action {
    readonly type = GET_CULTURES_ERROR;

    constructor(public error: string) {}
}

/*
    GET SUPPORTED CULTURE
*/

export class GetCulture implements Action {
    readonly type = GET_CULTURE;

    constructor(public id: string) {}
}

export class GetCultureSuccess implements Action {
    readonly type = GET_CULTURE_SUCCESS;

    constructor(public culture: SupportedCulture) {}
}

export class GetCultureError implements Action {
    readonly type = GET_CULTURE_ERROR;

    constructor(public error: string) {}
}

/*
    UPDATE SUPPORTED CULTURE
*/

export class UpdateCulture implements Action {
    readonly type = UPDATE_CULTURE;

    constructor(public culture: SupportedCulture) {}
}

export class UpdateCultureSuccess implements Action {
    readonly type = UPDATE_CULTURE_SUCCESS;

    constructor(public culture: SupportedCulture) {}
}

export class UpdateCultureError implements Action {
    readonly type = UPDATE_CULTURE_ERROR;

    constructor(public error: string) {}
}

/*
    DELETE SUPPORTED CULTURE
*/

export class DeleteCulture implements Action {
    readonly type = DELETE_CULTURE;

    constructor(public id: string) {}
}

export class DeleteCultureSuccess implements Action {
    readonly type = DELETE_CULTURE_SUCCESS;

    constructor(public id: string) {}
}

export class DeleteCultureError implements Action {
    readonly type = DELETE_CULTURE_ERROR;

    constructor(public error: string) {}
}

/*
    SET SUPPORTED CULTURE AS DEFAULT
*/

export class SetAsDefault implements Action {
    readonly type = SET_AS_DEFAULT;

    constructor(public id: string) {}
}

export class SetAsDefaultSuccess implements Action {
    readonly type = SET_AS_DEFAULT_SUCCESS;

    constructor(public culture: SupportedCulture) {}
}

export class SetAsDefaultError implements Action {
    readonly type = SET_AS_DEFAULT_ERROR;

    constructor(public error: string) {}
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
    | SetCulture
    | GetDropdownItems
    | GetDropdownItemsSuccess
    | GetDropdownItemsError
    | CreateCulture
    | CreateCultureSuccess
    | CreateCultureError
    | GetCultures
    | GetCulturesSuccess
    | GetCulturesError
    | GetCulture
    | GetCultureSuccess
    | GetCultureError
    | UpdateCulture
    | UpdateCultureSuccess
    | UpdateCultureError
    | DeleteCulture
    | DeleteCultureSuccess
    | DeleteCultureError
    | SetAsDefault
    | SetAsDefaultSuccess
    | SetAsDefaultError;
