import { Component, OnInit } from '@angular/core';
import { UserService, AuthenticationService } from '../_services';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
import { BarService } from '../_services/bar.service';

@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.less']
})
export class NewPasswordComponent implements OnInit {
    submitted = false;
    loading = false;
    password = '';
    secondPassword = '';

    constructor(private userService: UserService, private router: Router, private barService: BarService, private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
  }


    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.password.length < 10 || this.password != this.secondPassword) {
            return;
        }

        this.loading = true;
        this.userService.updatePassword(this.password)
            .pipe(first())
            .subscribe(
                data => {
                    this.barService.showInfo('Usplješno ste promijenili lozinku.');
                    this.loading = false;
                    this.authenticationService.logout();
                    location.reload(true);
                },
                error => {
                    this.barService.showError('Dogodila se greška. Molimo Vas pokušajte kasnije.\n Detalji: \n' + error);
                    this.loading = false;
                });
    }

}
