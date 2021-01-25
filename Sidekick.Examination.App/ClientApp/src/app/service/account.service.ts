import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';


import { User } from '../model/user';
import { environment } from '../../environments/environment';
import { WebsocketService } from './websocket.service';


@Injectable({ providedIn: 'root' })
export class AccountService {
  private userSubject: BehaviorSubject<User>;
  public user: Observable<User>;


  constructor(
    private router: Router,
    private http: HttpClient,
    private websocket: WebsocketService
  ) {
    this.userSubject = new BehaviorSubject<User>(JSON.parse(sessionStorage.getItem('user')));
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): User {
    this.userSubject = new BehaviorSubject<User>(JSON.parse(sessionStorage.getItem('user')));
    return this.userSubject.value;
  }

  loginSalt(username): Observable<any> {

    let loginSaltCommand = { command: "loginSalt", username: username };

    return this.websocket.send(loginSaltCommand)
      .pipe(map(data => {

        //login salt responds
        console.log(data);
        var jsonData = JSON.parse(data.JsonData);
        return jsonData;

      }));

  }

  logout() {
    sessionStorage.clear();
    sessionStorage.removeItem('user');
    this.router.navigate(['/account/login']);
    window.location.reload();
  }

  login(username, password, salt): Observable<any> {
    let hassedPass = this.websocket.jsSHA_hass(password, username);
    let hmac = this.websocket.hash_hmac(hassedPass, environment.secretKey);
    let loginChallenge = this.websocket.jsSHA_hass(salt, hmac);

    let loginCommand = { command: "login", usernameOrEmail: username, challenge: loginChallenge };
    return this.websocket.send(loginCommand)
      .pipe(map(data => {
        console.log(data);
        var jsonData = JSON.parse(data.JsonData);

        if (jsonData.success) {
          var user = new User();
          user.username = username;
          user.sessionId = jsonData.sessionId;
          sessionStorage.setItem('user', JSON.stringify(user));
          this.userSubject.next(user);
        }

        return jsonData;
      }));
  }


  emailCheckIfExist(email) {
    let command = { command: "checkEmail", email: email };
    return this.websocket.send(command)
      .pipe(map(data => {
        return JSON.parse(data.JsonData);
      }));
  }

  userNameCheckIfExist(username) {
    let command = { command: "checkUsername", username: username };
    return this.websocket.send(command)
      .pipe(map(data => {
        return JSON.parse(data.JsonData);
      }));
  }

  emailVerification(email, username) {
    let command = { command: "emailVerification", email: email, username: username };
    return this.websocket.send(command)
      .pipe(map(data => {
        return JSON.parse(data.JsonData);
      }));
  }

  register(user: User, code: string) {
    //return this.http.post(`${environment.apiUrl}/users/register`, user);
    let hassedPass = this.websocket.jsSHA_hass(user.password, user.username);
    let hassedPass2 = this.websocket.jsSHA_hass(user.password2, user.username);

    let registerCommand = {
      command: "register",
      username: user.username,
      displayName: user.displayName,
      password: hassedPass,
      password2: hassedPass2,
      email: user.email,
      verificationCode: code
    };

    return this.websocket.send(registerCommand)
      .pipe(map(saltData => {

        console.log(saltData);
        var jsonData = JSON.parse(saltData.JsonData);
        return jsonData;
      }));

  }




}
