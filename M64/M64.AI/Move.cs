#region Header
//+ <source name="Move.cs" language="C#" begin="04-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using M64.Engine;
#endregion

namespace M64.AI
{
    /// <summary>
    /// Represents a move in the problem space.
    /// </summary>
    public class Move
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class with the
        /// specified initial <see cref="Position"/>.
        /// </summary>
        /// <param name="from">The initial <see cref="Position"/>.</param>
        internal Move(Position from)
        {
            From = from;
            To = Position.Undefined;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class with the
        /// specified initial <see cref="Position"/> and destination
        /// <see cref="Position"/>.
        /// </summary>
        /// <param name="from">The initial <see cref="Position"/>.</param>
        /// <param name="to">The destination <see cref="Position"/>.</param>
        internal Move(Position from, Position to)
        {
            From = from;
            To = to;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the initial <see cref="Position"/> of this <see cref="Move"/>.
        /// </summary>
        /// <value>The initial <see cref="Position"/>.</value>
        public Position From
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the destination <see cref="Position"/> of this <see cref="Move"/>.
        /// </summary>
        /// <value>The destination <see cref="Position"/>.</value>
        public Position To
        {
            get;
            private set;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Move"/> classes for equality.
        /// </summary>
        /// <param name="move1">The first <see cref="Move"/> to compare.</param>
        /// <param name="move2">The Second <see cref="Move"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="From"/> and <see cref="To"/> of
        /// paramref name="move1"/> and <paramref name="move2"/> are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Move move1, Move move2)
        {
            return Equals(move1, move2);
        }

        /// <summary>
        /// Compares the specified <see cref="Move"/> classes for inequality.
        /// </summary>
        /// <param name="move1">The first <see cref="Move"/> to compare.</param>
        /// <param name="move2">The Second <see cref="Move"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="move1"/> and <paramref name="move2"/>
        /// have different <see cref="From"/> or <see cref="To"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Move move1, Move move2)
        {
            return !Equals(move1, move2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="Move"/> classes for equality.
        /// </summary>
        /// <param name="move1">The first <see cref="Move"/> to compare.</param>
        /// <param name="move2">The Second <see cref="Move"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="From"/> or <see cref="To"/> of
        /// <paramref name="move1"/> and <paramref name="move2"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool Equals(Move move1, Move move2)
        {
            if ((move1 as object) == (move2 as object)) {
                return true;
            }

            if (move1 as object == null || move2 as object == null) {
                return false;
            }

            return move1.From == move2.From && move1.To == move2.To;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Move"/>
        /// and whether it contains the same <see cref="From"/> and <see cref="To"/>
        /// as this <see cref="Move"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Move"/> and
        /// contains the same <see cref="From"/> and <see cref="To"/> as this
        /// <see cref="Move"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Move)) {
                return false;
            }

            return Equals(this, (Move) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Move"/> with this <see cref="Move"/>
        /// for equality.
        /// </summary>
        /// <param name="move">The <see cref="Move"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Move"/> contains
        /// the same <see cref="From"/> and <see cref="To"/> as this
        /// <see cref="Move"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Move move)
        {
            return Equals(this, move);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Move"/>.
        /// </summary>
        /// <returns>The hash code of this <see cref="Move"/>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Applies this <see cref="Move"/> to the specified <see cref="State"/>.
        /// </summary>
        /// <param name="state">The <see cref="State"/> to apply this <see cref="Move"/> to.</param>
        /// <returns>The new <see cref="State"/>.</returns>
        internal State ApplyTo(State state)
        {
            State newState = new State(state.Matrix.Clone());
            newState.Matrix.MoveFrom(From);
            To = state.Matrix.BlankPosition;

            return newState;
        }

        /// <summary>
        /// Returns a value indicating whether or not the specified <see cref="Move"/>
        /// is the inverse of this <see cref="Move"/>.
        /// </summary>
        /// <param name="move">The <see cref="Move"/> to check.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="move"/> is the inverse of
        /// this <see cref="Move"/>; otherwise, <see langword="false"/>.
        /// </returns>
        internal bool IsInverse(Move move)
        {
            return move != null && From == move.To;
        }
        #endregion
    }
}
