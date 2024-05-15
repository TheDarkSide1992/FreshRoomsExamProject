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

@Injectable({providedIn: 'root'})
export class WebsocketClientService
{
  public socketConnection: WebSocketSuperClass;
  currentAccount? : accountModdel;
  dailyForecast? : DailyWeatherModel;
  todaysForecast? : TodayWeatherModel;
  city?: string;
  sensorlist: Array<DeviceModel> = [];
  sensorTypeList: Array<DeviceTypesModel> = [];
  roomList: Array<RoomModel> = [];
  roomConfig?: RoomConfig;
  currentRoomId?: number;

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
  ServerSendsRoomConfigurations(dto: ServerSendsRoomConfigurations)
  {
     this.roomConfig = dto as RoomConfig;
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

  async ServerSendsErrorMessageToClient(dto: ServerSendsErrorMessageToClient){
    var t = await this.toast.create(
      {
        color: "warning",
        duration: 2000,
        message: dto.errorMessage,
      }
    )
    t.present();
  }
  async ServerRespondsToSensorVeryfication(dto: ServerRespondsToSensorVeryfication){
    let tempsensor: DeviceModel = {
      deviceTypeName: dto.deviceTypeName,
      sensorGuid: dto.sensorGuid,
    }
    this.sensorlist.push(tempsensor);
  }

  async ServerSendsDeviceTypes(dto: ServerSendsDeviceTypes){
    dto.deviceTypeList?.forEach(deviceType => {
      if (deviceType != undefined){
        let tempDeviceType: DeviceTypesModel = {
          deviceTypeId: deviceType.deviceTypeId,
          deviceTypeName: deviceType.deviceTypeName,
        }
        this.sensorTypeList.push(tempDeviceType);
      }
    }
    )}

  async ServerRespondsToUser(dto: ServerRespondsToUser){
    var t = await this.toast.create(
      {
        color: "success",
        duration: 2000,
        message: dto.message,
      }
    )
    t.present();
  }

  async ServerReturnsRoomList(dto: ServerReturnsRoomList) {
    this.roomList = []
    dto.roomList?.forEach(room => {
      if (room != undefined){
        let tempRoom: RoomModel = {
          roomId: room.roomId,
          name: room.name,
          creatorId: room.creatorId
        }
        this.roomList.push(tempRoom);
      }
    }
    )}

  async ServerReturnsCreatedRoom(dto: ServerReturnsCreatedRoom){
    let tempRoom: RoomModel = {
      roomId: dto.roomId,
      name: dto.name,
      creatorId: dto.creatorId,
      deviceList: dto.deviceList,
    }
    this.roomList.push(tempRoom);
  }
}


