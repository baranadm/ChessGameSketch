using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessGameSketch;
using System.Collections.Generic;
using System.Numerics;
using FluentAssertions;

namespace ChessGameSketchTests
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
            List<MoveDirection> possibleMoves = bishop.GetFigureMoves();

            // Assert
            List<MoveDirection> expected = new List<MoveDirection>()
            {
                new MoveDirection(new Vector2(1,1), true),
                new MoveDirection(new Vector2(-1,1), true),
                new MoveDirection(new Vector2(1,-1), true),
                new MoveDirection(new Vector2(-1,-1), true)
            };

            possibleMoves.Should().BeEquivalentTo(expected);
        }
    }
}