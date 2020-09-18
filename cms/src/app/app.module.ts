import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { HttpClientModule } from "@angular/common/http";

// App
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

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

@NgModule({
    declarations: [
        AppComponent,
        RoleListComponent,
        CreateRoleComponent,
        EditRoleComponent,
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
