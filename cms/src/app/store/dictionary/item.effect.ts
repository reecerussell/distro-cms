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
                    catchError((error) =>
                        of(new ItemActions.GetItemsError(error))
                    )
                )
            )
        )
    );
}
