import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { SupportedCulture } from "src/app/models";
import AppState from "src/app/store/app.state";
import { DictionaryState } from "src/app/store/dictionary/dictionary.state";
import * as DictionaryActions from "src/app/store/dictionary/dictionary.action";

@Component({
    selector: "app-culture-edit",
    templateUrl: "./culture-edit.component.html",
})
export class CultureEditComponent implements OnInit {
    dictionaryState$: Observable<DictionaryState>;
    culture: SupportedCulture;

    constructor(
        private store: Store<AppState>,
        private route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.dictionaryState$ = this.store.select((state) => state.dictionary);
        this.route.paramMap.subscribe((params) => {
            const id = params.get("id");
            this.store.dispatch(new DictionaryActions.GetCulture(id));
            this.dictionaryState$.subscribe((state) => {
                const culture = state.cultures.find((x) => x.id === id);
                if (culture) {
                    this.culture = { ...culture };
                }
            });
        });
    }

    save(): void {
        this.store.dispatch(new DictionaryActions.UpdateCulture(this.culture));
    }

    delete(): void {
        this.store.dispatch(
            new DictionaryActions.DeleteCulture(this.culture.id)
        );
    }
}
