import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import AppState from "src/app/store/app.state";
import { DictionaryState } from "src/app/store/dictionary/dictionary.state";
import * as DictionaryActions from "src/app/store/dictionary/dictionary.action";

@Component({
    selector: "app-culture-list",
    templateUrl: "./culture-list.component.html",
    styleUrls: ["./culture-list.component.scss"],
})
export class CultureListComponent implements OnInit {
    dictionaryState$: Observable<DictionaryState>;

    term: string = "";

    constructor(private state: Store<AppState>) {}

    ngOnInit(): void {
        this.dictionaryState$ = this.state.select((state) => state.dictionary);
        this.loadCultures();
    }

    loadCultures(e?): void {
        if (e) {
            e.preventDefault();
        }

        this.state.dispatch(new DictionaryActions.GetCultures(this.term));
    }
}
