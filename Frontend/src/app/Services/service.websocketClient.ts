import {WebSocketSuperClass} from "../Models/WebSocketSuperClass";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";
import {BaseDto} from "../Models/baseDto";
import {Injectable} from "@angular/core";
import {ServerAuthenticatesUserFromJwt} from "../Models/ServerAuthenticatesUserFromJwt";
import { ServerLogsInUser } from "../Models/ServerLogsInUser";
import {ToastController} from "@ionic/angular";
import { ServerSendsAccountData } from "../Models/ServerSendsAccountData";
import {accountModdel} from "../Models/objects/accountModdel";
import {ServerReturnsForecast} from "../Models/ServerReturnsDailyForecast";
import {DailyWeatherModel} from "../Models/objects/DailyForcastModels";
import {TodayWeatherModel} from "../Models/objects/TodaysForcastModels";
import { ServerLogsoutUser } from "../Models/ServerLogsoutUser";
import { ServerReturnsCity } from "../Models/ServerReturnsCity";

@Injectable({providedIn: 'root'})
export class WebsocketClientService
{
  public socketConnection: WebSocketSuperClass;
  currentAccount? : accountModdel;
  dailyForecast? : DailyWeatherModel;
  todaysForecast? : TodayWeatherModel;
  city?: string;

  constructor(public router: Router, public toast: ToastController) {
    this.socketConnection = new WebSocketSuperClass(environment.url)
    this.handleEvent();
  }

  handleEvent()
  {
    this.socketConnection.onmessage = (event) =>
    {
      const data = JSON.parse(event.data) as BaseDto<any>;
      //@ts-ignore
      this[data.eventType].call(this, data);
    }
  }

  ServerAuthenticatesUserFromJwt(dto: ServerAuthenticatesUserFromJwt)
  {
    this.router.navigate(['/home'])
  }

  ServerSendsAccountData(dto: ServerSendsAccountData)
  {
    this.currentAccount = dto as accountModdel;
  }

 async ServerLogsInUser(dto: ServerLogsInUser)
  {
    localStorage.setItem("jwt", dto.jwt!);
    this.router.navigate(['/home'])
    var t = await this.toast.create(
      {
        color: "success",
        duration: 2000,
        message: "Logged in successfully"
      }
    )
    t.present();
  }

  ServerReturnsForecast(dto: ServerReturnsForecast)
  {
    this.dailyForecast = dto.dailyForecast;
    this.todaysForecast = dto.todaysForecast;
  }

  async ServerLogsoutUser(dto: ServerLogsoutUser)
  {
    localStorage.setItem('jwt','');
    var t = await this.toast.create(
      {
        color: "success",
        duration: 2000,
        message: "Logged out successfully"
      }
    )
    t.present();
  }

  ServerReturnsCity(dto: ServerReturnsCity)
  {
    this.city = dto.city;
  }

}


