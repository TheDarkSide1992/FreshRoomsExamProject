import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {IonContent} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ClientWantsAccountInfo} from "../Models/ClientWantsAccountInfo";


@Component({
  template: `
      <div style="flex-direction: column;">
          <ion-toolbar>
              <ion-title mode="ios">
                  ROOM_PAGE : {{displayname}} ({{roomId}})
              </ion-title>
          </ion-toolbar>

          <br>

          <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row; height: 250px;">
              <ion-card style="height: 50%; width: 30%;">
                  Graph One
              </ion-card>
              <ion-card style="height: 50%; width: 30%;">
                  Graph Two
              </ion-card>
              <ion-card style="height: 50%; width: 30%;">
                  Graph Three
              </ion-card>
          </div>


          <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">

              <div style="flex: 1;"></div>

              <div style="display: flex; flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
                  <br>
                  <ion-card>
                      <h1>WINDOWS : OPEN</h1>
                      <br>
                      <ion-button>
                          <ion-icon name="stopwatch-outline"></ion-icon>
                          Close
                      </ion-button>

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
                    align-content: space-evenly; flex-direction: row;">
                              <ion-title style="width: 45%;">{{minTemp}} C</ion-title>
                              <ion-title style="width: 45%;">{{maxTemp}} C</ion-title>
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
                    align-content: space-evenly; flex-direction: row;">
                              <ion-title style="width: 45%;">{{minHum}} %</ion-title>
                              <ion-title style="width: 45%;">{{maxHum}} %</ion-title>
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
                    align-content: space-evenly; flex-direction: row;">
                              <ion-title style="width: 42%;">{{minCO2}} PPM</ion-title>
                              <ion-title style="width: 42%;">{{maxCO2}} PPM</ion-title>
                          </div>
                      </div>
                  </ion-card>

                  <ion-card>
                      <ion-button style="align-self: center">
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

  constructor(public wsService: WebsocketClientService, public route: ActivatedRoute, private readonly router: Router,) {

  }

  displayname: string = "@displayname";
  roomId: number = -1;

  minTemp: number = -1;
  maxTemp: number = -1
  minHum: number = -1;
  maxHum: number = -1
  minCO2: number = -1;
  maxCO2: number = -1;



  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      const name = params['room_name'];
      this.roomId = id;
      this.displayname = name;
    });

    setTimeout(() => {

    }, 2000)
  }


}
