import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import User, { UserCreate } from "src/app/models/user.model";
import AppState from "src/app/store/app.state";
import { CreateUser } from "src/app/store/users/user.action";
import { UsersState } from "src/app/store/users/user.state";

@Component({
    selector: "app-user-create-form",
    templateUrl: "./create.component.html",
})
export class CreateComponent implements OnInit {
    usersState$: Observable<UsersState>;
    user?: User;

    firstname: string = "";
    lastname: string = "";
    email: string = "";

    loading: boolean = false;
    posted: boolean = false;
    errorMap = new Map<string, string>([
        ["firstname", null],
        ["lastname", null],
        ["email", null],
    ]);

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.usersState$ = this.store.select((state) => state.users);
        this.usersState$.subscribe((state) => {
            if (state) {
                this.loading = state.loading;

                if (this.posted) {
                    const newUser = state.users.find(
                        (x) => x.email === this.email
                    );
                    if (newUser && !state.error) {
                        this.user = newUser;
                    }

                    if (state.error) {
                        this.posted = false;
                    }
                }
            }
        });
    }

    getError(name: string): string {
        return this.errorMap.get(name);
    }

    setError(name: string, value: string): void {
        this.errorMap.set(name, value);
    }

    validate(): boolean {
        let valid = true;

        if (this.firstname.length < 1) {
            this.setError("firstname", "Firstname is required.");
            valid = false;
        } else if (this.firstname.length > 255) {
            this.setError(
                "firstname",
                "Firstname cannot be greater than 255 characters long."
            );
            valid = false;
        } else {
            this.setError("firstname", null);
        }

        if (this.lastname.length < 1) {
            this.setError("lastname", "Lastname is required.");
            valid = false;
        } else if (this.lastname.length > 255) {
            this.setError(
                "lastname",
                "Lastname cannot be greater than 255 characters long."
            );
            valid = false;
        } else {
            this.setError("lastname", null);
        }

        if (this.email.length < 1) {
            this.setError("email", "Email is required.");
            valid = false;
        } else if (this.email.length > 255) {
            this.setError(
                "email",
                "Email cannot be greater than 255 characters long."
            );
            valid = false;
        } else if (
            !/[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$/.test(this.email)
        ) {
            this.setError("email", "Email is not valid.");
            valid = false;
        } else {
            this.setError("email", null);
        }

        return valid;
    }

    save(): void {
        if (!this.validate() || this.loading) {
            return;
        }

        const data = {
            firstname: this.firstname,
            lastname: this.lastname,
            email: this.email.toLowerCase(),
        };

        this.store.dispatch(new CreateUser(data));
        this.posted = true;
    }
}
