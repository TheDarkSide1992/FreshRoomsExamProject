import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, NavigationStart, Router} from "@angular/router";
import {ModalController} from "@ionic/angular";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {RoomSensorSetPage} from "./RoomSensorSet.page";
import {ClientWantsRoomConfigurations} from "../Models/ClientWantsRoomConfigurations";
import {ClientWantsDetailedRoom} from "../Models/ClientWantsDetailedRoom";


@Component({
  template: `
    <div style="flex-direction: column;">
      <ion-toolbar>
        <ion-title mode="ios">
          ROOM_PAGE : {{ this.wsService.currentRoom?.name }} ({{ roomId }})
        </ion-title>
      </ion-toolbar>

      <br>

      <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row; height: 250px;">
        <ion-card style="height: 50%; width: 30%;">
          Graph One
          <br>
          <br>
          <ion-title>Current temperature: {{ this.wsService.currenttemp?.toFixed(2) }}°C</ion-title>
        </ion-card>
        <ion-card style="height: 50%; width: 30%;">
          Graph Two
          <br>
          <br>
          <ion-title>Current Humidity: {{ this.wsService.currenthum?.toFixed(2) }}%</ion-title>
        </ion-card>
        <ion-card style="height: 50%; width: 30%;">
          Graph Three
          <br>
          <br>
          <ion-title>Current Air-Quality: {{ this.wsService.currentaq?.toFixed(2) }} ppm
          </ion-title>
        </ion-card>
      </div>


      <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">

        <div style="flex: 1;"></div>

        <div style="display: flex; flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
          <br>
          <ion-card *ngFor="let m of this.wsService.currentRoom?.motors">


              <div *ngIf="m.isOpen">
                <ion-card-header>
                  <ion-title>Window: Open</ion-title>
                </ion-card-header>
                <ion-card-content style="display: flex; flex-direction: column; flex-wrap: wrap;">
                <ion-button>
                  <ion-icon name="stopwatch-outline"></ion-icon>
                  Close
                </ion-button>
                </ion-card-content>
              </div>

              <div *ngIf="!m.isOpen">
                <ion-card-header>
                  <ion-title>Window: Closed</ion-title>
                </ion-card-header>
                <ion-card-content style="display: flex; flex-direction: column;">
                <div>
                <ion-button>
                  <ion-icon name="stopwatch-outline"></ion-icon>
                  Open
                </ion-button>
                </div>
                </ion-card-content>
              </div>
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
                <ion-title style="width: 45%;">{{ this.wsService.roomConfig?.minTemparature }} C</ion-title>
                <ion-title style="width: 45%;">{{ this.wsService.roomConfig?.maxTemparature }} C</ion-title>
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
                <ion-title style="width: 45%;">{{ this.wsService.roomConfig?.minHumidity }} %</ion-title>
                <ion-title style="width: 45%;">{{ this.wsService.roomConfig?.maxHumidity }} %</ion-title>
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
                  MIN air quality
                </ion-title>
                <ion-title style="width: max-content">
                  MAX air quality
                </ion-title>
              </div>
              <div style=" flex: 2; flex-wrap: wrap;
                    align-content: flex-start; flex-direction: row;">
                <ion-title style="width: 42%;">{{ this.wsService.roomConfig?.minAq }} PPM</ion-title>
                <ion-title style="width: 42%;">{{ this.wsService.roomConfig?.maxAq }} PPM</ion-title>
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

    this.route.params.subscribe(params => {
      const id = params['id'];
      this.roomId = parseInt(id);
    });

    this.router.events.subscribe(event =>
    {
      if(event instanceof NavigationStart)
      {
        this.getRoomInfo();
        this.getConfig();
      }
    });




  }

  displayname: string = "@displayname";
  roomId: number = -1;

  currentTemp: number = -1;
  currentHum: number = -1;
  currentAq: number = -1;



  ngOnInit(): void {

    if (localStorage.getItem('jwt') !== '' && localStorage.getItem('jwt') !== undefined) {

      setTimeout(() => {
        this.getConfig();
        this.getRoomInfo();
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

  getRoomInfo()
  {
    this.wsService.socketConnection.sendDto(new ClientWantsDetailedRoom({roomId : this.roomId}))
  }
}
