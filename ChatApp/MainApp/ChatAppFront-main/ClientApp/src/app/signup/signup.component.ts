import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, RouterModule,ReactiveFormsModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent{
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  signUpForm!: FormGroup;
  constructor(private fb : FormBuilder,private auth : AuthService, private router: Router){}
  ngOnInit() {
    this.auth.setShowToolbar(false);
    this.signUpForm = this.fb.group({
      Email:['', [Validators.required , Validators.email]],
      FirstName:['', Validators.required],
      LastName:['', Validators.required],
      UserName:['', Validators.required],
      Password:['', Validators.required]
    })
  }
  // public string FirstName { get; set; }
  //       [Required(ErrorMessage = "Last Name is Required.")]
  //       public string LastName { get; set; }
  //       [Required(ErrorMessage = "UserName is Required.")]
  //       public string UserName { get; set; }

  //       [Required(ErrorMessage = "Email is Required.")]
  //       [EmailAddress]
  //       public string Email { get; set; }

  //       [Required(ErrorMessage = "Password is Required.")]
  //       [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!@#$%^&*]{8,}$", ErrorMessage = "Password  is not valid.")]
  //       public string Password { get; set; }
  // 
  ngOnDestory():void {
    this.auth.setShowToolbar(true);
 }
  // private validateAllFormFields(formGroup: FormGroup){
  //   Object.keys(formGroup.controls).forEach(field =>{
  //     const control = formGroup.get(field);
  //     if(control instanceof FormControl){
  //       control.markAsDirty({onlySelf:true});
  //     }
  //     else if(control instanceof FormGroup){
  //       this.validateAllFormFields(control)

  //     }
  //   })
  // }
  
  onSignUp(){
    if (this.signUpForm.valid) {
      console.log(this.signUpForm.value);
      
      // alert("Your form valid");
      this.auth.signUp(this.signUpForm.value).subscribe({
        next: (res =>{
           alert(res.message)

        })
        ,error:(err =>{
          alert(err?.error.message)
        })
      })
      this.router.navigate(["/login"]);

    } else { 
     
      
      alert("Your form is invalid")
    }

    

  }
  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

}
