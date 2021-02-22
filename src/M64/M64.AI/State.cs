#region Header
//+ <source name="State.cs" language="C#" begin="04-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
using M64.Engine;
#endregion

namespace M64.AI
{
    /// <summary>
    /// Describes the state of a <see cref="Node"/>.
    /// </summary>
    public class State
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class with the
        /// specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">
        /// A <see cref="Matrix"/> that represents the this state.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="matrix"/> is <see langword="null"/>.
        /// </exception>
        public State(Matrix matrix)
        {
            if (matrix == null) { throw new ArgumentNullException("matrix"); }
            Matrix = matrix;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Matrix"/> that represents this <see cref="State"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Matrix"/> that represents this <see cref="State"/>.
        /// </value>
        public Matrix Matrix
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the possible moves from this <see cref="State"/>.
        /// </summary>
        /// <value>The possible moves from this <see cref="State"/>.</value>
        internal List<Move> PossibleMoves
        {
            get {
                List<Position> startPositions = Matrix.GetPossibleStartPositions(true);
                List<Move> moves = new List<Move>(startPositions.Count);

                foreach (Position position in startPositions) {
                    moves.Add(new Move(position));
                }

                return moves;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="State"/> classes for equality.
        /// </summary>
        /// <param name="state1">The first <see cref="State"/> to compare.</param>
        /// <param name="state2">The Second <see cref="State"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="State.Matrix"/> of <paramref name="state1"/>
        /// and <paramref name="state2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(State state1, State state2)
        {
            return Equals(state1, state2);
        }

        /// <summary>
        /// Compares the specified <see cref="State"/> classes for inequality.
        /// </summary>
        /// <param name="state1">The first <see cref="State"/> to compare.</param>
        /// <param name="state2">The Second <see cref="State"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="state1"/> and <paramref name="state2"/>
        /// have different <see cref="State.Matrix"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(State state1, State state2)
        {
            return !Equals(state1, state2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="State"/> classes for equality.
        /// </summary>
        /// <param name="state1">The first <see cref="State"/> to compare.</param>
        /// <param name="state2">The second <see cref="State"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="State.Matrix"/> of <paramref name="state1"/>
        /// and <paramref name="state2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(State state1, State state2)
        {
            if ((state1 as object) == (state2 as object)) {
                return true;
            }

            if (state1 as object == null || state2 as object == null) {
                return false;
            }

            return state1.Matrix == state2.Matrix;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="State"/> and
        /// whether it contains the same state as this <see cref="State"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="State"/> and
        /// contains the same <see cref="State.Matrix"/> as this <see cref="State"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is State)) {
                return false;
            }

            return Equals(this, (State) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="State"/> with this <see cref="State"/>
        /// for equality.
        /// </summary>
        /// <param name="state">The <see cref="State"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="State"/> contains
        /// the same <see cref="State.Matrix"/> as this <see cref="State"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(State state)
        {
            return Equals(this, state);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="State"/>.
        /// </summary>
        /// <returns>The hash code of this <see cref="State"/>.</returns>
        public override int GetHashCode()
        {
            return Matrix.GetHashCode();
        }
        #endregion
    }
}
