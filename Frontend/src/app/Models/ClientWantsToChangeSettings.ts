import {BaseDto} from "./baseDto";

export class ClientWantsToChangeSettings extends BaseDto<ClientWantsToChangeSettings>
{
  newNameDto? : string;
  newEmailDto? : string;
  newCityDto? : string;
  newPasswordDto? : string;
}
