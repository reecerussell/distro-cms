import { Component, Input, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { RoleListState, RoleState } from "src/app/store/roles/role.state";
import * as RoleActions from "src/app/store/roles/role.action";
import AppState from "src/app/store/app.state";
import { Observable } from "rxjs";
import { ActivatedRoute } from "@angular/router";

@Component({
    selector: "app-edit-role",
    templateUrl: "./edit-role.component.html",
})
export class EditRoleComponent implements OnInit {
    roleListState$: Observable<RoleListState>;
    role: RoleState;

    @Input() id: string;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.roleListState$ = this.store.select((state) => state.roles);
        this.store.dispatch(new RoleActions.GetRole(this.id));

        this.roleListState$.subscribe((state) => {
            if (state.roles)
                [
                    (this.role = {
                        ...state.roles.find((x) => x.id === this.id),
                    } as RoleState),
                ];
        });
    }

    onSubmit(): void {
        this.store.dispatch(new RoleActions.UpdateRole(this.role));
    }

    onDelete(): void {
        this.store.dispatch(new RoleActions.DeleteRole(this.role.id));
    }
}
