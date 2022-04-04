using System.Numerics;

namespace ChessGameSketch
{
    public class Tile
    {
        protected Vector2 position;
        private char color;
        private Figure? figureStanding;

        public Tile(Vector2 position)
        {
            this.position = position;
            if ((position.X + position.Y) % 2 == 0)
            {
                color = 'O';
            }
            else
            {
                color = ' ';
            }
        }

        public void PutFigure(Figure figure)
        {
            figureStanding = figure;
            figureStanding.UpdatePosition(this.position);
        }

        public Figure TakeFigure()
        {
            Figure taken = figureStanding;
            figureStanding = null;
            return taken;
        }

        public Figure FigureStanding()
        {
            return figureStanding;
        }

        public char Display()
        {
            if (figureStanding == null)
            {
                return color;
            }
            else
            {
                return figureStanding.sign;
            }
        }
    }
}