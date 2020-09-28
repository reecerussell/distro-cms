import { Injectable } from "@angular/core";
import { Action } from "@ngrx/store";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Observable, of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { DictionaryService, SupportedCultureService } from "src/app/api";

import * as DictionaryActions from "./dictionary.action";

@Injectable()
export class DictionaryEffects {
    constructor(
        private actions$: Actions,
        private dictionary: DictionaryService,
        private cultures: SupportedCultureService
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
                this.cultures.GetDropdownItems$().pipe(
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
}
