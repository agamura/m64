#region Header
//+ <source name="GamerJoinedEventArgs.cs" language="C#" begin="07-Apr-2012">
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
    /// Provides data for the <see cref="GameSession.GamerJoined"/> event.
    /// </summary>
    public class GamerJoinedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GamerJoinedEventArgs"/>
        /// class with the specified <see cref="Gamer"/>.
        /// </summary>
        /// <param name="gamer">
        /// The <see cref="Gamer"/> that joined the <see cref="GameSession"/>.
        /// </param>
        internal GamerJoinedEventArgs(Gamer gamer)
        {
            Gamer = gamer;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Gamer"/> that joined the <see cref="GameSession"/>.
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