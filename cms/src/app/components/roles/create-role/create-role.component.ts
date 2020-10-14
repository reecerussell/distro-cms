import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import Role from "src/app/models/role.model";
import { RoleListState } from "src/app/store/roles/role.state";
import * as RoleActions from "src/app/store/roles/role.action";
import AppState from "src/app/store/app.state";
import { Observable } from "rxjs";

@Component({
    selector: "app-create-role",
    templateUrl: "./create-role.component.html",
})
export class CreateRoleComponent implements OnInit {
    roleListState$: Observable<RoleListState>;
    role: Role;

    constructor(private store: Store<AppState>) {
        this.roleListState$ = this.store.select((state) => state.roles);
    }

    ngOnInit(): void {
        this.role = {
            ...Role.generateMockRole(),
        };
    }

    create(): void {
        this.store.dispatch(new RoleActions.CreateRole(this.role));
    }
}
