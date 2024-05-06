import {BaseDto} from "./baseDto";

export class ClientWantsToCreateUserDTO extends BaseDto<any> {
  name?: string;
  email?: string;
  password?: string;
  guid?: string;
}
