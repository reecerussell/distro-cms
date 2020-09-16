import { Component, OnDestroy, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import Role from "src/app/models/role.model";
import { RoleState, initializeRoleState } from "src/app/store/roles/role.state";
import * as RoleActions from "src/app/store/roles/role.action";
import * as ErrorActions from "src/app/store/errors/error.action";
import AppState from "src/app/store/app.state";

@Component({
    selector: "app-create-role",
    templateUrl: "./create-role.component.html",
    styleUrls: ["./create-role.component.scss"],
})
export class CreateRoleComponent implements OnInit, OnDestroy {
    role: RoleState;
    error: string = null;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.role = {
            ...initializeRoleState(),
            ...Role.generateMockRole(),
        };

        this.store.subscribe((state) => {
            console.log(state);
            if (state.errors) {
                if (state.errors.errors.length > 0) {
                    this.error = state.errors.errors[0];
                } else {
                    this.error = null;
                }
            }
        });
    }

    ngOnDestroy(): void {
        this.store.dispatch(new ErrorActions.ClearErrors());
    }

    onSubmit(): void {
        this.store.dispatch(new RoleActions.CreateRole(this.role));
    }
}
