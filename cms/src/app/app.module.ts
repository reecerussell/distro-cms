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

// Reducers
import * as RoleReducer from "./store/roles/role.reducer";
import * as ErrorReducer from "./store/errors/error.reducer";

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
            errors: ErrorReducer.ErrorReducer,
        }),
        EffectsModule.forRoot([RoleEffects]),
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
