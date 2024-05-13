import {Component, OnInit} from "@angular/core";
import {ModalController} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";




@Component({
  template:
    `
      <ion-header>
        <ion-toolbar>
          <ion-title>Edit Room Snesativity</ion-title>
          <ion-buttons slot="end">
            <ion-button (click)="dismissModal()">Close</ion-button>
          </ion-buttons>
        </ion-toolbar>
      </ion-header>

      <div style=" flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
        <ion-title style="align-self: center">Make Your Changes</ion-title>
        <br>
        <ion-card>
          <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: column;">
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: row;">
              <ion-title>
                MIN TEMPERATURE
              </ion-title>
              <ion-title>
                MAX TEMPERATURE
              </ion-title>
            </div>
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: row; width: 100%;">
              <ion-input [formControl]="SensorTempMin" label-placement="floating" style="width: 45%;" label="{{minTemp}} C"></ion-input>
              <ion-input [formControl]="SensorTempMax" label-placement="floating" style="width: 45%;" label="{{maxTemp}} C"></ion-input>
            </div>
          </div>
        </ion-card>

        <br>
        <ion-card>
          <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: column;">
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: row;">
              <ion-title>
                MIN HUMIDITY
              </ion-title>
              <ion-title>
                MAX HUMIDITY
              </ion-title>
            </div>
            <div style=" flex: 2; flex-wrap: wrap;
            align-content: space-evenly; flex-direction: row; width: 100%;">
              <ion-input [formControl]="SensorHumMin" label-placement="floating" style="width: 45%;" label="{{minHum}} %"></ion-input>
              <ion-input [formControl]="SensorHumMax" label-placement="floating" style="width: 45%;" label="{{maxHum}} %"></ion-input>
            </div>
          </div>
        </ion-card>

        <br>
        <ion-card>
          <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: column;">
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: row;">
              <ion-title>
                MIN CO2 Level
              </ion-title>
              <ion-title>
                MAX CO2 Level
              </ion-title>
            </div>
            <div style=" flex: 2; flex-wrap: wrap;
                    align-content: space-evenly; flex-direction: row; width: 100%;">
              <ion-input [formControl]="SensorAqMin" label-placement="floating" style="width: 42%;" label="{{minCO2}} PPM"></ion-input>
              <ion-input [formControl]="SensorAqMax" label-placement="floating" style="width: 42%;" label="{{maxCO2}} PPM "></ion-input>
            </div>
          </div>
        </ion-card>

        <ion-title STYLE="--ion-text-color: red">{{errorMessage}}</ion-title>

        <ion-card>
          <ion-button style="align-self: center" (click)="saveChanges()">
            <ion-icon name="save-outline"></ion-icon>
            Edit
          </ion-button>
        </ion-card>
      </div>

    `,
  styleUrls: ['roomInfo.page.scss'],
})
export class RoomSensorSetPage implements OnInit {

  errorMessage: string = "";
  minTemp: number = -1;
  maxTemp: number = -1
  minHum: number = -1;
  maxHum: number = -1
  minCO2: number = -1;
  maxCO2: number = -1;

  SensorTempMin : FormControl<number | null> = new FormControl(this.minTemp,[Validators.required,Validators.min(0),Validators.max(100)]);
  SensorTempMax : FormControl<number | null> = new FormControl(this.maxTemp,[Validators.required,Validators.min(0),Validators.max(100)]);

  SensorHumMin : FormControl<number | null> = new FormControl(this.minHum,[Validators.required,Validators.min(0),Validators.max(100)]);
  SensorHumMax : FormControl<number | null> = new FormControl(this.maxHum,[Validators.required,Validators.min(0),Validators.max(100)]);

  SensorAqMin : FormControl<number | null> = new FormControl(this.minCO2,[Validators.required,Validators.min(0),Validators.max(100)]);
  SensorAqMax : FormControl<number | null> = new FormControl(this.maxCO2,[Validators.required,Validators.min(0),Validators.max(100)]);




  constructor(private modalController: ModalController, protected ws: WebsocketClientService){}

  ngOnInit(): void {

    setTimeout(() => {

    }, 2000)
  }
  dismissModal() {
    this.modalController.dismiss();
  }

  saveChanges() {


    if(this.SensorTempMax.value! < this.SensorTempMin.value! ||
      this.SensorHumMax.value! < this.SensorHumMin.value! || this.SensorAqMax.value! < this.SensorAqMin.value!){

      this.errorMessage = "Minimum value cant be larger than the Maximum Value";

      return;
    }

    this.minTemp = this.SensorTempMax.value!;
    this.maxTemp = this.SensorTempMax.value!;
    this.minHum = this.SensorHumMin.value!;
    this.maxHum = this.SensorHumMax.value!;
    this.minCO2 = this.SensorAqMin.value!;
    this.maxCO2 = this.SensorAqMax.value!;


    //this.dismissModal();
  }
}
