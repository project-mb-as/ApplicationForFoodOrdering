import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Observable, Observer, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { BarService } from './bar.service';
//import { NotifierService } from 'angular-notifier';
//import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient, private barService: BarService) { }

  /**
    * Perform http get without params. If you need params use function get
    *
    * @param {string} url    URL
    */
  getAll(url: string): Observable<HttpResponse<Object>> {
    return this.http.get(environment.apiUrl + url, { observe: 'response' })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }


  /**
     * Perform http get and if handleError=true then hendel errors
     *
     * @param {string} url    URL
     * @param {HttpParams} params Get-params
     */
  get(url: string, params: HttpParams): Observable<HttpResponse<Object>> {
    return this.http.get(environment.apiUrl + url, { observe: 'response', params: params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  post(url: string, data: any): Observable<Object> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      };
    return this.http.post(environment.apiUrl + url, data, httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  postDataAndGetFile(url: string, data: any, format: string): Observable<Object> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      responseType: 'blob' as 'blob'
    };
    return this.http.post(environment.apiUrl + url, {
      "FormData": JSON.stringify(data), "FileFormat": format
    }, httpOptions)
      .pipe(
        catchError(this.handleError)
      );
  }

  getFile(url: string, params: HttpParams): Observable<Object> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      responseType: 'blob' as 'blob'
    };
    return this.http.get(environment.apiUrl + url + '?' + params, httpOptions)
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  ///////////////////////////////////////////////////////////
  //                ERORE HANDLERS                         //
  ///////////////////////////////////////////////////////////

  private handleError = (error: HttpErrorResponse) => {
    //this.translate.get('ServerError')
    //    .subscribe((data: string) => {
    //        this.notifier.notify('error', data + '  - - -  ' + error.statusText);
    //    });


    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An client-side error occurred:', error.error.message);
      this.barService.showError('An client - side error occurred: \n' + error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      let errorMessage = `An server - side error occurred: Backend returned code "${error.status}", ` + `body was "${error.error}"`;
        console.error(error);
        this.barService.showError('' + error);
    }
    // return an observable with a user-facing error message
      return throwError(error);
  };

  private handlePostError(error: HttpErrorResponse) {
    //this.translate.get('ServerError')
    //    .subscribe((data: string) => {
    //        this.notifier.notify('error', data);
    //    });
    return Observable.throw('Server Error');
  }
}
