import {BaseDto} from "../objects/baseDto";

export class ClientWantsToChangeSettings extends BaseDto<ClientWantsToChangeSettings>
{
  newNameDto? : string;
  newEmailDto? : string;
  newCityDto? : string;
  newPasswordDto? : string;
}
