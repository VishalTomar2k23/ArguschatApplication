import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { ViewProfile } from '../../models/view-profile.model';
import { EditProfile } from '../../models/edit-profile';
import { Login } from 'src/app/models/login.model';
import { CookieService } from 'ngx-cookie-service';
import { SignalRService } from '../signalr/signal-r.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl:string ="http://localhost:5050/api/Account"
  private showToolbar: boolean = true ;
  $user = new BehaviorSubject<any | undefined>(undefined);
  public ProfileDetails : ViewProfile ={
    firstName: '',
    lastName: '',
    email: '',
    userName: '',
    profileImageLocation: ''
  };
  setShowToolbar(show : boolean): void {
    this.showToolbar =show;
  }
    
  getShowToolbar(): boolean{
    return this.showToolbar;
  }

  constructor(private http : HttpClient, private router:Router, private cookieService: CookieService,private signalRService : SignalRService) { }
  

  signUp(userObj:any):Observable<any>{
    return this.http.post<any>(`${this.baseUrl}/Register`,userObj);
  }
  
  login(loginObj:any):Observable<Login>{
    return this.http.post<Login>(`${this.baseUrl}/Login`,loginObj);
  }
  
  viewProfile(profileObj : string):Observable<ViewProfile>{
    
    return this.http.get<any>(`${this.baseUrl}/get-user?UserName=${profileObj}`);
  }

  editProfile(profileObj : string, updateData :FormData){
    const params = new HttpParams().set('UserName', profileObj);

    return this.http.post<any>(`${this.baseUrl}/update-user`, updateData, { params });

  }
  
  getdetails(username : string){
    this.viewProfile(username).subscribe((next =>{
      this.ProfileDetails = next;
      console.log(this.ProfileDetails);
    }))
  }

  setUser(user: any): void{
    // 
    this.$user.next(user);
    localStorage.setItem('username',`${user}`);
  }
  
  user(): Observable<any| undefined>{
    return this.$user.asObservable();
  }
  
  getUser(): any{
    const username = localStorage.getItem('username');
    if(username){
      return username;
    }
    return undefined;
  }

  logout(): void{
    localStorage.clear();
    this.cookieService.delete('Authorization', '/');
    this.router.navigate(['/login']);
    this.$user.next(undefined);
    this.signalRService.hubConnection?.stop().catch(error=>{console.log(error)});
  }

}

