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
    using NUnit.Framework;

    [TestFixture]
    public class OccupiedHelperTest
    {
        [Test]
        public void OccupiedToStringEmpty()
        {
            string returnValue = OccupiedHelper.OccupiedToString(Occupied.Empty);
            Assert.AreSame(returnValue, "-");
        }

        [Test]
        public void OccupiedToStringPlayerX()
        {
            string returnValue = OccupiedHelper.OccupiedToString(Occupied.PlayerX);
            Assert.AreSame(returnValue, "X");
        }

        [Test]
        public void OccupiedToStringPlayerY()
        {
            string returnValue = OccupiedHelper.OccupiedToString(Occupied.PlayerY);
            Assert.AreSame(returnValue, "Y");
        }

        [Test]
        public void OccupiedToStringInvalid()
        {
            // invalid
            string returnValue = OccupiedHelper.OccupiedToString((Occupied)42);
            Assert.AreSame(returnValue, "?");
        }
    }
}
