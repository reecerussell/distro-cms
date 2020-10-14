import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HTTP_INTERCEPTORS } from "@angular/common/http";
import { Observable } from "rxjs";
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class Interceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService){}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!navigator.onLine) {
        this.toastr.error("You are not connected to the internet!", "Oops!", {closeButton: true, disableTimeOut: true})

        throw new Error();
    }

    return next.handle(request);
  }
}

const Interceptors = [
  {provide: HTTP_INTERCEPTORS, useClass: Interceptor, multi: true}
]

export default Interceptors;