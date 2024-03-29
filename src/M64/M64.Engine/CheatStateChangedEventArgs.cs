﻿#region Header
//+ <source name="CheatStateChangedEventArgs.cs" language="C#" begin="21-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
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
    /// Provides data for the <see cref="Cheat.StateChanged"/> event.
    /// </summary>
    public class CheatStateChangedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CheatStateChangedEventArgs"/>
        /// class with the specified <see cref="CheatStateChangedType"/>.
        /// </summary>
        /// <param name="cheatStateChangedType">
        /// One of the <see cref="CheatStateChangedType"/> values.
        /// </param>
        public CheatStateChangedEventArgs(CheatStateChangedType cheatStateChangedType)
        {
            CheatStateChangedType = cheatStateChangedType;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating how the <see cref="Cheat"/> state has changed.
        /// </summary>
        /// <value>One of the <see cref="CheatStateChangedType"/> values.</value>
        public CheatStateChangedType CheatStateChangedType
        {
            get;
            private set;
        }
        #endregion
    }
}
