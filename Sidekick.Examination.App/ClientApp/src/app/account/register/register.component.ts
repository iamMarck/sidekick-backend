import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from '../../service/account.service';
import { AlertService } from '../../service/alert.service';
import { MustMatch } from '../../helpers/validator.service';


@Component({ templateUrl: 'register.component.html' })
export class RegisterComponent implements OnInit {
  form: FormGroup;
  formCode: FormGroup;
  loading = false;
  submitted = false;


  emailExist: boolean = null;
  validEmail: boolean = null;
  usernameExist: boolean = null;
  validUsername: boolean = null;

  step: number;

  constructor(
    private formBuilder: FormBuilder,
    private formRegisterBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.step = 1;

    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      displayName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      password2: ['', [Validators.required, Validators.minLength(6)]],
      email: ['', Validators.required],
    }, {
        validator: MustMatch('password', 'password2')
    });

    this.formCode = this.formRegisterBuilder.group({
      verificationCode: ['', Validators.required],
    })
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }
  get fCode() { return this.formCode.controls; }

  checkEmail() {
    this.accountService.emailCheckIfExist(this.f.email.value)
      .pipe(first())
      .subscribe({
        next: (responds) => {
          this.loading = false;
          console.log(responds);
          this.emailExist = !responds.available;
          this.validEmail = responds.available;
        },
        error: error => {
          this.alertService.error(error);
          this.loading = false;
        }
      });
  }

  checkUserName() {
    this.accountService.userNameCheckIfExist(this.f.username.value)
      .pipe(first())
      .subscribe({
        next: (responds) => {
          this.loading = false;
          console.log(responds);
          this.usernameExist = !responds.available;
          this.validUsername = responds.available;
        },
        error: error => {
          this.alertService.error(error);
          this.loading = false;
        }
      });
  }

  onVerification() {
    this.submitted = true;

    // reset alerts on submit
    this.alertService.clear();

    // stop here if form is invalid


    this.loading = true;
    //SEND VERIFICATION CODE

    this.accountService.emailVerification(this.f.email.value, this.f.username.value)
      .pipe(first())
      .subscribe({
        next: (responds) => {
          this.loading = false;
          console.log(responds);
          if (responds.success) {
            this.step = 2;
          } else {
            this.alertService.error("Please verify given email!");
          }

        },
        error: error => {
          this.alertService.error(error);
          this.loading = false;
        }
      });
  }

  backRegisterEntry() {
    this.step = 1;
  }

  onSubmit() {
    this.submitted = true;
    // reset alerts on submit
    this.alertService.clear();
    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }

    this.onVerification();
   
  }

  onRegister() {

    this.submitted = true;

    // reset alerts on submit
    this.alertService.clear();

    // stop here if form is invalid
    if (this.form.invalid && this.formCode.invalid) {
      return;
    }

    this.loading = true;
    this.accountService.register(this.form.value, this.fCode.verificationCode.value)
      .pipe(first())
      .subscribe({
        next: (responds) => {
          console.log(responds);
          if (responds.success) {
            this.alertService.success('Registration Successful!!', { keepAfterRouteChange: true });
            this.router.navigate(['../login'], { relativeTo: this.route });
          }
          else {
            this.alertService.error("Registration Failed!! Please validate your given code.");
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
