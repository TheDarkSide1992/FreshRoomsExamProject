import {BaseDto} from "../objects/baseDto";

export class ClientWantsToAuthenticateWithJwt extends BaseDto<ClientWantsToAuthenticateWithJwt>
{
  jwt?: string;
}
