﻿#region Header
//+ <source name="SolveCheatTest.cs" language="C#" begin="19-Feb-2012">
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
using M64.Engine;
#endregion

namespace M64.AI.Test
{
    [TestFixture]
    class SolveCheatTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetInvalidMinSolveFactor()
        {
            SolveCheat solveCheat = new SolveCheat();
            solveCheat.SolveFactor = 0.0;
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetInvalidMaxSolveFactor()
        {
            SolveCheat solveCheat = new SolveCheat();
            solveCheat.SolveFactor = 1.1;
        }

        [Test]
        public void SolveFullPuzzle()
        {
            SolveCheat solveCheat = new SolveCheat(1, "", "");
            solveCheat.SolveFactor = 1.0;
            Game game = new Game(new Matrix(3, 3));
            
            game.Puzzle.Scramble();
            Assert.IsFalse(game.Puzzle.IsSolved);

            game.Cheats.Add(solveCheat.Id, solveCheat);
            solveCheat.IsActive = true;

            Assert.IsTrue(game.Puzzle.IsSolved);
            Assert.IsTrue(solveCheat.Done);
            Assert.IsFalse(solveCheat.IsActive);
        }

        [Test]
        public void SolvePartPuzzle()
        {
            SolveCheat solveCheat = new SolveCheat(1, "", "");
            Game game = new Game(new Matrix(3, 3));

            game.Puzzle.Scramble();
            Assert.IsFalse(game.Puzzle.IsSolved);

            game.Cheats.Add(solveCheat.Id, solveCheat);
            solveCheat.IsActive = true;

            Assert.IsFalse(game.Puzzle.IsSolved);
            Assert.IsTrue(solveCheat.Done);
            Assert.IsFalse(solveCheat.IsActive);
        }

        [Test]
        public void SolveTimeout()
        {
            SolveCheat solveCheat = new SolveCheat(1, "", "");
            solveCheat.SolveTimeout = 1;
            Game game = new Game(new Matrix(8, 8));

            game.Puzzle.Scramble(20.0);
            game.Cheats.Add(solveCheat.Id, solveCheat);
            solveCheat.IsActive = true;

            Assert.IsFalse(game.Puzzle.IsSolved);
            Assert.IsFalse(solveCheat.Done);
            Assert.IsFalse(solveCheat.IsActive);
        }
    }
}
