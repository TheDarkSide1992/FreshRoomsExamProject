import {Component} from '@angular/core';

import {WebsocketClientService} from "./Services/service.websocketClient";

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  constructor(public websocketservice: WebsocketClientService) {}
}
