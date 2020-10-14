import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { SupportedCultureService } from "src/app/api";
import { SupportedCulture, SupportedCultureCreate } from "src/app/models";
import * as DictionaryActions from "src/app/store/dictionary/dictionary.action";
import AppState from "src/app/store/app.state";

@Component({
    selector: "app-create-culture",
    templateUrl: "./create-culture.component.html",
})
export class CreateCultureComponent implements OnInit {
    culture: string;
    cloneCulture: string;
    loading: boolean = false;

    availableCultures: SupportedCulture[];
    supportedCultures: SupportedCulture[];

    constructor(
        private api: SupportedCultureService,
        private store: Store<AppState>
    ) {}

    ngOnInit(): void {
        this.loadCultures();
    }

    loadCultures(): void {
        this.loading = true;
        this.culture = "";

        this.api.GetAvailableDropdownItems$.subscribe((cultures) => {
            this.loading = false;
            this.culture = null;
            this.availableCultures = cultures;
        });

        this.api.GetDropdownItems$(true).subscribe((cultures) => {
            this.loading = false;
            this.cloneCulture = null;
            this.supportedCultures = cultures;
        });
    }

    create(): void {
        this.store.dispatch(
            new DictionaryActions.CreateCulture(
                new SupportedCultureCreate(this.culture, this.cloneCulture)
            )
        );
    }
}
