using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ChessGameSketch
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void GetAllowedMoves_ForPawn_CorrectFirstAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            board.PutFigure(new Pawn(new Vector2(0, 1), Player.White));

            // Act
            List<Vector2> result = board.GetAllowedMoves(board.FigureAt(new Vector2(0, 1)));

            //Assert
            List<Vector2> expected = new List<Vector2>() { new Vector2(0, 3), new Vector2(0, 2) };
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetAllowedMoves_ForPawn_CorrectSecondAllowedMove()
        {
            // Arrange
            Board board = new Board();
            board.PutFigure(new Pawn(new Vector2(0, 2), Player.White));

            // Act
            List<Vector2> result = board.GetAllowedMoves(board.FigureAt(new Vector2(0, 2)));

            //Assert
            List<Vector2> expected = new List<Vector2>() { new Vector2(0, 3)};
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void GetAllowedMoves_ForPawn_WhenOnTheLastTile_NoAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            board.PutFigure(new Pawn(new Vector2(0, 7), Player.White));

            // Act
            List<Vector2> result = board.GetAllowedMoves(board.FigureAt(new Vector2(0, 7)));

            //Assert
            result.Should().BeEmpty();
        }
        [TestMethod]
        public void GetAllowedMoves_ForPawn_WhenFigureAhead_NoAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            board.PutFigure(new Pawn(new Vector2(0, 3), Player.White));
            board.PutFigure(new Bishop(new Vector2(0, 4), Player.White));

            // Act
            List<Vector2> result = board.GetAllowedMoves(board.FigureAt(new Vector2(0, 3)));

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetAllowedMoves_ForBishop_WhenNoFiguresOnTheWay_CorrectAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            board.PutFigure(new Bishop(new Vector2(2, 0), Player.White));

            // Act
            List<Vector2> result = board.GetAllowedMoves(board.FigureAt(new Vector2(2, 0)));

            //Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(1, 1),
                new Vector2(3, 1),
                new Vector2(0, 2),
                new Vector2(4, 2),
                new Vector2(5, 3),
                new Vector2(6, 4),
                new Vector2(7, 5),
            };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForBishop_WhenFiguresOnTheWay_CorrectAllowedMoves()
        {
            // Arrange
            Bishop bishop = new Bishop(new Vector2(4, 4), Player.White);

            Pawn obstaclePawn1 = new Pawn(new Vector2(6, 6), Player.White);
            Pawn obstaclePawn2 = new Pawn(new Vector2(7, 1), Player.White);
            Queen obstacleQueen = new Queen(new Vector2(2, 2), Player.Black);

            Board board = new Board();
            board.PutFigure(bishop);
            board.PutFigure(obstaclePawn1);
            board.PutFigure(obstaclePawn2);
            board.PutFigure(obstacleQueen);

            // Act
            List<Vector2> result = board.GetAllowedMoves(board.FigureAt(bishop.position));

            //Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(1, 7),
                new Vector2(2, 6),
                new Vector2(3, 5),
                new Vector2(3, 3),
                new Vector2(5, 5),
                new Vector2(5, 3),
                new Vector2(6, 2),
            };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForKnight_WhenNoFiguresOnTheWay_CorrectAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            Knight knight = new Knight(new Vector2(6, 5), Player.White);
            board.PutFigure(knight);

            // Act
            List<Vector2> result = board.GetAllowedMoves(knight);

            //Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(4, 6),
                new Vector2(4, 4),
                new Vector2(5, 3),
                new Vector2(7, 3),
                new Vector2(7, 7),
                new Vector2(5, 7),
            };
            result.Should().BeEquivalentTo(expected);
        }
    }
}
