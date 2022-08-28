import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { GooglePlaceModule } from "ngx-google-places-autocomplete";
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxPaginationModule } from 'ngx-pagination';
import { ToastrModule } from 'ngx-toastr';
import { CountdownModule } from 'ngx-countdown';

import { OAuthModule } from 'angular-oauth2-oidc';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationBarComponent } from './components/navigation-bar/navigation-bar.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { UserComponent } from './components/user/user.component';
import { DashboardComponent } from './components/home/dashboard/dashboard.component';
import { ProductListComponent } from './components/home/dashboard/product/product-list/product-list.component';
import { ProductCardComponent } from './components/home/dashboard/product/product-card/product-card.component';
import { ShowUserProfileComponent } from './components/home/dashboard/show-user-profile/show-user-profile.component';
import { ChangeUserProfileComponent } from './components/home/user-profile/change-user-profile/change-user-profile.component';
import { CurrentOrderComponent } from './components/home/dashboard/current-order/current-order.component';
import { CartCardComponent } from './components/home/dashboard/cart-card/cart-card.component';
import { CartComponent } from './components/home/cart/cart.component';
import { HomeComponent } from './components/home/home.component';
import { HasRoleDirective } from './components/directives/has-role.directive';
import { OrderListComponent } from './components/home/dashboard/order-list/order-list.component';
import { DeliverersListComponent } from './components/home/dashboard/deliverers-list/deliverers-list.component';
import { AddProductComponent } from './components/home/dashboard/add-product/add-product.component';
import { AllOrdersComponent } from './components/home/dashboard/all-orders/all-orders.component';
import { StatusCardComponent } from './components/home/dashboard/status-card/status-card.component';
import { OrderHistoryComponent } from './components/home/order-history/order-history.component';
import { PaypalButtonComponent } from './components/home/cart/paypal-button/paypal-button.component';
import { MapComponent } from './components/home/dashboard/map/map.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationBarComponent,
    UserComponent,
    LoginComponent,
    RegistrationComponent,
    DashboardComponent,
    ProductListComponent,
    ProductCardComponent,
    ShowUserProfileComponent,
    ChangeUserProfileComponent,
    CurrentOrderComponent,
    CartCardComponent,
    CartComponent,
    HomeComponent,
    HasRoleDirective,
    OrderListComponent,
    DeliverersListComponent,
    AddProductComponent,
    AllOrdersComponent,
    StatusCardComponent,
    OrderHistoryComponent,
    PaypalButtonComponent,
    MapComponent,
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    GooglePlaceModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    BrowserAnimationsModule,
    NgxPaginationModule,
    OAuthModule.forRoot(),
    ToastrModule.forRoot(),
    CountdownModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
