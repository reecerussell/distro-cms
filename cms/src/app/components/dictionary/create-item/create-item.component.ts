import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
} from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import DictionaryItem from "src/app/models/dictionary-item.model";
import AppState from "src/app/store/app.state";
import * as DictionaryActions from "src/app/store/dictionary/dictionary.action";
import { DictionaryState } from "src/app/store/dictionary/dictionary.state";

@Component({
    selector: "app-create-item",
    templateUrl: "./create-item.component.html",
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateItemComponent {
    key: string = "";
    value: string = "";
    displayName: string = "";

    errorMap: Map<string, string> = new Map([["key", null],["value", null],["displayName", null]])

    @Input() culture: string;

    constructor(private store: Store<AppState>) {}

    create(): void {
        if (!this.validate()) {
            return;
        }

        const item = {
            ...DictionaryItem.generateMockItem(),
            key: this.key,
            value: this.value,
            displayName: this.displayName,
        };

        if (!item.displayName || item.displayName.length < 1) {
            item.displayName = item.key
        }

        this.store.dispatch(new DictionaryActions.CreateItem(item, this.culture));
    }

    validate(): boolean {
        if (this.key.length < 1) {
            this.errorMap.set("key", "Key is required.")
        } else if (this.key.length > 45) {
            this.errorMap.set("key", "Key cannot be greater than 45 characters long.")
        } else {
            this.errorMap.set("key", null);
        }

        if (this.displayName.length > 255) {
            this.errorMap.set("displayName", "Key cannot be greater than 255 characters long.")
        } else {
            this.errorMap.set("displayName", null);
        }

        if (this.value.length < 1) {
            this.errorMap.set("value", "Value is required.")
        } else if (this.value.length > 255) {
            this.errorMap.set("value", "Value cannot be greater than 255 characters long.")
        } else {
            this.errorMap.set("value", null);
        }

        return !(this.errorMap.get("key") || this.errorMap.get("value") || this.errorMap.get("displayName"));
    }
}
