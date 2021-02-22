#region Header
//+ <source name="GameOverEventArgs.cs" language="C#" begin="12-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
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
