#region Header
//+ <source name="Position.cs" language="C#" begin="14-Nov-2020">
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
    /// Represents an ordered pair of coordinates in a two-dimensional plane.
    /// </summary>
    public struct Position
    {
        #region Fields
        /// <summary>
        /// Represents an undefined <see cref="Position"/>.
        /// </summary>
        public static readonly Position Undefined;

        /// <summary>
        /// Gets or sets the zero-based x-coordinate of this <see cref="Position"/>.
        /// </summary>
        /// <value>The x-coordinate.</value>
        public int X;

        /// <summary>
        /// Gets or sets the zero-based y-coordinate of this <see cref="Position"/>.
        /// </summary>
        /// <value>The y-coordinate.</value>
        public int Y;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> structure
        /// with the specified zero-based coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes <see cref="Undefined"/>.
        /// </summary>
        static Position()
        {
            Position.Undefined = new Position(Int32.MinValue, Int32.MinValue);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Position"/> structures for equality.
        /// </summary>
        /// <param name="position1">The first <see cref="Position"/> to compare.</param>
        /// <param name="position2">The second <see cref="Position"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both the <see cref="Position.X"/> and <see cref="Position.Y"/>
        /// coordinates of <paramref name="position1"/> and <paramref name="position2"/>
        /// are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Position position1, Position position2)
        {
            return Equals(position1, position2);
        }

        /// <summary>
        /// Compares the specified <see cref="Position"/> structures for inequality.
        /// </summary>
        /// <param name="position1">The first <see cref="Position"/> to compare.</param>
        /// <param name="position2">The second <see cref="Position"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="position1"/> and <paramref name="position2"/>
        /// have different <see cref="Position.X"/> or <see cref="Position.Y"/> coordinates;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Position position1, Position position2)
        {
            return !Equals(position1, position2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="Position"/> structures for equality.
        /// </summary>
        /// <param name="position1">The first <see cref="Position"/> to compare.</param>
        /// <param name="position2">The second <see cref="Position"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both the <see cref="Position.X"/> and <see cref="Position.Y"/>
        /// coordinates of <paramref name="position1"/> and <paramref name="position2"/>
        /// are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Position position1, Position position2)
        {
            return position1.X == position2.X && position1.Y == position2.Y;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Position"/> and
        /// whether it contains the same coordinates as this <see cref="Position"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Position"/> and
        /// contains the same <see cref="Position.X"/> and <see cref="Position.Y"/> values
        /// as this <see cref="Position"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Position)) {
                return false;
            }

            return Equals(this, (Position) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Position"/> with this <see cref="Position"/>
        /// for equality.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Position"/> contains the 
        /// same <see cref="Position.X"/> and the same <see cref="Position.Y"/>
        /// values as this <see cref="Position"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Position position)
        {
            return Equals(this, position);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Position"/>.
        /// </summary>
        /// <returns>The hash code of this <see cref="Position"/>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents this <see cref="Position"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="Position"/>.</returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        /// <summary>
        /// Returns the specified zero-based offset as an ordered pair of coordinates.
        /// </summary>
        /// <param name="offset">The zero-based offset relative to the first element in the two-dimensional plane.</param>
        /// <param name="width">The width of the two-dimensional plane.</param>
        /// <returns>A <see cref="Position"/> that represents the ordered pair of coordinates.</returns>
        public static Position FromOffset(int offset, int width)
        {
            return new Position(offset % width, offset / width);
        }

        /// <summary>
        /// Returns the specified <see cref="Position"/> as a zero-based offset
        /// relative to the first element in the two-dimensional plane.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> in the two-dimensional plane.</param>
        /// <param name="width">The width of the two-dimensional plane.</param>
        /// <returns>The zero-based offset.</returns>
        public static int ToOffset(Position position, int width)
        {
            return position.X + (position.Y * width);
        }
        #endregion
    }
}
