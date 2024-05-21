import {Component, OnInit} from "@angular/core";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ModalController} from "@ionic/angular";
import {CreateRoomsModalPage} from "../CreateRooms/CreateRooms";
import {ClientWantsRoomList} from "../Models/ClientWantsRoomList";
import {ClientWantsToDeleteRoom} from "../Models/ClientWantsToDeleteRoom";
import {RoomModel} from "../Models/RoomModel";
import {ClientWantsBasicRoomStatus} from "../Models/ClientWantsBasicRoomStatus";
import {NavigationStart, Router} from "@angular/router";

@Component({
  template:
    `
      <body style="display: flex; justify-content: center; flex-direction: column;">
      <div style="display: flex; flex-direction: column;">
        <div style="display: flex; justify-content: flex-end; margin-top: 1%">
          <ion-button (click)="OpenCreateNewRoom()">
            <ion-icon name="add-outline"></ion-icon>
          </ion-button>
        </div>
      </div>

      <div
        style="height: 100%; display: flex; align-items: center; flex-direction: column; overflow-x: hidden;overflow-y: scroll;">

        <ion-card
          style="display: flex; min-height: 350px; max-height: 400px; min-width: 400px; width: 70%;  justify-content: space-around; flex-direction: row;"
          *ngFor="let room of this.ws.roomStatusList">

          <ion-card-content
            style="display: flex; flex-grow: 7; height: 100%; flex-direction: column; justify-items: start;">
            <div style="font-size: xx-large">Room: {{ room.roomName }}({{ room.roomId }})</div>

            <div style="width: 100%; height: 60%; flex-direction: column; flex-wrap: wrap">
              <ion-item>
                <ion-label style="width: 100%;">Temp: {{ room.basicCurrentTemp }}</ion-label>
              </ion-item>
              <ion-item>
                <ion-label style="width: 100%">Humidity: {{ room.basicCurrentHum }}</ion-label>
              </ion-item>
              <ion-item>
                <ion-label style="width: 100%">Air Quality: {{ room.basicCurrentAq }}</ion-label>
              </ion-item>
            </div>

            <div>
              <ion-item style="font-size: xx-large">Windows Status: {{ room.basicWindowStatus }}</ion-item>
            </div>
          </ion-card-content>

          <ion-card-content style="flex-grow: 3; flex-wrap: wrap">
            <div style="width: 100%; height: 15%; justify-content: flex-end">
              <ion-button (click)="DeleteRoom(room)">
                <ion-icon name="trash-outline"></ion-icon>
              </ion-button>
            </div>

            <div style="width: 100%; height: 60%; flex-direction: column; flex-wrap: wrap">
              <ion-title style="display: flex; text-align: center; width: 100%">Room Settings</ion-title>
              <ion-item>
                <ion-label style="width: 100%;">Temp: {{ room.basicTempSetting }}</ion-label>
              </ion-item>
              <ion-item>
                <ion-label style="width: 100%">Humidity: {{ room.basicHumSetting }}</ion-label>
              </ion-item>
              <ion-item>
                <ion-label style="width: 100%">Air Quality: {{ room.basicAqSetting }}</ion-label>
              </ion-item>
            </div>

            <div style="width: 100%; height: 30%; justify-content: center">
              <ion-button (click)="openRoom(room.roomId!)" style=" margin: 4%;  width: 35%; --border-radius: 25px;">
                <ion-icon name="play-outline"></ion-icon>
              </ion-button>
            </div>
          </ion-card-content>
        </ion-card>
      </div>
      </body>
    `,
  styleUrls: ['ManageRooms.scss'],
})
export class ManageRoomsPage implements OnInit {

  constructor(protected ws: WebsocketClientService, private modalcontroller: ModalController, private router: Router)
  {
    this.router.events.subscribe(event =>
    {
      if(event instanceof NavigationStart)
      {
        this.setup();
      }
    });
  }

  ngOnInit(): void {
        this.setup();
    }

  OpenCreateNewRoom() {

    this.modalcontroller.create({
      component: CreateRoomsModalPage,
      componentProps: {
      }
    }).then(res => {
      res.present();
    })
  }
  setup(){
    this.GetBasicRoomStatus();
  }

  async DeleteRoom(room:RoomModel){
    if (room != undefined){
      this.ws.socketConnection.sendDto(new ClientWantsToDeleteRoom({
        eventType: "ClientWantsToDeleteRoom",
        roomId: room.roomId,
        roomName: room.name,
      }))
      this.ws.roomStatusList.splice(this.ws.roomStatusList.indexOf(room), 1)
    }
  }

  async GetBasicRoomStatus() {
    this.ws.socketConnection.sendDto(new ClientWantsBasicRoomStatus({
      eventType: "ClientWantsBasicRoomStatusDto",
    }))
  }

  openRoom(roomdId:number){
    this.router.navigate(["RoomInfo/"+roomdId])
  }


}
