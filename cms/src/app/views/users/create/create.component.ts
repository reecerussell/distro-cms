import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import AppState from "src/app/store/app.state";
import { UsersState } from "src/app/store/users/user.state";
import * as UserActions from "src/app/store/users/user.action";

@Component({
    selector: "app-create",
    templateUrl: "./create.component.html",
})
export class CreateComponent implements OnInit {
    usersState$: Observable<UsersState>;

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.usersState$ = this.store.select((state) => state.users);
    }
}
