import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";
import {IonContent} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ClientWantsAccountInfo} from "../Models/ClientWantsAccountInfo";


@Component({
  template: `
      <div style="flex-direction: column;">

          <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">
              <div style="flex: 1;"></div>
              <div style="display: flex; flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
                  <br>
                  <h1>Name : </h1>
                  <h1>Email : </h1>
                  <h1>City : </h1>
              </div>

              <br>

              <div style=" flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
                  <br>
                  <h1>{{ rename }}</h1>
                  <h1>{{ mail }}</h1>
                  <h1>{{city}}</h1>
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
                  <ion-button (click)="getUser()">
                      <ion-icon name="build-outline"></ion-icon>
                      <p> Change Settings </p>
                  </ion-button>
              </div>
              <div style="flex: 4;"></div>
          </div>
      </div>
  `,
  styleUrls: ['account.page.scss'],
})

export class AccountPage implements OnInit{
  rename? : string = "N/A";
  city? : string = "N/A";
  mail? : string = "N/A";


  constructor(public wsService:WebsocketClientService) {

  }


  ngOnInit(): void {
    this.getUser();

    this.rename = this.wsService.currentAccount?.realname;
    this.city = this.wsService.currentAccount?.city;
    this.mail = this.wsService.currentAccount?.email;

    //TODO Get account data
  }

  getUser() {
    this.wsService.socketConnection.sendDto(new ClientWantsAccountInfo)

    this.rename = this.wsService.currentAccount?.realname;
    this.city = this.wsService.currentAccount?.city;
    this.mail = this.wsService.currentAccount?.email;
  }




}
