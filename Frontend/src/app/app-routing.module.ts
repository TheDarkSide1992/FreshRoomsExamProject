import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {SidebarmenuPage} from "./SidebarMenu/sidebarmenu.page";
import {LoginPage} from "./LoginPage/Login.Page";
import {RegisterAccountPage} from "./registerAccount/registerAccount.page";
import {ManageRoomsPage} from "./ViewRooms/ManageRooms";

const routes: Routes = [
  {
    path: '',
    component: SidebarmenuPage,
    children: [
      {
        path: 'managerooms',
        component: ManageRoomsPage,
      }

    ]
  },
  {
    path: 'login',
    component: LoginPage,
  },
  {
    path: "register",
    component: RegisterAccountPage,
  },
];
@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
