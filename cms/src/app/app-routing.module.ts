import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ItemListComponent } from "./components/dictionary/item-list/item-list.component";
import { CreateRoleComponent } from "./components/roles/create-role/create-role.component";
import { EditRoleComponent } from "./components/roles/edit-role/edit-role.component";
import { RoleListComponent } from "./components/roles/role-list/role-list.component";

const routes: Routes = [
    {
        path: "roles",
        children: [
            {
                path: "",
                component: RoleListComponent,
            },
            {
                path: "create",
                component: CreateRoleComponent,
            },
            {
                path: ":id",
                children: [
                    {
                        path: "edit",
                        component: EditRoleComponent,
                    },
                ],
            },
        ],
    },
    {
        path: "dictionary",
        children: [
            {
                path: "",
                component: ItemListComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: true })],
    exports: [RouterModule],
})
export class AppRoutingModule {}
