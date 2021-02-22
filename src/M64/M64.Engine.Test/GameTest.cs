#region Header
//+ <source name="GameTest.cs" language="C#" begin="27-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
#endregion

namespace M64.Engine.Test
{
    [TestFixture]
    class GameTest
    {
        private const int Width = 4;
        private const int Height = 4;

        private Cheat cheat1;
        private Cheat cheat2;

        [SetUp]
        public void Init()
        {
            cheat1 = new Cheat(1, "", "");
            cheat1.ElapseFactor = 2.0;
            cheat1.IncrementFactor = Counter.DefaultIncrementFactor;
            cheat1.DecrementFactor = Counter.DefaultDecrementFactor;
            cheat1.IsActive = false;

            cheat2 = new Cheat(2, "", "");
            cheat2.ElapseFactor = Stopwatch.DefaultElapseFactor;
            cheat2.IncrementFactor = 2;
            cheat2.DecrementFactor = Counter.DefaultDecrementFactor;
            cheat2.IsActive = false;
        }

        #region Tests
        [Test]
        public void CountMoves()
        {
            Game game = new Game(new Matrix(Width, Height));

            Position[] moves = {
                new Position(Width - 1, 0),
                new Position(0, 0),
                new Position(0, Height - 1),
                new Position(Width - 1, Height - 1)
            };

            foreach (Position position in moves) {
                game.Puzzle.MoveFrom(position);
            }

            Assert.AreEqual(moves.Length * (Width - 1), game.Counter.Current);
        }

        [Test]
        public void TimeGameplay()
        {
            Game game = new Game(new Matrix(Width, Height));

            Position[] moves = {
                new Position(Width - 1, 0),
                new Position(0, 0),
                new Position(0, Height - 1),
                new Position(Width - 1, Height - 1)
            };

            foreach (Position position in moves) {
                Thread.Sleep(1);
                game.Puzzle.MoveFrom(position);
            }

            Assert.IsTrue(game.Stopwatch.IsRunning);
            Assert.Greater(game.Stopwatch.ElapsedMilliseconds, 0);
        }

        [Test]
        public void ScoreGameplay()
        {
            int width = 3, height = 3;
            double score1, score2;
            Game game = new Game(new Matrix(width, height), false);

            Position[] moves = {
                new Position(width - 1, 0),
                new Position(0, 0),
                new Position(0, height - 1),
                new Position(width - 1, height - 1)
            };

            foreach (Position position in moves) {
                game.Puzzle.MoveFrom(position);
                Thread.Sleep(250);
            }

            score1 = game.Score;

            width = height = 5;
            game = new Game(new Matrix(width, height), false);

            moves[0] = new Position(width - 1, 0);
            moves[1] = new Position(0, 0);
            moves[2] = new Position(0, height - 1);
            moves[3] = new Position(width - 1, height - 1);

            foreach (Position position in moves) {
                game.Puzzle.MoveFrom(position);
                Thread.Sleep(250);
            }

            score2 = game.Score;

            Assert.Greater(score2, score1);
        }

        [Test]
        public void EnableCheat()
        {
            Game game = new Game(new Matrix(Width, Height), false);

            cheat1.IsActive = true;
            cheat2.IsActive = true;

            game.Cheats.Add(cheat1.Id, cheat1);
            game.Cheats.Add(cheat2.Id, cheat2);

            Assert.AreEqual(2, game.Cheats.Count);
            Assert.AreEqual(cheat1.ElapseFactor, game.Stopwatch.ElapseFactor);
            Assert.AreEqual(cheat2.IncrementFactor, game.Counter.IncrementFactor);
        }

        [Test]
        public void DisableCheat()
        {
            Game game = new Game(new Matrix(Width, Height), false);

            cheat1.IsActive = true;
            cheat2.IsActive = true;

            game.Cheats.Add(cheat1.Id, cheat1);
            game.Cheats.Add(cheat2.Id, cheat2);

            Assert.AreEqual(2, game.Cheats.Count);

            game.Cheats.Remove(cheat1.Id);
            game.Cheats.Remove(cheat2.Id);

            Assert.AreEqual(0, game.Cheats.Count);
            Assert.AreEqual(Stopwatch.DefaultElapseFactor, game.Stopwatch.ElapseFactor);
            Assert.AreEqual(Counter.DefaultIncrementFactor, game.Counter.IncrementFactor);
        }

        [Test]
        public void ActivateCheat()
        {
            Game game = new Game(new Matrix(Width, Height), false);

            cheat1.IsActive = false;
            cheat2.IsActive = false;

            game.Cheats.Add(cheat1.Id, cheat1);
            game.Cheats.Add(cheat2.Id, cheat2);

            Assert.AreEqual(2, game.Cheats.Count);
            Assert.AreEqual(Stopwatch.DefaultElapseFactor, game.Stopwatch.ElapseFactor);
            Assert.AreEqual(Counter.DefaultIncrementFactor, game.Counter.IncrementFactor);

            game.Cheats[cheat1.Id].IsActive = true;
            game.Cheats[cheat2.Id].IsActive = true;

            Assert.AreEqual(2, game.Cheats.Count);

            Assert.AreEqual(cheat1.ElapseFactor, game.Stopwatch.ElapseFactor);
            Assert.AreEqual(cheat2.IncrementFactor, game.Counter.IncrementFactor);
        }

        [Test]
        public void DeactivateCheat()
        {
            Game game = new Game(new Matrix(Width, Height), false);

            cheat1.IsActive = true;
            cheat2.IsActive = true;

            game.Cheats.Add(cheat1.Id, cheat1);
            game.Cheats.Add(cheat2.Id, cheat2);

            Assert.AreEqual(2, game.Cheats.Count);
            Assert.AreEqual(cheat1.ElapseFactor, game.Stopwatch.ElapseFactor);
            Assert.AreEqual(cheat2.IncrementFactor, game.Counter.IncrementFactor);

            game.Cheats[cheat1.Id].IsActive = false;
            game.Cheats[cheat2.Id].IsActive = false;

            Assert.AreEqual(2, game.Cheats.Count);
            Assert.AreEqual(Stopwatch.DefaultElapseFactor, game.Stopwatch.ElapseFactor);
            Assert.AreEqual(Counter.DefaultIncrementFactor, game.Counter.IncrementFactor);
        }

        [Test]
        public void LogSession()
        {
            Game game = new Game(new Matrix(Width, Height), true);
            Position[] moves = {
                new Position(Width - 1, 0),
                new Position(0, 0),
                new Position(0, Height - 1),
                new Position(Width - 1, Height - 1)
            };

            foreach (Position position in moves) {
                game.Puzzle.MoveFrom(position);
            }

            Assert.IsNull(game.GameLog.Puzzle);
            Assert.AreEqual(moves.Length * (Width - 1), game.GameLog.Entries.Count);

            for (int i = 0, j = 0, y = -1; i < moves.Length; j++, y++) {
                if (j == Width - 1) {
                    j = 0;
                    Assert.AreEqual(moves[i++], game.GameLog.Entries[y].StartPosition);
                }
            }
        }

        [Test]
        public void ResetGame()
        {
            Game game = new Game(new Matrix(Width, Height), true);

            cheat1.IsActive = true;
            cheat2.IsActive = true;

            game.Cheats.Add(cheat1.Id, cheat1);
            game.Cheats.Add(cheat2.Id, cheat2);

            Position[] moves = {
                new Position(Width - 1, 0),
                new Position(0, 0),
                new Position(0, Height - 1),
                new Position(Width - 1, Height - 1)
            };

            foreach (Position position in moves) {
                game.Puzzle.MoveFrom(position);
            }

            Assert.AreEqual(moves.Length * (Width - 1), game.GameLog.Entries.Count);
            Assert.IsFalse(game.Puzzle.IsSolved);

            foreach (KeyValuePair<int, Cheat> cheat in game.Cheats) {
                Assert.IsTrue(cheat.Value.IsActive);
            }

            game.Reset(new Matrix(Width, Height), true);

            Assert.AreEqual(0, game.GameLog.Entries.Count);
            Assert.IsTrue(game.Puzzle.IsSolved);

            foreach (KeyValuePair<int, Cheat> cheat in game.Cheats) {
                Assert.IsFalse(cheat.Value.IsActive);
            }

            foreach (Position position in moves) {
                game.Puzzle.MoveFrom(position);
            }

            Assert.AreNotEqual(0, game.GameLog.Entries.Count);
        }

        [Test]
        public void Playback()
        {
            Game game1 = new Game(new Matrix(Width, Height), true);
            Game game2 = new Game(game1.Puzzle.Clone());

            cheat1.IsActive = true;
            cheat2.IsActive = true;

            game1.Cheats.Add(cheat1.Id, cheat1);
            game1.Cheats.Add(cheat2.Id, cheat2);

            Position[] moves = {
                new Position(Width - 1, 0),
                new Position(0, 0),
                new Position(0, Height - 1),
                new Position(Width - 1, Height - 1)
            };

            foreach (Position position in moves) {
                game1.Puzzle.MoveFrom(position);
            }

            foreach (GameLogEntry logEntry in game1.GameLog.Entries) {
                game2.Puzzle.MoveFrom(logEntry.StartPosition);
            }

            Assert.AreEqual(game1.Puzzle, game2.Puzzle);
        }
        #endregion
    }
}
