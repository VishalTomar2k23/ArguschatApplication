import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EditProfile } from 'src/app/models/edit-profile';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [CommonModule,FormsModule,NgbModule],
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent {
  loggedInUser : EditProfile = {
    firstName : "",
    lastName : "",
    userName : "",
    email : "", 
    ProfileImage : null
  };
  
  changepassword : {old:string,newp:string,verify:string} = {
    old:"",newp:"",verify:""
  };
  constructor( private service :AuthService, private modalService: NgbModal) { }

  ngOnInit(): void {
    
    this.loggedInUser = {
      ...this.service.ProfileDetails,
      ProfileImage : null 
    }

    this.changepassword={
      newp: '',
      old: '',
      verify:''
    };
  }

  onFileSelected(event: any) {
    if (event.target.files.length > 0) {
      this.loggedInUser.ProfileImage = (event.target as HTMLInputElement).files[0];
      console.log(this.loggedInUser.ProfileImage);
    }
  }

  onSubmit(){
    const formdata = new FormData();
    formdata.append('username',this.loggedInUser.userName);
    formdata.append('firstName' ,this.loggedInUser.firstName);
    formdata.append('lastName' ,this.loggedInUser.lastName);
    formdata.append('email' ,this.loggedInUser.email);
    formdata.append('profileImage',this.loggedInUser.ProfileImage);
    this.service.editProfile(this.loggedInUser.userName,formdata).subscribe((data)=>{
      alert("Profile Updated Successfully");
      this.loggedInUser = data;
    })
  }

  change(){
    
  }

  openBasicModal(content: any) {
    this.changepassword.newp='';
    this.changepassword.old='';
    this.changepassword.verify='';
    this.modalService.open(content, {}).result.then((result) => {
    }).catch((res) => {});
  }
}
