import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ItemState } from "../../../store/dictionary/item.state";

@Component({
    selector: "app-item-list-edit-field",
    templateUrl: "./item-list-edit-field.component.html",
})
export class ItemListEditFieldComponent implements OnInit {
    @Input() item: ItemState;

    @Output() updated = new EventEmitter<ItemState>();

    constructor() {}

    ngOnInit(): void {}

    edit(): void {
        this.item = {
            ...this.item,
            editing: true,
        };
    }

    onSave(): void {
        this.updated.emit(this.item);
    }
}
