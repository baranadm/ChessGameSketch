using ChessGameSketch;
using ChessGameSketch.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChessGameSketchTests
{
    [TestClass]
    public class BoardTests
    {

        [TestMethod]
        public void FigureOn_WhenFigureFound_ReturnsCorrectFigure()
        {

            // Arrange
            Board underTest = new Board();
            Pawn pawn = new Pawn(new Vector2(0, 4), Player.White);
            underTest.PutFigure(pawn);

            // Act

            // Assert
            underTest.FigureOn(pawn.Position).Should().Be(pawn);
        }

        [TestMethod]
        public void FigureOn_WhenFigureNotFound_ReturnsNull()
        {
            // Arrange
            Board underTest = new Board();

            // Act

            // Assert
            underTest.FigureOn(new Vector2(0, 0)).Should().BeNull();
        }

        [TestMethod]
        public void PutFigure_OnEmptyField_AddsFigureToList()
        {
            // Arrange
            Board underTest = new Board();
            Queen queen = new Queen(new Vector2(3, 3), Player.Black);

            // Act
            underTest.PutFigure(queen);

            // Assert
            underTest.FigureOn(queen.Position).Should().Be(queen);
        }

        [TestMethod]
        public void PutFigure_OnOccupiedField_ThrowsException()
        {
            // Arrange
            Board underTest = new Board();
            Queen queen = new Queen(new Vector2(7, 7), Player.Black);

            // Act
            underTest.PutFigure(queen);

            // Assert
            underTest.Invoking(b => b.PutFigure(queen)).Should().Throw<FieldOccupiedException>();
        }

        [TestMethod]
        public void PutFigure_OnOuterField_ThrowsException()
        {
            // Arrange
            Board underTest = new Board();
            Queen tooLowX = new Queen(new Vector2(-1, 7), Player.Black);
            Queen tooBigX = new Queen(new Vector2(9, 0), Player.Black);
            Queen tooLowY = new Queen(new Vector2(0, -1), Player.Black);
            Queen tooBigY = new Queen(new Vector2(7, 8), Player.Black);

            // Act

            // Assert
            underTest.Invoking(b => b.PutFigure(tooLowX)).Should().Throw<OutOfBoundsException>();
            underTest.Invoking(b => b.PutFigure(tooBigX)).Should().Throw<OutOfBoundsException>();
            underTest.Invoking(b => b.PutFigure(tooLowY)).Should().Throw<OutOfBoundsException>();
            underTest.Invoking(b => b.PutFigure(tooBigY)).Should().Throw<OutOfBoundsException>();
        }

        [TestMethod]
        public void DeleteFigure_WhenFigureExists_RemovesFigureFromList()
        {
            // Arrange
            Board board = new Board();
            Rook rook = new Rook(new Vector2(3, 3), Player.White);
            board.PutFigure(rook);

            // Act
            board.DeleteFigure(rook);

            // Assert
            board.FiguresOnBoard.Should().NotContain(rook);
        }

        [TestMethod]
        public void DeleteFigure_WhenFigureDoesNotExist_Throws()
        {
            // Arrange
            Board board = new Board();
            Rook rook = new Rook(new Vector2(3, 3), Player.White);
            board.PutFigure(rook);

            // Act
            King blackKing = new King(Vector2.Zero, Player.Black);
            board.DeleteFigure(rook);

            // Assert
            board.Invoking(b => b.DeleteFigure(rook)).Should().Throw<FigureNotFoundException>();
        }

        [TestMethod]
        public void MoveFigure_UpdatesFiguresPositionInList()
        {
            // Arrange
            Board board = new Board();

            Vector2 initialPosition = new Vector2(3, 3);
            Rook rook = new Rook(initialPosition, Player.White);
            board.PutFigure(rook);

            // Act
            Vector2 newPosition = new Vector2(4, 4);
            board.MoveFigure(rook, newPosition);

            // Assert
            board.FigureOn(initialPosition).Should().BeNull();
            board.FigureOn(newPosition).Should().Be(rook);
        }

        [TestMethod]
        public void MoveFigure_WhenFriendlyFigureOnNextPosition_Throw()
        {
            // Arrange
            Board board = new Board();

            Rook rook = new Rook(new Vector2(3, 3), Player.White);
            board.PutFigure(rook);

            Pawn pawn = new Pawn(new Vector2(5, 5), Player.White);
            board.PutFigure(pawn);

            // Act

            // Assert
            board.Invoking(b => b.MoveFigure(rook, pawn.Position)).Should().Throw<FieldOccupiedException>();
        }

        [TestMethod]
        public void MoveFigure_WhenNoFigure_Throws()
        {
            // Arrange
            Board board = new Board();

            // Act

            // Assert
            board.Invoking(b => b.MoveFigure(new Rook(new Vector2(3, 3), Player.White), new Vector2(4, 5))).Should().Throw<FigureNotFoundException>();
        }

        public void InitBoardForEnPassantTests()
        {
            List<Figure> initialFigures = new List<Figure>()
            {
                new Pawn(new Vector2(0, 4), Player.White),
                new Bishop(new Vector2(1, 1), Player.White),

                new Pawn(new Vector2(1, 4), Player.Black),
                new Rook(new Vector2(7, 7), Player.Black)
            };

            Board underTest = new Board(initialFigures);
            underTest.EnPassantField = new Vector2(1, 5);
        }
    }
}
