import ReconnectingWebSocket from "reconnecting-websocket";
import {BaseDto} from "./baseDto";
import {ClientWantsToAuthenticateWithJwt} from "./ClientWantsToAuthenticateWithJwt";

export class WebSocketSuperClass extends ReconnectingWebSocket
{
  private messageQueue: Array<BaseDto<any>> = [];


  constructor(address: string) {
    super(address);
    this.onopen = this.handleOpen.bind(this)
  }

  sendDto(dto: BaseDto<any>)
  {
    if(this.readyState === WebSocketSuperClass.OPEN)
    {
      this.send(JSON.stringify(dto));
    }
    else
    {
      this.messageQueue.push(dto)
    }
  }

  private handleOpen()
  {
    let jwt = localStorage.getItem('jwt');
    if(jwt && jwt != '')
    {
     this.sendDto(new ClientWantsToAuthenticateWithJwt({jwt: jwt}));
     while (this.messageQueue.length > 0)
     {
       const dto = this.messageQueue.shift()
       if(dto)
       {
         this.send(JSON.stringify(dto));
       }
     }
    }
  }
}
