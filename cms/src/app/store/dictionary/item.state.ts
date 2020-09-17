import DictionaryItem from "../../models/dictionary-item.model";

export interface ItemState extends DictionaryItem {
    loading: boolean;
    editing: boolean;
    error: string;
}

export const initializeItemState = () => ({
    loading: false,
    editing: false,
    error: null,
});

export interface ItemListState {
    items: ItemState[];
    culture: string;
    loading: boolean;
    error: string;
}

export const initializeItemListState = () => ({
    loading: false,
    culture: navigator.language,
    items: [],
    error: null,
});
