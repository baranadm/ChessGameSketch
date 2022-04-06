using ChessGameSketch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ChessGameSketchTests
{
    [TestClass]
    internal class BoardTests
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
        public void DeleteFigure_WhenFigureDoesNotExist_RemovesFigureFromList()
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
    }
}
