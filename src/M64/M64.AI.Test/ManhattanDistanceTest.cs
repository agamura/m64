#region Header
//+ <source name="ManhattanDistanceTest.cs" language="C#" begin="10-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using NUnit.Framework;
#endregion

namespace M64.AI.Test
{
    [TestFixture]
    class ManhattanDistanceTest
    {
        #region Tests
        [Test]
        public void SetInvalidDivertFactor()
        {
            ManhattanDistance heuristicFunction = new ManhattanDistance();
            Assert.That(() =>
                heuristicFunction.DivertFactor = 0.5,
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion
    }
}
