﻿#region Header
//+ <source name="PositionTest.cs" language="C#" begin="14-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using NUnit.Framework;
#endregion

namespace M64.Engine.Test
{
    [TestFixture]
    class PositionTest
    {
        #region Tests
        [Test]
        public void Equal()
        {
            Position position1 = new Position(1, 2);
            Position position2 = new Position(1, 2);
            Assert.AreEqual(position1, position2);
        }

        [Test]
        public void NotEqual()
        {
            Position position1 = new Position(1, 2);
            Position position2 = new Position(1, 4);
            Assert.AreNotEqual(position1, position2);
        }

        [Test]
        public void PositionToOffset()
        {
            int width = 3;
            Position position = new Position(1, 1);
            Assert.AreEqual(position.X + (position.Y * width), Position.ToOffset(position, 3));
        }

        [Test]
        public void OffsetToPosition()
        {
            int width = 3;
            Position position = new Position(1, 1);
            Assert.AreEqual(position, Position.FromOffset(position.X + (position.Y * width), width));
        }
        #endregion
    }
}
