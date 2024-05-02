import {Component} from "@angular/core";
import {FormControl, Validators} from "@angular/forms";

@Component({
  template:
    `
      <body>

            <div style="display: flex; justify-items: center; flex-direction: column; align-items: center;" >
              <ion-title style="margin-top: 5%" >Register</ion-title>
              <ion-input [formControl]="AAC" style="width: 50%; margin-top: 5%" label-placement="floating"
                         label="Account Activation Code" placeholder="Account Activation Code">
              </ion-input>
              <div *ngIf="AAC.invalid && AAC.touched" class="error">
                Must enter a proper activation key
              </div>
              <ion-input [formControl]="AName" style="width: 50%; margin-top: 3%" label-placement="floating"
                         label="Name" placeholder="Name">
              </ion-input>
              <div *ngIf="AName.invalid && AName.touched" class="error">
                Name must be between 1-100 letters
              </div>
              <ion-input [formControl]="AEmail" style="width: 50%; margin-top: 3%" label-placement="floating"
                         label="Email" placeholder="Email">
              </ion-input>
              <div *ngIf="AEmail.invalid && AEmail.touched" class="error">
                Must be a valid Email
              </div>
              <ion-input [formControl]="APassword" style="width: 50%; margin-top: 3%" label-placement="floating"
                         label="Password" placeholder="Password" [type]="true ? 'password' : 'text'" required>
              </ion-input>
              <div *ngIf="APassword.invalid && APassword.touched" class="error">
                Must be between 8-32 characters long and contain atleast 1 capital letter
              </div>
              <ion-input [formControl]="APasswordRepeat" style="width: 50%;" label-placement="floating"
                         label="Repeat Password" placeholder="Repeat Password" [type]="true ? 'password' : 'text'" required>
              </ion-input>
              <div
                *ngIf="APasswordRepeat.invalid && APasswordRepeat.touched && APassword.value !== APasswordRepeat.value"
                class="error">
                Both passwords must match
              </div>
              <ion-button style=" margin-top: 5%" [disabled]= " !(APasswordRepeat.value === APassword.value && APassword.valid && APasswordRepeat.valid && AEmail.valid && AName.valid) ">Register
              </ion-button>
            </div>
      </body>
    `,
})
export class RegisterAccountPage {

  AAC = new FormControl("",[Validators.required,Validators.minLength(36),Validators.maxLength(36)]);
  AName = new FormControl("",[Validators.required,Validators.minLength(1),Validators.maxLength(100)]);
  AEmail = new FormControl("",[Validators.required, Validators.email]);
  APassword = new FormControl("",[Validators.required, Validators.minLength(8),Validators.maxLength(32)]);
  APasswordRepeat: FormControl = new FormControl("", [Validators.required]);

  constructor(){}

}
