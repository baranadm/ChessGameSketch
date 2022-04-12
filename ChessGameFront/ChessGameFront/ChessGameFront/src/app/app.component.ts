import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public figures?: Figure[];

  constructor(http: HttpClient) {
    http.get<Figure[]>('https://localhost:7024/Chess').subscribe(result => {
      this.figures = result;
    }, error => console.error(error));
  }

  title = 'ChessGameFront';
}

interface Figure {
  id: string;
  x: number;
  y: number;
  player: string;
  figureType: string;
}
