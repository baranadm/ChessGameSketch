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
            FigureMoves possibleMoves = bishop.GetFigureMoves();

            // Assert
            FigureMoves expected = new FigureMoves(new List<Vector2>()
            {
                new Vector2(1,1),
                new Vector2(-1,1),
                new Vector2(1,-1),
                new Vector2(-1,-1)
            },
            true);

            possibleMoves.Directions.Should().BeEquivalentTo(expected.Directions);
            possibleMoves.Repeatable.Should().Be(expected.Repeatable);
        }
    }
}