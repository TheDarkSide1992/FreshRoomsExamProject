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
import {ServerSendsErrorMessageToClient} from "../Models/ServerSendsErrorMessageToClient";
import {DeviceModel, DeviceTypesModel} from "../Models/DeviceModel";
import {ServerRespondsToSensorVeryfication} from "../Models/ServerRespondsToSensorVeryfication";
import { ServerSendsDeviceTypes } from "../Models/ServerSendsDeviceTypes";
import {ServerRespondsToUser} from "../Models/ServerRespondsToUser";
import {ServerReturnsRoomList} from "../Models/ServerReturnsRoomList";
import {RoomModel, RoomModelDto} from "../Models/RoomModel";
import {ServerReturnsCreatedRoom} from "../Models/ServerReturnsCreatedRoom";
import {ServerSendsRoomConfigurations} from "../Models/ServerSendsRoomConfigurations";
import {RoomConfig} from "../Models/objects/roomConfig";
import {DetailedRoomModel} from "../Models/objects/DetailedRoomModel";
import {ServerReturnsDetailedRoomToUser} from "../Models/ServerReturnsDetailedRoomToUser";
import {ServerReturnsNewestSensorData} from "../Models/ServerReturnsNewestSensorData";
import {ServerReturnsBasicRoomStatus} from "../Models/ServerReturnsBasicRoomStatus";
import {BasicRoomStatusModel} from "../Models/objects/BasicRoomStatusModel";


@Injectable({providedIn: 'root'})
export class WebsocketClientService {
  public socketConnection: WebSocketSuperClass;
  currentAccount?: accountModdel;
  dailyForecast?: DailyWeatherModel;
  todaysForecast?: TodayWeatherModel;
  city?: string;
  sensorlist: Array<DeviceModel> = [];
  sensorTypeList: Array<DeviceTypesModel> = [];
  roomStatusList: Array<BasicRoomStatusModel> = [];
  roomConfig?: RoomConfig;
  currentRoom?: DetailedRoomModel;
  currenttemp?: number;
  currenthum?: number;
  currentaq?: number;

  constructor(public router: Router, public toast: ToastController) {
    this.socketConnection = new WebSocketSuperClass(environment.url)
    this.handleEvent();
  }

  handleEvent() {
    this.socketConnection.onmessage = (event) => {
      const data = JSON.parse(event.data) as BaseDto<any>;
      //@ts-ignore
      this[data.eventType].call(this, data);
    }
  }

  ServerAuthenticatesUserFromJwt(dto: ServerAuthenticatesUserFromJwt) {
    this.router.navigate(['/home'])
  }

  ServerSendsAccountData(dto: ServerSendsAccountData) {
    this.currentAccount = dto as accountModdel;
  }

  ServerSendsRoomConfigurations(dto: ServerSendsRoomConfigurations) {
    this.roomConfig = dto as RoomConfig;
  }

  async ServerLogsInUser(dto: ServerLogsInUser) {
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

  ServerReturnsForecast(dto: ServerReturnsForecast) {
    this.dailyForecast = dto.dailyForecast;
    this.todaysForecast = dto.todaysForecast;
  }

  async ServerLogsoutUser(dto: ServerLogsoutUser) {
    localStorage.setItem('jwt', '');
    var t = await this.toast.create(
      {
        color: "success",
        duration: 2000,
        message: "Logged out successfully"
      }
    )
    t.present();
  }

  ServerReturnsCity(dto: ServerReturnsCity) {
    this.city = dto.city;
  }

  async ServerSendsErrorMessageToClient(dto: ServerSendsErrorMessageToClient) {
    var t = await this.toast.create(
      {
        color: "warning",
        duration: 2000,
        message: dto.errorMessage,
      }
    )
    t.present();
  }

  async ServerRespondsToSensorVeryfication(dto: ServerRespondsToSensorVeryfication) {
    let tempsensor: DeviceModel = {
      deviceTypeName: dto.deviceTypeName,
      sensorGuid: dto.sensorGuid,
    }
    this.sensorlist.push(tempsensor);
  }

  async ServerRespondsToUser(dto: ServerRespondsToUser) {
    var t = await this.toast.create(
      {
        color: "success",
        duration: 2000,
        message: dto.message,
      }
    )
    t.present();
  }

  ServerReturnsDetailedRoomToUser(dto: ServerReturnsDetailedRoomToUser) {
    console.log(dto)
    if (dto.room) {
      this.currentRoom = dto.room;
    }
    let temp = 0;
    let hum = 0;
    let aq = 0;
    for (var sensor of this.currentRoom?.sensors!) {
      temp = temp + sensor.Temperature
      hum = hum + sensor.Humidity
      aq = aq + sensor.CO2
    }

    this.currentaq = aq / this.currentRoom?.sensors?.length!
    this.currenttemp = temp / this.currentRoom?.sensors?.length!
    this.currenthum = hum / this.currentRoom?.sensors?.length!
  }

  ServerReturnsNewestSensorData(dto: ServerReturnsNewestSensorData) {
    console.log(this.currentRoom?.sensors);
    var index = this.currentRoom?.sensors?.findIndex(function (item) {
      return item.sensorId == dto.data.sensorId
    });
    this.currentRoom?.sensors?.splice(0, 1, dto.data);
    console.log(this.currentRoom?.sensors);
    let temp = 0;
    let hum = 0;
    let aq = 0;
    for (var sensor of this.currentRoom?.sensors!) {
      temp = temp + sensor.Temperature
      console.log(temp);
      hum = hum + sensor.Humidity
      console.log(hum);
      aq = aq + sensor.CO2
      console.log(aq);
    }

    this.currentaq = aq / this.currentRoom?.sensors?.length!
    this.currenttemp = temp / this.currentRoom?.sensors?.length!
    this.currenthum = hum / this.currentRoom?.sensors?.length!
  }

  async ServerReturnsBasicRoomStatus(dto: ServerReturnsBasicRoomStatus) {
    if (dto.basicRoomListData != undefined) {
      this.roomStatusList = dto.basicRoomListData;
    }
  }
}
