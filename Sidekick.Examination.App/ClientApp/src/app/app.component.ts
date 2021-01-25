import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './model/user';
import { AccountService } from './service/account.service';


@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
  user: User;

  constructor(private accountService: AccountService, private router: Router,) {
    this.accountService.user.subscribe(x => this.user = x);
  }

  logout() {
    sessionStorage.clear();
    sessionStorage.removeItem('user');
    this.router.navigate(['/account/login']);
    this.accountService.logout();
  }
}
