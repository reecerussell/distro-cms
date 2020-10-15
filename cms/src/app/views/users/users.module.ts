import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { UsersRoutingModule } from "./users-routing.module";

// Components
import { ListComponent } from "../../components/users/list/list.component";

// Views
import { ListComponent as ListView } from "./list/list.component";
import { EditComponent as EditView } from "./edit/edit.component";

@NgModule({
    imports: [CommonModule, FormsModule, UsersRoutingModule],
    declarations: [ListComponent, ListView, EditView],
})
export class UsersModule {}
