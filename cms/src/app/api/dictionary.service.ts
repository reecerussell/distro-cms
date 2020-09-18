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
import { ResponseBody } from "./responses";

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
            map((responseBody: ResponseBody) => {
                const { statusCode, data, error } = responseBody;
                if (statusCode === 200) {
                    return data as DictionaryItem[];
                }

                throw new Error(error);
            })
        );
    }

    Create$(item: DictionaryItem, culture: string): Observable<DictionaryItem> {
        const options = {
            headers: new HttpHeaders({
                "API-Culture": culture,
                "Content-Type": "application/json",
            }),
        };

        return this.http
            .post(
                environment.api_base_url + "items",
                JSON.stringify(item),
                options
            )
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as DictionaryItem;
                    }

                    throw new Error(error);
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
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as DictionaryItem;
                    }

                    throw new Error(error);
                })
            );
    }
}
