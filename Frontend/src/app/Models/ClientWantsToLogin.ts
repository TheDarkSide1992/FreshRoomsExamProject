import {BaseDto} from "./baseDto";

export class ClientWantsToLogin extends BaseDto<ClientWantsToLogin>
{
  email? : string;
  password?: string;
}
