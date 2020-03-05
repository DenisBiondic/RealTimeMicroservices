import { Component, OnInit } from '@angular/core';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { MessageService } from 'primeng/api';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [MessageService],
})
export class AppComponent implements OnInit {
  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    let connection = new HubConnectionBuilder()
      .withUrl(environment.backendUrl + "/hubs/notificationhub")
      .build();

    connection.on("NewNotification", (msg) => {
      console.log(`Notification recieved ${msg}`);
      this.messageService.add({ severity: 'warn', summary: 'Notification', detail: msg });
    });

    connection.start();
  }
}