import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
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
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: true })],
    exports: [RouterModule],
})
export class AppRoutingModule {}
