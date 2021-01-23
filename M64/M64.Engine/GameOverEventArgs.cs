#region Header
//+ <source name="GameOverEventArgs.cs" language="C#" begin="12-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Provides data for the <c>GameOver</c> event.
    /// </summary>
    public class GameOverEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverEventArgs"/> with
        /// the specified winner.
        /// </summary>
        /// <param name="winner"></param>
        internal GameOverEventArgs(Gamer winner)
        {
            Winner = winner;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Gamer"/> that first solved the <see cref="Game.Puzzle"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Gamer"/> that first solved the <see cref="Game.Puzzle"/>
        /// </value>
        public Gamer Winner
        {
            get;
            private set;
        }
        #endregion
    }
}
