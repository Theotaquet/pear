import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { of } from 'rxjs/observable/of';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs/Rx';

@Injectable()
export class SessionService {

  private sessionsUrl = 'api/sessions';
  sessions: any[];

  constructor(private http: HttpClient) { }

  getAllSessions(): Observable<any[]> {
    return this.http.get<any[]>(this.sessionsUrl);
  }

  refresh() {
    return new Promise(resolve => {
      this.getAllSessions().subscribe(
          sessions => {
            this.sessions = sessions;
            resolve(this.sessions);
          }
      )
    });
  }

//   getSession(id: string): Observable<any> {
//     return this.http.get<any>(`${this.sessionsUrl}/${id}`);
//   }
}
