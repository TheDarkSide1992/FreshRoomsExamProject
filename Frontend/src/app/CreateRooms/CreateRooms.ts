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

                <div style="flex-direction: row;  align-content: space-evenly;">
                    <ion-input [formControl]="RSensorId" style=" margin-top: 5%" label-placement="floating"
                               label="Sensor Id" placeholder="Sensor Id">
                    </ion-input>

                    <ion-button (click)="createTempSensor()" [disabled]="!RSensorId.valid"
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

            <ion-card style="display: flex; flex-direction: row; width: 50%" *ngFor="let sensor of this.messages">
                <ion-card-content>{{ sensor.name }}</ion-card-content>
                <ion-button (click)="removeTempSensor()">-</ion-button>
            </ion-card>
        </ion-content>


        <ion-button style="margin-bottom: 5%; margin-left: 5%; width: 25%">Create Room</ion-button>



    `,
  styleUrls: ['CreateRooms.scss'],
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
      name: "Sensor: "+(this.messages.length+1),
      sensorGuid: this.RSensorId.value?.toString(),
    }
    console.log("test create sensor")
    this.messages.push(tempsensor);
  }
  removeTempSensor(){
    //this.messages.
  }

  verifySensorId(){

  }

}
