import { Figure } from "./figure";

export interface PlayingFigure extends Figure {
  id: string;
  x: number;
  y: number;
}
