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
    public class CellEventArgsTest
    {
        [Test]
        public void CellEventArgsPropertiesSet()
        {
            Cell cell = new Cell(new Location(0, 0));
            CellEventArgs args = new CellEventArgs(cell);

            Assert.IsTrue(args.Cell == cell);
            Assert.IsTrue(args.Cell.Location == cell.Location);
            Assert.IsTrue(args.Loc == cell.Location);
        }

        [Test]
        public void CellEventArgsWithNullLocation()
        {
            CellEventArgs args = new CellEventArgs(null);

            Assert.IsTrue(args.Cell == null);
            Assert.IsTrue(args.Loc == Location.Null);
        }
    }
}
