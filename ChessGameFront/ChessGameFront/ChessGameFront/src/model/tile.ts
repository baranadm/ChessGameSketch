import { PlayingFigure } from "./playing-figure";

export interface Tile {
  x: number;
  y: number;
  class: string;
  occupiedBy?: PlayingFigure;
  readablePosition: string;
  markedToMove: boolean;
}
