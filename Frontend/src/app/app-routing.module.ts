import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {RegisterAccountPage} from "./registerAccount/registerAccount.page";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'folder/FreshRooms',
    pathMatch: 'full'
  },
  {
    path: "register",
    component: RegisterAccountPage,
  },
  {
    path: 'folder/:id',
    loadChildren: () => import('./folder/folder.module').then( m => m.FolderPageModule)
  },

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
