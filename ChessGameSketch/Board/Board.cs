using System.Numerics;

namespace ChessGameSketch
{
    public class Board
    {
        public Tile[,] tiles;

        public Board()
        {
            tiles = CreateTiles();
        }
        
        public void Show()
        {
            for (int y = tiles.GetLength(1) - 1; y >= 0; y--)
            {
                Console.Write($"Y{y}: ");
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Console.Write($"{tiles[x, y].Display()}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("------------------------------");
        }
        
        public void PutFigure(Figure newFigure)
        {
            tiles[(int)newFigure.position.X, (int)newFigure.position.Y].PutFigure(newFigure);
        }
        
        public void MoveFigure(Vector2 actualPosition, Vector2 newPosition)
        {
            Figure movedFigure = tiles[(int)actualPosition.X, (int)actualPosition.Y].TakeFigure();
            tiles[(int)newPosition.X, (int)newPosition.Y].PutFigure(movedFigure);
        }
        
        public void DeleteFigure(int x, int y)
        {
            tiles[x, y].TakeFigure();
        }

        public List<Vector2> GetAllowedMoves(Figure figure)
        {
            PossibleMoves possibleMoves = figure.GetPossibleMoves();

            List<Vector2> allowedMoves = new List<Vector2>();
            for(int i=0; i < possibleMoves.Directions.Count; i++)
            {
                Vector2 actualDirection = possibleMoves.Directions[i];
                Vector2 actualPosition = figure.position;
                while (true)
                {
                    Vector2 nextPosition = Vector2.Add(actualPosition, actualDirection);

                    if (nextPosition.X < 0 || nextPosition.X > 7 || nextPosition.Y < 0 || nextPosition.Y > 7) break;
                    if (FigureAt(nextPosition) == null)
                    {
                        allowedMoves.Add(nextPosition);
                        actualPosition.X = nextPosition.X;
                        actualPosition.Y = nextPosition.Y;
                    }
                    if (FigureAt(nextPosition) != null) break;
                    if (!possibleMoves.Repeatable) break;
                }

            }
            return allowedMoves;
        }

        public Figure FigureAt(Vector2 position)
        {
            return tiles[(int)position.X, (int)position.Y].FigureStanding();
        }

        private Tile[,] CreateTiles()
        {
            Tile[,] tiles = new Tile[8, 8];
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    tiles[x, y] = new Tile(new Vector2(x, y));
                }
            }
            return tiles;
        }
    }
}