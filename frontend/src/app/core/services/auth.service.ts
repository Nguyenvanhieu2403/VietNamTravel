import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser$: Observable<any>;
  private isBrowser: boolean;

  constructor(
    private apiService: ApiService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    
    let initialUser = null;
    if (this.isBrowser) {
      const storedUser = localStorage.getItem('currentUser');
      if (storedUser) {
        try {
          initialUser = JSON.parse(storedUser);
        } catch {
          localStorage.removeItem('currentUser');
        }
      }
    }
    
    this.currentUserSubject = new BehaviorSubject<any>(initialUser);
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): any {
    return this.currentUserSubject.value;
  }

  public get token(): string | null {
    return this.currentUserValue?.accessToken || null;
  }

  login(credentials: any): Observable<any> {
    return this.apiService.post<any>('auth/login', credentials).pipe(
      map(user => {
        if (user && user.accessToken) {
          if (this.isBrowser) {
            localStorage.setItem('currentUser', JSON.stringify(user));
          }
          this.currentUserSubject.next(user);
        }
        return user;
      })
    );
  }

  register(userData: any): Observable<any> {
    return this.apiService.post<any>('auth/register', userData);
  }

  refreshToken(): Observable<any> {
    const payload = {
      accessToken: this.token,
      refreshToken: this.currentUserValue?.refreshToken || ''
    };

    return this.apiService.post<any>('auth/refresh-token', payload).pipe(
      map(user => {
        if (user && user.accessToken) {
          const updatedUser = { ...this.currentUserValue, ...user };
          if (this.isBrowser) {
            localStorage.setItem('currentUser', JSON.stringify(updatedUser));
          }
          this.currentUserSubject.next(updatedUser);
        }
        return user;
      }),
      catchError(err => {
        this.logout();
        return throwError(() => err);
      })
    );
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem('currentUser');
    }
    this.currentUserSubject.next(null);
  }

  hasRole(role: string): boolean {
    return this.currentUserValue?.role === role;
  }
}
