import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './components/auth/auth.guard';
import { CartComponent } from './components/home/cart/cart.component';
import { DashboardComponent } from './components/home/dashboard/dashboard.component';
import { HomeComponent } from './components/home/home.component';
import { ChangeUserProfileComponent } from './components/home/user-profile/change-user-profile/change-user-profile.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { UserComponent } from './components/user/user.component';
import { OrderHistoryComponent } from './components/home/order-history/order-history.component';

const routes: Routes = [
  {path:'', redirectTo:'/user/login',pathMatch:'full'},
  {
    path:'user', component: UserComponent,
    children:[
      { path: 'login', component: LoginComponent},
      { path: 'registration', component: RegistrationComponent},
    ]
  },
  {path:'home', component:HomeComponent, canActivate: [AuthGuard] ,
   data:{
      role1: "Admin",
      role2: "Deliverer",
      role3: "NormalUser"
   },
   children: [
      { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard],
        data: {
          role1: "Admin",
          role2: "Deliverer",
          role3: "NormalUser"
        }
      },
      { path: 'cart', component: CartComponent, canActivate: [AuthGuard],
        data: {
          role3: "NormalUser"
        }
      },
      { path: 'profile', component: ChangeUserProfileComponent, canActivate: [AuthGuard],
        data: {
          role1: "Admin",
          role2: "Deliverer",
          role3: "NormalUser"
        }
      },
      { path: 'history', component: OrderHistoryComponent, canActivate: [AuthGuard],
        data:{
          role2: "Deliverer",
          role3: "NormalUser"
        }
      }
   ]

  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
