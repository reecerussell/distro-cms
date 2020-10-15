import { Injectable } from "@angular/core";
import { Action } from "@ngrx/store";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Observable, of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { UsersService } from "../../api/users.service";
import * as UserActions from "./user.action";

@Injectable()
export class UserEffects {
    constructor(private actions$: Actions, private users: UsersService) {}

    GetUsers$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.GET_USERS),
            mergeMap((action: UserActions.GetUsers) =>
                this.users.GetList$(action.searchTerm, action.roleId).pipe(
                    map((data) => new UserActions.GetUsersSuccess(data)),
                    catchError((error: Error) =>
                        of(new UserActions.GetUsersError(error.message))
                    )
                )
            )
        )
    );

    GetUser$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.GET_USER),
            mergeMap((action: UserActions.GetUser) =>
                this.users.Get$(action.id).pipe(
                    map((data) => new UserActions.GetUserSuccess(data)),
                    catchError((error: Error) =>
                        of(new UserActions.GetUserError(error.message))
                    )
                )
            )
        )
    );

    Update$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.UPDATE_USER),
            mergeMap((action: UserActions.UpdateUser) =>
                this.users.Update$(action.user).pipe(
                    map((data) => new UserActions.UpdateUserSuccess(data)),
                    catchError((error: Error) =>
                        of(new UserActions.UpdateUserError(error.message))
                    )
                )
            )
        )
    );
}
