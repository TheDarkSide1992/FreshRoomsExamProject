import {Component, OnInit} from "@angular/core";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {openWeatherWMOToEmoji} from '@akaguny/open-meteo-wmo-to-emoji';
import {clientWantsToGetWeatherForcast} from "../Models/Client/clientWantsToGetWeatherForcast";
import {ClientWantsCity} from "../Models/Client/ClientWantsCity";
import {ActivatedRoute} from "@angular/router";

@Component({
  template: `
    <ion-content>
      <div>
        <ion-toolbar>
          <ion-title mode="ios">Home</ion-title>
        </ion-toolbar>
        <div *ngIf="localStorage.getItem('jwt') !== '' && this.websocketservice.city !==''">
          <ion-card>
            <ion-card-content>
              <div style="display: flex; overflow-x: scroll; justify-content: space-between;">
                <div *ngFor="let time of this.websocketservice.todaysForecast?.hourly?.time; let i=index;">
                  <ion-card>
                    <ion-item>
                  <span
                    style="display: flex; font-size: 60px; justify-content: center;">{{ openWeatherWMOToEmoji(this.websocketservice.todaysForecast?.hourly?.weather_code?.[i]).value }}</span>
                    </ion-item>
                    <ion-item>
                      <ion-card-title mode="ios"
                                      style="display: flex; justify-content: center">{{ this.websocketservice.todaysForecast?.hourly?.time?.[i]?.substr(-5) }}
                      </ion-card-title>
                    </ion-item>
                    <ion-item>
                      <ion-text>
                        Tempratur: {{ this.websocketservice.todaysForecast?.hourly?.temperature_2m?.[i] }}{{ this.websocketservice.todaysForecast?.hourly_units?.temperature_2m }}
                      </ion-text>
                    </ion-item>
                    <ion-item>
                      <ion-text>Feels
                        like: {{ this.websocketservice.todaysForecast?.hourly?.apparent_temperature?.[i] }}{{ this.websocketservice.todaysForecast?.hourly_units?.apparent_temperature }}
                      </ion-text>
                    </ion-item>
                    <ion-item>
                      <ion-text>
                        Precipitation: {{ this.websocketservice.todaysForecast?.hourly?.precipitation?.[i] }} {{ this.websocketservice.todaysForecast?.hourly_units?.precipitation }}
                      </ion-text>
                    </ion-item>
                  </ion-card>
                </div>
              </div>
            </ion-card-content>
          </ion-card>
        <ion-card>
          <ion-card-content>
            <div style="display: flex; overflow-x: scroll;">

              <ion-card *ngFor="let time of websocketservice.dailyForecast?.daily?.time; let i=index;"
                        style="display: flex; flex-flow: column;">
                <ion-item>
                  <span
                    style="display: flex; font-size: 60px; justify-content: center;">{{ openWeatherWMOToEmoji(this.websocketservice.dailyForecast?.daily?.weather_code?.[i]).value }}</span>
                </ion-item>
                <ion-item>
                  <ion-card-title mode="ios"
                                  style="display: flex; justify-content: center">{{ this.websocketservice.dailyForecast?.daily?.time?.[i] }}
                  </ion-card-title>
                </ion-item>
                <ion-item>
                  <ion-text>
                    Tempratur: {{ this.websocketservice.dailyForecast?.daily?.temperature_2m_max?.[i] }}{{ this.websocketservice.dailyForecast?.daily_units?.temperature_2m_max }}
                  </ion-text>
                </ion-item>
                <ion-item>
                  <ion-text>Feels
                    like: {{ this.websocketservice.dailyForecast?.daily?.apparent_temperature_max?.[i] }}{{ this.websocketservice.dailyForecast?.daily_units?.apparent_temperature_max }}
                  </ion-text>
                </ion-item>
                <ion-item>
                  <ion-text>
                    Precipitation: {{ this.websocketservice.dailyForecast?.daily?.precipitation_probability_max?.[i] }}
                    %
                  </ion-text>
                </ion-item>
              </ion-card>
            </div>
          </ion-card-content>
        </ion-card>
        </div>
      </div>
    </ion-content>`
})
export class HomePage implements OnInit{


  constructor(public websocketservice: WebsocketClientService, public  route : ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.getforecasts();
    });
  }
  getforecasts() {
      this.websocketservice.socketConnection.sendDto(new clientWantsToGetWeatherForcast);
  }

  getcity()
  {
    this.websocketservice.socketConnection.sendDto(new ClientWantsCity());
  }

  ngOnInit(): void {
    setTimeout(() => {
      if(localStorage.getItem('jwt') !== '' && localStorage.getItem('jwt') !== undefined)
      {
        this.getcity()
      }
      setTimeout(() => {
      if(this.websocketservice.city !== '' && this.websocketservice.city !== undefined)
      {
        this.getforecasts()
      }}, 1000)
    }, 2000)
  }

  protected readonly localStorage = localStorage;
  protected readonly openWeatherWMOToEmoji = openWeatherWMOToEmoji;

}
