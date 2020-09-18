import { Injectable } from "@angular/core";
import {
    HttpClient,
    HttpErrorResponse,
    HttpHeaders,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import DictionaryItem from "../models/dictionary-item.model";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: "root",
})
export class DictionaryService {
    constructor(private http: HttpClient) {}

    GetList$(culture: string): Observable<DictionaryItem[]> {
        const options = {
            headers: new HttpHeaders({ "API-Culture": culture }),
        };

        return this.http.get(environment.api_base_url + "items", options).pipe(
            map((data) => data["data"]),
            catchError((resp: HttpErrorResponse) => {
                throw new Error(resp.error?.error ?? resp.statusText);
            })
        );
    }

    Update$(item: DictionaryItem): Observable<DictionaryItem> {
        const options = {
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            }),
        };

        return this.http
            .put(
                environment.api_base_url + "items",
                JSON.stringify(item),
                options
            )
            .pipe(
                map((data) => data["data"]),
                catchError((resp: HttpErrorResponse) => {
                    throw new Error(resp.error?.error ?? resp.statusText);
                })
            );
    }
}
