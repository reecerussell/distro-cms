import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import AppState from "src/app/store/app.state";
import { RoleListState } from "src/app/store/roles/role.state";

@Component({
    selector: "app-create-role-view",
    templateUrl: "./create.component.html",
})
export class CreateComponent implements OnInit {
    roleState$: Observable<RoleListState>;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.roleState$ = this.store.select((state) => state.roles);
    }
}
