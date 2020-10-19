import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { UsersRoutingModule } from "./users-routing.module";

// Components
import { ListComponent } from "../../components/users/list/list.component";
import { CreateComponent } from "../../components/users/create/create.component";

// Views
import { ListComponent as ListView } from "./list/list.component";
import { EditComponent as EditView } from "./edit/edit.component";
import { CreateComponent as CreateView } from "./create/create.component";

@NgModule({
    imports: [CommonModule, FormsModule, UsersRoutingModule],
    declarations: [
        ListComponent,
        ListView,
        EditView,
        CreateComponent,
        CreateView,
    ],
})
export class UsersModule {}
