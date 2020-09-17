import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import AppState from "src/app/store/app.state";
import { ItemListState } from "src/app/store/dictionary/item.state";
import * as ItemActions from "src/app/store/dictionary/item.action";
import { mergeMap } from "rxjs/operators";

@Component({
    selector: "app-item-list",
    templateUrl: "./item-list.component.html",
    styleUrls: ["./item-list.component.scss"],
})
export class ItemListComponent implements OnInit {
    itemListState$: Observable<ItemListState>;
    culture: string;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.itemListState$ = this.store.select((state) => state.dictionary);
        this.store.dispatch(
            new ItemActions.GetItems(
                localStorage.getItem("DICTIONARY_CULTURE") ?? navigator.language
            )
        );
        this.itemListState$.subscribe((state) => {
            this.culture =
                state?.culture ??
                localStorage.getItem("DICTIONARY_CULTURE") ??
                navigator.language;
        });
    }

    onCultureChange(): void {
        this.store.dispatch(new ItemActions.SetCulture(this.culture));
    }
}
