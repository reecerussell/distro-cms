import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { RoleState } from "src/app/store/roles/role.state";
import * as RoleActions from "src/app/store/roles/role.action";
import AppState from "src/app/store/app.state";
import { Observable } from "rxjs";
import { ActivatedRoute } from "@angular/router";

@Component({
    selector: "app-edit-role",
    templateUrl: "./edit-role.component.html",
    styleUrls: ["./edit-role.component.scss"],
})
export class EditRoleComponent implements OnInit {
    roleListState$: Observable<RoleState[]>;
    role: RoleState;
    error: string = null;

    constructor(
        private store: Store<AppState>,
        private route: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.roleListState$ = this.store.select((state) => state.roles.roles);
        this.route.paramMap.subscribe((params) => {
            const id = params.get("id");
            this.store.dispatch(new RoleActions.GetRole(id));

            this.roleListState$.subscribe(
                (roles) =>
                    (this.role = {
                        ...roles.find((x) => x.id === id),
                    } as RoleState)
            );
        });

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

    onSubmit(): void {
        this.store.dispatch(new RoleActions.UpdateRole(this.role));
    }

    onDelete(): void {
        this.store.dispatch(new RoleActions.DeleteRole(this.role.id));
    }
}
