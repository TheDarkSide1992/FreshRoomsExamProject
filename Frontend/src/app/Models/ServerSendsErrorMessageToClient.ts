import {BaseDto} from "./baseDto";

export class ServerSendsErrorMessageToClient extends BaseDto<ServerSendsErrorMessageToClient> {
  recivedMessage? : string;
  errorMessage? : string;
}
