import { Component, OnInit } from "@angular/core";
import { RoleListState, RoleState } from "../../../store/roles/role.state";
import { Store } from "@ngrx/store";
import * as RoleAction from "../../../store/roles/role.action";
import { Observable } from "rxjs";
import Role from "src/app/models/role.model";

@Component({
    selector: "app-role-list",
    templateUrl: "./role-list.component.html",
})
export class RoleListComponent implements OnInit {
    roleListState$: Observable<RoleState[]>;
    searchTerm: string;

    constructor(private store: Store<RoleListState>) {}

    ngOnInit(): void {
        this.roleListState$ = this.store.select((state) => state.roles);
        this.loadRoles()
    }

    loadRoles() {
        this.store.dispatch(new RoleAction.GetRoles(this.searchTerm));
    }

    onCreate(role) {
        this.store.dispatch(new RoleAction.CreateRole(role));
    }

    onDelete(role: Role) {
        this.store.dispatch(new RoleAction.DeleteRole(role.id));
    }

    onEdit(role: Role) {
        this.store.dispatch(new RoleAction.UpdateRole(role));
    }
}
