import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment";
import { ResponseBody } from "./responses";
import SupportedCulture, {
    SupportedCultureCreate,
} from "../models/supported-culture.model";
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: "root",
})
export class SupportedCultureService {
    constructor(private http: HttpClient, private toastr: ToastrService) {}

    GetDropdownItems$(forceRefresh?: boolean): Observable<SupportedCulture[]> {
        const cacheKey = "SUPPORTED_CULTURES:DROPDOWN_ITEMS";

        if (!forceRefresh) {
            try {
                const cachedItemsData = JSON.parse(
                    localStorage.getItem(cacheKey)
                );
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

    GetAvailableDropdownItems$: Observable<SupportedCulture[]> = this.http
        .get(
            environment.api_base_url + "dictionary/cultures/dropdown/available"
        )
        .pipe(
            map((responseBody: ResponseBody) => {
                const { statusCode, data, error } = responseBody;
                if (statusCode === 200) {
                    return data as SupportedCulture[];
                }

                throw new Error(error);
            })
        );

    Create$(culture: SupportedCultureCreate): Observable<SupportedCulture> {
        const options = {
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            }),
        };

        return this.http
            .post(
                environment.api_base_url + "dictionary/cultures",
                JSON.stringify(culture),
                options
            )
            .pipe(
                map((response: ResponseBody) => {
                    const { statusCode, data, error } = response;
                    if (statusCode === 200) {
                        this.toastr.success("Successfully created a new supported culture.")
                        return data as SupportedCulture;
                    }

                    this.toastr.error(error)
                    throw new Error(error);
                })
            );
    }

    GetList$(term: string): Observable<SupportedCulture[]> {
        return this.http
            .get(environment.api_base_url + "dictionary/cultures?term=" + term)
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as SupportedCulture[];
                    }

                    throw new Error(error);
                })
            );
    }

    Get$(id: string): Observable<SupportedCulture> {
        return this.http
            .get(environment.api_base_url + "dictionary/cultures/" + id)
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as SupportedCulture;
                    }

                    throw new Error(error);
                })
            );
    }

    Update$(culture: SupportedCultureCreate): Observable<SupportedCulture> {
        const options = {
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            }),
        };

        return this.http
            .put(
                environment.api_base_url + "dictionary/cultures",
                JSON.stringify(culture),
                options
            )
            .pipe(
                map((response: ResponseBody) => {
                    const { statusCode, data, error } = response;
                    if (statusCode === 200) {
                        this.toastr.success("Successfully saved supported culture.")
                        return data as SupportedCulture;
                    }

                    throw new Error(error);
                })
            );
    }

    Delete$(id: string): Observable<any> {
        return this.http
            .delete(environment.api_base_url + "dictionary/cultures/" + id)
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        this.toastr.success("Successfully deleted supported culture.")
                        return data;
                    }

                    throw new Error(error);
                })
            );
    }

    SetAsDefault$(id: string): Observable<SupportedCulture> {
        return this.http
            .post(
                environment.api_base_url +
                    "dictionary/cultures/setAsDefault/" +
                    id,
                null
            )
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        this.toastr.success("Successfully set the supported culture as default.")
                        return data as SupportedCulture;
                    }

                    throw new Error(error);
                })
            );
    }
}
