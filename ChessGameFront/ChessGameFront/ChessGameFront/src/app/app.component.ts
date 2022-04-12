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
  private readonly httpClient;
  public figuresOnBoard: FigureOnBoard[] = [];
  public tiles: Tile[][] = [];

  constructor(http: HttpClient) {
    this.httpClient = http;

    this.httpClient.get<FigureOnBoard[]>(this.API_URL).subscribe(result => this.loadFigures(result), error => console.error(error));
  }

  ngOnInit(): void {
    this.tiles = this.generateBoard();
  }

  onPieceClicked(clicked: any) {
    this.unmarkAllowedToMove();
    let figure = clicked as FigureOnBoard;

    this.tiles.forEach(row => row.forEach(tile => {
      if (tile.x == figure.x && tile.y == figure.y) tile.class += ' to-move-tile';
    }));
    this.httpClient.get<AllowedToMove[]>(this.API_URL + '/getAllowedMoves/' + figure.id).subscribe(result => this.markAsAllowedToMove(result));
  }

  markAsAllowedToMove(result: AllowedToMove[]): void {
    this.tiles.forEach(row => row.forEach(tile => {
      let isAllowedToMove = result.find(move => move.x == tile.x && move.y == tile.y);
      if (isAllowedToMove) {
        tile.class += ' to-move-tile';
      }
    }))
  }

  unmarkAllowedToMove() {
    this.tiles.forEach(row => row.forEach(tile => {
      tile.class = tile.class.replace('to-move-tile', '');
    }))
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
    result.forEach(fig => {
      this.figuresOnBoard.push({
        id: fig.id,
        x: fig.x,
        y: fig.y,
        player: fig.player,
        figureType: fig.figureType,
        imagePath: imagePathFor(fig)
      });
    })
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

function imagePathFor(fig: FigureOnBoard): string {
  return 'assets/pieces/' + fig.figureType.toLowerCase() + fig.player.charAt(0).toUpperCase() + '.png';
}

interface AllowedToMove {
  x: number;
  y: number;
}
interface FigureOnBoard extends Figure {
  id: string;
  x: number;
  y: number;
}
interface Figure {
  player: string;
  figureType: string;
  imagePath: string;
}

interface Tile {
  x: number;
  y: number;
  class: string;
  occupiedBy?: Figure;
  readablePosition: string;
}

