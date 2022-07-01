import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/user/login/login.component';

const routes: Routes = [
  {path:'', redirectTo:'/user/login',pathMatch:'full'},
  {
    path:'user',
    children:[
      { path: 'login',component: LoginComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
