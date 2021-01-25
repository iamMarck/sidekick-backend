import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from '../../service/account.service';
import { AlertService } from '../../service/alert.service';


@Component({ templateUrl: 'login.component.html' })
export class LoginComponent implements OnInit {
  form: FormGroup;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    this.alertService.clear();

    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.accountService.loginSalt(this.f.username.value)
      .pipe(first())
      .subscribe({
        next: (responds) => {
          console.log(responds);
          this.login(this.f.username.value, this.f.password.value, responds.salt);
        },
        error: error => {
          this.alertService.error(error);
          this.loading = false;
        }
      });
  }

  login(username, password, salt) {
    this.accountService.login(username, password, salt)
      .pipe(first())
      .subscribe({
        next: (responds) => {
          console.log(responds);
          if (responds.success) {
            const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
            this.router.navigateByUrl(returnUrl);
          }
          else {
            this.alertService.error("Invalid login/password !!");
            this.loading = false;
          }
        },
        error: error => {
          this.alertService.error(error);
          this.loading = false;
        }
      });
  }
}
