import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { ActionReducerMap, StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { HttpClientModule } from "@angular/common/http";
import Interceptors from "./api/interceptor";
import { environment } from "../environments/environment";

// App
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

// Effects
import { RoleEffects } from "./store/roles/role.effect";
import { DictionaryEffects } from "./store/dictionary/dictionary.effect";
import { UserEffects } from "./store/users/user.effect";

// Reducers
import * as RoleReducer from "./store/roles/role.reducer";
import * as DictionaryReducer from "./store/dictionary/dictionary.reducer";
import * as UserReducer from "./store/users/user.reducer";
import AppState from "./store/app.state";

// Toastr
import { ToastrModule } from "ngx-toastr";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NavbarComponent } from "./components/layout/navbar/navbar.component";
import { FormsModule } from "@angular/forms";
import { StoreDevtoolsModule } from "@ngrx/store-devtools";

@NgModule({
    declarations: [AppComponent, NavbarComponent],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        StoreModule.forRoot({
            roles: RoleReducer.RoleReducer,
            dictionary: DictionaryReducer.DictionaryReducer,
            users: UserReducer.UserReducer,
        } as ActionReducerMap<AppState>),
        StoreDevtoolsModule.instrument({
            maxAge: 25, // Retains last 25 states
            logOnly: environment.production, // Restrict extension to log-only mode
        }),
        EffectsModule.forRoot([RoleEffects, DictionaryEffects, UserEffects]),
        BrowserAnimationsModule,
        ToastrModule.forRoot(),
        FormsModule,
    ],
    providers: [Interceptors],
    bootstrap: [AppComponent],
})
export class AppModule {}
