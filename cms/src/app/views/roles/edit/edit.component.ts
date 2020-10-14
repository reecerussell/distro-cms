import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import AppState from 'src/app/store/app.state';
import { RoleListState, RoleState } from 'src/app/store/roles/role.state';
import * as RoleActions from "src/app/store/roles/role.action";

@Component({
    selector: "app-edit-role-view",
    templateUrl: "./edit.component.html",
})
export class EditComponent implements OnInit {
    roleState$: Observable<RoleListState>;
    role: RoleState;

    constructor(private route: ActivatedRoute, private store: Store<AppState>) {}

    ngOnInit(): void {
        this.roleState$ = this.store.select(state => state.roles);
        this.route.paramMap.subscribe((params) => this.onIdChange(params.get("id")));
    }

    onIdChange(id): void {
        this.store.dispatch(new RoleActions.GetRole(id));
        this.roleState$.subscribe(state => (this.role = state.roles.find(x => x.id === id)))
    }

    onSave(): void {
        this.store.dispatch(new RoleActions.UpdateRole(this.role));
    }

    onDelete(): void {
        if (!this.role) {
            return;
        }

        this.store.dispatch(new RoleActions.DeleteRole(this.role.id))
    }
}
