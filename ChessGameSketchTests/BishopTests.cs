using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessGameSketch;
using System.Collections.Generic;
using System.Numerics;
using FluentAssertions;

namespace ChessGameSketch
{
    [TestClass]
    public class BishopTests
    {

        [TestMethod]
        public void GetPossibleMoves_ReturnsCorrectMoves()
        {
            // Arrange
            Bishop bishop = new Bishop(new Vector2(0, 0), Player.White);

            // Act
            List<FigureMove> possibleMoves = bishop.GetFigureMoves();

            // Assert
            List<FigureMove> expected = new List<FigureMove>()
            {
                new FigureMove(new Vector2(1,1), true),
                new FigureMove(new Vector2(-1,1), true),
                new FigureMove(new Vector2(1,-1), true),
                new FigureMove(new Vector2(-1,-1), true)
            };

            possibleMoves.Should().BeEquivalentTo(expected);
        }
    }
}