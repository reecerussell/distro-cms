import { Injectable } from "@angular/core";
import { Action } from "@ngrx/store";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { Router } from "@angular/router";
import { RolesService } from "src/app/api";
import { Role } from "src/app/models";

import * as RoleActions from "./role.action";

@Injectable()
export class RoleEffects {
    constructor(
        private http: HttpClient,
        private actions$: Actions,
        private router: Router,
        private roles: RolesService
    ) {}

    GetRoles$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.GET_ROLES),
            mergeMap((action) =>
                this.roles.GetList$().pipe(
                    map((data) => new RoleActions.GetRolesSuccess(data)),
                    catchError((error: Error) =>
                        of(new RoleActions.GetRolesError(error.message))
                    )
                )
            )
        )
    );

    GetRole$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.GET_ROLE),
            mergeMap((action: RoleActions.GetRole) =>
                this.roles.Get$(action.id).pipe(
                    map((data) => new RoleActions.GetRoleSuccess(data)),
                    catchError((error: Error) =>
                        of(new RoleActions.GetRoleError(error.message))
                    )
                )
            )
        )
    );

    CreateRole$: Observable<Action> = createEffect(() => {
        return this.actions$.pipe(
            ofType(RoleActions.CREATE_ROLE),
            mergeMap((action: RoleActions.CreateRole) =>
                this.roles.Create$(action.role).pipe(
                    map((role: Role) => {
                        this.router.navigateByUrl(
                            "/roles/" + role.id + "/edit"
                        );
                        return new RoleActions.CreateRoleSuccess(role);
                    }),
                    catchError((error: Error) =>
                        of(new RoleActions.CreateRoleError(error.message))
                    )
                )
            )
        );
    });

    UpdateRole$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.UPDATE_ROLE),
            mergeMap((action: RoleActions.UpdateRole) =>
                this.roles.Update$(action.role).pipe(
                    map((data) => new RoleActions.UpdateRoleSuccess(data)),
                    catchError((error: Error) =>
                        of(
                            new RoleActions.UpdateRoleError(
                                action.role,
                                error.message
                            )
                        )
                    )
                )
            )
        )
    );

    DeleteRole$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.DELETE_ROLE),
            mergeMap((action: RoleActions.DeleteRole) =>
                this.roles.Delete$(action.id).pipe(
                    map((data) => {
                        this.router.navigateByUrl("/roles");
                        return new RoleActions.DeleteRoleSuccess(action.id);
                    }),
                    catchError((resp: HttpErrorResponse) =>
                        of(
                            new RoleActions.DeleteRoleError(
                                resp.error?.error ?? resp.statusText
                            )
                        )
                    )
                )
            )
        )
    );
}
