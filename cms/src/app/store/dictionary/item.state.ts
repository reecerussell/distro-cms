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
    loading: boolean;
    error: string;
}

export const initializeItemListState = () => ({
    loading: false,
    items: [],
    error: null,
});
