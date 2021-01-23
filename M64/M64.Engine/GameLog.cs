#region Header
//+ <source name="GameLog.cs" language="C#" begin="26-Nov-2011">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2011">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Represents a <see cref="Game"/> log and provides functionality for logging
    /// <see cref="Game.Puzzle"/> moves.
    /// </summary>
    public class GameLog
    {
        #region Fields
        private Matrix puzzle;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameLog"/> class.
        /// </summary>
        internal GameLog() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLog"/> class
        /// with the specified puzzle <see cref="Matrix"/>.
        /// </summary>
        /// <param name="puzzle">The puzzle <see cref="Matrix"/> to log.</param>
        internal GameLog(Matrix puzzle)
        {
            Entries = new List<GameLogEntry>();
            Reset(puzzle);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the puzzle <see cref="Matrix"/> in its initial state before
        /// any move.
        /// </summary>
        /// <value>The puzzle <see cref="Matrix"/> in its initial state.</value>
        /// <remarks>
        /// <see cref="GameLog.Puzzle"/> is actually a clone of the
        /// <see cref="Matrix"/> being logged.
        /// </remarks>
        public Matrix Puzzle
        {
            get { return puzzle != null ? puzzle.Clone() : null; }
        }

        /// <summary>
        /// Gets the <see cref="GameLog"/> entries.
        /// </summary>
        /// <value>The <see cref="GameLog"/> entries.</value>
        public IList<GameLogEntry> Entries
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Re-initializes this <see cref="GameLog"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="GameLog.Reset()"/> also clears the log <see cref="GameLog.Entries"/>.
        /// </remarks>
        internal void Reset()
        {
            Reset(null);
        }

        /// <summary>
        /// Re-initializes this <see cref="GameLog"/> with the specified
        /// puzzle <see cref="Matrix"/>.
        /// </summary>
        /// <param name="puzzle">The new puzzle <see cref="Matrix"/> to log.</param>
        /// <remarks>
        /// <see cref="GameLog.Reset(Matrix)"/> also clears the log <see cref="GameLog.Entries"/>.
        /// </remarks>
        internal void Reset(Matrix puzzle)
        {
            this.puzzle = puzzle != null ? puzzle.Clone() : null;
            Entries.Clear();
        }
        #endregion
    }
}
