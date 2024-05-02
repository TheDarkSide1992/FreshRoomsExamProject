import { Component } from '@angular/core';
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  public appPages = [
    { title: 'Home', url: '/folder/Home', icon: 'home' },
    { title: 'Manage', url: '/folder/Manage', icon: 'build' },
    { title: 'Rooms', url: '/folder/Rooms', icon: 'list' },
    { title: 'Account', url: '/folder/User', icon: 'person-circle' },
  ];
  public labels = ['Living Room', 'Bathroom', 'Bedroom'];
  constructor() {}

}
