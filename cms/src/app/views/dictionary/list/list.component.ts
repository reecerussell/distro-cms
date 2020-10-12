import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import AppState from 'src/app/store/app.state';
import * as DictionaryActions from 'src/app/store/dictionary/dictionary.action';
import { DictionaryState } from 'src/app/store/dictionary/dictionary.state';

@Component({
    selector: "app-dictionary-list-view",
    templateUrl: "./list.component.html",
})
export class ListComponent implements OnInit {
    dictionaryState$: Observable<DictionaryState>
    culture: string;
    
    constructor(private store: Store<AppState>, private router: Router) {
        this.dictionaryState$ = store.select(state => state.dictionary)
    }

    ngOnInit(): void {
        this.store.dispatch(new DictionaryActions.GetDropdownItems())

        this.dictionaryState$.subscribe(state => {
            this.culture = state?.culture ??
            localStorage.getItem("DICTIONARY_CULTURE") ??
            navigator.language;
        })
    }

    onCultureChange(): void {
        if (this.culture === "--") {
            this.router.navigateByUrl("/dictionary/cultures/create")
            return;
        }

        this.store.dispatch(new DictionaryActions.SetCulture(this.culture));
    }
}
