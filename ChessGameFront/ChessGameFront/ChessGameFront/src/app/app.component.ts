import { Component, OnInit } from '@angular/core';
import { NewFigureDto } from '../dto/new-figure-dto';
import { GameManagerService } from '../manager/game-manager.service';
import { Figure } from '../model/figure';
import { PlayingFigure } from '../model/playing-figure';
import { Position } from '../model/position';
import { Tile } from '../model/tile';
import { ChessApiClientService } from '../service/chess-api-client.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  private apiService: ChessApiClientService;
  private gameManager: GameManagerService;
  public figuresOffBoard: Figure[] = [];
  public tiles: Tile[][] = [];

  constructor(apiService: ChessApiClientService, gameManager: GameManagerService) {
    this.apiService = apiService;
    this.gameManager = gameManager;
  }

  ngOnInit(): void {
    this.apiService.getNewFigures().subscribe(result => this.onNewFiguresResult(result), error => console.error());
    this.gameManager.getTiles().subscribe(result => this.tiles = result, error => console.error(error));
  }

  public cancelSelection() {
    this.gameManager.cancelSelection();
  }

  onTileClicked(tileClicked: Tile) {
    this.gameManager.onTileClicked(tileClicked);
  }
  onNewFigureClicked(figureClicked: Figure) {
    this.gameManager.setNewFigureAsActive(figureClicked);
  }

  onNewFiguresResult(result: Figure[]) {
    result.forEach(fig => {
      this.figuresOffBoard.push({
        player: fig.player,
        figureType: fig.figureType,
        imagePath: imagePathFor(fig)
      });
    })
  }

  title = 'ChessGameFront';
}

//TODO export creation to another file
export function imagePathFor(fig: PlayingFigure | Figure): string {
  return 'assets/pieces/' + fig.figureType.toLowerCase() + fig.player.charAt(0).toUpperCase() + '.png';
}
