import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { SupportedCultureService } from "src/app/api";
import { SupportedCulture } from "src/app/models";

@Component({
    selector: "app-create-culture",
    templateUrl: "./create-culture.component.html",
})
export class CreateCultureComponent implements OnInit {
    culture: string;
    loading: boolean = false;

    cultures: SupportedCulture[];

    @Output()
    created = new EventEmitter<SupportedCulture>();

    constructor(private api: SupportedCultureService) {}

    ngOnInit(): void {
        this.loadCultures();
    }

    loadCultures(): void {
        this.loading = true;
        this.culture = "";

        this.api.GetAvailableDropdownItems$.subscribe((cultures) => {
            this.loading = false;
            this.culture = null;
            this.cultures = cultures;
        });
    }

    create(): void {
        const culture = SupportedCulture.generateMockItem();
        culture.name = this.culture;

        this.created.emit(culture);
    }
}
