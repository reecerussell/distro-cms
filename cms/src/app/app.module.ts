import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { HttpClientModule } from "@angular/common/http";

// App
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

// Effects
import { RoleEffects } from "./store/roles/role.effect";
import { ItemEffects } from "./store/dictionary/item.effect";

// Reducers
import * as RoleReducer from "./store/roles/role.reducer";
import * as ItemReducer from "./store/dictionary/item.reducer";

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
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
