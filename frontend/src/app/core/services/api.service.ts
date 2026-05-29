import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { Observable, throwError, TimeoutError } from 'rxjs';
import { timeout, catchError, retry } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private readonly baseUrl: string;
  private readonly apiTimeout: number;
  private isBrowser: boolean;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) platformId: Object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.baseUrl = environment.apiUrl;
    this.apiTimeout = environment.apiTimeout;
  }

  get<T>(path: string, params: HttpParams = new HttpParams()): Observable<T> {
    return this.http
      .get<T>(`${this.baseUrl}/${path}`, { params })
      .pipe(timeout(this.apiTimeout), retry(1), catchError(this.handleError));
  }

  post<T>(path: string, body: any = {}): Observable<T> {
    return this.http
      .post<T>(`${this.baseUrl}/${path}`, body)
      .pipe(timeout(this.apiTimeout), catchError(this.handleError));
  }

  put<T>(path: string, body: any = {}): Observable<T> {
    return this.http
      .put<T>(`${this.baseUrl}/${path}`, body)
      .pipe(timeout(this.apiTimeout), catchError(this.handleError));
  }

  delete<T>(path: string): Observable<T> {
    return this.http
      .delete<T>(`${this.baseUrl}/${path}`)
      .pipe(timeout(this.apiTimeout), catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    let errorMessage = 'An error occurred';

    const isBrowserError = typeof ErrorEvent !== 'undefined' && error?.error instanceof ErrorEvent;

    if (error instanceof TimeoutError) {
      errorMessage = 'Request timeout - please try again';
    } else if (isBrowserError) {
      errorMessage = `Client Error: ${error.error.message}`;
    } else if (error.status) {
      errorMessage = `Server Error: ${error.status} - ${error.message}`;

      if (error.error?.message) {
        errorMessage = error.error.message;
      }
    }

    if (environment.enableDebugLogs) {
      console.error('API Error:', error);
    }

    return throwError(() => new Error(errorMessage));
  }
}
