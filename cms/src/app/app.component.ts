import { Component } from "@angular/core";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.scss"],
})
export class AppComponent {
    title = "cms";
    showMenu: boolean = false;

    toggleMenu() {
        this.showMenu = !this.showMenu;
    }
}
