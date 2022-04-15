import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NewFigureDto } from '../dto/new-figure-dto';
import { GameManagerService } from '../manager/game-manager.service';
import { Figure } from '../model/figure';
import { PlayingFigure } from '../model/playing-figure';
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
  public message: string = "";

  constructor(apiService: ChessApiClientService, gameManager: GameManagerService) {
    this.apiService = apiService;
    this.gameManager = gameManager;
  }

  ngOnInit(): void {
    this.apiService.getNewFigures().subscribe(result => this.onNewFiguresResult(result), error => this.handleError(error));

    this.gameManager.getTiles().subscribe(result => this.tiles = result, error => this.handleError(error));
    this.gameManager.getMessage().subscribe(result => this.message = result, error => this.handleError(error));
  }

  public cancelSelection() {
    this.gameManager.cancelSelection();
  }

  onTileClicked(tileClicked: Tile) {
    this.gameManager.onTileClicked(tileClicked);
    this.showMessage(tileClicked);
  }
  private showMessage(clicked: any) {

    if ("occupiedBy" in clicked) {
      this.message = `Selected tile [${clicked.x}, ${clicked.y}]`;
      if (clicked.occupiedBy) this.message += ` with ${clicked.occupiedBy.player} ${clicked.occupiedBy.figureType}.`;
      else this.message += ' with no figure.';
    } else if ("player" in clicked && !("x" in clicked)){
      this.message = `New ${clicked.player} ${clicked.figureType} selected.`;

    }
  }

  onNewFigureClicked(figureClicked: Figure) {
    this.gameManager.setNewFigureAsActive(figureClicked);
    this.showMessage(figureClicked);
  }

  onNewFiguresResult(result: NewFigureDto[]) {
    result.forEach(fig => {
      
      this.figuresOffBoard.push({
        player: fig.player,
        figureType: fig.figureType,
        imagePath: imagePathFor(fig)
      });
    })
  }

  title = 'ChessGameFront';

  handleError(error: HttpErrorResponse) {
    this.message = error.error;
    console.log(error);
  }
}


//TODO export creation to another file
export function imagePathFor(fig: PlayingFigure | Figure | NewFigureDto): string {
  return 'assets/pieces/' + fig.figureType.toLowerCase() + fig.player.charAt(0).toUpperCase() + '.png';
}
