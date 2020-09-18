import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { FormsModule } from "@angular/forms";
import { StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { HttpClientModule } from "@angular/common/http";

// Components
import { RoleListComponent } from "./components/roles/role-list/role-list.component";
import { CreateRoleComponent } from "./components/roles/create-role/create-role.component";
import { EditRoleComponent } from "./components/roles/edit-role/edit-role.component";

// Effects
import { RoleEffects } from "./store/roles/role.effect";
import { ItemEffects } from "./store/dictionary/item.effect";

// Reducers
import * as RoleReducer from "./store/roles/role.reducer";
import * as ItemReducer from "./store/dictionary/item.reducer";
import { ItemListComponent } from "./components/dictionary/item-list/item-list.component";
import { ItemListEditFieldComponent } from "./components/dictionary/item-list-edit-field/item-list-edit-field.component";

@NgModule({
    declarations: [
        AppComponent,
        RoleListComponent,
        CreateRoleComponent,
        EditRoleComponent,
        ItemListComponent,
        ItemListEditFieldComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
        StoreModule.forRoot({
            roles: RoleReducer.RoleReducer,
            dictionary: ItemReducer.ItemReducer,
        }),
        EffectsModule.forRoot([RoleEffects, ItemEffects]),
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
