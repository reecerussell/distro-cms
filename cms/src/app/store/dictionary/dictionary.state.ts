import { DictionaryItem, SupportedCulture } from "src/app/models";

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

export interface CultureState extends SupportedCulture {
    loading: boolean;
    editing: boolean;
    error: string;
}

export const initializeCultureState = () => ({
    loading: false,
    editing: false,
    error: null,
});

export interface DictionaryState {
    loading: boolean;
    error: string;
    items: ItemState[];
    cultures: CultureState[];
    culture: string;
}
