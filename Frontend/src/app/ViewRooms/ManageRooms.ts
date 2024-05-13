import {Component, OnInit} from "@angular/core";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ModalController} from "@ionic/angular";
import {CreateRoomsModalPage} from "../CreateRooms/CreateRooms";
import {RoomModelDto} from "../Models/RoomModel";
import {ClientWantsRoomList} from "../Models/ClientWantsRoomList";

@Component({
  template:
    `


      <body>

            <div style="display: flex; flex-direction: column;" >
              <div style="display: flex; justify-items: start; margin-top: 1%">
                <ion-button (click)="openCreateNewRoom()" >
                  <ion-icon name="add-outline"></ion-icon>
                </ion-button>
                <ion-button>
                  <ion-icon name="remove-outline"></ion-icon>
                </ion-button>
              </div>

            </div>
            <ion-content style="display: flex; flex-wrap: wrap; align-content: center;
                     justify-content: space-evenly; flex-direction: column;" #textWindow id="Textcontainer"
                         [scrollEvents]="true">

              <ion-card style="display: flex; justify-content: space-around; flex-direction: row;"
                        *ngFor="let room of this.ws.roomList">
                <ion-card-content style="display: flex; justify-items: start; font-size: xx-large">Room: {{ room.roomId }}: {{room.name}}</ion-card-content>

              </ion-card>
            </ion-content>
      </body>
    `,
})
export class ManageRoomsPage implements OnInit {



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
