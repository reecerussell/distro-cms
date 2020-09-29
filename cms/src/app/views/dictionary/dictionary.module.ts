import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { DictionaryRoutingModule } from "./dictionary-routing.module";

// Components
import { ItemListComponent } from "../../components/dictionary/item-list/item-list.component";
import { CreateItemComponent } from "../../components/dictionary/create-item/create-item.component";
import { CreateCultureComponent } from "../../components/dictionary/create-culture/create-culture.component";

// Views
import { ListComponent } from "./list/list.component";
import { CreateCultureComponent as CreateCultureView } from "./create-culture/create-culture.component";

@NgModule({
    imports: [CommonModule, FormsModule, DictionaryRoutingModule],
    declarations: [
        ListComponent,
        ItemListComponent,
        CreateItemComponent,
        CreateCultureComponent,
        CreateCultureView,
    ],
})
export class DictionaryModule {}
