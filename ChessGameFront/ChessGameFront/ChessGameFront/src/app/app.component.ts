import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { generate } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  private readonly API_URL = 'https://localhost:7024/Chess';
  public figuresOnBoard: FigureOnBoard[] = [];
  public tiles: Tile[][] = [];

  constructor(http: HttpClient) {
    http.get<FigureOnBoard[]>(this.API_URL).subscribe(result => this.loadFigures(result), error => console.error(error));
  }

  ngOnInit(): void {
    this.tiles = this.generateBoard();
  }

  generateBoard(): Tile[][] {
    let tiles: Tile[][] = [];
    let asciiNumberOfLetterA = 65;
    for (let y = 0; y <= 7; y++) {
      let readableRowNumber: string = (-(y - 8)).toString();
      let column: Tile[] = [];
      for (let x = 7; x >= 0; x--) {
        let readableColumnName = String.fromCharCode(asciiNumberOfLetterA + x);
        let currentTileClass = (x + y) % 2 == 1 ? 'black-tile' : 'white-tile';
        let tile: Tile = { x: x, y: 7-y,class: currentTileClass, occupiedBy: undefined, readablePosition: readableColumnName + readableRowNumber };
        column[x] = tile;
      }
      tiles[y] = column;
    }
    return tiles;
  }

  loadFigures(result: FigureOnBoard[]) {
    console.info(result);
    this.figuresOnBoard = result;
    this.updateBoard();
  }

  updateBoard() {
    this.tiles.forEach(row => {
      row.forEach(tile => {
        tile.occupiedBy = this.figuresOnBoard?.find(fig => fig.x == tile.x && fig.y == tile.y);
      })
    })
  }

  title = 'ChessGameFront';
}

interface FigureOnBoard extends Figure {
  id: string;
  x: number;
  y: number;
}
interface Figure {
  player: string;
  figureType: string;
}

interface Tile {
  x: number;
  y: number;
  class: string;
  occupiedBy?: Figure;
  readablePosition: string;
}


