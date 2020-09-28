import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { ActionReducerMap, StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { HttpClientModule } from "@angular/common/http";

// App
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

// Effects
import { RoleEffects } from "./store/roles/role.effect";
import { DictionaryEffects } from "./store/dictionary/dictionary.effect";

// Reducers
import * as RoleReducer from "./store/roles/role.reducer";
import * as DictionaryReducer from "./store/dictionary/dictionary.reducer";
import AppState from "./store/app.state";

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        StoreModule.forRoot({
            roles: RoleReducer.RoleReducer,
            dictionary: DictionaryReducer.DictionaryReducer,
        } as ActionReducerMap<AppState>),
        EffectsModule.forRoot([RoleEffects, DictionaryEffects]),
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
