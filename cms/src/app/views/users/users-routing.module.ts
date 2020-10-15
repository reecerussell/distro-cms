import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { EditComponent } from "./edit/edit.component";
import { ListComponent } from "./list/list.component";

const roleRoutes: Routes = [
    {
        path: "",
        component: ListComponent,
    },
    {
        path: ":id",
        children: [{ path: "edit", component: EditComponent }],
    },
];

@NgModule({
    imports: [RouterModule.forChild(roleRoutes)],
    exports: [RouterModule],
})
export class UsersRoutingModule {}
