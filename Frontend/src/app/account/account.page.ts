import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {IonContent} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ClientWantsAccountInfo} from "../Models/ClientWantsAccountInfo";


@Component({
  template: `
    <div style="flex-direction: column;">
      <ion-toolbar>
        <ion-title mode="ios">
          ACCOUNT
        </ion-title>
      </ion-toolbar>

      <br>

      <ion-card>
        <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">
          <div style="flex: 1;"></div>
          <div style="display: flex; flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
            <br>
            <ion-item>
              <h1>Name : </h1>
            </ion-item>
            <ion-item>
              <h1>Email : </h1>
            </ion-item>
            <ion-item>
              <h1>City : </h1>
            </ion-item>
          </div>

          <br>

          <div style=" flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
            <br>
            <ion-item>
              <h1>{{ this.wsService.currentAccount?.realname }}</h1>
            </ion-item>
            <ion-item>
              <h1>{{ this.wsService.currentAccount?.email }}</h1>
            </ion-item>
            <ion-item>
              <h1>{{this.wsService.currentAccount?.city}}</h1>
            </ion-item>
          </div>
          <div style="flex: 4;"></div>
        </div>

        <div style="height: 15px"></div>

        <div style=" flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">

          <div style="flex: 1;"></div>
          <div style=" flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
            <br>
            <h2>Change settings</h2>
          </div>

          <div style="flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
            <br>
            <ion-button (click)="changeSettings()">
              <ion-icon name="build-outline"></ion-icon>
              <p> Change Settings </p>
            </ion-button>
          </div>
          <div style="flex: 4;"></div>
        </div>
      </ion-card>
    </div>
  `,
  styleUrls: ['account.page.scss'],
})

export class AccountPage implements OnInit {

  constructor(public wsService: WebsocketClientService, private readonly router: Router,) {

  }


  ngOnInit(): void {
    if (localStorage.getItem('jwt') !== '' && localStorage.getItem('jwt') !== undefined) {

      setTimeout(() => {
        this.getUser()

      }, 2000)
    }
  }

  async getUser() {
    await this.wsService.socketConnection.sendDto(new ClientWantsAccountInfo)
  }

  changeSettings() {
    //TODO implement later
    this.router.navigate(["account/settings"])
  }


}
