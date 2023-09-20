import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthenticationService } from '@app/_services';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            let error = '';
            switch (err.status) {
                case 401: {
                    this.authenticationService.logout();
                    location.reload(true);
                    break;
                }
                case 403: {
                    error = 'Nemate pravo pristupa!';
                    break;
                }
                default: {
                    if (err.error) {
                        error = err.error.detail || err.error.message || err.statusText;
                    } else {
                        error = JSON.stringify(err);
                    }
                    break;
                }
            }
            //const error = err.error.detail || err.error.message || err.statusText;
            return throwError(error);
        }))
    }
}
