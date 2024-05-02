import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.component.css'],
})
export class HomePageComponent implements OnInit {
  public items = [];

  ngOnInit() {
    for (let i = 1; i < 51; i++) {
      // @ts-ignore
      this.cards.push(`Card ${i}`);
    }
  }
}
