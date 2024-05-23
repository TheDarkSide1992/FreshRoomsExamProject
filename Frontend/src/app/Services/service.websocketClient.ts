import {WebSocketSuperClass} from "../Models/WebSocketSuperClass";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";
import {BaseDto} from "../Models/baseDto";
import {Injectable} from "@angular/core";
import {ServerAuthenticatesUserFromJwt} from "../Models/ServerAuthenticatesUserFromJwt";
import {ServerLogsInUser} from "../Models/ServerLogsInUser";
import {ToastController} from "@ionic/angular";
import {ServerSendsAccountData} from "../Models/ServerSendsAccountData";
import {accountModdel} from "../Models/objects/accountModdel";
import {ServerReturnsForecast} from "../Models/ServerReturnsDailyForecast";
import {DailyWeatherModel} from "../Models/objects/DailyForcastModels";
import {TodayWeatherModel} from "../Models/objects/TodaysForcastModels";
import {ServerLogsoutUser} from "../Models/ServerLogsoutUser";
import {ServerReturnsCity} from "../Models/ServerReturnsCity";
import {ServerSendsErrorMessageToClient} from "../Models/ServerSendsErrorMessageToClient";
import {DeviceModel} from "../Models/objects/DeviceModel";
import {ServerRespondsToSensorVeryfication} from "../Models/ServerRespondsToSensorVeryfication";
import {ServerRespondsToUser} from "../Models/ServerRespondsToUser";
import {ServerSendsRoomConfigurations} from "../Models/ServerSendsRoomConfigurations";
import {RoomConfig} from "../Models/objects/roomConfig";
import {DetailedRoomModel} from "../Models/objects/DetailedRoomModel";
import {ServerReturnsDetailedRoomToUser} from "../Models/ServerReturnsDetailedRoomToUser";
import {ServerReturnsNewestSensorData} from "../Models/ServerReturnsNewestSensorData";
import {ServerReturnsBasicRoomStatus} from "../Models/ServerReturnsBasicRoomStatus";
import {BasicRoomStatusModel} from "../Models/objects/BasicRoomStatusModel";
import {ServerReturnsNewMotorStatusForAllMotorsInRoom} from "../Models/ServerReturnsNewMotorStatusForAllMotorsInRoom";
import {ServerReturnsNewMotorStatusForOneMotor} from "../Models/ServerReturnsNewMotorStatusForOneMotor";


@Injectable({providedIn: 'root'})
export class WebsocketClientService {
  public socketConnection: WebSocketSuperClass;
  currentAccount?: accountModdel;
  dailyForecast?: DailyWeatherModel;
  todaysForecast?: TodayWeatherModel;
  city?: string;
  deviceList: Array<DeviceModel> = [];
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
    let tempDevice: DeviceModel = {
      deviceTypeName: dto.deviceTypeName,
      deviceGuid: dto.sensorGuid,
    }
    this.deviceList.push(tempDevice);
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
      temp = temp + sensor.temperature
      hum = hum + sensor.humidity
      aq = aq + sensor.co2
    }

    this.currentaq = aq / this.currentRoom?.sensors?.length!
    this.currenttemp = temp / this.currentRoom?.sensors?.length!
    this.currenthum = hum / this.currentRoom?.sensors?.length!
  }

  ServerReturnsNewestSensorData(dto: ServerReturnsNewestSensorData) {
    console.log(this.currentRoom?.sensors);
    var index = this.currentRoom?.sensors?.findIndex(function (item) {
      return item.sensorId == dto.data?.sensorId
    });
    this.currentRoom?.sensors?.splice(index!, 1, dto.data!);
    console.log(this.currentRoom?.sensors);
    let temp = 0;
    let hum = 0;
    let aq = 0;
    for (var sensor of this.currentRoom?.sensors!) {
      temp = temp + sensor.temperature
      hum = hum + sensor.humidity
      aq = aq + sensor.co2
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

  async ServerReturnsNewMotorStatusForAllMotorsInRoom(dto: ServerReturnsNewMotorStatusForAllMotorsInRoom)
  {
    for(let m of dto.motors)
    {
      var index = this.currentRoom?.motors?.findIndex(function (item) {
        return item.motorId == m.motorId
      });
      this.currentRoom?.motors?.splice(index!, 1, m);
    }
    this.currentRoom?.motors == dto.motors;
    var t = await this.toast.create(
      {
        color: "success",
        duration: 2000,
        message: dto.message,
      }
    )
    t.present();
  }

  async ServerReturnsNewMotorStatusForOneMotor(dto: ServerReturnsNewMotorStatusForOneMotor)
  {
    var index = this.currentRoom?.motors?.findIndex(function (item) {
      return item.motorId == dto.motor?.motorId
    });
    this.currentRoom?.motors?.splice(index!, 1, dto.motor!);

    var t = await this.toast.create(
      {
        color: "success",
        duration: 2000,
        message: dto.message,
      }
    )
    t.present();
  }
}
