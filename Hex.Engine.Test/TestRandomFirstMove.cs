namespace Hex.Engine.Test
{
    using System.Collections.Generic;
    using Hex.Board;
    using NUnit.Framework;

    [TestFixture]
    public class TestRandomFirstMove
    {
        [Test]
        public void RandomsTest()
        {
            RandomFirstMove rands = new RandomFirstMove(10);
            Assert.AreEqual(10, rands.BoardSize);

            List<Location> locs = new List<Location>();
            int diffCount = 0;

            for (int i = 0; i < 100; i++)
            {
                locs.Add(rands.RandomMove());

                // set the hasDiff flag if this random loc is different from the previous one
                // i.e. test that theyre not all the same
                if (i > 0)
                {
                    if (!locs[i].Equals(locs[i - 1]))
                    {
                        diffCount++;
                    }
                }
            }

            Assert.Greater(diffCount, 0);
        }
    }
}
