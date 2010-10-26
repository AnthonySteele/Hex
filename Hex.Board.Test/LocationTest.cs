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
    /// Test methods for struct location 
    /// </summary>
    [TestFixture]
    public class LocationTest
    {
        [Test]
        public void ConstructorSetsProperties()
        {
            Location location = new Location(3, 4);
            Assert.AreEqual(3, location.X);
            Assert.AreEqual(4, location.Y);
        }

        [Test]
        public void CopyMakesEqualObject()
        {
            Location location = new Location(3, 4);
            Location copiedLocation = location;

            Assert.AreEqual(3, copiedLocation.X);
            Assert.AreEqual(4, copiedLocation.Y);
            Assert.AreEqual(location, copiedLocation);
        }
        
        [Test]
        public void NullLocationIsEqualToNull()
        {
            Assert.AreEqual(Location.Null, Location.Null);
            Assert.AreNotEqual(Location.Null, new Location(1, 1));
        }

        [Test]
        public void NullLocationIsNull()
        {
            Assert.IsTrue(Location.Null.IsNull());
            Assert.IsFalse(new Location(1, 1).IsNull());
        }

        [Test]
        public void EqualityTest()
        {
            Location loc1 = new Location(2, 3);
            Location loc2 = new Location(3, 2);
            Location loc1Dup = new Location(2, 3);

            Assert.AreNotEqual(loc1, loc2);

            Assert.AreEqual(loc1, loc1);
            Assert.AreEqual(loc2, loc2);
            Assert.AreEqual(loc1, loc1Dup);

            Assert.IsTrue(loc1 == loc1Dup);
            Assert.IsFalse(loc1 == loc2);

            Assert.IsTrue(loc1.Equals(loc1));
            Assert.IsTrue(loc1.Equals(loc1Dup));
            Assert.IsFalse(loc1.Equals(loc2));

            Assert.IsFalse(loc1.Equals(null));
            Assert.IsFalse(loc1.Equals(3));
            Assert.IsFalse(loc1.Equals("hello"));

            Assert.IsFalse(loc1 != loc1Dup);
            Assert.IsTrue(loc1 != loc2);
        }

        [Test]
        public void IsInListWithEmptyListTest()
        {
            Location[] targetLocs = new Location[0];

            Location testLoc = new Location(1, 1);

            Assert.IsFalse(testLoc.IsInList(targetLocs));
            Assert.AreEqual(-1, testLoc.ListIndex(targetLocs));
        }

        [Test]
        public void IsInListWithOneItem()
        {
            Location[] targetLocs = new[]
                {
                    new Location(1, 1)
                };

            Location testLoc11 = new Location(1, 1);
            Location testLoc21 = new Location(2, 1);

            Assert.IsTrue(testLoc11.IsInList(targetLocs));
            Assert.AreEqual(0, testLoc11.ListIndex(targetLocs));

            Assert.IsFalse(testLoc21.IsInList(targetLocs));
            Assert.AreEqual(-1, testLoc21.ListIndex(targetLocs));
        }

        [Test]
        public void IsInListWithTowItems()
        {
            Location[] targetLocs = new[]
                {                                                
                    new Location(1, 1),
                    new Location(2, 1)
                };

            Location testLoc11 = new Location(1, 1);
            Location testLoc21 = new Location(2, 1);
            Location testLoc22 = new Location(2, 2);

            Assert.IsTrue(testLoc11.IsInList(targetLocs));
            Assert.AreEqual(0, testLoc11.ListIndex(targetLocs));

            Assert.IsTrue(testLoc21.IsInList(targetLocs));
            Assert.AreEqual(1, testLoc21.ListIndex(targetLocs));

            Assert.IsFalse(testLoc22.IsInList(targetLocs));
            Assert.AreEqual(-1, testLoc22.ListIndex(targetLocs));
        }

        [Test]
        public void DifferentCellsHaveDifferentHashCodes()
        {
            Location loc1 = new Location(1, 1);
            Location loc2 = new Location(1, 2);
            Location loc3 = new Location(2, 2);

            Assert.AreNotEqual(loc1.GetHashCode(), loc2.GetHashCode());
            Assert.AreNotEqual(loc1.GetHashCode(), loc3.GetHashCode());
            Assert.AreNotEqual(loc2.GetHashCode(), loc3.GetHashCode());
        }

        [Test]
        public void EquivalentCellsHaveSameHashCodes()
        {
            Location loc1 = new Location(2, 1);
            Location loc2 = new Location(2, 1);

            Assert.AreEqual(loc1.GetHashCode(), loc2.GetHashCode());
        }

        [Test]
        public void ToStringReturnsCorrectString()
        {
            Location location = new Location(12, 33);
            Assert.AreEqual("12,33", location.ToString());
        }
    }
}
