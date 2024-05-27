import {Component} from "@angular/core";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {FormControl, Validators} from "@angular/forms";
import {ClientWantsToLogin} from "../Models/Client/ClientWantsToLogin";
import {NavigationStart, Router} from "@angular/router";

@Component({
  templateUrl: `Login.page.html`
})
export class LoginPage {
  emailcontrol = new FormControl("", [Validators.required, Validators.email]);
  passwordcontrol = new FormControl("", [Validators.required,Validators.minLength(8),Validators.maxLength(32)])
  constructor(public websocketservice: WebsocketClientService,
              private readonly router: Router,) {
    this.router.events.subscribe(event =>    {
      if(event instanceof NavigationStart) {
        this.emailcontrol.setValue("");
        this.passwordcontrol.setValue("");
      }
    })
  }

  login() {
    this.websocketservice.socketConnection.sendDto(new ClientWantsToLogin({email: this.emailcontrol.getRawValue()!,password: this.passwordcontrol.getRawValue()!}))
  }
}

