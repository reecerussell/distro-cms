import { ChangeDetectionStrategy, Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import AppState from "src/app/store/app.state";
import { ItemState } from "src/app/store/dictionary/dictionary.state";
import * as ItemActions from "src/app/store/dictionary/dictionary.action";
import DictionaryItem from "src/app/models/dictionary-item.model";
import { DictionaryState } from "src/app/store/dictionary/dictionary.state";

@Component({
    selector: "app-dictionary-item-list",
    templateUrl: "./item-list.component.html",
    styleUrls: ["./item-list.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ItemListComponent implements OnInit {
    itemListState$: Observable<DictionaryState>;
    items: ItemState[];
    culture: string;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.itemListState$ = this.store.select((state) => state.dictionary);
        this.itemListState$.subscribe((state) =>
            console.log("Dictionary State", state)
        );

        this.store.dispatch(
            new ItemActions.GetItems(
                localStorage.getItem("DICTIONARY_CULTURE") ?? navigator.language
            )
        );

        this.store
            .select((state) => state.dictionary)
            .subscribe((state) => {
                this.culture =
                    state?.culture ??
                    localStorage.getItem("DICTIONARY_CULTURE") ??
                    navigator.language;

                this.items = state?.items.map((i) => ({ ...i } as ItemState));
            });
    }

    onCultureChange(): void {
        this.store.dispatch(new ItemActions.SetCulture(this.culture));
    }

    toggleEdit(item: ItemState): void {
        this.store.dispatch(new ItemActions.SetEditing(item, !item.editing));
    }

    saveItem(item: ItemState): void {
        this.store.dispatch(new ItemActions.UpdateItem(item));
    }

    deleteItem(item: ItemState): void {
        this.store.dispatch(new ItemActions.DeleteItem(item.id));
    }
}
