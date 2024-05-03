import {WebSocketSuperClass} from "../Models/WebSocketSuperClass";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";
import {BaseDto} from "../Models/baseDto";
import {Injectable} from "@angular/core";
import {ServerAuthenticatesUserFromJwt} from "../Models/ServerAuthenticatesUserFromJwt";

@Injectable({providedIn: 'root'})
export class WebsocketClientService
{
  public socketConnection: WebSocketSuperClass;

  constructor(public router: Router) {
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
}


