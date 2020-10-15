import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ResponseBody } from "./responses";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import User from "../models/user.model";
import { environment } from "../../environments/environment";
import { ToastrService } from "ngx-toastr";

@Injectable({
    providedIn: "root",
})
export class UsersService {
    constructor(private http: HttpClient, private toastr: ToastrService) {}

    GetList$(term: string = "", roleId: string = ""): Observable<User[]> {
        return this.http
            .get(
                environment.api_base_url +
                    `auth/users?term=${term}&roleId=${roleId}`
            )
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as User[];
                    }

                    this.toastr.error(error);
                    throw new Error(error);
                })
            );
    }

    Get$(id: string): Observable<User> {
        return this.http
            .get(environment.api_base_url + "auth/users/" + id)
            .pipe(
                map((responseBody: ResponseBody) => {
                    const { statusCode, data, error } = responseBody;
                    if (statusCode === 200) {
                        return data as User;
                    }

                    this.toastr.error(error);
                    throw new Error(error);
                })
            );
    }
}
