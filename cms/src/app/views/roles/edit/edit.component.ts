import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
    selector: "app-edit-role-view",
    templateUrl: "./edit.component.html",
})
export class EditComponent implements OnInit {
    id: string;

    constructor(private route: ActivatedRoute) {}

    ngOnInit(): void {
        this.route.paramMap.subscribe((params) => (this.id = params.get("id")));
    }
}
