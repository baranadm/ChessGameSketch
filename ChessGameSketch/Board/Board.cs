using System.Numerics;

namespace ChessGameSketch
{
    public class Board
    {
        public Tile[,] tiles;
        public List<Figure> figuresAtBoard;

        public Board()
        {
            figuresAtBoard = new List<Figure>();
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
            figuresAtBoard.Add(newFigure);
        }
        
        public void MoveFigure(Vector2 actualPosition, Vector2 newPosition)
        {
            Figure movedFigure = tiles[(int)actualPosition.X, (int)actualPosition.Y].TakeFigure();
            tiles[(int)newPosition.X, (int)newPosition.Y].PutFigure(movedFigure);
        }
        
        public void DeleteFigure(Vector2 position)
        {
            Figure deleted = tiles[(int)position.X, (int)position.Y].TakeFigure();
            figuresAtBoard.Remove(deleted);
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

                    // break if out of bounds
                    if (nextPosition.X < 0 || nextPosition.X > 7 || nextPosition.Y < 0 || nextPosition.Y > 7) break;

                    Figure nextPositionFigure = FigureAt(nextPosition);
                    //break if next tile is occupied by another figure with same color (player)
                    if (nextPositionFigure != null && nextPositionFigure.player.Equals(figure.player)) break;

                    // if next tile is occupied by another figure of another color (different player), add this tile to possible moves (taking a figure) and break
                    if (nextPositionFigure != null && !nextPositionFigure.player.Equals(figure.player))
                    {
                        allowedMoves.Add(nextPosition);
                        break;
                    }

                    // if after moving king of the same color will be attacked, then break

                    // if ok, then add next tile to possible moves and proceed to to next tile
                    if (FigureAt(nextPosition) == null)
                    {
                        allowedMoves.Add(nextPosition);
                        actualPosition.X = nextPosition.X;
                        actualPosition.Y = nextPosition.Y;
                    }

                    // break if move is not repetable
                    if (!possibleMoves.Repeatable) break;
                }

            }
            return allowedMoves;
        }

        public King? GetKingUnderAttack()
        {
            King whiteKing = (King) figuresAtBoard.Find(fig => fig.GetType().Name.Equals("King") && fig.player.Equals(Player.White));
            if (whiteKing == null) throw new NoKingException("No white king on the board.");
            King blackKing = (King) figuresAtBoard.Find(fig => fig.GetType().Name.Equals("King") && fig.player.Equals(Player.Black));
            if (blackKing == null) throw new NoKingException("No black king on the board.");

            foreach (Figure figure in figuresAtBoard)
            {
                GetAllowedMoves(figure).ForEach(vector => Console.Write($"{vector} "));
                if (figure.player.Equals(Player.Black) &&
                    GetAllowedMoves(figure).Contains(whiteKing.position)) return whiteKing;
                if (figure.player.Equals(Player.White) &&
                    GetAllowedMoves(figure).Contains(blackKing.position)) return blackKing;
            }
            return null;
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