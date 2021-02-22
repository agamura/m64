#region Header
//+ <source name="SolveCheat.cs" language="C#" begin="17-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using M64.Engine;
#endregion

namespace M64.AI
{
    /// <summary>
    /// Provides AI functionality for solving a <see cref="Game.Puzzle"/> or
    /// part of it according to a given solve factor.
    /// </summary>
    /// <seealso cref="Cheat"/>
    public class SolveCheat : Cheat
    {
        #region Fields
        /// <summary>
        /// Gets the minimum solve factor.
        /// </summary>
        /// <remarks>The value of this constant is 0.1.</remarks>
        public const double MinSolveFactor = 0.1;

        /// <summary>
        /// Gets the maximum solve factor.
        /// </summary>
        /// <remarks>The value of this constant is 1.0.</remarks>
        public const double MaxSolveFactor = 1.0;

        /// <summary>
        /// Gets the default solve factor.
        /// </summary>
        /// <remarks>The value of this constant is 0.5.</remarks>
        public const double DefaultSolveFactor = 0.5;

        /// <summary>
        /// Gets the default solving time, in milliseconds.
        /// </summary>
        /// <remarks>The value of this constant is 100.</remarks>
        public const long DefaultSolveTimeout = 100;

        private IDAStar idaStar;
        private double solveFactor = SolveCheat.DefaultSolveFactor;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SolveCheat"/> class.
        /// </summary>
        public SolveCheat()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolveCheat"/> class with the
        /// specified id, name, and hint.
        /// </summary>
        /// <param name="id">The id of the <see cref="Cheat"/>.</param>
        /// <param name="name">The name of the <see cref="Cheat"/>.</param>
        /// <param name="hint">A short text that describes what the <see cref="Cheat"/> does.</param>
        public SolveCheat(int id, string name, string hint) : base(id, name, hint)
        {
            idaStar = new IDAStar();
            idaStar.Timeout = SolveCheat.DefaultSolveTimeout;

            base.ElapseFactor = 4.0;
            base.IncrementFactor = 4;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether or not this <see cref="SolveCheat"/>
        /// has solved the current <see cref="Game.Puzzle"/> or part of it
        /// according to <see cref="SolveFactor"/>.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the current <see cref="Game.Puzzle"/>, or
        /// part of it, has been solved; otherwise, <see langword="false"/>.
        /// </value>
        public bool Done
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this <see cref="SolveCheat"/>
        /// is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="SolveCheat"/> is active; otherwise,
        /// <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// <see cref="SolveCheat.IsActive"/> is automatically reset to
        /// <see langword="false"/> after the current <see cref="Game.Puzzle"/>
        /// has been fully or partially solved.
        /// </remarks>
        public override bool IsActive
        {
            get { return base.IsActive; }
            set {
                if (value) {
                    base.IsActive = true;
                    SolvePuzzle();
                    base.IsActive = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how much puzzle to solve.
        /// </summary>
        /// 
        public double SolveFactor
        {
            get { return solveFactor; }
            set {
                if (value < SolveCheat.MinSolveFactor || value > SolveCheat.MaxSolveFactor) {
                    throw new ArgumentOutOfRangeException();
                }
                solveFactor = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time after which this <see cref="SolveCheat"/>
        /// stops trying to solve the current <see cref="Game.Puzzle"/>.
        /// </summary>
        /// <value>The solve timeout, in milliseconds.</value>
        public long SolveTimeout
        {
            get { return idaStar.Timeout; }
            set { idaStar.Timeout = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Solves the current <see cref="Game.Puzzle"/> or part of it according
        /// to <see cref="SolveCheat.SolveFactor"/>.
        /// </summary>
        private void SolvePuzzle()
        {
            if (base.Game == null || base.Game.Puzzle.IsSolved) { return; }
            Done = false;

            State initialState = new State(base.Game.Puzzle);
            State goalState = new State(new Matrix(base.Game.Puzzle.Width, base.Game.Puzzle.Height));

            try {
                Solution solution = idaStar.Solve(initialState, goalState);

                int moveCount = (int) (solution.Path.Nodes.Count * solveFactor);
                foreach (Node node in solution.Path.Nodes) {
                    if (node.Move != null) {
                        base.Game.Puzzle.MoveFrom(node.Move.From);
                        if (--moveCount == 0) { break; }
                    }
                }

                Done = true;
            } catch (TimeoutException) {}
        }
        #endregion
    }
}
