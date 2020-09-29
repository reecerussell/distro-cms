import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { SupportedCultureService } from "src/app/api";
import { SupportedCulture, SupportedCultureCreate } from "src/app/models";

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

    @Output()
    created = new EventEmitter<SupportedCultureCreate>();

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
            this.availableCultures = cultures;
        });

        this.api.GetDropdownItems$(true).subscribe((cultures) => {
            this.loading = false;
            this.cloneCulture = null;
            this.supportedCultures = cultures;
        });
    }

    create(): void {
        this.created.emit(
            new SupportedCultureCreate(this.culture, this.cloneCulture)
        );
    }
}
