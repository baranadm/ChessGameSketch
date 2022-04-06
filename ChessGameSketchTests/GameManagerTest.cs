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
    public class GameManagerTest
    {
        GameManager underTest = new GameManager();

        [TestMethod]
        public void GetAllowedMoves_ForPawn_CorrectFirstAllowedMoves()
        {
            // Arrange
            Board board = new Board();

            Pawn pawn = new Pawn(new Vector2(0, 1), Player.White);
            board.PutFigure(pawn);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, pawn);

            //Assert
            List<Vector2> expected = new List<Vector2>() { new Vector2(0, 3), new Vector2(0, 2) };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForPawn_CorrectSecondAllowedMove()
        {
            // Arrange
            Board board = new Board();

            Pawn pawn = new Pawn(new Vector2(0, 2), Player.White);
            board.PutFigure(pawn);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, pawn);

            //Assert
            List<Vector2> expected = new List<Vector2>() { new Vector2(0, 3) };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForPawn_WhenOnTheLastTile_NoAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            
            Pawn pawn = new Pawn(new Vector2(0, 7), Player.White);
            board.PutFigure(pawn);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, pawn);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetAllowedMoves_ForPawn_WhenFigureAhead_NoAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            
            Pawn pawn = new Pawn(new Vector2(0, 3), Player.White);
            board.PutFigure(pawn);
            
            Bishop bishop = new Bishop(new Vector2(0, 4), Player.White);
            board.PutFigure(bishop);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, pawn);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetAllowedMoves_ForBishop_WhenNoFiguresOnTheWay_CorrectAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            
            Bishop bishop = new Bishop(new Vector2(2, 0), Player.White);
            board.PutFigure(bishop);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, bishop);

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
            List<Vector2> result = underTest.GetAllowedMoves(board, bishop);

            //Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(1, 7),
                new Vector2(2, 6),
                new Vector2(3, 5),
                new Vector2(3, 3),
                new Vector2(5, 5),
                new Vector2(5, 3),
                new Vector2(6, 2),
                new Vector2(2, 2),
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
            List<Vector2> result = underTest.GetAllowedMoves(board, knight);

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

        [TestMethod]
        public void GetAllowedMoves_ForKing_WhenFiguresOnTheWay_CorrectAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            King king = new King(new Vector2(0, 1), Player.White);
            Pawn obstaclePawn = new Pawn(new Vector2(1, 1), Player.White);
            board.PutFigure(king);
            board.PutFigure(obstaclePawn);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, king);

            //Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(0, 2),
                new Vector2(1, 2),
                new Vector2(0, 0),
                new Vector2(1, 0),
            };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForQueen_WhenFiguresOnTheWay_CorrectAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            Queen queen = new Queen(new Vector2(3, 4), Player.White);
            Pawn obstaclePawn1 = new Pawn(new Vector2(1, 2), Player.White);
            Pawn obstaclePawn2 = new Pawn(new Vector2(3, 5), Player.White);
            Pawn obstaclePawn3 = new Pawn(new Vector2(4, 5), Player.White);
            board.PutFigure(queen);
            board.PutFigure(obstaclePawn1);
            board.PutFigure(obstaclePawn2);
            board.PutFigure(obstaclePawn3);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, queen);

            //Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(0, 7),
                new Vector2(0, 4),
                new Vector2(1, 6),
                new Vector2(1, 4),
                new Vector2(2, 5),
                new Vector2(2, 4),
                new Vector2(2, 3),
                new Vector2(3, 3),
                new Vector2(3, 2),
                new Vector2(3, 1),
                new Vector2(3, 0),
                new Vector2(4, 4),
                new Vector2(4, 3),
                new Vector2(5, 4),
                new Vector2(5, 2),
                new Vector2(6, 4),
                new Vector2(6, 1),
                new Vector2(7, 4),
                new Vector2(7, 0)
            };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForRook_WhenFiguresOnTheWay_CorrectAllowedMoves()
        {
            // Arrange
            Board board = new Board();
            Rook rook = new Rook(new Vector2(5, 4), Player.Black);
            Pawn obstaclePawn = new Pawn(new Vector2(3, 4), Player.Black);
            board.PutFigure(rook);
            board.PutFigure(obstaclePawn);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, rook);

            //Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(5, 7),
                new Vector2(5, 6),
                new Vector2(5, 5),
                new Vector2(5, 3),
                new Vector2(5, 2),
                new Vector2(5, 1),
                new Vector2(5, 0),
                new Vector2(4, 4),
                new Vector2(6, 4),
                new Vector2(7, 4),
            };
            result.Should().BeEquivalentTo(expected);
        }
        //TODO move to Board tests
        //[TestMethod]
        //public void PutFigure_AddsFigureToList_AndPutsFigureOnTile()
        //{
        //    // Arrange
        //    Board board = new Board();
        //    Rook rook = new Rook(new Vector2(3, 3), Player.White);

        //    // Act
        //    board.PutFigure(rook);

        //    // Assert
        //    underTest.FigureAt(rook.Position).Should().Be(rook);
        //}

        //[TestMethod]
        //public void MoveFigure_UpdatesFiguresPositionInList_AndUpdatesTiles()
        //{
        //    // Arrange
        //    Board board = new Board();
        //    Vector2 initialPosition = new Vector2(3, 3);
        //    Rook rook = new Rook(initialPosition, Player.White);
        //    board.PutFigure(rook);

        //    // Act
        //    Vector2 newPosition = new Vector2(4, 4);
        //    board.MoveFigure(rook, newPosition);

        //    // Assert
        //    board.FiguresOnBoard.Find(new Predicate<Figure>(fig => fig.Equals(rook))).position.Equals(newPosition);
        //    underTest.FigureAt(initialPosition).Should().BeNull();
        //    underTest.FigureAt(newPosition).Should().Be(rook);
        //}

        //[TestMethod]
        //public void DeleteFigure_RemovesFigureFromList_AndFromTiles()
        //{
        //    // Arrange
        //    Board board = new Board();
        //    Rook rook = new Rook(new Vector2(3, 3), Player.White);
        //    board.PutFigure(rook);

        //    // Act
        //    board.DeleteFigure(rook);

        //    // Assert
        //    underTest.FigureAt(rook.Position).Should().BeNull();
        //}

        [TestMethod]
        public void GetAllowedMoves_ForWhiteKing_WhenBlackRookAttacksKing_ReturnsCorrectMoves()
        {
            // Arrange
            Board board = new Board();
            King blackKing = new King(new Vector2(5, 5), Player.Black);
            board.PutFigure(blackKing);
            King whiteKing = new King(new Vector2(0, 4), Player.White);
            board.PutFigure(whiteKing);
            Rook rook = new Rook(new Vector2(4, 4), Player.Black);
            board.PutFigure(rook);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, whiteKing);

            // Assert
            List<Vector2> expected = new List<Vector2>() { 
                new Vector2(0, 5),
                new Vector2(1, 5),
                new Vector2(0, 3),
                new Vector2(1, 3),
            };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForBlackKing_WhenWhiteRookAttacksKing_ReturnsCorrectMoves()
        {
            // Arrange
            Board board = new Board();
            King whiteKing = new King(new Vector2(5, 5), Player.White);
            board.PutFigure(whiteKing);
            King blackKing = new King(new Vector2(0, 4), Player.Black);
            board.PutFigure(blackKing);
            Rook rook = new Rook(new Vector2(4, 4), Player.White);
            board.PutFigure(rook);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, blackKing);

            // Assert
            List<Vector2> expected = new List<Vector2>() { 
                new Vector2(0, 5),
                new Vector2(1, 5),
                new Vector2(0, 3),
                new Vector2(1, 3),
            };
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GetAllowedMoves_ForWhitePawn_WhenWhiteKingIsCoveredByThatPawn_ReturnsEmpty()
        {
            // Arrange
            Board board = new Board();
            King whiteKing = new King(new Vector2(0, 4), Player.White);
            board.PutFigure(whiteKing);
            King blackKing = new King(new Vector2(5, 5), Player.Black);
            board.PutFigure(blackKing);
            Pawn coveringPawn = new Pawn(new Vector2(1, 4), Player.White);
            board.PutFigure(coveringPawn);
            Rook rook = new Rook(new Vector2(4, 4), Player.Black);
            board.PutFigure(rook);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, coveringPawn);

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetAllowedMoves_ForWhitePawn_WhenWhitePawnCanAttackOrMove_ReturnsCorrectMoves()
        {
            // Arrange
            Board board = new Board();

            Pawn whitePawn = new Pawn(new Vector2(4, 4), Player.White);
            board.PutFigure(whitePawn);

            Rook blackRookUpperRight = new Rook(new Vector2(5, 5), Player.Black);
            board.PutFigure(blackRookUpperRight);

            Rook blackRookUpperLeft = new Rook(new Vector2(3, 5), Player.Black);
            board.PutFigure(blackRookUpperLeft);

            // Act
            List<Vector2> result = underTest.GetAllowedMoves(board, whitePawn);

            // Assert
            List<Vector2> expected = new List<Vector2>() {
                new Vector2(4, 5),
                new Vector2(3, 5),
                new Vector2(5, 5),
            };
            result.Should().BeEquivalentTo(expected);
        }
    }
}
