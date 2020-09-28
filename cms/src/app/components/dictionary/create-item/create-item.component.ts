import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    OnInit,
    Output,
} from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import DictionaryItem from "src/app/models/dictionary-item.model";
import AppState from "src/app/store/app.state";
import { DictionaryState } from "src/app/store/dictionary/dictionary.state";

@Component({
    selector: "app-create-item",
    templateUrl: "./create-item.component.html",
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateItemComponent implements OnInit {
    itemListState$: Observable<DictionaryState>;
    count: number;

    key: string = "";
    value: string = "";
    displayName: string = "";

    @Output() created = new EventEmitter<DictionaryItem>();

    constructor(private store: Store<AppState>) {}

    ngOnInit(): void {
        this.itemListState$ = this.store.select((state) => state.dictionary);
        this.itemListState$.subscribe((state) => {
            if (!this.count) {
                this.count = state?.items.length ?? 0;
            }

            if (this.count < state?.items.length ?? 0) {
                this.key = "";
                this.value = "";
            }
        });
    }

    create(): void {
        const item = {
            ...DictionaryItem.generateMockItem(),
            key: this.key,
            value: this.value,
            displayName: this.displayName,
        };

        this.created.emit(item);
    }
}
