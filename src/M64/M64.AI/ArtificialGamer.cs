#region Header
//+ <source name="ArtificialGamer.cs" language="C#" begin="13-Feb-2021">
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
using M64.Engine;
using M64.Engine.Utilities;
#endregion

namespace M64.AI
{
    /// <summary>
    /// Represents an artificial game player that takes part in a <see cref="GameSession"/>.
    /// </summary>
    /// <seealso cref="GameSession"/>
    public class ArtificialGamer : Gamer
    {
        #region Fields
        private IQProfile iqProfile;
        private Queue<Position> moves;
        private IDAStar idaStar;
        private Matrix puzzle;
        private long lastMoveTime;
        private long currentThinkTime;
        private long lastCheatTime;
        private long currentCheatTime;
        private int minCheatTime;
        private int maxCheatTime;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArtificialGamer"/> class
        /// with the specified gamertag and <see cref="IQProfile"/>.
        /// </summary>
        /// <param name="gamertag">
        /// A string that uniquely identifies the <see cref="ArtificialGamer"/>.
        /// </param>
        /// <param name="iqProfile">The IQ profile of the <see cref="ArtificialGamer"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamertag"/> is <see langword="null"/> or empty.
        /// </exception>
        public ArtificialGamer(string gamertag, IQProfile iqProfile) : this(Guid.NewGuid(), gamertag, iqProfile)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtificialGamer"/>
        /// class with the specified globally unique identifier, gamertag, and
        /// <see cref="IQProfile"/>.
        /// </summary>
        /// <param name="id">
        /// A globally unique identifier that identifies this <see cref="ArtificialGamer"/>.
        /// </param>
        /// <param name="gamertag">
        /// A string that uniquely identifies the <see cref="ArtificialGamer"/>.
        /// </param>
        /// <param name="iqProfile">The IQ profile of the <see cref="ArtificialGamer"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamertag"/> is <see langword="null"/> or empty.
        /// </exception>
        public ArtificialGamer(Guid id, string gamertag, IQProfile iqProfile) : base(id, gamertag)
        {
            moves = new Queue<Position>();
            idaStar = new IDAStar();
            IQProfile = iqProfile;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the IQ profile of this <see cref="ArtificialGamer"/>.
        /// </summary>
        /// <value>
        /// The IQ profile of this <see cref="ArtificialGamer"/>.
        /// </value>
        public IQProfile IQProfile
        {
            get { return iqProfile; }
            set {
                iqProfile = value;
                idaStar.Timeout = iqProfile.MaxSolveTime;
                ManhattanDistance heuristicFunction = idaStar.HeuristicFunction as ManhattanDistance;
                heuristicFunction.DivertFactor = iqProfile.DivertFactor;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ArtificialGamer"/> is
        /// thinking.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="ArtificialGamer"/> is
        /// thinking; otherwise, <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// When <see cref="ArtificialGamer.IsThinking"/> is <see langword="true"/>,
        /// <see cref="ArtificialGamer.GetNextMove"/> returns <see cref="Position.Undefined"/>.
        /// </remarks>
        public bool IsThinking
        {
            get {
                long currentTime = DateTime.UtcNow.Ticks;

                if (currentTime - lastMoveTime < currentThinkTime) {
                   return true;
                }

                lastMoveTime = currentTime;

                currentThinkTime = RandomUtility.GetRandomInt(
                    iqProfile.MinThinkTime,
                    iqProfile.MaxThinkTime);

                currentThinkTime *= TimeSpan.TicksPerMillisecond;

                return false;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets the start <see cref="Position"/> of the next move issued by this
        /// <see cref="ArtificialGamer"/>.
        /// </summary>
        /// <value>
        /// The start <see cref="Position"/> of the next move, or <see cref="Position.Undefined"/>
        /// if no moves are available or this <see cref="ArtificialGamer"/> is still thinking.
        /// </value>
        public override Position GetNextMove()
        {
            if (puzzle == null && base.GameSession[this] != null) {
                puzzle = base.GameSession[this].Puzzle;
            }

            if (puzzle == null || puzzle.IsSolved) {
                moves.Clear(); return Position.Undefined;
            }

            if (IsThinking) {
                HandleCheats(); return Position.Undefined;
            }

            if (moves.Count == 0) {
                try {
                    State initialState = new State(puzzle);
                    State goalState = new State(new Matrix(puzzle.Width, puzzle.Height));
                    Solution solution = idaStar.Solve(initialState, goalState);
                    foreach (Node node in solution.Path.Nodes) {
                        if (node.Move != null) {
                            moves.Enqueue(node.Move.From);
                        }
                    }
                } catch (TimeoutException) {
                    List<Position> possibleStartPositions = puzzle.GetPossibleStartPositions(true);
                    return possibleStartPositions[RandomUtility.GetRandomInt(possibleStartPositions.Count)];
                }

                CalculateCheatTime();
            }

            return moves.Dequeue();
        }

        /// <summary>
        /// Determines whether and what <see cref="Cheat"/> to enable or disable.
        /// </summary>
        private void HandleCheats()
        {
            long currentTime = DateTime.UtcNow.Ticks;

            if (currentTime - lastCheatTime < currentCheatTime) {
                return;
            }

            lastCheatTime = currentTime;
            currentCheatTime = RandomUtility.GetRandomInt(minCheatTime, maxCheatTime);
            currentCheatTime *= TimeSpan.TicksPerMillisecond;

            foreach (KeyValuePair<int, Cheat> cheat in base.GameSession[this].Cheats) {
                if (cheat.Value.IsActive) { cheat.Value.IsActive = false; return; }
            }

            int i = RandomUtility.GetRandomInt(base.GameSession[this].Cheats.Count);
            foreach (KeyValuePair<int, Cheat> cheat in base.GameSession[this].Cheats) {
                if (i-- == 0) { cheat.Value.IsActive = true; return; }
            }
        }

        /// <summary>
        /// Determines how long a <see cref="Cheat"/> can remain active, in
        /// milliseconds, according to the current <see cref="IQProfile"/>.
        /// </summary>
        private void CalculateCheatTime()
        {
            if (moves.Count == 0) {
                minCheatTime = iqProfile.MinThinkTime;
                maxCheatTime = iqProfile.MinThinkTime + iqProfile.MaxThinkTime;
                return;
            }

            double midThinkTime = (iqProfile.MinThinkTime + iqProfile.MaxThinkTime) / 2.0;
            double moveWeight = midThinkTime * 0.1;

            minCheatTime = (int) Math.Round(midThinkTime);
            maxCheatTime = (int) Math.Round(moves.Count * moveWeight); 
        }
        #endregion
    }
}
