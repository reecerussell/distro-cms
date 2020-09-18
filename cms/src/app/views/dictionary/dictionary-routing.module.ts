import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { ListComponent } from "./list/list.component";

const dictionaryRoutes: Routes = [
    {
        path: "",
        component: ListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(dictionaryRoutes)],
    exports: [RouterModule],
})
export class DictionaryRoutingModule {}
