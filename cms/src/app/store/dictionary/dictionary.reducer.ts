import {
    initializeItemState,
    initializeCultureState,
    ItemState,
    CultureState,
} from "./dictionary.state";
import { DictionaryState } from "./dictionary.state";
import * as DictionaryActions from "./dictionary.action";

export type Action = DictionaryActions.All;

const defaultState: DictionaryState = {
    items: [],
    cultures: [],
    loading: false,
    error: null,
    culture: navigator.language,
};

/**
 * Merges a new item with the existing state.
 * @param existingItems items that already exist in state.
 * @param newItem ither a new or replacement item, to be appended to the state
 */
const mergeItems = (existingItems: ItemState[], newItem: ItemState) => {
    const sortFunc = (a, b) => {
        if (a.displayName < b.displayName) {
            return -1;
        }
        if (a.displayName > b.displayName) {
            return 1;
        }
        return 0;
    };

    return existingItems
        .filter((x) => x.id !== newItem.id)
        .concat(newItem)
        .sort(sortFunc);
};

const mergeCultures = (
    existingCultures: CultureState[],
    newCulture: CultureState
) => {
    const sortFunc = (a, b) => {
        if (a.displayName < b.displayName) {
            return -1;
        }
        if (a.displayName > b.displayName) {
            return 1;
        }
        return 0;
    };

    return existingCultures
        .filter((x) => x.id !== newCulture.id)
        .concat(newCulture)
        .sort(sortFunc);
};

export const DictionaryReducer = (state = defaultState, action: Action) => {
    switch (action.type) {
        /*
            GET DICTIONARY ITEMS
        */
        case DictionaryActions.GET_ITEMS:
            return { ...state, loading: true };
        case DictionaryActions.GET_ITEMS_SUCCESS:
            return {
                ...state,
                items: action.items.map(
                    (item) =>
                        ({
                            ...initializeItemState(),
                            ...item,
                        } as ItemState)
                ),
                loading: false,
                error: null,
                culture:
                    localStorage.getItem("DICTIONARY_CULTURE") ?? state.culture,
            };
        case DictionaryActions.GET_ITEMS_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        /*
            CREATE DICTIONARY ITEM
        */
        case DictionaryActions.CREATE_ITEM:
            return { ...state, loading: true };
        case DictionaryActions.CREATE_ITEM_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                items: mergeItems(state.items, {
                    ...initializeItemState(),
                    ...action.item,
                } as ItemState),
            };
        case DictionaryActions.CREATE_ITEM_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        /*
            UPDATE DICTIONARY ITEM
        */
        case DictionaryActions.UPDATE_ITEM:
            const items = [...state.items];
            const idx = items.findIndex((x) => x.id == action.item.id);
            items[idx] = { ...items[idx], loading: true };
            return { ...state, items: items, loading: true };

        case DictionaryActions.UPDATE_ITEM_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                items: mergeItems(state.items, {
                    ...initializeItemState(),
                    ...action.item,
                } as ItemState),
            };

        case DictionaryActions.UPDATE_ITEM_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        /*
            DELETE DICTIONARY ITEM
        */
        case DictionaryActions.DELETE_ITEM:
            return { ...state, loading: true };

        case DictionaryActions.DELETE_ITEM_SUCCESS:
            return {
                ...state,
                items: state.items.filter((x) => x.id !== action.id),
                loading: false,
            };

        case DictionaryActions.DELETE_ITEM_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        /*
            SET DICTIONARY ITEM AS EDITING
        */
        case DictionaryActions.SET_EDITING:
            return {
                ...state,
                items: mergeItems(state.items, {
                    ...action.item,
                    editing: action.editing,
                } as ItemState),
            };

        /*
            GET CULTURE DROPDOWN LIST ITEMS
        */
        case DictionaryActions.GET_DROPDOWN_ITEMS:
            return { ...state, loading: true };
        case DictionaryActions.GET_DROPDOWN_ITEMS_SUCCESS:
            return {
                ...state,
                cultures: action.items.map(
                    (item) =>
                        ({
                            ...initializeCultureState(),
                            ...item,
                        } as CultureState)
                ),
                loading: false,
                error: null,
            };
        case DictionaryActions.GET_DROPDOWN_ITEMS_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        /*
            CREATE SUPPORTED CULTURE
        */
        case DictionaryActions.CREATE_CULTURE:
            return {
                ...state,
                loading: true,
            };
        case DictionaryActions.CREATE_CULTURE_SUCCESS:
            return {
                ...state,
                cultures: mergeCultures(state.cultures, {
                    ...initializeCultureState(),
                    ...action.culture,
                }),
                loading: false,
                error: null,
            };
        case DictionaryActions.CREATE_CULTURE_ERROR:
            return { ...state, loading: false, error: action.error };

        /*
            GET SUPPORTED CULTURES
        */
        case DictionaryActions.GET_CULTURES:
            return { ...state, loading: true };
        case DictionaryActions.GET_CULTURES_SUCCESS:
            return {
                ...state,
                cultures: action.cultures,
                loading: false,
                error: null,
            };
        case DictionaryActions.GET_CULTURES_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
    }
};
