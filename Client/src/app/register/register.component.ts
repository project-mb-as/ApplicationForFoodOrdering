import { Component, OnInit, ViewChild } from '@angular/core';
import { first } from 'rxjs/operators';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthenticationService, UserService } from '../_services';
import { Router, ActivatedRoute } from '@angular/router';
import { ROLES } from '../globas';
import { User } from '../_models';
import { MatDialog } from '@angular/material';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';
import { BarService } from '../_services/bar.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.less']
})
export class RegisterComponent implements OnInit {
    registerForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    roles: string[];
    error = '';
    qpSupcription: Subscription;
    users: Array<User>;
    activatedUsers: number;
    userFilter: string = "";


    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private dialog: MatDialog,
        private barService: BarService
    ) {

    }

    ngOnInit() {
        this.roles = Object.keys(ROLES).map(key => ROLES[key]);
        this.registerForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            role: [ROLES.member, Validators.required]
        });
        this.getUsers();


    }

    getUsers() {
        this.loading = true;
        this.userService.getAll().pipe(first()).subscribe(users => {
            this.loading = false;
            this.users = users;
            this.activatedUsers = users.filter(o => o.activated).length;
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.registerForm.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.registerForm.invalid) {
            return;
        }

        this.loading = true;
        this.authenticationService.register(this.registerForm.value.email, this.registerForm.value.role)
            .pipe(first())
            .subscribe(
                data => {
                    this.barService.showInfo(data.message);
                    this.loading = false;
                    this.getUsers();
                    this.registerForm.reset();
                },
                error => {
                    this.error = error;
                    this.barService.showError(error);
                    this.loading = false;
                });
    }

    resetPassword(user: User) {
        this.loading = true;
        this.userService.resetPassword(user.userId).subscribe(
            data => {
                this.barService.showInfo(data.message);
                this.loading = false;
                this.getUsers();
            },
            error => {
                this.error = error;
                this.barService.showError(error);
                this.loading = false;
            })
    }

    delete(user: User) {
        console.log(user);
        const dialogRef = this.dialog.open(DeleteDialogComponent, {
            width: '500px',
            height: '200px',
            disableClose: true,
            data: {
                message: `Da li ste sigurni da želite obrisati korisnika "${user.email}"?`
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.userService.delete(user.userId).subscribe(
                    () => {
                        this.barService.showInfo(`Uspješno ste obrisali korisnika "${user.email}".`);
                        this.getUsers();
                    },
                    (error) => this.barService.showError(`Greška prilikom brisanja korisnika "${user.email}".\n Greška: ${error}`)
                );
            }
        });
    }

    ngOnDestroy() {
        //this.qpSupcription.unsubscribe();
    }
}
