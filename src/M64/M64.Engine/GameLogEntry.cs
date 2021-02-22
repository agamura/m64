#region Header
//+ <source name="GameLogEntry.cs" language="C#" begin="26-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Collections.Generic;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Represents a single entry in the <see cref="GameLog"/>.
    /// </summary>
    public struct GameLogEntry
    {
        #region Fields
        private List<Cheat> activeCheats; 

        /// <summary>
        /// Gets or sets the <see cref="Game"/> elapsed time.
        /// </summary>
        /// <value>The <see cref="Game"/> elapsed time.</value>
        public long ElapsedTime;

        /// <summary>
        /// Gets or set the <see cref="Game"/> move count.
        /// </summary>
        /// <value>The <see cref="Game"/> move count.</value>
        public int MoveCount;

        /// <summary>
        /// Gets or set the starting <see cref="Position"/>.
        /// </summary>
        /// <value>The starting <see cref="Position"/>.</value>
        public Position StartPosition;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the active <see cref="Game"/> cheats.
        /// </summary>
        /// <value>The active <see cref="Game"/> cheats, if any; otherwise, <see langword="null"/>.</value>
        public List<Cheat> ActiveCheats
        {
            get {
                if (activeCheats == null) {
                    activeCheats = new List<Cheat>(10);
                }

                return activeCheats;
            }
        }
        #endregion
    }
}
