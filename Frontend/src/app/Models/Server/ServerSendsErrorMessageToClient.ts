import {BaseDto} from "../objects/baseDto";

export class ServerSendsErrorMessageToClient extends BaseDto<ServerSendsErrorMessageToClient> {
  recivedMessage? : string;
  errorMessage? : string;
}
