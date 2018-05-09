import { Component, OnInit } from '@angular/core';

import { Session } from '../session';
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
    this.sessionService.getAllSessions()
        .subscribe(sessions => this.sessions = sessions);
  }
}
