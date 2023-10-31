import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ViewProfile } from 'src/app/models/view-profile.model';
import { ChatComponent } from '../../layout/chat/chat.component';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';

@Component({
  selector: 'app-view-profile',
  standalone: true,
  imports: [CommonModule,ChatComponent, SidebarComponent],
  templateUrl: './view-profile.component.html',
  styleUrls: ['./view-profile.component.css']
})
export class ViewProfileComponent implements OnInit {
   host : string = "http://localhost:5050";
  currentUserName : string = localStorage.getItem('username');;
  constructor(private service:AuthService  ){
    
  }
  username : string ;
  profile : ViewProfile = {
    firstName: "",
    lastName: "",
    email: "",
    profileImageLocation : "",
    userName : ""
  } ;
  imageSource : string = "";
   ngOnInit(){
   this.service.viewProfile(this.currentUserName).subscribe(res=>{
    this.profile=res;
   });
    //this.service.getdetails(this.currentUserName);
    //this.profile = this.service.ProfileDetails;
  }
   
  
  
   
  showImage(){

    var imagePath = this.host + this.profile.profileImageLocation ; 
    console.log(imagePath);
    return imagePath;
  }
  
  
}
