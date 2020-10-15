import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import * as UserAction from "../../../store/users/user.action";
import { UsersState } from "../../../store/users/user.state";
import AppState from "../../../store/app.state";
import { Observable } from "rxjs";
import { RolesService } from "src/app/api";
import { RoleDropdownItem } from "src/app/models/role.model";

@Component({
    selector: "app-users-list",
    templateUrl: "./list.component.html",
})
export class ListComponent implements OnInit {
    usersState$: Observable<UsersState>;
    roles: RoleDropdownItem[];
    loading: boolean;

    searchTerm: string;
    roleId: string = "";

    constructor(
        private store: Store<AppState>,
        private rolesService: RolesService
    ) {}

    ngOnInit(): void {
        this.usersState$ = this.store.select((state) => state.users);
        this.rolesService.Dropdown$.subscribe((roles) => (this.roles = roles));
        this.usersState$.subscribe((state) => (this.loading = state.loading));

        this.loadUsers();
    }

    loadUsers() {
        if (this.loading) {
            return;
        }

        this.store.dispatch(
            new UserAction.GetUsers(this.searchTerm, this.roleId)
        );
    }
}
