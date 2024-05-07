import { Component } from '@angular/core';
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ClientWantsToLogout} from "../Models/ClientWantsToLogout";

@Component({
  template: `<ion-app>
    <ion-split-pane contentId="main-content" >
      <ion-menu contentId="main-content" type="overlay" style="width: 100px;">
        <ion-content>
          <ion-list id="fresh-rooms-list">
            <ion-list-header>FreshRooms</ion-list-header>
            <ion-item>
              <ion-label routerLink="home">Home</ion-label>
            </ion-item>
            <ion-item>
              <ion-label routerLink="account">Account</ion-label>
            </ion-item>
            <ion-menu-toggle auto-hide="fales">
            </ion-menu-toggle>
          </ion-list>
        </ion-content>
        <ng-template #notLoggedin>
          <ion-button routerLink="/login">
            <ion-icon name="log-in-outline"></ion-icon>
            Login
          </ion-button>
          <ion-button routerLink="/register">Sign-up</ion-button>
        </ng-template>
        <ion-button (click)="logout()" style="--background: dimgray" *ngIf="localStorage.getItem('jwt') else notLoggedin">
          <ion-icon name="log-out-outline"></ion-icon>
          logout
        </ion-button>

      </ion-menu>
      <ion-router-outlet id="main-content"></ion-router-outlet>
    </ion-split-pane>
  </ion-app>`,
})
export class SidebarmenuPage {
  constructor(public websocketservice: WebsocketClientService) {}

  logout() {
    this.websocketservice.socketConnection.sendDto(new ClientWantsToLogout());
  }

  protected readonly localStorage = localStorage;
}
