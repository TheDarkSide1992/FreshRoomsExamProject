import {BaseDto} from "./baseDto";

export class ClientWantsToChangeSettings extends BaseDto<ClientWantsToChangeSettings>
{
  newNameDto? : string | null;
  newEmailDto? : string | null;
  newCityDto? : string | null;
  newPasswordDto? : string | null;
}
