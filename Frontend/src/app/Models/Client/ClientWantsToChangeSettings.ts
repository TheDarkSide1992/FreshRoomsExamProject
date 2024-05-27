import {BaseDto} from "../objects/baseDto";

export class ClientWantsToChangeSettings extends BaseDto<ClientWantsToChangeSettings>
{
  newNameDto? : string | null;
  newEmailDto? : string | null;
  newCityDto? : string | null;
  newPasswordDto? : string | null;
}
