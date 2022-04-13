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
  public figuresOnBoard: Figure[] = [];
  public figuresOffBoard: NewFigure[] = [];
  public tiles: Tile[][] = [];
  public activeFigure?: Figure;

  constructor(http: HttpClient) {
    this.httpClient = http;
    this.tiles = this.generateBoard();
  }

  private getFiguresOnBoardFromApi() {
    return this.httpClient.get<Figure[]>(this.API_URL);
  }

  private getFiguresOffBoardFromApi() {
    return this.httpClient.get<Figure[]>(this.API_URL + '/available');
  }

  ngOnInit(): void {
    this.getFiguresOnBoardFromApi().subscribe(result => this.onFiguresResult(result), error => console.error(error));
    this.getFiguresOffBoardFromApi().subscribe(result => this.onFiguresOfBoardResult(result), error => console.error());
  }

  onTileClicked(tileClicked: Tile) {
    if (this.activeFigure == undefined) { // without active figure
      if (tileClicked.occupiedBy != undefined) { // tile clicked has figure
        this.selectFigureAndShowMoves(tileClicked.occupiedBy as Figure);
      }
    } else if (this.activeFigure != undefined) { // with active figure
      if ("id" in this.activeFigure) { // active figure is on a board
        let activeFromBoard = this.activeFigure as Figure;
        if (tileClicked.occupiedBy?.player == activeFromBoard.player) { // if tileClicked has friendly figure
          this.selectFigureAndShowMoves(activeFromBoard);
        } else { // if tileClicked is free or has opponent
          let desiredPosition: Position = {
            x: tileClicked.x,
            y: tileClicked.y
          }
          this.sendMoveFigureRequest(activeFromBoard, desiredPosition).subscribe(
            result => this.onMoveSuccess(result),
            error => this.onMoveFailure(error));
        }
      }
      else {
        this.sendPutNewFigureRequest(this.activeFigure).subscribe(
          result => this.onNewFigureSuccess(result),
          error => this.onNewFigureFailure(error)
        );
      }
    }
  }
  selectFigureAndShowMoves(figure: Figure) {
    this.activeFigure = figure;
    this.showMovesForFigure(figure);
  }

  onMoveSuccess(result: Figure[]) {
    this.cancelSelection();
    this.onFiguresResult(result);
  }

  onMoveFailure(error: any) {
    this.cancelSelection();
    console.error(error);
  }

  onNewFigureSuccess(result: Figure[]) {
    this.cancelSelection();
    this.onFiguresResult(result);
  }

  onNewFigureFailure(error: any) {
    this.cancelSelection();
    console.error(error);
  }

  onFiguresResult(result: Figure[]) {
    this.reloadFiguresOnBoard(result);
    this.refreshAndPopulateTiles();
  }

  onFiguresOfBoardResult(result: NewFigure[]) {
    result.forEach(fig => {
      this.figuresOffBoard.push({
        player: fig.player,
        figureType: fig.figureType,
        imagePath: imagePathFor(fig)
      });
    })
  }

  reloadFiguresOnBoard(result: Figure[]) {
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

  refreshAndPopulateTiles() {
    this.tiles.forEach(
      row => row.forEach(
        tile => tile.occupiedBy = this.figuresOnBoard.find(
          fig => fig.x == tile.x && fig.y == tile.y)
      )
    );
    this.figuresOnBoard.forEach(fig => this.tiles[fig.x][fig.y].occupiedBy = fig);
  }

  private sendPutNewFigureRequest(newFigure: Figure) {
    let requestUrl = this.API_URL;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    let request: Observable<Figure[]> = this.httpClient.post<Figure[]>(requestUrl, newFigure, httpOptions);
    return request;
  }

  private sendMoveFigureRequest(figure: Figure, desiredPosition: Position) {
    let requestUrl = this.API_URL + '/' + figure.id;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    let request: Observable<Figure[]> = this.httpClient.put<Figure[]>(requestUrl, desiredPosition, httpOptions);
    return request;
  }

  private showMovesForFigure(figure: Figure) {
    // mark possible move's tiles
    this.httpClient.get<Position[]>(this.API_URL + '/getAllowedMoves/' + figure.id).subscribe(result => this.markAsAllowedToMove(result));
  }

  markAsAllowedToMove(result: Position[]): void {
    this.tiles.forEach(row => row.forEach(tile => {
      let isAllowedToMove = result.find(move => move.x == tile.x && move.y == tile.y);
      if (isAllowedToMove) {
        tile.markedToMove = true;
      }
    }))
  }

  cancelSelection() {
    this.activeFigure = undefined;
    this.unmarkAllTiles();
  }

  unmarkAllTiles() {
    this.tiles.forEach(row => row.forEach(tile => {
      tile.markedToMove = false;
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
        let tile: Tile = { x: x, y: y, class: currentTileClass, occupiedBy: undefined, readablePosition: readableColumnName + readableRowNumber, markedToMove: false };
        column[y] = tile;
      }
      tiles[x] = column;
    }
    return tiles;
  }



  title = 'ChessGameFront';
}

function imagePathFor(fig: Figure | NewFigure): string {
  return 'assets/pieces/' + fig.figureType.toLowerCase() + fig.player.charAt(0).toUpperCase() + '.png';
}

interface Position {
  x: number;
  y: number;
}

interface NewFigure {
  player: string;
  figureType: string;
  imagePath: string;
}
interface Figure {
  id: string;
  x: number;
  y: number;
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
  markedToMove: boolean;
}

