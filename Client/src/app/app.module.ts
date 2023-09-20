import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import {NgxImageCompressService} from 'ngx-image-compress';

// used to create fake backend
import { fakeBackendProvider } from './_helpers';

import { AppComponent } from './app.component';
import { appRoutingModule } from './app.routing';

import { StarRatingModule } from 'angular-star-rating';

import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
    MatCardModule,
    MatFormFieldModule,
    MatGridListModule,
    MatRadioModule,
    MatDatepickerModule,
    MatExpansionModule,
    MatIconModule,
    MAT_DATE_LOCALE,
    MatCheckboxModule,
    MatSnackBarModule,
    MatDialogModule,
    MatDividerModule,
    MatListModule,
    MatSlideToggleModule,
    MatToolbarModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatSortModule

} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

import { BookcaseComponent } from './bookcase/bookcase.component';
import { MeniComponent } from './meni/meni.component';
import { NoviMeniComponent } from './novi-meni/novi-meni.component';
import { ErrorBarComponent } from './error-bar/error-bar.component';
import { DatePickerComponent } from './date-picker/date-picker.component';
import { CreateFoodDialogComponent } from './create-food-dialog/create-food-dialog.component';
import { InfoBarComponent } from './info-bar/info-bar.component';
import { WarningBarComponent } from './warning-bar/warning-bar.component';
import { RegisterComponent } from './register/register.component';
import { DeleteDialogComponent } from './delete-dialog/delete-dialog.component';
import { NewPasswordComponent } from './new-password/new-password.component';
import { OrdersReportComponent } from './orders-report/orders-report.component';
import { OptionsComponent } from './options/options.component';
import { CommentsComponent } from './comments/comments.component';


@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        FormsModule,
        HttpClientModule,
        appRoutingModule,
        BrowserAnimationsModule,
        MatCardModule,
        MatGridListModule,
        MatFormFieldModule,
        MatDatepickerModule,
        MatRadioModule,
        MatMomentDateModule,
        MatExpansionModule,
        MatIconModule,
        MatCheckboxModule,
        FlexLayoutModule,
        MatSnackBarModule,
        MatDialogModule,
        MatDividerModule,
        MatListModule,
        MatSlideToggleModule,
        MatToolbarModule,
        MatSelectModule,
        MatInputModule,
        MatButtonModule,
        MatTableModule,
        MatSortModule,
        StarRatingModule.forRoot()
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        BookcaseComponent,
        MeniComponent,
        NoviMeniComponent,
        ErrorBarComponent,
        DatePickerComponent,
        CreateFoodDialogComponent,
        InfoBarComponent,
        WarningBarComponent,
        RegisterComponent,
        DeleteDialogComponent,
        NewPasswordComponent,
        OrdersReportComponent,
        OptionsComponent,
        CommentsComponent

    ],
    providers: [
        NgxImageCompressService,
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: MAT_DATE_LOCALE, useValue: 'sr' },

        // provider used to create fake backend
        //fakeBackendProvider
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
