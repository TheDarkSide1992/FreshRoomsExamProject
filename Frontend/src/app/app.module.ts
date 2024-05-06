import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppRoutingModule } from './app-routing.module';
import {RegisterAccountPage} from "./registerAccount/registerAccount.page";
import {ReactiveFormsModule} from "@angular/forms";
import { AppComponent } from './app.component';
import {SidebarmenuPage} from "./SidebarMenu/sidebarmenu.page";
import {LoginPage} from "./LoginPage/Login.Page";
import {ManageRoomsPage} from "./ViewRooms/ManageRooms";
import {CreateRoomsModalPage} from "./CreateRooms/CreateRooms";



@NgModule({
  declarations: [AppComponent, SidebarmenuPage, LoginPage, RegisterAccountPage, ManageRoomsPage, CreateRoomsModalPage],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, ReactiveFormsModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
