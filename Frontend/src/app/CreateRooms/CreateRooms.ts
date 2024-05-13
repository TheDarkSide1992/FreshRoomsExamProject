import {Component, OnInit} from "@angular/core";
import {ModalController} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {DeviceModel, SensorModelDto} from "../Models/DeviceModel";
import {ClientWantsSensorTypes} from "../Models/ClientWantsSensorTypes";
import {RoomModel} from "../Models/RoomModel";



@Component({
  template:
    `
      <ion-header>
        <ion-toolbar>
          <ion-title>Create Room</ion-title>
          <ion-buttons slot="end">
            <ion-button (click)="dismissModal()">Close</ion-button>
          </ion-buttons>
        </ion-toolbar>
      </ion-header>


      <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">

        <div style="flex: 1;"></div>


        <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: column;">

          <div>
            <ion-input [formControl]="RName" style=" margin-top: 5%" label-placement="floating"
                       label="Room Name" placeholder="Room Name">
            </ion-input>

          </div>

          <div *ngIf="RName.invalid && RName.touched" class="error">
            Must enter a name
          </div>


            <ion-item style="margin-top: 8%">
              <ion-select [formControl]="RSensorName" aria-label="DeviceType" interface="popover" placeholder="Select DeviceType"  >
                <ion-select-option  *ngFor="let tempDeviceType of this.ws.sensorTypeList"  value="{{tempDeviceType.deviceTypeName}}">{{tempDeviceType.deviceTypeName}}</ion-select-option>
              </ion-select>
            </ion-item>

          <div style="flex-direction: row;  align-content: space-evenly;">
            <ion-input [formControl]="RSensorId" label-placement="floating"
                       label="Sensor Id" placeholder="Sensor Id">
            </ion-input>

            <ion-button (click)="verifySensorId()" [disabled]="!(RSensorId.valid && RSensorName.valid)"
                        style=" margin-top: 10%; height: 20px">Create Sensor
            </ion-button>

          </div>

          <div *ngIf="RSensorId.invalid && RSensorId.touched" class="error">
            Must enter a valid sensor id
          </div>
        </div>
        <div style="flex: 1;"></div>

      </div>


      <ion-content style="display: flex; flex-wrap: wrap; align-content: center;
                     justify-content: space-evenly; flex-direction: column;" #textWindow id="Textcontainer"
                   [scrollEvents]="true">

        <ion-card style="display: flex; justify-content: space-around; flex-direction: row; background: transparent;"
                  *ngFor="let sensor of this.ws.sensorlist">
          <ion-card-content>{{ sensor.deviceTypeName }}: {{sensor.sensorGuid}}</ion-card-content>
          <ion-button (click)="removeTempSensor(sensor.valueOf())">
            <ion-icon name="remove-outline"></ion-icon>
          </ion-button>
        </ion-card>
      </ion-content>


      <ion-button (click)="createRoom()" [disabled]="!(this.ws.sensorlist.length>=1 && RName.valid)" style="margin-bottom: 5%; margin-left: 5%; width: 25%">Create Room</ion-button>



    `,
  styleUrls: ['CreateRooms.scss'],
})
export class CreateRoomsModalPage implements OnInit {

  RName = new FormControl("",[Validators.required,Validators.minLength(1),Validators.maxLength(100)]);

  RSensorName = new FormControl("",[Validators.required,Validators.minLength(1),Validators.maxLength(100)]);

  RSensorId = new FormControl("",[Validators.required,Validators.minLength(36),Validators.maxLength(36)]);




  constructor(private modalController: ModalController, protected ws: WebsocketClientService){}

  ngOnInit(): void {this.setup()}
  setup(){
    this.ws.sensorlist = [];
    this.ws.sensorTypeList = [];
    this.getDeviceTypes();
  }

  dismissModal() {
    this.modalController.dismiss();
  }


  removeTempSensor(sensor: Object){
    this.ws.sensorlist.splice(this.ws.sensorlist.indexOf(sensor), 1)
  }

  async getDeviceTypes(){
    this.ws.socketConnection.sendDto(new ClientWantsSensorTypes({
      eventType: "ClientWantsSensorTypes",
    }))
  }

  async verifySensorId(){

    this.ws.socketConnection.sendDto(new SensorModelDto({
      eventType: "ClientWantsToVerifySensor",
      deviceTypeName: this.RSensorName.value?.toString(),
      sensorGuid: this.RSensorId.value?.toString(),
    }))
  }

  async createRoom(){
    this.ws.socketConnection.sendDto(new RoomModel({
      eventType: "ClientWantsToCreateRoom",
      name: this.RName.value?.toString(),
      deviceList: this.ws.sensorlist,
    }))
    this.dismissModal();
  }
}
