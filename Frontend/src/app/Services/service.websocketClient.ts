import {WebSocketSuperClass} from "../Models/WebSocketSuperClass";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";
import {BaseDto} from "../Models/objects/baseDto";
import {Injectable} from "@angular/core";
import {ServerAuthenticatesUserFromJwt} from "../Models/Server/ServerAuthenticatesUserFromJwt";
import {ServerLogsInUser} from "../Models/Server/ServerLogsInUser";
import {ToastController} from "@ionic/angular";
import {ServerSendsAccountData} from "../Models/Server/ServerSendsAccountData";
import {accountModdel} from "../Models/objects/accountModdel";
import {ServerReturnsForecast} from "../Models/Server/ServerReturnsDailyForecast";
import {DailyWeatherModel} from "../Models/objects/DailyForcastModels";
import {TodayWeatherModel} from "../Models/objects/TodaysForcastModels";
import {ServerLogsoutUser} from "../Models/Server/ServerLogsoutUser";
import {ServerReturnsCity} from "../Models/Server/ServerReturnsCity";
import {ServerSendsErrorMessageToClient} from "../Models/Server/ServerSendsErrorMessageToClient";
import {DeviceModel} from "../Models/objects/DeviceModel";
import {ServerRespondsToDeviceVerification} from "../Models/Server/ServerRespondsToDeviceVerification";
import {ServerRespondsToUser} from "../Models/Server/ServerRespondsToUser";
import {ServerSendsRoomConfigurations} from "../Models/Server/ServerSendsRoomConfigurations";
import {RoomConfig} from "../Models/objects/roomConfig";
import {DetailedRoomModel} from "../Models/objects/DetailedRoomModel";
import {ServerReturnsDetailedRoomToUser} from "../Models/Server/ServerReturnsDetailedRoomToUser";
import {ServerReturnsNewestSensorData} from "../Models/Server/ServerReturnsNewestSensorData";
import {ServerReturnsBasicRoomStatus} from "../Models/Server/ServerReturnsBasicRoomStatus";
import {BasicRoomStatusModel} from "../Models/objects/BasicRoomStatusModel";
import {
  ServerReturnsNewMotorStatusForAllMotorsInRoom
} from "../Models/Server/ServerReturnsNewMotorStatusForAllMotorsInRoom";
import {ServerReturnsNewMotorStatusForOneMotor} from "../Models/Server/ServerReturnsNewMotorStatusForOneMotor";


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

  async ServerRespondsToDeviceVerification(dto: ServerRespondsToDeviceVerification) {
    let tempDevice: DeviceModel = {
      deviceTypeName: dto.deviceTypeName,
      deviceGuid: dto.deviceGuid,
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
