import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {SidebarmenuPage} from "./SidebarMenu/sidebarmenu.page";
import {LoginPage} from "./LoginPage/Login.Page";
import {ReactiveFormsModule} from "@angular/forms";
import {HomePage} from "./Home/Home.page";
import {RegisterAccountPage} from "./registerAccount/registerAccount.page";





@NgModule({
  declarations: [AppComponent, SidebarmenuPage, LoginPage, RegisterAccountPage,HomePage],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, ReactiveFormsModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
