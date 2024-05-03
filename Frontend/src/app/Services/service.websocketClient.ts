import {WebSocketSuperClass} from "../Models/WebSocketSuperClass";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";
import {BaseDto} from "../Models/baseDto";
import {Injectable} from "@angular/core";
import {ServerAuthenticatesUserFromJwt} from "../Models/ServerAuthenticatesUserFromJwt";
import { ServerLogsInUser } from "../Models/ServerLogsInUser";
import {ToastController} from "@ionic/angular";

@Injectable({providedIn: 'root'})
export class WebsocketClientService
{
  public socketConnection: WebSocketSuperClass;

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

 async ServerLogsInUser(dto: ServerLogsInUser)
  {
    console.log(dto)
    localStorage.setItem("jwt", dto.jwt!);
    console.log("second")
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
}


