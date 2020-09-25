import { Injectable } from "@angular/core";
import { Action } from "@ngrx/store";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { Observable, of } from "rxjs";
import { catchError, map, mergeMap } from "rxjs/operators";
import { DictionaryService } from "../../api/dictionary.service";

import * as ItemActions from "./item.action";

@Injectable()
export class ItemEffects {
    constructor(private actions$: Actions, private api: DictionaryService) {}

    GetItems$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(ItemActions.GET_ITEMS),
            mergeMap((action: ItemActions.GetItems) =>
                this.api.GetList$(action.culture).pipe(
                    map((items) => new ItemActions.GetItemsSuccess(items)),
                    catchError((error: Error) =>
                        of(new ItemActions.UpdateItemError(error.message))
                    )
                )
            )
        )
    );

    CreateItem$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(ItemActions.CREATE_ITEM),
            mergeMap((action: ItemActions.CreateItem) =>
                this.api.Create$(action.item, action.culture).pipe(
                    map((data) => new ItemActions.CreateItemSuccess(data)),
                    catchError((error: Error) =>
                        of(new ItemActions.UpdateItemError(error.message))
                    )
                )
            )
        )
    );

    UpdateItem$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(ItemActions.UPDATE_ITEM),
            mergeMap((action: ItemActions.UpdateItem) =>
                this.api.Update$(action.item).pipe(
                    map((data) => new ItemActions.UpdateItemSuccess(data)),
                    catchError((error: Error) =>
                        of(new ItemActions.UpdateItemError(error.message))
                    )
                )
            )
        )
    );

    DeleteItem$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(ItemActions.DELETE_ITEM),
            mergeMap((action: ItemActions.DeleteItem) =>
                this.api.Delete$(action.id).pipe(
                    map((data) => new ItemActions.DeleteItemSuccess(action.id)),
                    catchError((error: Error) =>
                        of(new ItemActions.DeleteItemError(error.message))
                    )
                )
            )
        )
    );

    SetCulture$: Observable<Action> = createEffect(() =>
        this.actions$.pipe(
            ofType(ItemActions.SET_CULTURE),
            mergeMap((action: ItemActions.SetCulture) => {
                localStorage.setItem("DICTIONARY_CULTURE", action.culture);

                return of(new ItemActions.GetItems(action.culture));
            })
        )
    );
}
