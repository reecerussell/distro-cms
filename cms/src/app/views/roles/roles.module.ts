import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RolesRoutingModule } from "./roles-routing.module";

// Components
import { RoleListComponent } from "../../components/roles/role-list/role-list.component";
import { CreateRoleComponent } from "../../components/roles/create-role/create-role.component";

import { ListComponent } from "./list/list.component";
import { CreateComponent } from './create/create.component';
import { EditComponent } from './edit/edit.component';

@NgModule({
    imports: [CommonModule, FormsModule, RolesRoutingModule],
    declarations: [
        ListComponent,
        RoleListComponent,
        CreateRoleComponent,
        CreateComponent,
        EditComponent,
    ],
})
export class RolesModule {}
