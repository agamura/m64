#region Header
//+ <source name="ManhattanDistanceTest.cs" language="C#" begin="10-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetInvalidDivertFactor()
        {
            ManhattanDistance heuristicFunction = new ManhattanDistance();
            heuristicFunction.DivertFactor = 0.5;
        }
        #endregion
    }
}
