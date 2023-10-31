import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder,FormControl,FormGroup,Validator, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { Router, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { SignalRService } from '../services/signalr/signal-r.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,RouterModule,ReactiveFormsModule],
  providers:[AuthService,CookieService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit, OnDestroy {
  email!: string;
  password!: string;
  message!: string;
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  loginForm: FormGroup;
  loginObj: { Email: string; UserName: string; Password: string;  };
  constructor(
    private fb: FormBuilder,
    private auth:AuthService,
    private router :Router,
    private cookieService : CookieService,
    private signalRService :SignalRService
  ) {}
  
  ngOnInit() {
    
    this.auth.setShowToolbar(false);
    this.loginForm = this.fb.group({
      EmailAddress: ['', Validators.required],
      Username :['',[Validators.minLength(4),Validators.maxLength(10)] ],
      Password: ['', Validators.required],
     
    });
    

  }


  ngOnChanges()  {
    console.log("heyyyyyyy")
  }

  ngOnDestroy():void {
    console.log("des")
     this.auth.setShowToolbar(true);
  }
  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  onSubmit() {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
      this.auth.login(this.loginForm.value).subscribe(
         (res) => {
              this.cookieService.set('Authorization',`${res.token}`);
              this.auth.setUser(res.username);
              this.signalRService.startConnection(res.username);
              this.router.navigate(['/chat-page']);
      }
      
      );

     
    } else { 
     
      
      this.validateAllFormFields(this.loginForm);
      alert("Your form is invalid")
    }
  }
  private validateAllFormFields(formGroup: FormGroup)
  {
    Object.keys(formGroup.controls).forEach(field =>
   {
      const control = formGroup.get(field);
      if(control instanceof FormControl){
        control.markAsDirty({onlySelf:true});
      }
      else if(control instanceof FormGroup){
        this.validateAllFormFields(control)

      }
    })
  } 
}