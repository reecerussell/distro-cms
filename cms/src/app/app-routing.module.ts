import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

export const routes: Routes = [
    {
        path: "roles",
        loadChildren: () =>
            import("./views/roles/roles.module").then((c) => c.RolesModule),
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
