import {Component, OnInit} from "@angular/core";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ModalController} from "@ionic/angular";
import {CreateRoomsModalPage} from "../CreateRooms/CreateRooms";
import {RoomModelDto} from "../Models/RoomModel";
import {ClientWantsRoomList} from "../Models/ClientWantsRoomList";

@Component({
  template:
    `


      <body style="display: flex; justify-content: center; flex-direction: column;">

            <div style="display: flex; flex-direction: column;" >
              <div style="display: flex; justify-content: flex-end; margin-top: 1%">
                <ion-button (click)="openCreateNewRoom()" >
                  <ion-icon name="add-outline"></ion-icon>
                </ion-button>
              </div>

            </div>
            <ion-content style="display: flex; align-content: center;
                     justify-content: center;" #textWindow id="Textcontainer"
                         [scrollEvents]="true">

              <ion-card style="display: flex; min-width: 400px; width: 70%; min-height: 200px; height: 40%; justify-content: space-around; flex-direction: row;"
                        *ngFor="let room of this.ws.roomList">

                <ion-card-content style="display: flex; flex-grow: 7; height: 100%; flex-direction: column; justify-items: start;">
                <div style="font-size: xx-large">Room: {{room.name}}({{room.roomId}})</div>
                  <div style="width: 100%; height: 60%; flex-direction: column; flex-wrap: wrap">
                    <ion-item><ion-label style="width: 100%;">Temp: {{this.currentTemp}}</ion-label></ion-item>
                    <ion-item><ion-label style="width: 100%">Humidity: {{this.currentHum}}</ion-label></ion-item>
                    <ion-item><ion-label style="width: 100%">Air Quality:</ion-label></ion-item>
                  </div>
                  <div>
                    <ion-item  style="font-size: xx-large">Windows Status: {{this.windowStatus}}</ion-item>
                  </div>
                </ion-card-content>

                <ion-card-content style="flex-grow: 3; flex-wrap: wrap">
                  <div style="width: 100%; height: 15%; justify-content: flex-end">
                    <ion-button>
                      <ion-icon name="trash-outline"></ion-icon>
                    </ion-button>
                  </div>
                  <div style="width: 100%; height: 60%; flex-direction: column; flex-wrap: wrap">
                    <ion-title style="display: flex; text-align: center; width: 100%">Room Settings</ion-title>
                    <ion-item><ion-label style="width: 100%;">Temp: {{this.settingTemp}}</ion-label></ion-item>
                    <ion-item><ion-label style="width: 100%">Humidity: {{this.settingHum}}</ion-label></ion-item>
                    <ion-item><ion-label style="width: 100%">Air Quality: {{this.settingAq}}</ion-label></ion-item>
                  </div>
                  <div style="width: 100%; height: 30%; justify-content: center">
                    <ion-button style=" margin: 4%;  width: 35%; --border-radius: 25px;" >
                      <ion-icon name="play-outline"></ion-icon>
                    </ion-button>
                  </div>
                </ion-card-content>
              </ion-card>
            </ion-content>
      </body>
    `,
  styleUrls: ['ManageRooms.scss'],
})
export class ManageRoomsPage implements OnInit {


  public windowStatus: string = "Pending";
  public currentAq: string = "Pending";
  public currentTemp: string = "Pending";
  public currentHum: string = "Pending";
  public settingAq: string = "Pending";
  public settingTemp: string = "Pending";
  public settingHum: string = "Pending";

  constructor(protected ws: WebsocketClientService, private modalcontroller: ModalController){}

  ngOnInit(): void {
        this.setup();
    }

  openCreateNewRoom() {

    this.modalcontroller.create({
      component: CreateRoomsModalPage,
      componentProps: {
      }
    }).then(res => {
      res.present();
    })
  }
  setup(){
    this.requestRoomList();
  }
  async requestRoomList(){
    this.ws.socketConnection.sendDto(new ClientWantsRoomList({
      eventType: "ClientWantsRoomList",
    }))
  }


}
