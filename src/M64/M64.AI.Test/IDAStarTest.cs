#region Header
//+ <source name="IDAStarTest.cs" language="C#" begin="05-Feb-2021">
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
using M64.Engine;
#endregion

namespace M64.AI.Test
{
    [TestFixture]
    class IDAStarTest
    {
        #region Tests
        [Test]
        public void SolveTimeout()
        {
            Matrix puzzle = new Matrix(8, 8);
            puzzle.Scramble();

            IDAStar idaStar = new IDAStar();
            ((ManhattanDistance) idaStar.HeuristicFunction).DivertFactor = 4.0;
            idaStar.Timeout = 1;
            Assert.That(() =>
                idaStar.Solve(new State(puzzle), new State(new Matrix(puzzle.Width, puzzle.Height))),
                Throws.TypeOf<TimeoutException>());
        }

        [Test]
        public void InvokeSolveWithNullInitialState()
        {
            IDAStar idaStar = new IDAStar();
            Assert.That(() =>
                idaStar.Solve(null, new State(new Matrix(3, 3))),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void InvokeSolveWithNullGoalState()
        {
            IDAStar idaStar = new IDAStar();
            Assert.That(() =>
                idaStar.Solve(new State(new Matrix(3, 3)), null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Solve3X3Puzzle()
        {
            Matrix puzzle = new Matrix(3, 3);
            puzzle.Scramble();
            Assert.IsTrue(SolvePuzzle(puzzle).IsSolved);
        }

        [Test]
        public void Solve4X4Puzzle()
        {
            Matrix puzzle = new Matrix(4, 4);
            puzzle.Scramble();
            Assert.IsTrue(SolvePuzzle(puzzle).IsSolved);
        }

        [Test]
        public void Solve5X5Puzzle()
        {
            Matrix puzzle = new Matrix(5, 5);
            puzzle.Scramble();
            Assert.IsTrue(SolvePuzzle(puzzle).IsSolved);
        }

        private Matrix SolvePuzzle(Matrix puzzle)
        {
            IDAStar idaStar = new IDAStar();

            long startMemory = System.GC.GetTotalMemory(true);
            Solution solution = idaStar.Solve(new State(puzzle), new State(new Matrix(puzzle.Width, puzzle.Height)));
            long endMemory = System.GC.GetTotalMemory(true);

            Console.WriteLine("Memory used to solve a {0}X{1} puzzle: {2} bytes.",
                puzzle.Width,
                puzzle.Height,
                endMemory - startMemory);

            Console.WriteLine("Expanded nodes: {0}", solution.ExpandedNodeCount);
            Console.WriteLine("Path length: {0}", solution.Path.Nodes.Count);
            Console.WriteLine("Execution time: {0}", solution.Time);

            return solution.Path.Nodes[solution.Path.Nodes.Count - 1].State.Matrix;
        }
        #endregion
    }
}
