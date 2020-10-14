import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import DictionaryItem from "../models/dictionary-item.model";
import { environment } from "../../environments/environment";
import { ResponseBody } from "./responses";
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: "root",
})
export class DictionaryService {
    constructor(private http: HttpClient, private toastr: ToastrService) {}

    GetList$(culture: string): Observable<DictionaryItem[]> {
        const options = {
            headers: new HttpHeaders({ "API-Culture": culture }),
        };

        return this.http
            .get(environment.api_base_url + "dictionary/items", options)
            .pipe(
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
                environment.api_base_url + "dictionary/items",
                JSON.stringify(item),
                options
            )
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        this.toastr.success("Successfully added a dictionary value.")
                        return data as DictionaryItem;
                    }

                    this.toastr.error(error)
                    throw new Error();
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
                environment.api_base_url + "dictionary/items",
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

    Delete$(id: string): Observable<any> {
        return this.http
            .delete(environment.api_base_url + "dictionary/items/" + id)
            .pipe(
                map((responseBody: any) => {
                    if (!responseBody || responseBody.statusCode === 200) {
                        return null;
                    }

                    throw new Error(responseBody.error);
                })
            );
    }
}
