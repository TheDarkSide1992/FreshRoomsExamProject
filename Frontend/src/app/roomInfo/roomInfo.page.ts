import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, NavigationStart, Router} from "@angular/router";
import {ModalController} from "@ionic/angular";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {RoomSensorSetPage} from "./RoomSensorSet.page";
import {ClientWantsRoomConfigurations} from "../Models/Client/ClientWantsRoomConfigurations";
import {ClientWantsDetailedRoom} from "../Models/Client/ClientWantsDetailedRoom";
import {ClientWantsToOpenOrCloseAllWindowsInRoom} from "../Models/Client/ClientWantsToOpenOrCloseAllWindowsInRoom";
import {MotorModel} from "../Models/objects/MotorModel";
import {ClientWantsToOpenOrCloseWindow} from "../Models/Client/ClientWantsToOpenOrCloseWindow";
import {ClientWantsToDisableOrEnableOneMotor} from "../Models/Client/ClientWantsToDisableOrEnableOneMotor";
import {
  ClientWantsToDisableOrEnableAllMotorsFromRoom
} from "../Models/Client/ClientWantsToDisableOrEnableAllMotorsFromRoom";
import {ClientWantsToLeaveRoom} from "../Models/Client/ClientWantsToLeaveRoom";


@Component({
  template: `
    <ion-content>
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
          <ion-content>
            <ion-toolbar>
              <ion-buttons>
                <ion-button *ngIf="!checkallIsOpen()" (click)="OpenAllWindows()">Open all windows</ion-button>
                <ion-button *ngIf="checkallIsOpen()" (click)="closeAllWindows()">Close all windows</ion-button>
                <ion-button *ngIf="checkallIsDisabled()" (click)="EnableAllWindows()">Enable all windows</ion-button>
                <ion-button *ngIf="!checkallIsDisabled()" (click)="DisableAllWindows()">Disable all windows</ion-button>
              </ion-buttons>
            </ion-toolbar>
            <br>
            <ion-card *ngFor="let m of this.wsService.currentRoom?.motors">


              <div *ngIf="m.isOpen">
                <ion-card-header>
                  <ion-title>Window: Open</ion-title>
                </ion-card-header>
                <ion-card-content style="display: flex; flex-direction: column; flex-wrap: wrap;">
                  <ion-button (click)="closeWindow(m)">
                    <ion-icon name="stopwatch-outline"></ion-icon>
                    Close
                  </ion-button>
                  <ion-button *ngIf="m.isDisabled" (click)="EnableWindow(m)">Enable</ion-button>
                  <ion-button *ngIf="!m.isDisabled" (click)="DisableWindow(m)">Disable</ion-button>
                </ion-card-content>
              </div>

              <div *ngIf="!m.isOpen">
                <ion-card-header>
                  <ion-title>Window: Closed</ion-title>
                </ion-card-header>
                <ion-card-content style="display: flex; flex-direction: column;">
                  <div>
                    <ion-button (click)="openWindow(m)">
                      <ion-icon name="stopwatch-outline"></ion-icon>
                      Open
                    </ion-button>
                    <ion-button *ngIf="m.isDisabled" (click)="EnableWindow(m)">Enable</ion-button>
                    <ion-button *ngIf="!m.isDisabled" (click)="DisableWindow(m)">Disable</ion-button>
                  </div>
                </ion-card-content>
              </div>
              <br>

            </ion-card>
          </ion-content>
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
    </ion-content>
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
        this.removeFromPreviousRoom();
        this.getRoomInfo();
        this.getConfig();
      };
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

  OpenAllWindows() {
    this.wsService.socketConnection.sendDto(new ClientWantsToOpenOrCloseAllWindowsInRoom({id : this.wsService.currentRoom?.roomId, open : true}))
  }

  closeAllWindows()
  {
    this.wsService.socketConnection.sendDto(new ClientWantsToOpenOrCloseAllWindowsInRoom({id : this.wsService.currentRoom?.roomId, open : false}))
  }

  openWindow(motor:MotorModel)
  {
    this.wsService.socketConnection.sendDto(new ClientWantsToOpenOrCloseWindow({roomId: this.wsService.currentRoom?.roomId, motor: motor, open: true}))
  }
  closeWindow(motor: MotorModel)
  {
    this.wsService.socketConnection.sendDto(new ClientWantsToOpenOrCloseWindow({roomId: this.wsService.currentRoom?.roomId, motor: motor, open: false}))
  }

  DisableWindow(motor:MotorModel) {
    this.wsService.socketConnection.sendDto(new ClientWantsToDisableOrEnableOneMotor({roomId: this.wsService.currentRoom?.roomId, motor: motor, disable: true}))
  }

  EnableWindow(motor: MotorModel) {
    this.wsService.socketConnection.sendDto(new ClientWantsToDisableOrEnableOneMotor({roomId: this.wsService.currentRoom?.roomId, motor: motor, disable: false}))
  }

  EnableAllWindows() {
    this.wsService.socketConnection.sendDto(new ClientWantsToDisableOrEnableAllMotorsFromRoom({roomId: this.wsService.currentRoom?.roomId, disable: false}))
  }

  DisableAllWindows() {
    this.wsService.socketConnection.sendDto(new ClientWantsToDisableOrEnableAllMotorsFromRoom({roomId: this.wsService.currentRoom?.roomId, disable: true}))
  }

 checkallIsOpen() {
    if(!this.wsService.currentRoom?.motors?.every(motor => motor.isOpen == true))
    {
      return false;
    }
    else
    {
      return true;
    }
  }

  checkallIsDisabled() {
    if(!this.wsService.currentRoom?.motors?.every(motor => motor.isDisabled == true))
    {
      return false;
    }
    else
    {
      return true;
    }
  }

  private removeFromPreviousRoom() {
    this.wsService.socketConnection.sendDto(new ClientWantsToLeaveRoom());
  }
}
