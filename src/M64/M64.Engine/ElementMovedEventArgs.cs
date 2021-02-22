#region Header
//+ <source name="ElementMovedEventArgs.cs" language="C#" begin="18-Nov-2020">
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
    /// Provides data for the <see cref="Matrix.ElementMoved"/> event.
    /// </summary>
    public class ElementMovedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementMovedEventArgs"/> class
        /// with the specified new and old position of the moving element.
        /// </summary>
        /// <param name="newPosition">The new <see cref="Position"/> of the moving element.</param>
        /// <param name="oldPosition">The old <see cref="Position"/> of the moving element.</param>
        /// <param name="isFirst">
        /// A value indicating whether this <see cref="ElementMovedEventArgs"/> contains
        /// the data of the first <see cref="Matrix.ElementMoved"/> event.
        /// </param>
        /// <param name="isLast">
        /// A value indicating whether this <see cref="ElementMovedEventArgs"/> contains
        /// the data of the last <see cref="Matrix.ElementMoved"/> event.
        /// </param>
        /// <param name="isScrambling">
        /// A value indicating whether the <see cref="Matrix.ElementMoved"/>
        /// event occurred while scrambling.
        /// </param>
        internal ElementMovedEventArgs(Position newPosition, Position oldPosition,
            bool isFirst, bool isLast, bool isScrambling)
        {
            IsFirst = isFirst;
            IsLast = isLast;
            IsScrambling = isScrambling;
            NewPosition = newPosition;
            OldPosition = oldPosition;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether this <see cref="ElementMovedEventArgs"/>
        /// contains the data of the first <see cref="Matrix.ElementMoved"/> event.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="ElementMovedEventArgs"/> contains the
        /// data of the first <see cref="Matrix.ElementMoved"/> event;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsFirst
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ElementMovedEventArgs"/>
        /// contains the data of the last <see cref="Matrix.ElementMoved"/> event.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="ElementMovedEventArgs"/> contains the
        /// data of the last <see cref="Matrix.ElementMoved"/> event;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsLast
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Matrix.ElementMoved"/>
        /// event occurred while scrambling.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Matrix.ElementMoved"/> event
        /// occurred while scrambling; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsScrambling
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the new <see cref="Position"/> of the moving element.
        /// </summary>
        /// <value>The new <see cref="Position"/> of the moving element.</value>
        public Position NewPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the old <see cref="Position"/> of the moving element.
        /// </summary>
        /// <value>The old <see cref="Position"/> of the moving element.</value>
        public Position OldPosition
        {
            get;
            private set;
        }
        #endregion
    }
}
