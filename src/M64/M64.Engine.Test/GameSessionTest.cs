#region Header
//+ <source name="GameSessionTest.cs" language="C#" begin="13-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
#endregion

namespace M64.Engine.Test
{
    [TestFixture]
    class GameSessionTest
    {
        private const int Width = 4;
        private const int Height = 4;

        private static Position[] Moves = {
            new Position(Width - 1, 0),
            new Position(0, 0),
            new Position(0, Height - 1),
            new Position(Width - 1, Height - 1)
        };

        class TestGamer : Gamer
        {
            int updateCount;
            int cheatCount;
            int activeCheatCount;
            int scramblingCount;
            int scrambledCount;
            bool gameover;
            private Queue<Position> moves;

            public TestGamer(string gamertag) : base(gamertag)
            {
                moves = new Queue<Position>();
                moves.Enqueue(new Position(0, Height - 1));
                moves.Enqueue(new Position(0, 0));
                moves.Enqueue(new Position(Width - 1, 0));
                moves.Enqueue(new Position(Width -1, Height - 1));

                base.OnCheatsChanged = _OnCheatsChanged;
                base.OnCheatStateChanged = _OnCheatStateChanged;
                base.OnPuzzleElementMoved = _OnPuzzleElementMoved;
                base.OnPuzzleScrambling = _OnPuzzleScrambling;
                base.OnPuzzleScrambled = _OnPuzzleScrambled;
                base.OnGameOver = _OnGameOver;
            }

            public int ScramblingCount
            {
                get { return scramblingCount; }
            }

            public int ScrambledCount
            {
                get { return scrambledCount; }
            }

            public int UpdateCount
            {
                get { return updateCount; }
            }

            public int CheatCount
            {
                get { return cheatCount; }
            }

            public int ActiveCheatCount
            {
                get { return activeCheatCount; }
            }

            public bool GameOver
            {
                get { return gameover; }
            }

            public override Position GetNextMove()
            {
                if (moves.Count == 0) {
                    return Position.Undefined;
                }
                return moves.Dequeue();
            }

            private void _OnCheatsChanged(object sender, DictionaryChangedEventArgs<int, Cheat> args)
            {
                switch (args.ListChangedType) {
                    case ListChangedType.ItemAdded:
                        cheatCount++;
                        break;
                    case ListChangedType.ItemRemoved:
                        cheatCount--;
                        break;
                    default:
                        break;
                }
            }

            private void _OnCheatStateChanged(object sender, CheatStateChangedEventArgs args)
            {
                switch (args.CheatStateChangedType) {
                    case CheatStateChangedType.Activated:
                        activeCheatCount++;
                        break;
                    case CheatStateChangedType.Deactivated:
                        activeCheatCount--;
                        break;
                    default:
                        break;
                }
            }

            private void _OnPuzzleScrambling(object sender, EventArgs args)
            {
                scramblingCount++;
            }

            private void _OnPuzzleScrambled(object sender, EventArgs args)
            {
                scrambledCount++;
            }

            private void _OnPuzzleElementMoved(object sender, ElementMovedEventArgs args)
            {
                if (args.IsLast && !args.IsScrambling) { updateCount++; }
            }

            private void _OnGameOver(object sender, GameOverEventArgs args)
            {
                gameover = true;
            }
        }

        #region Tests
        [Test]
        public void CreateGameSessionWithNullPuzzle()
        {
            Assert.That(() => new GameSession(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetGameWithNullGamer()
        {
            GameSession gameSession = new GameSession(new Matrix(Width, Height));
            gameSession.Join(new TestGamer("Test Gamer"));
            Assert.That(() => gameSession[null], Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void JoinGameSessionWithNullGamer()
        {
            GameSession gameSession = new GameSession(new Matrix(Width, Height));
            Assert.That(() => gameSession.Join(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void LeaveGameSessionWithNullGamer()
        {
            GameSession gameSession = new GameSession(new Matrix(Width, Height));
            gameSession.Join(new TestGamer("Test Gamer"));
            Assert.That(() => gameSession.Leave(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void JoinActiveGameSession()
        {
            TestGamer gamer1 = new TestGamer("Test Gamer 1");
            TestGamer gamer2 = new TestGamer("Test Gamer 2");
            GameSession gameSession = new GameSession(new Matrix(Width, Height));

            foreach (Position position in Moves) {
                gameSession.Puzzle.MoveFrom(position);
            }

            gameSession.Join(gamer1);
            gameSession.Update();

            while (gameSession.IsActive) {
                gameSession.Update();
                Assert.That(() => gameSession.Join(gamer2), Throws.TypeOf<InvalidOperationException>());
            }
        }

        [Test]
        public void AddCheat()
        {
            TestGamer[] gamers = {
                new TestGamer("Test Gamer 1"),
                new TestGamer("Test Gamer 2"),
                new TestGamer("Test Gamer 3")
            };

            GameSession gameSession = new GameSession(new Matrix(Width, Height));
            Cheat cheat = new Cheat(1, "", "");

            foreach (TestGamer gamer in gamers) {
                gameSession.Join(gamer);
            }

            (gameSession[gamers[0]].Cheats as ObservableDictionary<int, Cheat>).Add(cheat.Id, cheat);
            Assert.AreEqual(1, gamers[0].CheatCount);
            Assert.AreEqual(0, gamers[1].CheatCount);
            Assert.AreEqual(0, gamers[2].CheatCount);

            (gameSession[gamers[0]].Cheats as ObservableDictionary<int, Cheat>).Remove(cheat.Id);
            Assert.AreEqual(0, gamers[0].CheatCount);
            Assert.AreEqual(0, gamers[1].CheatCount);
            Assert.AreEqual(0, gamers[2].CheatCount);
        }

        [Test]
        public void ActivateCheat()
        {
            TestGamer[] gamers = {
                new TestGamer("Test Gamer 1"),
                new TestGamer("Test Gamer 2"),
                new TestGamer("Test Gamer 3")
            };

            GameSession gameSession = new GameSession(new Matrix(Width, Height));
            Cheat cheat = new Cheat(1, "", "");

            foreach (TestGamer gamer in gamers) {
                gameSession.Join(gamer);
                (gameSession[gamer].Cheats as ObservableDictionary<int, Cheat>).Add(cheat.Id, cheat.Clone());
            }

            gameSession[gamers[0]].Cheats[cheat.Id].IsActive = true;
            Assert.AreEqual(1, gamers[0].ActiveCheatCount);
            Assert.AreEqual(0, gamers[1].ActiveCheatCount);
            Assert.AreEqual(0, gamers[2].ActiveCheatCount);

            gameSession[gamers[0]].Cheats[cheat.Id].IsActive = false;
            Assert.AreEqual(0, gamers[0].ActiveCheatCount);
            Assert.AreEqual(0, gamers[1].ActiveCheatCount);
            Assert.AreEqual(0, gamers[2].ActiveCheatCount);
        }

        [Test]
        public void GetGameInstances()
        {
            TestGamer[] gamers = {
                new TestGamer("Test Gamer 1"),
                new TestGamer("Test Gamer 2"),
                new TestGamer("Test Gamer 3")
            };

            GameSession gameSession = new GameSession(new Matrix(Width, Height));

            foreach (Position position in Moves) {
                gameSession.Puzzle.MoveFrom(position);
            }

            foreach (TestGamer gamer in gamers) {
                gameSession.Join(gamer);
            }

            Assert.AreEqual(gamers.Length, gameSession.GameInstances.Length);
        }

        [Test]
        public void SingleGamerSession()
        {
            TestGamer gamer = new TestGamer("Test Gamer");
            GameSession gameSession = new GameSession(new Matrix(Width, Height));

            foreach (Position position in Moves) {
                gameSession.Puzzle.MoveFrom(position);
            }

            gameSession.Join(gamer);

            while (true) {
                gameSession.Update();
                Thread.Sleep(100);
                if (!gameSession.IsActive) { break; }
            }

            Assert.IsTrue(gameSession[gamer].Puzzle.IsSolved);
            Assert.AreNotEqual(TimeSpan.Zero, gameSession.Duration);
            Assert.AreEqual(gameSession.Winner, gamer);
            Assert.AreEqual(Moves.Length, gamer.UpdateCount);

            gameSession[gamer].Puzzle.Scramble();
            Assert.AreEqual(Moves.Length, gamer.UpdateCount);
            Assert.AreEqual(1, gamer.ScramblingCount);
            Assert.AreEqual(1, gamer.ScrambledCount);
        }

        [Test]
        public void MultiGamerSession()
        {
            TestGamer[] gamers = {
                new TestGamer("Test Gamer 1"),
                new TestGamer("Test Gamer 2"),
                new TestGamer("Test Gamer 3")
            };

            GameSession gameSession = new GameSession(new Matrix(Width, Height));

            foreach (Position position in Moves) {
                gameSession.Puzzle.MoveFrom(position);
            }

            foreach (TestGamer gamer in gamers) {
                gameSession.Join(gamer);
            }

            while (true) {
                gameSession.Update();
                Thread.Sleep(100);
                if (!gameSession.IsActive) { break; }
            }

            Assert.AreNotEqual(TimeSpan.Zero, gameSession.Duration);
            Assert.IsNotNull(gameSession.Winner);

            foreach (TestGamer gamer in gamers) {
                Assert.IsTrue(gameSession[gamer].Puzzle.IsSolved);
                Assert.AreEqual(Moves.Length, gamer.UpdateCount);

                gameSession[gamer].Puzzle.Scramble();
                Assert.AreEqual(1, gamer.ScramblingCount);
                Assert.AreEqual(1, gamer.ScrambledCount);
            }
        }

        [Test]
        public void MultiGamerSessionWithGameOver()
        {
            TestGamer[] gamers = {
                new TestGamer("Test Gamer 1"),
                new TestGamer("Test Gamer 2"),
                new TestGamer("Test Gamer 3")
            };

            TestGamer intendedWinner = gamers[0];
            GameSession gameSession = new GameSession(new Matrix(Width, Height), true);

            foreach (Position position in Moves) {
                gameSession.Puzzle.MoveFrom(position);
            }

            foreach (TestGamer gamer in gamers) {
                gameSession.Join(gamer);
            }

            while (true) {
                gameSession.Update();
                foreach (TestGamer gamer in gamers) {
                    if (gamer != intendedWinner) { Position nextMove = gamer.GetNextMove(); }
                }
                Thread.Sleep(100);
                if (!gameSession.IsActive) { break; }
            }

            Assert.AreNotEqual(TimeSpan.Zero, gameSession.Duration);
            Assert.AreEqual(intendedWinner, gameSession.Winner);
            Assert.IsTrue(gameSession[intendedWinner].Puzzle.IsSolved);

            foreach (TestGamer gamer in gamers) {
                if (gamer != intendedWinner) {
                    Assert.IsTrue(gamer.GameOver);
                    Assert.IsFalse(gameSession[gamer].Puzzle.IsSolved);
                } else {
                    Assert.IsFalse(gamer.GameOver);
                }
            }
        }

        [Test]
        public void ResetGameSessionWithNullPuzzle()
        {
            GameSession gameSession = new GameSession(new Matrix(Width, Height));
            gameSession.Puzzle.Scramble();
            gameSession.Reset(null);

            Assert.IsTrue(gameSession.Puzzle.IsSolved);
        }
        #endregion
    }
}
