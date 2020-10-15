import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import User, { UserUpdate } from "src/app/models/user.model";
import AppState from "src/app/store/app.state";
import { UsersState } from "src/app/store/users/user.state";
import * as UserActions from "src/app/store/users/user.action";

@Component({
    selector: "app-user-edit-view",
    templateUrl: "./edit.component.html",
})
export class EditComponent implements OnInit {
    usersState$: Observable<UsersState>;
    user: User;

    constructor(
        private route: ActivatedRoute,
        private store: Store<AppState>
    ) {}

    ngOnInit(): void {
        this.usersState$ = this.store.select((state) => state.users);
        this.route.paramMap.subscribe((params) =>
            this.onIdChange(params.get("id"))
        );
    }

    onIdChange(id: string): void {
        this.store.dispatch(new UserActions.GetUser(id));
        this.usersState$.subscribe(
            (state) => (this.user = { ...state.users.find((x) => x.id === id) })
        );
    }

    save(): void {
        this.store.dispatch(
            new UserActions.UpdateUser(this.user as UserUpdate)
        );
    }
}
