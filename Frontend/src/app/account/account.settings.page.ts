import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {IonContent} from "@ionic/angular";
import {FormControl, Validators} from "@angular/forms";
import {WebsocketClientService} from "../Services/service.websocketClient";
import {ClientWantsAccountInfo} from "../Models/ClientWantsAccountInfo";
import {navigate} from "ionicons/icons";
import {ClientWantsToChangeSettings} from "../Models/ClientWantsToChangeSettings";


@Component({
  template: `
      <div style="flex-direction: column;">
          <ion-toolbar>
              <ion-title mode="ios">
                  ACCOUNT
              </ion-title>
          </ion-toolbar>

          <br>

          <ion-card>
              <div style="flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">
                  <div style="flex: 1;"></div>
                  <div style="display: flex; flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
                      <br>
                      <ion-item>
                          <h1>Name : </h1>
                      </ion-item>
                      <ion-item>
                          <h1>Email : </h1>
                      </ion-item>
                      <ion-item>
                          <h1>City : </h1>
                      </ion-item>
                      <ion-item>
                          <h1>Password : </h1>
                      </ion-item>
                      <ion-item>
                          <h1>Repeat Password : </h1>
                      </ion-item>
                  </div>

                  <br>

                  <div style=" flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
                      <br>
                      <ion-item>
                          <ion-input [formControl]="changeNameForm"
                                     id="newName">{{ this.wsService.currentAccount?.realname }}</ion-input>
                      </ion-item>
                      <ion-item>
                          <ion-input [formControl]="changeEmailForm"
                                     id="newMail">{{ this.wsService.currentAccount?.email }}</ion-input>
                      </ion-item>
                      <ion-item>
                          <ion-input [formControl]="changeCityForm"
                                     id="newCity">{{this.wsService.currentAccount?.city}}</ion-input>
                      </ion-item>
                      <ion-item>
                          <ion-input [formControl]="changePasswordForm" onPaste="return false"
                                     onCopy="return false" onCut="return false"
                                     onDrag="return false" onDrop="return false"
                                     autocomplete=off [type]="true ? 'password' : 'text'" id="newPassword">
                              ***********
                          </ion-input>
                      </ion-item>
                      <ion-item>
                          <ion-input [formControl]="passwordRepeat" onPaste="return false"
                                     onCopy="return false" onCut="return false"
                                     onDrag="return false" onDrop="return false"
                                     autocomplete=off [type]="true ? 'password' : 'text'" id="repeatPassword">Repeat
                              Password
                          </ion-input>
                      </ion-item>
                  </div>
                  <div style="flex: 4;"></div>
              </div>

              <div style="height: 15px"></div>

              <div
                      *ngIf="passwordRepeat.invalid && passwordRepeat.touched && changePasswordForm.value !== passwordRepeat.value"
                      class="error">
                  Both passwords must match
              </div>

              <div style=" flex-wrap: wrap; align-content: space-evenly;
      justify-content: space-evenly; flex-direction: row;">

                  <div style="flex: 1;"></div>
                  <div style=" flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: column;">
                      <br>

                  </div>
                  <div style="flex: 4; flex-wrap: wrap;
              align-content: space-evenly; flex-direction: row ;">

                      <ion-button (click)="cancel()">
                          <ion-icon name="stop-circle-outline"></ion-icon>
                          <p> cancel </p>
                      </ion-button>

                      <br>

                      <ion-button (click)="saveChanges()"
                                  [disabled]=" !(passwordRepeat.value?.trim() === changePasswordForm.value?.trim())">
                          <ion-icon name="save-outline"></ion-icon>
                          <p> Save </p>
                      </ion-button>
                  </div>
                  <div style="flex: 4;"></div>
              </div>
          </ion-card>

      </div>
  `,
  styleUrls: ['account.page.scss'],
})

export class AccountSettingsPage implements OnInit{

  changeNameForm: FormControl<string | null> = new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(250)]);
  changeEmailForm: FormControl<string | null> = new FormControl("",[Validators.required, Validators.email]);
  changeCityForm: FormControl<string | null> = new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(250)]);
  changePasswordForm: FormControl<string | null> = new FormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(250)]);
  passwordRepeat: FormControl<string | null> = new FormControl("", [Validators.required]);

  constructor(public wsService:WebsocketClientService,  private readonly router: Router,) {

  }


  ngOnInit(): void {
    if(localStorage.getItem('jwt') !== '' && localStorage.getItem('jwt') !== undefined)
    {

    }
  }


  async saveChanges() {
    let newName: string = "";
    let newEmail: string = "";
    let newCity: string = "";
    let newPassword: string = "";

    if (this.changeNameForm.value != null && this.changeNameForm.value?.trim() != "") {
      newName = this.changeNameForm.value;
    } else {
      newName = "N/A";
    }
    if (this.changeEmailForm.value != null && this.changeEmailForm.value?.trim() != "") {
      newEmail = this.changeEmailForm.value;
    } else {
      newEmail = "N/A";
    }
    if (this.changeCityForm.value != null && this.changeCityForm.value?.trim() != "") {
      newCity = this.changeCityForm.value;
    } else {
      newCity = "N/A";
    }
    if (this.changePasswordForm.value != null && this.changePasswordForm.value?.trim() != "") {
      newPassword = this.changePasswordForm.value;
    } else {
      newPassword = "N/A";
    }


    console.log("newName: " + newName + "\n" +
      "newEmail" + newEmail + "\n" +
      "newCity" + newCity + "\n" +
      "newPassword" + newPassword + "\n" +
      "")

    await this.wsService.socketConnection.sendDto(new ClientWantsToChangeSettings({
      newNameDto: newName,
      newEmailDto: newEmail,
      newCityDto: newCity,
      newPasswordDto: newPassword
    }))
    //TODO SEND TO API

    await this.wsService.socketConnection.sendDto(new ClientWantsAccountInfo)

    this.router.navigate(["account"])
  }

  cancel(){
    this.router.navigate(["account"])

  }

}
