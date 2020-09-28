import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { ResponseBody } from "./responses";
import SupportedCulture from "../models/supported-culture.model";

@Injectable({
    providedIn: "root",
})
export class SupportedCultureService {
    constructor(private http: HttpClient) {}

    GetDropdownItems$(): Observable<SupportedCulture[]> {
        const cacheKey = "SUPPORTED_CULTURES:DROPDOWN_ITEMS";

        try {
            const cachedItemsData = JSON.parse(localStorage.getItem(cacheKey));
            const currentTime = new Date();
            if (
                cachedItemsData.lastUpdated >
                new Date().setMinutes(currentTime.getMinutes() - 5)
            ) {
                return of(cachedItemsData.items as SupportedCulture[]);
            }
        } catch (error) {
            console.error(
                "An error occured while reading the cached supported cultures dropdown data.",
                error
            );
        }

        return this.http
            .get(environment.api_base_url + "dictionary/cultures/dropdown")
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        const cultures = data as SupportedCulture[];

                        const cacheData = {
                            lastUpdated: new Date().getTime(),
                            items: cultures,
                        };
                        localStorage.setItem(
                            cacheKey,
                            JSON.stringify(cacheData)
                        );

                        return cultures;
                    }

                    throw new Error(error);
                })
            );
    }
}
