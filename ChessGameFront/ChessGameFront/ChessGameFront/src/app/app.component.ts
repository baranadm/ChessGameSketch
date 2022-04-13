import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { generate, Observable } from 'rxjs';

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
    this.tiles = this.generateBoard();
  }

    private getFiguresFromApi() {
        return this.httpClient.get<FigureOnBoard[]>(this.API_URL);
    }

  ngOnInit(): void {
    this.getFiguresFromApi().subscribe(result => this.onFiguresResult(result), error => console.error(error));
  }

  onFigureClicked(clicked: any) {
    this.unmarkAllTiles();
  }

  onTileClicked(tileClicked: Tile) {

    // without active figure
    if (this.activeFigure == undefined) {
      // tile clicked has figure
      if (tileClicked.occupiedBy != undefined) {
        this.actionForNewActiveFgure(tileClicked);
      } else {
        this.unmarkAllTiles();
      }
    } else {
      if (tileClicked.occupiedBy?.player == this.activeFigure.player) {
        this.actionForNewActiveFgure(tileClicked);
      } else {
        let desiredPosition: Position = {
          x: tileClicked.x,
          y: tileClicked.y
        }

        this.sendMoveFigureRequest(this.activeFigure, desiredPosition).subscribe(result => this.onFiguresResult(result),
          error => console.error(error));
      }
    }
  }

  onFiguresResult(result: FigureOnBoard[]) {
    this.processFiguresResponse(result);
    this.flushTiles();
  }

  processFiguresResponse(result: FigureOnBoard[]) {
    // cleans board
    this.figuresOnBoard = [];
    // populates figures array with mapped result's figures
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
  }

  flushTiles() {
    this.tiles.forEach(
      row => row.forEach(
        tile => tile.occupiedBy = this.figuresOnBoard.find(
          fig => fig.x == tile.x && fig.y == tile.y)
      )
    );
    this.figuresOnBoard.forEach(fig => this.tiles[fig.x][fig.y].occupiedBy = fig);
  }

  private sendMoveFigureRequest(figure: FigureOnBoard, desiredPosition: Position) {
    let requestUrl = this.API_URL + '/' + figure.id;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    let request: Observable<FigureOnBoard[]> = this.httpClient.put<FigureOnBoard[]>(requestUrl, desiredPosition, httpOptions);
    return request;
  }

  private actionForNewActiveFgure(tileClicked: Tile) {
    this.unmarkAllTiles();
    console.info(`Old active figure: ${this.activeFigure}`);
    this.activeFigure = tileClicked.occupiedBy as FigureOnBoard;
    console.info(`New active figure:`);
    console.info(this.activeFigure);
    // mark current figure's tile
    tileClicked.class += " tile-to-move";

    // mark possible move's tiles
    this.httpClient.get<Position[]>(this.API_URL + '/getAllowedMoves/' + this.activeFigure.id).subscribe(result => this.markAsAllowedToMove(result));
    }

  markAsAllowedToMove(result: Position[]): void {
    this.tiles.forEach(row => row.forEach(tile => {
      let isAllowedToMove = result.find(move => move.x == tile.x && move.y == tile.y);
      if (isAllowedToMove) {
        tile.class += ' tile-to-move';
      }
    }))
  }

  cancelSelection() {
    this.activeFigure = undefined;
    this.unmarkAllTiles();
  }

  unmarkAllTiles() {
    this.tiles.forEach(row => row.forEach(tile => {
      tile.class = tile.class.replace('tile-to-move', '');
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
        let currentTileClass = (x + y) % 2 == 0 ? 'tile-black' : 'tile-white';
        let tile: Tile = { x: x, y: y, class: currentTileClass, occupiedBy: undefined, readablePosition: readableColumnName + readableRowNumber };
        column[y] = tile;
      }
      tiles[x] = column;
    }
    return tiles;
  }



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

