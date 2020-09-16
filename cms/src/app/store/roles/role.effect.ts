import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Action } from "@ngrx/store";
import { Actions, createEffect, ofType } from "@ngrx/effects";

import * as RoleActions from "./role.action";
import * as ErrorActions from "../errors/error.action";

import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { Router } from "@angular/router";

@Injectable()
export class RoleEffects {
    constructor(
        private http: HttpClient,
        private actions$: Actions,
        private router: Router
    ) {}

    GetRoles$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.GET_ROLES),
            mergeMap((action) =>
                this.http.get(environment.api_base_url + "roles").pipe(
                    map(
                        (data) => new RoleActions.GetRolesSuccess(data["data"])
                    ),
                    catchError((resp) => {
                        if (resp.error?.error) {
                            return [
                                new ErrorActions.AddError(resp.error.error),
                                new RoleActions.GetRolesError(),
                            ];
                        }

                        return [new RoleActions.GetRolesError()];
                    })
                )
            )
        )
    );

    GetRole$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.GET_ROLE),
            mergeMap((action: RoleActions.GetRole) =>
                this.http
                    .get(environment.api_base_url + "roles/" + action.payload)
                    .pipe(
                        map(
                            (data) =>
                                new RoleActions.GetRoleSuccess(data["data"])
                        ),
                        catchError((resp) => {
                            if (resp.error?.error) {
                                return [
                                    new ErrorActions.AddError(resp.error.error),
                                    new RoleActions.GetRoleError(),
                                ];
                            }

                            return [new RoleActions.GetRoleError()];
                        })
                    )
            )
        )
    );

    CreateRole$: Observable<Action> = createEffect(() => {
        return this.actions$.pipe(
            ofType(RoleActions.CREATE_ROLE),
            mergeMap((action: RoleActions.CreateRole) =>
                this.http
                    .post(
                        environment.api_base_url + "roles",
                        JSON.stringify(action.payload),
                        {
                            headers: new HttpHeaders({
                                "Content-Type": "application/json",
                            }),
                        }
                    )
                    .pipe(
                        map((data) => {
                            const role = data["data"];
                            this.router.navigateByUrl(
                                "/roles/" + role.id + "/edit"
                            );
                            return new RoleActions.CreateRoleSuccess(role);
                        }),
                        catchError((resp) => {
                            if (resp.error?.error) {
                                return [
                                    new ErrorActions.AddError(resp.error.error),
                                    new RoleActions.CreateRoleError(),
                                ];
                            }

                            return [new RoleActions.CreateRoleError()];
                        })
                    )
            )
        );
    });

    UpdateRole$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.UPDATE_ROLE),
            mergeMap((action: RoleActions.UpdateRole) =>
                this.http
                    .put(
                        environment.api_base_url + "roles",
                        JSON.stringify(action.payload),
                        {
                            headers: new HttpHeaders({
                                "Content-Type": "application/json",
                            }),
                        }
                    )
                    .pipe(
                        map(
                            (data) =>
                                new RoleActions.UpdateRoleSuccess(data["data"])
                        ),
                        catchError((resp) => {
                            if (resp.error?.error) {
                                return [
                                    new ErrorActions.AddError(resp.error.error),
                                    new RoleActions.UpdateRoleError(),
                                ];
                            }

                            return [new RoleActions.UpdateRoleError()];
                        })
                    )
            )
        )
    );

    DeleteRole$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(RoleActions.DELETE_ROLE),
            mergeMap((action: RoleActions.DeleteRole) =>
                this.http
                    .delete(
                        environment.api_base_url + "roles/" + action.payload
                    )
                    .pipe(
                        map((data) => {
                            this.router.navigateByUrl("/roles");
                            return new RoleActions.DeleteRoleSuccess(
                                action.payload
                            );
                        }),
                        catchError((resp) => {
                            if (resp.error?.error) {
                                return [
                                    new ErrorActions.AddError(resp.error.error),
                                    new RoleActions.DeleteRoleError(),
                                ];
                            }

                            return [new RoleActions.DeleteRoleError()];
                        })
                    )
            )
        )
    );
}
