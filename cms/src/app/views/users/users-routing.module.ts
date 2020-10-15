import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { ListComponent } from "./list/list.component";

const roleRoutes: Routes = [
    {
        path: "",
        component: ListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(roleRoutes)],
    exports: [RouterModule],
})
export class UsersRoutingModule {}
