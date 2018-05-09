import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { of } from 'rxjs/observable/of';
import { HttpClient } from '@angular/common/http';
import { Session } from './session';

@Injectable()
export class SessionService {

  private sessionsUrl = 'api/sessions';

  constructor(private http: HttpClient) { }

  getAllSessions(): Observable<any[]> {
    return this.http.get<any[]>(this.sessionsUrl);
  }

  getSession(id: string): Observable<any> {
    return this.http.get<any>(`${this.sessionsUrl}/${id}`);
  }
}
