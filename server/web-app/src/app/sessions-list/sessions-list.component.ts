import { Component, OnInit } from '@angular/core';

import { SessionService } from '../session.service';

@Component({
  selector: 'app-sessions-list',
  templateUrl: './sessions-list.component.html',
  styleUrls: ['./sessions-list.component.css']
})
export class SessionsListComponent implements OnInit {

  sessions: any[];

  constructor(private sessionService: SessionService) { }

  ngOnInit() {
    if(this.sessionService.sessions == undefined) {
    //   this.sessionService.refresh(sessions => this.sessions = sessions);
      this.sessionService.refresh().then(sessions => this.sessions = sessions);
    }
    else {
      this.sessions = this.sessionService.sessions;
    }
  }

  refresh() {
    this.sessionService.refresh().then(sessions => this.sessions = sessions);
  }

//   getAllSessions() {
//     this.sessionService.getAllSessions()
//         .subscribe(sessions => this.sessions = sessions);
//   }
}
