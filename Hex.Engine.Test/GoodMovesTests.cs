//-----------------------------------------------------------------------
// <copyright> 
// Copyright (c) Anthony Steele 
//  This source code is part of Hex http://github.com/AnthonySteele/Hex
//  and is made available under the terms of the Microsoft Reciprocal License (Ms-RL)
//  http://www.opensource.org/licenses/ms-rl.html
// </copyright>
//----------------------------------------------------------------------- 
namespace Hex.Engine.Test
{
    using System.Collections.Generic;
    using Hex.Board;
    using Hex.Engine.Lookahead;
    using NUnit.Framework;
    
    /// <summary>
    /// Test methods for class Cell
    /// </summary>
    [TestFixture]
    public class TestGoodMoves
    {
        private GoodMoves goodMoves;

        [SetUp]
        public void SetUp()
        {
            this.goodMoves = new GoodMoves();
        }

        [TearDown]
        public void TearDown()
        {
            this.goodMoves = null;
        }

        /// <summary>
        /// test that the depth can be extended without losing data
        /// </summary>
        [Test]
        public void TestDepthExtension()
        {
            const int MaxDepth = 100;
            var theMove = new Location(5, 5);

            for (int depthLoop = 0; depthLoop < MaxDepth; depthLoop++)
            {
                this.goodMoves.AddGoodMove(depthLoop, theMove);

                for (int depthTest = 0; depthTest <= depthLoop; depthTest++)
                {
                    Assert.IsTrue(this.goodMoves.GetCount(depthTest) == 1);

                    Location[] oneMove = this.goodMoves.GetGoodMoves(depthTest);

                    Assert.IsTrue(oneMove.Length == 1);
                    Assert.IsTrue(oneMove[0].Equals(theMove));
                }
            }
        }

        /// <summary>
        /// test that an item can be moved to the front of the list
        /// without distubing other data
        /// </summary>
        [Test]
        public void TestDataMove()
        {
            for (int i = 0; i < 10; i++)
            {
                var insertLoc = new Location(i, i);
                this.goodMoves.AddGoodMove(0, insertLoc);

                Location[] insertOutMoves = this.goodMoves.GetGoodMoves(0);

                // length should be the same as the number inserted
                Assert.IsTrue(this.goodMoves.GetCount(0) == (i + 1), "Failed count at " + i);
                Assert.IsTrue(insertOutMoves.Length == (i + 1));

                // new element at the start
                Assert.IsTrue(insertLoc.Equals(insertOutMoves[0]));

                // first element at the end
                Assert.IsTrue(insertOutMoves[i].Equals(0, 0));
            }

            Location[] outMoves = this.goodMoves.GetGoodMoves(0);
            Assert.IsTrue(outMoves.Length == 10);
            Assert.IsTrue(outMoves[0].Equals(9, 9));

            // bring to front
            this.goodMoves.AddGoodMove(0, new Location(5, 5));

            outMoves = this.goodMoves.GetGoodMoves(0);
            Assert.IsTrue(outMoves.Length == 10);
            Assert.IsTrue(outMoves[0].Equals(5, 5));

            // bring various to front, test all are still present
            for (int i = 9; i >= 0; i--)
            {
                this.goodMoves.AddGoodMove(0, new Location(i, i));
                outMoves = this.goodMoves.GetGoodMoves(0);

                Assert.IsTrue(outMoves.Length == 10);
                Assert.IsTrue(outMoves[0].Equals(i, i));

                IsAllPresentOnce(outMoves);
            }
        }

        [Test]
        public void TestEmptyAndOne()
        {
            for (int i = 0; i < this.goodMoves.Depth; i++)
            {
                Assert.IsTrue(this.goodMoves.GetCount(i) == 0);
                Location[] noMoves = this.goodMoves.GetGoodMoves(i);

                Assert.IsTrue(noMoves.Length == 0);

                var location = new Location(1, 1);
                this.goodMoves.AddGoodMove(i, location);

                Assert.IsTrue(this.goodMoves.GetCount(i) == 1);
                Location[] oneMove = this.goodMoves.GetGoodMoves(i);

                Assert.IsTrue(oneMove.Length == 1);
                Assert.IsTrue(oneMove[0].Equals(location));
            }
        }

        [Test]
        public void TestFill()
        {
            for (int i = 0; i < this.goodMoves.Depth; i++)
            {
                this.DoTestFillDepth(i);
            }
        }

        // all present, and present once only
        private static void IsAllPresentOnce(Location[] items)
        {
            int length = items.Length;
            var present = new bool[length];

            for (int testIndex = 0; testIndex < length; testIndex++)
            {
                int index = items[testIndex].X;

                // in range
                Assert.IsTrue(index < length, "bad range");

                // not encountered before
                Assert.IsFalse(present[index], "Encountered before");
                present[index] = true;
            }

            // all encountered 
            for (int testIndex = 0; testIndex < length; testIndex++)
            {
                Assert.IsTrue(present[testIndex], "Not encountered");
            }
        }

        private static void AssertAllCoordinatesGreaterThanZero(IEnumerable<Location> values)
        {
            foreach (Location loc in values)
            {
                Assert.IsTrue(loc.X >= 0);
                Assert.IsTrue(loc.Y >= 0);
            }
        }

        private void DoTestFillDepth(int depth)
        {
            // add too many for the list to hold - it will discard some
            for (int loopIndex = 0; loopIndex < (GoodMoves.GoodMovesCount * 3); loopIndex++)
            {
                var location = new Location(loopIndex, loopIndex);
                this.goodMoves.AddGoodMove(depth, location);

                int currentLen = loopIndex + 1;
                if (currentLen > GoodMoves.GoodMovesCount)
                {
                    currentLen = GoodMoves.GoodMovesCount;
                }

                Assert.IsTrue(this.goodMoves.GetCount(depth) == currentLen);
                Location[] outMoves = this.goodMoves.GetGoodMoves(depth);
                Assert.IsTrue(outMoves.Length == currentLen);

                AssertAllCoordinatesGreaterThanZero(outMoves);
            }
        }
    }
}

