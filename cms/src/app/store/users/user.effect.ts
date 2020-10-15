import { Injectable } from "@angular/core";
import { Action } from "@ngrx/store";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { UsersService } from "../../api/users.service";
import * as UserActions from "./user.action";

@Injectable()
export class UserEffects {
    constructor(
        private http: HttpClient,
        private actions$: Actions,
        private users: UsersService
    ) {}

    GetUsers$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(UserActions.GET_USERS),
            mergeMap((action: UserActions.GetUsers) =>
                this.users.GetList$(action.type, action.roleId).pipe(
                    map((data) => new UserActions.GetUsersSuccess(data)),
                    catchError((error: Error) =>
                        of(new UserActions.GetUsersError(error.message))
                    )
                )
            )
        )
    );
}
