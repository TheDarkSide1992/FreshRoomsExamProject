import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {SidebarmenuPage} from "./SidebarMenu/sidebarmenu.page";
import {LoginPage} from "./LoginPage/Login.Page";
import {HomePage} from "./Home/Home.page";
import {RegisterAccountPage} from "./registerAccount/registerAccount.page";
import {AccountPage} from "./account/account.page";
import {AccountSettingsPage} from "./account/account.settings.page";


const routes: Routes = [
  {
    path: '',
    component: SidebarmenuPage,
    children:
    [
      {
        path: 'home',
        component: HomePage,
      },
      {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full',
      },
    ]
  },
  {
    path: 'login',
    component: LoginPage,
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full',
  },
  {
    path: "register",
    component: RegisterAccountPage,
  },
  {
    path: "account",
    component: AccountPage,
  },
  {
    path: "account/settings",
    component: AccountSettingsPage,
  },
];
@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
