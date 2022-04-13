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
  private activeFigure?: FigureOnBoard;

  constructor(http: HttpClient) {
    this.httpClient = http;

    this.getFiguresFromApi().subscribe(result => this.loadFigures(result), error => console.error(error));
  }

    private getFiguresFromApi() {
        return this.httpClient.get<FigureOnBoard[]>(this.API_URL);
    }

  ngOnInit(): void {
    this.tiles = this.generateBoard();
  }

  onFigureClicked(clicked: any) {
    this.unmarkAllowedToMove();
    this.activeFigure = clicked as FigureOnBoard;
    let figure = clicked as FigureOnBoard;

    this.tiles.forEach(row => row.forEach(tile => {
      if (tile.x == figure.x && tile.y == figure.y) tile.class += ' to-move-tile';
    }));
    this.httpClient.get<Position[]>(this.API_URL + '/getAllowedMoves/' + figure.id).subscribe(result => this.markAsAllowedToMove(result));
  }

  onTileClicked(destination: Tile) {
    if (this.activeFigure != undefined) {
      let actualFigure = this.activeFigure;
      let desiredPosition: Position = {
        x: destination.x,
        y: destination.y
      }
      this.httpClient.post<FigureOnBoard[]>(this.API_URL + actualFigure.id, desiredPosition)
        .subscribe(result => this.loadFigures(result),
                    error => console.error(error));
    }
  }

  markAsAllowedToMove(result: Position[]): void {
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
    for (let x = 0; x <= 7; x++) {
      let readableRowNumber: string = (-(x - 8)).toString();
      let column: Tile[] = [];
      for (let y = 0; y <= 7; y++) {
        let readableColumnName = String.fromCharCode(asciiNumberOfLetterA + y);
        let currentTileClass = (x + y) % 2 == 0 ? 'black-tile' : 'white-tile';
        let tile: Tile = { x: x, y: y, class: currentTileClass, occupiedBy: undefined, readablePosition: readableColumnName + readableRowNumber };
        column[y] = tile;
      }
      tiles[x] = column;
    }

    // reversing Y order, in order to make border look naturally
    return tiles;
  }

  loadFigures(result: FigureOnBoard[]) {
    // removes all figures from the board
    this.figuresOnBoard = [];

    // populates figures array
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

    this.flushTiles();
  }

  flushTiles() {
    //this.forEachTile(tile => tile.occupiedBy = this.figuresOnBoard?.find(fig => fig.x == tile.x && fig.y == tile.y));
    this.figuresOnBoard.forEach(fig => this.tiles[fig.x][fig.y].occupiedBy = fig);
    //this.tiles.forEach(row => {
    //  row.forEach(tile => {
    //    tile.occupiedBy = this.figuresOnBoard?.find(fig => fig.x == tile.x && fig.y == tile.y);
    //  })
    //})
  }

  //forEachTile(action: Function) {
  //  this.tiles.forEach(row => {
  //    row.forEach(tile => {
  //      action(tile);
  //    })
  //}

  title = 'ChessGameFront';
}

function imagePathFor(fig: FigureOnBoard): string {
  return 'assets/pieces/' + fig.figureType.toLowerCase() + fig.player.charAt(0).toUpperCase() + '.png';
}

interface Position {
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

