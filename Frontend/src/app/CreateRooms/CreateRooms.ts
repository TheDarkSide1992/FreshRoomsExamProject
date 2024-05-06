import {Component, OnInit} from "@angular/core";
import {ModalController} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {SensorModel} from "./SensorModel";


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
        </\ion-header>
        <div style="display: flex; flex-direction: column; align-items: center;" >
          <ion-input [formControl]="RName" style="width: 50%; margin-top: 5%" label-placement="floating"
                     label="Room Name" placeholder="Room Name">
          </ion-input>
          <div *ngIf="RName.invalid && RName.touched" class="error">
            Must enter a name
          </div>
          <div style="display: flex; justify-content: center" >Creater Sensor</div>
          <div style="display: flex; justify-content: center">
            <ion-input [formControl]="RSensorId" style=" margin-top: 5%" label-placement="floating"
                       label="Sensor Name" placeholder="Sensor Name"></ion-input></div>
          <div style="display: flex; flex-direction: row; justify-content: center">
              <ion-input [formControl]="RSensorId" style=" margin-top: 5%" label-placement="floating"
                         label="Sensor Id" placeholder="Sensor Id">

              </ion-input>
              <ion-button (click)="createTempSensor()" [disabled]="!RSensorId.valid" style="height: 20px">test</ion-button>



          </div>

          <div *ngIf="RSensorId.invalid && RSensorId.touched" class="error">
            Must enter a valid sensor id
          </div>
        </div>

        <ion-content style="display: flex; flex-direction: row; justify-content: center;"  #textWindow id="Textcontainer" [scrollEvents]="true">

          <ion-card style="display: flex; flex-direction: row; width: 50%" *ngFor="let sensor of this.messages">
            <ion-card-content>{{ sensor.sensorGuid}}</ion-card-content>
            <ion-button (click)="removeTempSensor(sensor.valueOf())">-</ion-button>


          </ion-card>
        </ion-content>








      <ion-button style="margin-bottom: 5%; margin-left: 5%; width: 25%">Create Room</ion-button>




    `,
})
export class CreateRoomsModalPage implements OnInit {

  RName = new FormControl("",[Validators.required,Validators.minLength(1),Validators.maxLength(100)]);

  RSensorId = new FormControl("",[Validators.required,Validators.minLength(36),Validators.maxLength(36)]);

  messages: Array<SensorModel> = [];

  constructor(private modalController: ModalController, protected ws: WebsocketClientService){}

  ngOnInit(): void {this.setup()}
  setup(){}

  dismissModal() {
    this.modalController.dismiss();
  }

  createTempSensor(){
    let tempsensor: SensorModel = {
      name: "",
      sensorGuid: this.RSensorId.value?.toString(),
    }
    this.messages.push(tempsensor);
  }

  removeTempSensor(sensor: Object){
    this.messages.splice(this.messages.indexOf(sensor), 1)
  }

  verifySensorId(){

  }

}
