import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {LoginPage} from "./Login/login.page";
import {RegisterAccountPage} from "./registerAccount/registerAccount.page";

const routes: Routes = [
  /*{
    path: '',
    redirectTo: 'folder/FreshRooms',
    pathMatch: 'full'
  },
  {
    path: 'folder/:id',
    loadChildren: () => import('./folder/folder.module').then( m => m.FolderPageModule)
  }*/
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
