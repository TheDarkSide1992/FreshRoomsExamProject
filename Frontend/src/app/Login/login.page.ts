import {Component} from "@angular/core";

class WebSocketClientService {
}

@Component({
  selector: 'app-root',
  templateUrl: 'login.page.html',
})

export class LoginPage
{


  constructor(public webSocketClientService: WebSocketClientService) {
  }
}
