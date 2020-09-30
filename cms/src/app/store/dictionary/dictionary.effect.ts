import { Injectable } from "@angular/core";
import { Action } from "@ngrx/store";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Observable, of } from "rxjs";
import { catchError, map, mergeMap, switchMap, tap } from "rxjs/operators";
import { DictionaryService, SupportedCultureService } from "src/app/api";

import * as DictionaryActions from "./dictionary.action";
import { Router } from "@angular/router";

@Injectable()
export class DictionaryEffects {
    constructor(
        private actions$: Actions,
        private dictionary: DictionaryService,
        private cultures: SupportedCultureService,
        private router: Router
    ) {}

    GetItems$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.GET_ITEMS),
            mergeMap((action: DictionaryActions.GetItems) =>
                this.dictionary.GetList$(action.culture).pipe(
                    map(
                        (items) => new DictionaryActions.GetItemsSuccess(items)
                    ),
                    catchError((error: Error) =>
                        of(new DictionaryActions.UpdateItemError(error.message))
                    )
                )
            )
        )
    );

    CreateItem$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.CREATE_ITEM),
            mergeMap((action: DictionaryActions.CreateItem) =>
                this.dictionary.Create$(action.item, action.culture).pipe(
                    map(
                        (data) => new DictionaryActions.CreateItemSuccess(data)
                    ),
                    catchError((error: Error) =>
                        of(new DictionaryActions.UpdateItemError(error.message))
                    )
                )
            )
        )
    );

    UpdateItem$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.UPDATE_ITEM),
            mergeMap((action: DictionaryActions.UpdateItem) =>
                this.dictionary.Update$(action.item).pipe(
                    map(
                        (data) => new DictionaryActions.UpdateItemSuccess(data)
                    ),
                    catchError((error: Error) =>
                        of(new DictionaryActions.UpdateItemError(error.message))
                    )
                )
            )
        )
    );

    DeleteItem$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.DELETE_ITEM),
            mergeMap((action: DictionaryActions.DeleteItem) =>
                this.dictionary.Delete$(action.id).pipe(
                    map(
                        (data) =>
                            new DictionaryActions.DeleteItemSuccess(action.id)
                    ),
                    catchError((error: Error) =>
                        of(new DictionaryActions.DeleteItemError(error.message))
                    )
                )
            )
        )
    );

    SetCulture$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.SET_CULTURE),
            mergeMap((action: DictionaryActions.SetCulture) => {
                console.log("Set culture to: ", action.culture);
                localStorage.setItem("DICTIONARY_CULTURE", action.culture);

                return [
                    new DictionaryActions.GetItems(action.culture),
                    new DictionaryActions.GetDropdownItems(),
                ];
            })
        )
    );

    GetDropdownItems$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.GET_DROPDOWN_ITEMS),
            mergeMap((action: DictionaryActions.GetDropdownItems) =>
                this.cultures.GetDropdownItems$(action.forceRefresh).pipe(
                    map(
                        (data) =>
                            new DictionaryActions.GetDropdownItemsSuccess(data)
                    ),
                    catchError((error: Error) =>
                        of(
                            new DictionaryActions.GetDropdownItemsError(
                                error.message
                            )
                        )
                    )
                )
            )
        )
    );

    CreateCulture$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.CREATE_CULTURE),
            switchMap((action: DictionaryActions.CreateCulture) =>
                this.cultures.Create$(action.culture).pipe(
                    switchMap((data) => {
                        this.router.navigateByUrl("/dictionary");

                        return [
                            new DictionaryActions.CreateCultureSuccess(data),
                            new DictionaryActions.GetDropdownItems(true),
                            new DictionaryActions.SetCulture(data.name),
                        ];
                    }),
                    catchError((error: Error) =>
                        of(new DictionaryActions.CreateItemError(error.message))
                    )
                )
            )
        )
    );

    GetCultures$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.GET_CULTURES),
            mergeMap((action: DictionaryActions.GetCultures) =>
                this.cultures.GetList$(action.term).pipe(
                    map(
                        (data) => new DictionaryActions.GetCulturesSuccess(data)
                    ),
                    catchError((error: Error) =>
                        of(
                            new DictionaryActions.GetCulturesError(
                                error.message
                            )
                        )
                    )
                )
            )
        )
    );

    GetCulture$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.GET_CULTURE),
            mergeMap((action: DictionaryActions.GetCulture) =>
                this.cultures.Get$(action.id).pipe(
                    map(
                        (data) => new DictionaryActions.GetCultureSuccess(data)
                    ),
                    catchError((error: Error) =>
                        of(new DictionaryActions.GetCultureError(error.message))
                    )
                )
            )
        )
    );

    Update$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.UPDATE_CULTURE),
            mergeMap((action: DictionaryActions.UpdateCulture) =>
                this.cultures.Update$(action.culture).pipe(
                    map(
                        (data) =>
                            new DictionaryActions.UpdateCultureSuccess(data)
                    ),
                    catchError((error: Error) =>
                        of(
                            new DictionaryActions.UpdateCultureError(
                                error.message
                            )
                        )
                    )
                )
            )
        )
    );

    Delete$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(DictionaryActions.DELETE_CULTURE),
            mergeMap((action: DictionaryActions.DeleteCulture) =>
                this.cultures.Delete$(action.id).pipe(
                    map((data) => {
                        this.router.navigateByUrl("/dictionary/cultures");

                        return new DictionaryActions.DeleteCultureSuccess(
                            action.id
                        );
                    }),
                    catchError((error: Error) =>
                        of(
                            new DictionaryActions.DeleteCultureError(
                                error.message
                            )
                        )
                    )
                )
            )
        )
    );
}
