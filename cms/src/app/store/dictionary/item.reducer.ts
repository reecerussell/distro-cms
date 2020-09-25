import { initializeItemState, ItemListState, ItemState } from "./item.state";
import * as ItemActions from "./item.action";
import DictionaryItem from "src/app/models/dictionary-item.model";

export type Action = ItemActions.All;

const defaultState: ItemListState = {
    items: [],
    loading: false,
    error: null,
    culture: navigator.language,
};

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

export const ItemReducer = (state = defaultState, action: Action) => {
    switch (action.type) {
        case ItemActions.GET_ITEMS:
            return { ...state, loading: true };
        case ItemActions.GET_ITEMS_SUCCESS:
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
        case ItemActions.GET_ITEMS_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        case ItemActions.CREATE_ITEM:
            return { ...state, loading: true };
        case ItemActions.CREATE_ITEM_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                items: mergeItems(state.items, {
                    ...initializeItemState(),
                    ...action.item,
                } as ItemState),
            };
        case ItemActions.CREATE_ITEM_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        case ItemActions.UPDATE_ITEM:
            const items = [...state.items];
            const idx = items.findIndex((x) => x.id == action.item.id);
            items[idx] = { ...items[idx], loading: true };
            return { ...state, items: items, loading: true };

        case ItemActions.UPDATE_ITEM_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                items: mergeItems(state.items, {
                    ...initializeItemState(),
                    ...action.item,
                } as ItemState),
            };

        case ItemActions.UPDATE_ITEM_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        case ItemActions.DELETE_ITEM:
            return { ...state, loading: true };

        case ItemActions.DELETE_ITEM_SUCCESS:
            return {
                ...state,
                items: state.items.filter((x) => x.id !== action.id),
                loading: false,
            };

        case ItemActions.DELETE_ITEM_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };

        case ItemActions.SET_EDITING:
            return {
                ...state,
                items: mergeItems(state.items, {
                    ...action.item,
                    editing: action.editing,
                } as ItemState),
            };
    }
};
