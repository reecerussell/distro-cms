import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { DictionaryRoutingModule } from "./dictionary-routing.module";

// Components
import { ItemListComponent } from "../../components/dictionary/item-list/item-list.component";
import { ItemListEditFieldComponent } from "../../components/dictionary/item-list-edit-field/item-list-edit-field.component";
import { CreateItemComponent } from "../../components/dictionary/create-item/create-item.component";

import { ListComponent } from "./list/list.component";

@NgModule({
    imports: [CommonModule, FormsModule, DictionaryRoutingModule],
    declarations: [
        ListComponent,
        ItemListComponent,
        ItemListEditFieldComponent,
        CreateItemComponent,
    ],
})
export class DictionaryModule {}
