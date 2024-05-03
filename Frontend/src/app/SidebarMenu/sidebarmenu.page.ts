import { Component } from '@angular/core';

@Component({
  template: `<ion-app>
    <ion-split-pane contentId="main-content">
      <ion-menu contentId="main-content" type="overlay">
        <ion-content>
          <ion-list id="fresh-rooms-list">
            <ion-list-header>FreshRooms</ion-list-header>

            <ion-menu-toggle auto-hide="false">

            </ion-menu-toggle>
          </ion-list>

          <ion-list>
            <ion-button routerLink="login">Login
              <ion-icon name="log-in-outline"></ion-icon>
            </ion-button>
          </ion-list>
        </ion-content>
      </ion-menu>
      <ion-router-outlet id="main-content"></ion-router-outlet>
    </ion-split-pane>
  </ion-app>`,
})
export class SidebarmenuPage {
  constructor() {}
}
