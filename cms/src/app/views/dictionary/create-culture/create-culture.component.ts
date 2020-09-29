import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { SupportedCultureCreate } from "src/app/models";
import * as DictionaryActions from "src/app/store/dictionary/dictionary.action";
import AppState from "src/app/store/app.state";
import { Observable } from "rxjs";
import { DictionaryState } from "src/app/store/dictionary/dictionary.state";
import { Router } from "@angular/router";

@Component({
    selector: "app-create-culture-view",
    templateUrl: "./create-culture.component.html",
})
export class CreateCultureComponent implements OnInit {
    dictionaryState$: Observable<DictionaryState>;

    constructor(private store: Store<AppState>, private router: Router) {}

    ngOnInit(): void {
        this.dictionaryState$ = this.store.select((state) => state.dictionary);
    }

    onCreate(culture: SupportedCultureCreate): void {
        this.store.dispatch(new DictionaryActions.CreateCulture(culture));
    }
}
