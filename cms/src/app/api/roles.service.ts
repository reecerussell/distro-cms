import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ResponseBody } from "./responses";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Role } from "../models";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: "root",
})
export class RolesService {
    constructor(private http: HttpClient) {}

    GetList$(term: string = ""): Observable<Role[]> {
        return this.http
            .get(environment.api_base_url + "auth/roles?term=" + term)
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as Role[];
                    }

                    throw new Error(error);
                })
            );
    }

    Get$(id: string): Observable<Role> {
        return this.http
            .get(environment.api_base_url + "auth/roles/" + id)
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as Role;
                    }

                    throw new Error(error);
                })
            );
    }

    Create$(role: Role): Observable<Role> {
        const options = {
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            }),
        };

        return this.http
            .post(
                environment.api_base_url + "auth/roles",
                JSON.stringify(role),
                options
            )
            .pipe(
                map((response: ResponseBody) => {
                    const { statusCode, data, error } = response;
                    if (statusCode === 200) {
                        return data as Role;
                    }

                    throw new Error(error);
                })
            );
    }

    Update$(role: Role): Observable<Role> {
        const options = {
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            }),
        };

        return this.http
            .put(
                environment.api_base_url + "auth/roles",
                JSON.stringify(role),
                options
            )
            .pipe(
                map((response: ResponseBody) => {
                    const { statusCode, data, error } = response;
                    if (statusCode === 200) {
                        return data as Role;
                    }

                    throw new Error(error);
                })
            );
    }

    Delete$(id: string): Observable<any> {
        return this.http
            .delete(environment.api_base_url + "auth/roles/" + id)
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data;
                    }

                    throw new Error(error);
                })
            );
    }
}
