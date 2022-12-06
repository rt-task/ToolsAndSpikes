import { environment } from './../environments/environment';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { User } from 'src/models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  users: User[] = [];

  constructor(private httpClient: HttpClient) {
  }

  getUsers() {
    return this.httpClient.get<User[]>(environment.apiurl).subscribe(
      (next) => this.users = next
    );
  }

  postUsers() {
    return this.httpClient.post(environment.apiurl, null).subscribe(
      (next) => this.getUsers()
    );
  }
}
