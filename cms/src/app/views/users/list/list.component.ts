import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import AppState from "src/app/store/app.state";
import { UsersState } from "src/app/store/users/user.state";

@Component({
    selector: "app-users-list-view",
    templateUrl: "./list.component.html",
})
export class ListComponent implements OnInit {
    usersState$: Observable<UsersState>;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.usersState$ = this.store.select((state) => state.users);
    }
}
