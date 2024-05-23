import {BaseDto} from "../objects/baseDto";

export class ClientWantsToCreateUserDTO extends BaseDto<any> {
  name?: string;
  email?: string;
  password?: string;
  guid?: string;
}
