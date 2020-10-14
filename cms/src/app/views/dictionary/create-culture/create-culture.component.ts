import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import AppState from "src/app/store/app.state";
import { Observable } from "rxjs";
import { DictionaryState } from "src/app/store/dictionary/dictionary.state";

@Component({
    selector: "app-create-culture-view",
    templateUrl: "./create-culture.component.html",
})
export class CreateCultureComponent implements OnInit {
    dictionaryState$: Observable<DictionaryState>;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.dictionaryState$ = this.store.select((state) => state.dictionary);
    }
}
