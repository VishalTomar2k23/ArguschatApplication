import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'; // import signalR
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  constructor(private cookieService: CookieService) {

  }

  token: string = this.cookieService.get("Authorization");
  hubConnection: signalR.HubConnection
  hubUrl: string = "http://localhost:5050/ChatHub";
  
  ngOnInit(): void {
    this.startConnection("dsf");
  }

  startConnection(userName: string){
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl,
        {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
        })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => {
        const value = userName;
        console.log('hub Connection Started ' + userName);
        console.log(JSON.stringify(userName));
        this.hubConnection.invoke("ConnectDone", userName).then((response)=> {
          console.log(response);
        }).catch((error) => { console.log(error) });

        console.log("exited the data.");

      }).catch(error => { console.log(error) });

  }
}
