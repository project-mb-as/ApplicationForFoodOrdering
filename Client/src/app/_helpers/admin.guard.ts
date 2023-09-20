import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from '@app/_services';
import { ROLES } from '../globas';

@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let ret: boolean;
        const currentUser = this.authenticationService.currentUserValue;

        if (currentUser && currentUser.roles.indexOf(ROLES.admin) != -1) {
            ret = true;
        } else {
            ret = false;
        }
        //this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return ret;
    }
}
