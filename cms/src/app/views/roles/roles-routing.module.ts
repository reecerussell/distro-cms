import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { ListComponent } from "./list/list.component";
import { CreateComponent } from "./create/create.component";
import { EditComponent } from "./edit/edit.component";

const roleRoutes: Routes = [
    {
        path: "",
        component: ListComponent,
    },
    {
        path: "create",
        component: CreateComponent,
    },
    {
        path: ":id",
        children: [
            {
                path: "edit",
                component: EditComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(roleRoutes)],
    exports: [RouterModule],
})
export class RolesRoutingModule {}
