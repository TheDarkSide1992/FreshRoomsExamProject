import {BaseDto} from "../objects/baseDto";

export class ClientWantsToLogin extends BaseDto<ClientWantsToLogin>
{
  email? : string;
  password?: string;
}
