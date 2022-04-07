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
        Board underTest;

        [TestInitialize]
        public void TestInitialize()
        {
            List<Figure> initialFigures = new List<Figure>()
            {
                new Pawn(new Vector2(0, 4), Player.White),
                new Bishop(new Vector2(1, 1), Player.White),

                new Pawn(new Vector2(1, 4), Player.Black),
                new Rook(new Vector2(7, 7), Player.Black)
            };

            underTest = new Board(initialFigures);
            underTest.EnPassantField = new Vector2(1, 5);
        }

        [TestMethod]
        public void PutFigure_OnEmptyField_AddsFigureToList()
        {
            // Arrange
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
            Queen queen = new Queen(new Vector2(7, 8), Player.Black);

            // Act
            underTest.PutFigure(queen);

            // Assert
            underTest.Invoking(b => b.PutFigure(queen)).Should().Throw<OutOfBoundsException>();
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
            underTest.FiguresOnBoard.Should().NotContain(rook);
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
            underTest.Invoking(b => b.DeleteFigure(rook)).Should().Throw<FigureNotFoundException>();
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
            underTest.FigureOn(initialPosition).Should().BeNull();
            underTest.FigureOn(newPosition).Should().Be(rook);
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

            Rook rook = new Rook(new Vector2(3, 3), Player.White);
            board.PutFigure(rook);

            // Act

            // Assert
            board.Invoking(b => b.MoveFigure(rook, new Vector2(4, 5))).Should().Throw<FigureNotFoundException>();
        }
    }
}
