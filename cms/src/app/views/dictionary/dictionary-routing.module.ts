import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { ListComponent } from "./list/list.component";
import { CreateCultureComponent } from "./create-culture/create-culture.component";
import { CultureListComponent } from "./culture-list/culture-list.component";

const dictionaryRoutes: Routes = [
    {
        path: "",
        children: [
            {
                path: "",
                component: ListComponent,
            },
            {
                path: "cultures",
                children: [
                    {
                        path: "",
                        component: CultureListComponent,
                    },
                    {
                        path: "create",
                        component: CreateCultureComponent,
                    },
                ],
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(dictionaryRoutes)],
    exports: [RouterModule],
})
export class DictionaryRoutingModule {}
