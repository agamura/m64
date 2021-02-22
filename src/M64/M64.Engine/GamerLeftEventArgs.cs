#region Header
//+ <source name="GamerLeftEventArgs.cs" language="C#" begin="07-Apr-2021">
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
    /// Provides data for the <see cref="GameSession.GamerLeft"/> event.
    /// </summary>
    public class GamerLeftEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GamerLeftEventArgs"/>
        /// class with the specified <see cref="Gamer"/>.
        /// </summary>
        /// <param name="gamer">
        /// The <see cref="Gamer"/> that left the <see cref="GameSession"/>.
        /// </param>
        internal GamerLeftEventArgs(Gamer gamer)
        {
            Gamer = gamer;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Gamer"/> that left the <see cref="GameSession"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Gamer"/> that joined the <see cref="GameSession"/>.
        /// </value>
        public Gamer Gamer
        {
            get;
            private set;
        }
        #endregion
    }
}
