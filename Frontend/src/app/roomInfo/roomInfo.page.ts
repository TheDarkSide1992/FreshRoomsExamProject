import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {IonContent, ModalController} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ClientWantsAccountInfo} from "../Models/ClientWantsAccountInfo";
import {RoomSensorSetPage} from "./RoomSensorSet.page";
import {ClientWantsRoomConfigurations} from "../Models/ClientWantsRoomConfigurations";


@Component({
  template: `
    <div style="flex-direction: column;">
      <ion-toolbar>
        <ion-title mode="ios">
          ROOM_PAGE : {{displayname}} ({{roomId}})
        </ion-title>
      </ion-toolbar>

      <br>

      <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row; height: 250px;">
        <ion-card style="height: 50%; width: 30%;">
          Graph One
          <br>
          <br>
          <ion-title>Current temperature: {{currentTemp}}</ion-title>
        </ion-card>
        <ion-card style="height: 50%; width: 30%;">
          Graph Two
          <br>
          <br>
          <ion-title>Current Humidity: {{currentHum}}</ion-title>
        </ion-card>
        <ion-card style="height: 50%; width: 30%;">
          Graph Three
          <br>
          <br>
          <ion-title>Current Air-Quality(CO<sub>2</sub> Level): {{currentAq}}</ion-title>
        </ion-card>
      </div>


      <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">

        <div style="flex: 1;"></div>

        <div style="display: flex; flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
          <br>
          <ion-card>
            <h1>WINDOWS : OPEN</h1>
            <br>
            <ion-button>
              <ion-icon name="stopwatch-outline"></ion-icon>
              Close
            </ion-button>

            <br>
          </ion-card>
        </div>

        <br>

        <div style=" flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
          <ion-title style="align-self: center">Prefrences</ion-title>
          <br>
          <ion-card>
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: column; width: 100%;">
              <div style=" flex: 2; flex-wrap: wrap;
                    align-content: flex-start; flex-direction: row; width: 100%;">
                <ion-title style="width: max-content">
                  MIN TEMPERATURE
                </ion-title>
                <ion-title style="width: max-content">
                  MAX TEMPERATURE
                </ion-title>
              </div>
              <div style=" flex: 2; flex-wrap: wrap;
                    align-content: flex-start; flex-direction: row;">
                <ion-title style="width: 45%;">{{this.wsService.roomConfig?.minTemparature}} C</ion-title>
                <ion-title style="width: 45%;">{{this.wsService.roomConfig?.maxTemparature}} C</ion-title>
              </div>
            </div>
          </ion-card>

          <br>
          <ion-card>
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: column; width: 100%;">
              <div style=" flex: 2; flex-wrap: wrap;
                    align-content: flex-start; flex-direction: row; width: 100%;">
                <ion-title style="width: max-content">
                  MIN HUMIDITY
                </ion-title>
                <ion-title style="width: max-content">
                  MAX HUMIDITY
                </ion-title>
              </div>
              <div style=" flex: 2; flex-wrap: wrap;
                    align-content: flex-start; flex-direction: row;">
                <ion-title style="width: 45%;">{{this.wsService.roomConfig?.minHumidity}} %</ion-title>
                <ion-title style="width: 45%;">{{this.wsService.roomConfig?.maxHumidity}} %</ion-title>
              </div>
            </div>
          </ion-card>

          <br>
          <ion-card>
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: column; width: 100%;">
              <div style=" flex: 2; flex-wrap: wrap;
                    align-content: flex-start; flex-direction: row; width: 100%;">
                <ion-title style="width: max-content">
                  MIN CO<sub>2</sub> Level
                </ion-title>
                <ion-title style="width: max-content">
                  MAX CO<sub>2</sub> Level
                </ion-title>
              </div>
              <div style=" flex: 2; flex-wrap: wrap;
                    align-content: flex-start; flex-direction: row;">
                <ion-title style="width: 42%;">{{this.wsService.roomConfig?.minAq}} PPM</ion-title>
                <ion-title style="width: 42%;">{{this.wsService.roomConfig?.maxAq}} PPM</ion-title>
              </div>
            </div>
          </ion-card>

          <ion-card>
            <ion-button style="align-self: center" (click)="openRoomSetSettings()">
              <ion-icon name="create-outline"></ion-icon>
              Edit
            </ion-button>
          </ion-card>
        </div>
        <div style="flex: 1;"></div>
      </div>
    </div>
  `,
  styleUrls: ['roomInfo.page.scss'],
})

export class RoomInfoPage implements OnInit {

  constructor(public wsService: WebsocketClientService, private modalcontroller: ModalController, public route: ActivatedRoute, private readonly router: Router,) {

  }

  displayname: string = "@displayname";
  roomId: number = -1;

  currentTemp: number = -1;
  currentHum: number = -1;
  currentAq: number = -1;



  ngOnInit(): void {


      this.route.params.subscribe(params => {
        const id = params['id'];
        const name = params['room_name'];
        this.roomId = parseInt(id);
        this.displayname = name;
      });

    if (localStorage.getItem('jwt') !== '' && localStorage.getItem('jwt') !== undefined) {

      setTimeout(() => {
        this.getConfig();
      }, 2000)
    }
  }

  async getConfig() {
    await this.wsService.socketConnection.sendDto(new ClientWantsRoomConfigurations({roomID:this.roomId}))
  }

  openRoomSetSettings() {
    this.modalcontroller.create({
      component: RoomSensorSetPage,
      componentProps: {
      }
    }).then(res => {
      res.present();
    })
  }
}
