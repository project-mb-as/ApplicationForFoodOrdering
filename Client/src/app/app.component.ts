import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './_services';
import { User } from './_models';
import { ROLES } from './globas';

@Component({ selector: 'app', templateUrl: 'app.component.html' })
export class AppComponent {
    currentUser: User;
    isAdmin: boolean;
    isCook: boolean;


    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUser = x;
            this.isAdmin = false;
            if (x) {
                this.isAdmin = x.roles.indexOf(ROLES.admin) != -1;
            }
            this.isCook = false;
            if (x) {
                this.isCook = x.roles.indexOf(ROLES.cook) != -1;
            }

        });
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}
