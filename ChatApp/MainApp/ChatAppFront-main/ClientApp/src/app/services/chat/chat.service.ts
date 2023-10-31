import { HttpClient ,HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  // http://localhost:5050/api/Chat/GetMessages
  private baseUrl:string ="http://localhost:5050/api/Chat";
  public Username = new Subject<string>();
  public Message = new Subject<string>();
  MarkAsSeenChanged = new Subject<string>();
  // public Conversations = new Subject<any[]>();
  constructor(private http : HttpClient, private router:Router) { }

  // Get All Messages 
  viewMessages(userObj : string, otherObj:string):Observable<any>{
    return this.http.get<any>(`${this.baseUrl}/GetMessages?username=${userObj}&selusername=${otherObj}`);
  }
  //Mark all as Read Function
  MarkAsRead(username: string, seluserusername: string) {
    const params = new HttpParams()
      .set('username', username)
      .set('selusername', seluserusername);
    return this.http.post<any>(`${this.baseUrl}/markAsRead`, params);
  }
  
  // Send Messages 
  sendMessage(userObj:any,otherObj:any,messageObj:string,replyedto :any,Type:string):Observable<any>{

    return this.http.post<any>(`${this.baseUrl}/Addmsg`,{
      SenderId : userObj,
      ReceiverId : otherObj,
      Content : messageObj,
      ReplyedToId : replyedto,
      Type : Type
    });
  }
 
  sendReplyMessage(content:string,senderId:string,receiverId:string,type: string,replyedto: number,messageId: number){
    // const params = new HttpParams()
    //   .set('messageId', messageId)
    return this.http.post<any>(`${this.baseUrl}/Addmsg`,{
      SenderId : senderId,
      ReceiverId : receiverId,
      Content : content,
      ReplyedToId : replyedto,
      Type : type,
    },)
  }
  
  // Send File Messages 
  sendFileMessages(formData :FormData): Observable<any>{
      return this.http.post<any>(`${this.baseUrl}/AddFilemsg`,formData);
      
  }

  // Search bar 
  SearchOthers(userObj:string,otheruserObj:string){
     return this.http.get<any>(`${this.baseUrl}/SearchOthers?searchname=${otheruserObj}&username=${userObj}`);
  }

  // Recent Messages 
  RecentMessages(userObj:string):Observable<any>{
    return this.http.get<any>(`${this.baseUrl}/RecentMessages`,
    {
      params: {
        userName : userObj
      }
    });
  }

  // Get User id by Username 
  GetUserId(userObj:string):Observable<any>{
    return this.http.get<any>(`${this.baseUrl}/GetUserId`,
    {
      params: {
        userName : userObj
      }
    });
  }
  
  // function to get how long ago message was sent
  recentChatDate(date: Date): string {
    const recievedDate = new Date(date)
    const today = new Date();
    let curDate = recievedDate.getDate();
    let curMonth = recievedDate.getMonth();
    let curYear = recievedDate.getFullYear()
    if (
      curDate === today.getDate() &&
      curMonth === today.getMonth() &&
      curYear === today.getFullYear()
    ) {
      // Date is same as today, return time
      const hours = recievedDate.getHours().toString().padStart(2, '0');
      const minutes = recievedDate.getMinutes().toString().padStart(2, '0');
      return `${hours}:${minutes}`;
    } else {
      // Date is different from today, return full date
      return `${curDate}/${curMonth}/${curYear}`;
    }
  }

}
