import {WebSocketSuperClass} from "../Models/WebSocketSuperClass";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";
import {BaseDto} from "../Models/baseDto";
import {Injectable} from "@angular/core";
import {ServerAuthenticatesUserFromJwt} from "../Models/ServerAuthenticatesUserFromJwt";
import { ServerSendsAccountData } from "../Models/ServerSendsAccountData";
import {accountModdel} from "../Models/objects/accountModdel";

@Injectable({providedIn: 'root'})
export class WebsocketClientService
{
  public socketConnection: WebSocketSuperClass;
  currentAccount? : accountModdel;

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

  ServerSendsAccountData(dto: ServerSendsAccountData)
  {
    this.currentAccount = dto as accountModdel;
  }
}


