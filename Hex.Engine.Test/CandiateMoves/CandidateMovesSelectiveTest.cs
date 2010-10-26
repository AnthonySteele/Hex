//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine.Test.CandiateMoves
{
    using System.Collections.Generic;
    using System.Linq;

    using Hex.Board;
    using Hex.Engine.CandiateMoves;
    using Hex.Engine.Lookahead;

    using NUnit.Framework;

    [TestFixture]
    public class CandidateMovesSelectiveTest
    {
        private const int BoardSize = 10;
        private const int BoardCellCount = BoardSize * BoardSize;

        [Test]
        public void TestCreate()
        {
            GoodMoves goods = new GoodMoves();
            CandidateMovesSelective moveFinder = new CandidateMovesSelective(goods, 0);
            Assert.IsNotNull(moveFinder);
        }

        [Test]
        public void CountOneMoveTest()
        {
            GoodMoves goods = new GoodMoves();
            CandidateMovesSelective moveFinder = new CandidateMovesSelective(goods, 0);
            HexBoard testBoard = new HexBoard(BoardSize);

            testBoard.PlayMove(5, 5, true);

            IEnumerable<Location> moves = moveFinder.CandidateMoves(testBoard, 0);

            Assert.Greater(BoardCellCount - 1, moves.Count());
        }
    }
}
