//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Board.Test
{
    using Hex.Board;
    using NUnit.Framework;

    /// <summary>
    /// Test methods for class BoardCache
    /// </summary>
    [TestFixture]
    public class BoardCacheTest
    {
        [Test]
        public void CreateIsEmptyCache()
        {
            BoardCache boardCache = new BoardCache(10);

            // empty cache
            Assert.IsNotNull(boardCache);
            Assert.AreEqual(0, boardCache.BoardCount);            
        }

        [Test]
        public void AddBoardIncreasesCount()
        {
            BoardCache boardCache = new BoardCache(10);

            const int TestSize = 10;
            HexBoard[] usedBoards = new HexBoard[TestSize];

            // add some boards
            for (int i = 0; i < TestSize; i++)
            {
                usedBoards[i] = boardCache.GetBoard();
                Assert.IsNotNull(usedBoards[i]);
                Assert.AreEqual(i + 1, boardCache.BoardCount);
            }            
        }

        [Test]
        public void ReleaseBoardIncreasesCount()
        {
            BoardCache boardCache = new BoardCache(10);

            const int TestSize = 10;
            HexBoard[] usedBoards = new HexBoard[TestSize];

            // add some boards
            for (int i = 0; i < TestSize; i++)
            {
                usedBoards[i] = boardCache.GetBoard();
            }

            // remove them
            for (int i = 0; i < TestSize; i++)
            {
                boardCache.Release(usedBoards[i]);

                // count doesn't go down
                Assert.AreEqual(TestSize, boardCache.BoardCount);
                Assert.AreEqual(i + 1, boardCache.AvailableCount);
            }
        }

        [Test]
        public void GetAgainDoesNotIncreaseCount()
        {
            BoardCache boardCache = new BoardCache(10);

            const int TestSize = 10;
            HexBoard[] usedBoards = new HexBoard[TestSize];

            // add some boards
            for (int i = 0; i < TestSize; i++)
            {
                usedBoards[i] = boardCache.GetBoard();
            }

            // remove them
            for (int i = 0; i < TestSize; i++)
            {
                boardCache.Release(usedBoards[i]);
            }

            // get again, count should not change since there are now boards ready to use
            for (int i = 0; i < TestSize; i++)
            {
                usedBoards[i] = boardCache.GetBoard();
                Assert.IsNotNull(usedBoards[i]);
                Assert.AreEqual(TestSize, boardCache.BoardCount);
            }
        }
    }
}

