import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import * as UserAction from "../../../store/users/user.action";
import { UsersState } from "../../../store/users/user.state";
import AppState from "../../../store/app.state";
import { Observable } from "rxjs";

@Component({
    selector: "app-list",
    templateUrl: "./list.component.html",
})
export class ListComponent implements OnInit {
    usersState$: Observable<UsersState>;
    searchTerm: string;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.usersState$ = this.store.select((state) => state.users);
        this.loadUsers();
    }

    loadUsers() {
        this.store.dispatch(new UserAction.GetUsers(this.searchTerm));
    }
}
