import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { AuthGuard } from './_helpers';

import { BookcaseComponent } from './bookcase/bookcase.component';
import { MeniComponent } from './meni/meni.component';
import { NoviMeniComponent } from './novi-meni/novi-meni.component';
import { AdminGuard } from './_helpers/admin.guard';
import { RegisterComponent } from './register/register.component';
import { NewUserGuard } from './_helpers/newUser.guard';
import { NewPasswordComponent } from './new-password/new-password.component';
import { OrdersReportComponent } from './orders-report/orders-report.component';
import { OptionsComponent } from './options/options.component';
import { CookGuard } from './_helpers/cook.guard';

const routes: Routes = [
    { path: '', component: NoviMeniComponent, canActivate: [AuthGuard, NewUserGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'new-password', component: NewPasswordComponent, canActivate: [AuthGuard] },
    //{ path: 'meni', component: MeniComponent, canActivate: [AuthGuard, NewUserGuard] },
    //{ path: 'meni', component: NoviMeniComponent, canActivate: [AuthGuard, NewUserGuard] },
    { path: 'orders-report', component: OrdersReportComponent, canActivate: [CookGuard] },
    { path: 'options', component: OptionsComponent, canActivate: [AuthGuard, NewUserGuard] },
    { path: 'user-managment', component: RegisterComponent, canActivate: [AdminGuard] },

    // otherwise redirect to menu
    { path: '**', redirectTo: '' }
];

export const appRoutingModule = RouterModule.forRoot(routes);

