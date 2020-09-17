import { initializeItemState, ItemListState, ItemState } from "./item.state";
import * as ItemActions from "./item.action";

export type Action = ItemActions.All;

const defaultState: ItemListState = {
    items: [],
    loading: false,
    error: null,
    culture: navigator.language,
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
    }
};
