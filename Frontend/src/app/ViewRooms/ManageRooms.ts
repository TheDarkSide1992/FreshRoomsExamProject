import {Component} from "@angular/core";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ClientWantsToCreateUserDTO} from "../registerAccount/RegisterMessage.model";
import {ModalController} from "@ionic/angular";
import {CreateRoomsModalPage} from "../CreateRooms/CreateRooms";

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
      </body>
    `,
})
export class ManageRoomsPage {



  constructor(protected ws: WebsocketClientService, private modalcontroller: ModalController){}

  openCreateNewRoom() {

    this.modalcontroller.create({
      component: CreateRoomsModalPage,
      componentProps: {
      }
    }).then(res => {
      res.present();
    })
  }


}
