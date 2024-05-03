import {Component} from "@angular/core";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {FormControl, Validators} from "@angular/forms";
import {validateContent} from "ionicons/dist/types/components/icon/validate";
import {ClientWantsToLogin} from "../Models/ClientWantsToLogin";

@Component({
  templateUrl: `Login.page.html`
})
export class LoginPage {
  emailcontrol = new FormControl("", [Validators.required, Validators.email]);
  passwordcontrol = new FormControl("", [Validators.required,Validators.minLength(8),Validators.maxLength(32)])
  constructor(public websocketservice: WebsocketClientService) {}

  login() {
    this.websocketservice.socketConnection.sendDto(new ClientWantsToLogin({email: this.emailcontrol.getRawValue()!,password: this.passwordcontrol.getRawValue()!}))
  }
}

