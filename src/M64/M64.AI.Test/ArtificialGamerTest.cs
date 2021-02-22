#region Header
//+ <source name="ArtificialGamerTest.cs" language="C#" begin="16-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using NUnit.Framework;
using M64.Engine;
#endregion

namespace M64.AI.Test
{
    [TestFixture]
    class ArtificialGamerTest
    {
        #region Tests
        [Test]
        public void IsThinking()
        {
            IQProfile iqProfile = new IQProfile(1.0, 1000, 0, 1000);
            ArtificialGamer gamer = new ArtificialGamer("Artificial Gamer", iqProfile);

            GameSession gameSession = new GameSession(new Matrix(3, 3));
            gameSession.Join(gamer);

            gameSession.Puzzle.Scramble();
            Game game = gameSession[gamer];
            game.Puzzle.MoveFrom(game.Puzzle.GetPossibleStartPositions(true)[0]);

            Assert.IsFalse(gamer.IsThinking);
            gameSession.Update();
            Assert.IsTrue(gamer.IsThinking);
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

        [Test]
        public void HandleCheats()
        {
            int cheatCount = 0;
            int cheatActivationCount = 0;

            IQProfile iqProfile = new IQProfile(1.0, 1000, 0, 500);
            ArtificialGamer gamer = new ArtificialGamer("Artificial Gamer", iqProfile);
            gamer.OnCheatsChanged = delegate(object sender, DictionaryChangedEventArgs<int, Cheat> args) { cheatCount++; };
            gamer.OnCheatStateChanged = delegate(object sender, CheatStateChangedEventArgs args) { cheatActivationCount++; };

            GameSession gameSession = new GameSession(new Matrix(3, 3));
            gameSession.Puzzle.Scramble();

            Cheat[] cheats = {
                new Cheat(1, "", ""),
                new Cheat(2, "", "")
            };

            Game game = gameSession.Join(gamer);

            foreach (Cheat cheat in cheats) {
                game.Cheats.Add(cheat.Id, cheat);
            }

            do {
                gameSession.Update();
            } while (gameSession.IsActive);

            Assert.AreEqual(cheats.Length, cheatCount);
            Assert.AreNotEqual(0, cheatActivationCount);
        }

        [Test]
        public void GameOver()
        {
            ArtificialGamer[] gamers = {
                new ArtificialGamer("Artificial Gamer 1", new IQProfile(1.0, 1000, 0, 500)),
                new ArtificialGamer("Artificial Gamer 2", new IQProfile(1.0, 500, 0, 1000))
            };

            Gamer winner = null;
            GameSession gameSession = new GameSession(new Matrix(3, 3), true);
            gameSession.Puzzle.Scramble();

            foreach (ArtificialGamer gamer in gamers) {
                gameSession.Join(gamer);
                gamer.OnGameOver = delegate(object sender, GameOverEventArgs args) { winner = args.Winner; };
            }

            do {
                gameSession.Update();
            } while (gameSession.IsActive);

            Assert.IsNotNull(winner);
        }

        private Matrix SolvePuzzle(Matrix puzzle)
        {
            IQProfile iqProfile = new IQProfile(1.0, 1000, 0, 500);
            ArtificialGamer gamer = new ArtificialGamer("Artificial Gamer", iqProfile);

            GameSession gameSession = new GameSession(puzzle);
            gameSession.Join(gamer);

            do {
                gameSession.Update();
            } while (gameSession.IsActive);

            return gameSession[gamer].Puzzle;
        }
        #endregion
    }
}
