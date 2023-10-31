import { Component, HostListener, NgModule, ViewChild  ,ElementRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { ChatService } from 'src/app/services/chat/chat.service';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, NgbModule, FormsModule,RouterLink],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  constructor(private modalService: NgbModal, private chatService: ChatService, private authService : AuthService) { }
  // @ViewChild('basicModal') basicModal: any;
  username = localStorage.getItem('username');
  x=205;
  profileImageUrl = '../../assets/images/noPic.svg';
  private searchTerms = new Subject<string>();
  results : any[] ;
  isDropdownOpen = false;
  conversations = [] ;

  ngOnInit() : void{
     // Load the users who i have chatted before 
     this.chatService.RecentMessages(this.username).subscribe((data)=>{
        this.conversations = data;
        console.log(data);
     });

     this.chatService.Message.subscribe((data)=>{
         this.chatService.RecentMessages(data).subscribe((recent)=>{
            this.conversations = recent;
         })
     })

     this.searchTerms.pipe(
      debounceTime(500),
      distinctUntilChanged()
     ).subscribe(term =>{
      if (term === ""){
         this.chatService.RecentMessages(this.username).subscribe(data => {
          this.conversations = data;
         })  
      }
      this.chatService.SearchOthers(this.username,term).subscribe(data =>{
          this.conversations = data ;
          
      });
      
     })
  }
  
  recentChatDate(date :Date){
    return this.chatService.recentChatDate(date);
  }

  openBasicModal(content: any) {
    this.modalService.open(content, {}).result.then((result) => {
    }).catch((res) => { });
  }

  onClick(Username: string) {
    this.chatService.Username.next(Username);
  }
  
  
  
  search(term: Event): void {
    const event = (term.target as HTMLInputElement).value;
    this.searchTerms.next(event);
    console.log(event);
  }

  logout(){
    this.authService.logout();

  }
  MarkAllAsRead(){
    this.chatService.MarkAsRead(this.username,'All').subscribe();
    this.chatService.MarkAsSeenChanged.next("Changed");
  }
  
  MarkAsRead(e:Event , selusername:string){
    e.preventDefault();
    e.stopPropagation();
    this.chatService.MarkAsRead(this.username,selusername).subscribe((data)=>{
      console.log(data);
    });
    this.chatService.MarkAsSeenChanged.next("Changed.");
   }

}
