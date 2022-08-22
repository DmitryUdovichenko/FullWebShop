import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, of, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiHost;
  private currentUserSrc = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSrc.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  loadCurrentUser(token: string){
    if(token == null){
      this.currentUserSrc.next(null);
      return of(null);
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get(this.baseUrl+'account',{headers}).pipe(
      map((user: IUser) => {
        if(user){
          localStorage.setItem('token', user.token);
          this.currentUserSrc.next(user);
        }
      })
    );
  }

  login(values: any){
    return this.http.post(this.baseUrl+'account/login', values).pipe(
      map((user: IUser) => {
        if(user){
          localStorage.setItem('token', user.token);
          this.currentUserSrc.next(user);
        }
      })
    );
  }

  register(values: any){
    return this.http.post(this.baseUrl+'account/registration', values).pipe(
      map((user: IUser) => {
        if(user){
          localStorage.setItem('token',user.token);
          this.currentUserSrc.next(user);
        }
      })
    );
  }

  logout(){
    localStorage.removeItem('token');
    this.currentUserSrc.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExist(email: string){
    return this.http.get(this.baseUrl+'account/emailexists?email='+email);
  }
}
