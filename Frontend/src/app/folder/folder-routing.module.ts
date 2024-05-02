import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FolderPage } from './folder.page';
import {RegisterAccountPage} from "../registerAccount/registerAccount.page";

const routes: Routes = [
  {
    path: '',
    component: FolderPage
  },
  {
    path: 'register',
    component: RegisterAccountPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FolderPageRoutingModule {}
