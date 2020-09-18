import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

// Components
import { CreateRoleComponent } from "./components/roles/create-role/create-role.component";
import { EditRoleComponent } from "./components/roles/edit-role/edit-role.component";
import { RoleListComponent } from "./components/roles/role-list/role-list.component";

export const routes: Routes = [
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
        loadChildren: () =>
            import("./views/dictionary/dictionary.module").then(
                (c) => c.DictionaryModule
            ),
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: true })],
    exports: [RouterModule],
})
export class AppRoutingModule {}
