import { HttpErrorResponse } from '@angular/common/http';
import { not } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { imagePathFor } from '../app/app.component';
import { NewFigureDto } from '../dto/new-figure-dto';
import { Figure } from '../model/figure';
import { PlayingFigure } from '../model/playing-figure';
import { Position } from '../model/position';
import { Tile } from '../model/tile';
import { ChessApiClientService } from '../service/chess-api-client.service';

@Injectable({
  providedIn: 'root'
})
export class GameManagerService {
  private apiService;
  private figuresOnBoard: PlayingFigure[] = [];
  private activeFigure?: PlayingFigure | Figure;
  private tiles: Tile[][] = [];
  private tilesSubject = new Subject<Tile[][]>();
  private messageSubject = new Subject<string>();

  constructor(apiService: ChessApiClientService) {
    this.apiService = apiService;
    this.tiles = this.generateBoard();
    this.apiService.getPlayingFigures().subscribe(result => this.onFiguresResponse(result), error => console.error(error));
  }

  public getTiles() {
    return this.tilesSubject.asObservable();
  }

  public getMessage() {
    return this.messageSubject.asObservable();
}

  public setNewFigureAsActive(newFigure: Figure) {
    this.activeFigure = newFigure;
  }

  public cancelSelection() {
    this.activeFigure = undefined;
    this.unmarkAllTiles();
    return this.tiles;
  }

  public onTileClicked(tileClicked: Tile) {
    console.info(`Tile clicked: ${tileClicked.x}, ${tileClicked.y}`);
    if (!this.activeFigure && tileClicked.occupiedBy) {
      this.selectFigureAndShowMoves(tileClicked.occupiedBy as PlayingFigure);
    } else if (this.activeFigure && "id" in this.activeFigure) {
      // with active figure playing
      if (tileClicked.occupiedBy?.player == this.activeFigure.player) {
        // if tileClicked is occupied by friendly figure
        this.cancelSelection();
        this.selectFigureAndShowMoves(tileClicked.occupiedBy);
      } else {
        // if tileClicked is free or has opponent
        this.moveFigure(tileClicked, this.activeFigure);
      }
    } else if (this.activeFigure && !("id" in this.activeFigure)) {
      // if active figure is not playing (new figure)
      this.putFigure(tileClicked, this.activeFigure);

    }
  }

  private putFigure(tileClicked: Tile, newFigure: Figure) {
    console.info(`Action: put figure`);
    let desiredFigure: NewFigureDto = {
      x: tileClicked.x,
      y: tileClicked.y,
      player: newFigure.player,
      figureType: newFigure.figureType
    };
    this.apiService.putNewFigure(desiredFigure).subscribe(
      result => this.onFiguresResponse(result),
      error => this.onFiguresResponseError(error)
    );
  }

  private moveFigure(destinationTile: Tile, figure: PlayingFigure) {
    console.info(`Action: move figure`);
    let desiredPosition: Position = {
      x: destinationTile.x,
      y: destinationTile.y
    };
    this.apiService.moveFigure(figure, desiredPosition).subscribe(
      result => this.onFiguresResponse(result),
      error => this.onFiguresResponseError(error));
  }

  private selectFigureAndShowMoves(figure: PlayingFigure) {
    console.info(`Action: select and show moves`);
    this.activeFigure = figure;
    this.apiService.getMovesForFigure(figure).subscribe(result => this.onAllowedMovesResponse(result), error => this.onFiguresResponseError(error));
  }

  private onFiguresResponse(result: PlayingFigure[]) {
    this.cancelSelection();
    this.refreshContext(result);
    this.shareMessage("Ok");
  }

  private onFiguresResponseError(error: HttpErrorResponse) {
    this.cancelSelection();
    console.error(error);
    this.shareMessage(error.error);
  }

  private onAllowedMovesResponse(result: Position[]): void {
    this.markAllowedToMoveTiles(result);
  }

  private refreshContext(result: PlayingFigure[]) {
    this.reloadPlayingFigures(result);
    this.updateFiguresOnBoard();
  }

  private reloadPlayingFigures(result: PlayingFigure[]) {
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

  private updateFiguresOnBoard() {
    this.tiles.forEach(
      row => row.forEach(
        tile => tile.occupiedBy = this.figuresOnBoard.find(
          fig => fig.x == tile.x && fig.y == tile.y)
      )
    );
    this.figuresOnBoard.forEach(fig => this.tiles[fig.x][fig.y].occupiedBy = fig);

    this.shareResult();
  }


  private markAllowedToMoveTiles(result: Position[]) {
    this.tiles.forEach(row => row.forEach(tile => {
      let isAllowedToMove = result.find(move => move.x == tile.x && move.y == tile.y);
      if (isAllowedToMove) {
        tile.markedToMove = true;
      }
    }));
    this.shareResult();
  }

  private unmarkAllTiles() {
    this.tiles.forEach(row => row.forEach(tile => {
      tile.markedToMove = false;
    }))

    this.shareResult();
  }

  private shareMessage(message: string) {
    this.messageSubject.next(message);
  }

  private shareResult() {
    this.tilesSubject.next(this.tiles);
  }

  private generateBoard(): Tile[][] {
    let tiles: Tile[][] = [];
    let asciiNumberOfLetterA = 65;
    for (let x = 0; x <= 7; x++) {
      let readableColumnName = String.fromCharCode(asciiNumberOfLetterA + x);
      let column: Tile[] = [];
      for (let y = 0; y <= 7; y++) {
      let readableRowNumber: string = (y+1).toString();
        let currentTileClass = (x + y) % 2 == 0 ? 'tile-black' : 'tile-white';
        let tile: Tile = { x: x, y: y, class: currentTileClass, occupiedBy: undefined, readablePosition: readableColumnName + readableRowNumber, markedToMove: false };
        column[y] = tile;
      }
      tiles[x] = column;
    }
    return tiles;
  }
}
