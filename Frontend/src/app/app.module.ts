import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {SidebarmenuPage} from "./SidebarMenu/sidebarmenu.page";
import {LoginPage} from "./LoginPage/Login.Page";
import {ManageRoomsPage} from "./ViewRooms/ManageRooms";
import {CreateRoomsModalPage} from "./CreateRooms/CreateRooms";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HomePage} from "./Home/Home.page";
import {RegisterAccountPage} from "./registerAccount/registerAccount.page";
import {AccountPage} from "./account/account.page";
import {ServerSendsDeviceTypes} from "./Models/ServerSendsDeviceTypes";
import {AccountSettingsPage} from "./account/account.settings.page";

@NgModule({
  declarations: [AppComponent,AccountPage, SidebarmenuPage, LoginPage,HomePage, RegisterAccountPage, AccountSettingsPage, ManageRoomsPage, CreateRoomsModalPage],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, ReactiveFormsModule, FormsModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
