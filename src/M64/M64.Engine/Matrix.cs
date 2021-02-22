#region Header
//+ <source name="Matrix.cs" language="C#" begin="09-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using M64.Engine.Utilities;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Represents a matrix consisting of sliding elements in random order with one
    /// empty element called the <b>Blank</b>. <c>Matrix</c> provides functionality
    /// for sliding adjacent elements toward the Blank and eventually solving their
    /// order.
    /// </summary>
    public class Matrix : IEnumerable
    {
        #region Types
        private struct Hook
        {
            public int Order;
            public object Element;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Gets the minimum dimension of the <see cref="Matrix"/>.
        /// </summary>
        /// <remarks>The value of this constant is 3.</remarks>
        public const int MinDimensionLength = 3;

        /// <summary>
        /// Gets the maximum dimension of the <see cref="Matrix"/>.
        /// </summary>
        /// <remarks>The value of this constant is 8.</remarks>
        public const int MaxDimensionLength = 8;

        /// <summary>
        /// Gets the default scrambling magnitude.
        /// </summary>
        /// <remarks>The value of this constant is 10.0.</remarks>
        public const double DefaultScramblingMagnitude = 10.0;

        private int hashCode = -1;
        private ulong solvingCondition;
        private ulong currentCondition;
        private Hook[,] hooks;
        private Position blankPosition;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a <see cref="Matrix"/> element has been moved.
        /// </summary>
        public event EventHandler<ElementMovedEventArgs> ElementMoved;

        /// <summary>
        /// Occurs when the <see cref="Matrix"/> is about to be scrambled.
        /// </summary>
        public event EventHandler Scrambling;

        /// <summary>
        /// Occurs when the <see cref="Matrix"/> has finished scrambling.
        /// </summary>
        public event EventHandler Scrambled;

        /// <summary>
        /// Occurs when the <see cref="Matrix"/> has been reset.
        /// </summary>
        public event EventHandler InstanceReset;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with
        /// the specified dimension.
        /// </summary>
        /// <param name="width">The width of the <see cref="Matrix"/>.</param>
        /// <param name="height">The height of the <see cref="Matrix"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> is less than
        /// <see cref="MinDimensionLength"/> or greater than <see cref="MaxDimensionLength"/>.
        /// </exception>
        public Matrix(int width, int height)
        {
            Reset(width, height);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the element at the specified <see cref="Position"/>.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> of the element.</param>
        /// <value>The element at the specified <see cref="Position"/>.</value>
        public object this[Position position]
        {
            get { return hooks[position.Y, position.X].Element; }
            set { hooks[position.Y, position.X].Element = value; }
        }

        /// <summary>
        /// Gets or sets the element at the specified zero-based coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the element.</param>
        /// <param name="y">The y-coordinate of the element.</param>
        /// <value>The element at the specified coordinates.</value>
        public object this[int x, int y]
        {
            get { return hooks[y, x].Element; }
            set { hooks[y, x].Element = value; }
        }

        /// <summary>
        /// Gets the width of this <see cref="Matrix"/>.
        /// </summary>
        /// <value>The width of this <see cref="Matrix"/>.</value>
        public int Width
        {
            get { return hooks.GetLength(1); }
        }

        /// <summary>
        /// Gets the height of this <see cref="Matrix"/>.
        /// </summary>
        /// <value>
        /// The height of this <see cref="Matrix"/>.
        /// </value>
        public int Height
        {
            get { return hooks.GetLength(0); }
        }

        /// <summary>
        /// Gets the <see cref="Position"/> of the Blank.
        /// </summary>
        /// <value>The <see cref="Position"/> of the Blank.</value>
        public Position BlankPosition
        {
            get { return blankPosition; }
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="Matrix"/>
        /// is solved. A <see cref="Matrix"/> is solved when all its
        /// elements are ordered as they were before scrambling.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Matrix"/> is solved;
        /// otherwise, <see langword="false"/>.
        /// </value>
        /// <seealso cref="Matrix.Scramble()"/>
        public bool IsSolved
        {
            get {
                return currentCondition == solvingCondition;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Matrix"/> classes for equality.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix"/> to compare.</param>
        /// <param name="matrix2">The Second <see cref="Matrix"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the elements of <paramref name="matrix1"/> and
        /// <paramref name="matrix2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Matrix matrix1, Matrix matrix2)
        {
            return Equals(matrix1, matrix2);
        }

        /// <summary>
        /// Compares the specified <see cref="Matrix"/> classes for inequality.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix"/> to compare.</param>
        /// <param name="matrix2">The Second <see cref="Matrix"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the elements of <paramref name="matrix1"/> and
        /// <paramref name="matrix2"/> are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Matrix matrix1, Matrix matrix2)
        {
            return !Equals(matrix1, matrix2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a shallow copy of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// A shallow copy of this <see cref="Matrix"/>.
        /// </returns>
        public Matrix Clone()
        {
            Matrix matrix = new Matrix(Width, Height);
            CopyTo(matrix);
            return matrix;
        }

        /// <summary>
        /// Copies this <see cref="Matrix"/> to the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The destination <see cref="Matrix"/>.</param>
        /// <exception cref="InvalidOperationException">
        /// At least one dimension of <paramref name="matrix"/> differs from the
        /// corrisponding dimension of this <see cref="Matrix"/>.
        /// </exception>
        public void CopyTo(Matrix matrix)
        {
            if (Width != matrix.Width || Height != matrix.Height) {
                string message = String.Format(
                     "Attempting to copy a {0}x{1} matrix to a {2}x{3} matrix.",
                      Width, Height,
                      matrix.Width, matrix.Height
                );
                throw new InvalidOperationException(message);
            }

            matrix.blankPosition = blankPosition;
            matrix.currentCondition = currentCondition;
            matrix.hashCode = hashCode;

            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    matrix.hooks[y, x].Order = hooks[y, x].Order;
                    matrix.hooks[y, x].Element = hooks[y, x].Element;
                }
            }
        }

        /// <summary>
        /// Compares the specified <see cref="Matrix"/> classes for equality.
        /// </summary>
        /// <param name="matrix1">The first <see cref="Matrix"/> to compare.</param>
        /// <param name="matrix2">The second <see cref="Matrix"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both <paramref name="matrix1"/> and <paramref name="matrix2"/>
        /// contain the same elements; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Matrix matrix1, Matrix matrix2)
        {
            if ((matrix1 as object) == (matrix2 as object)) {
                return true;
            }

            if (matrix1 as object == null || matrix2 as object == null) {
                return false;
            }

            if (matrix1.Height != matrix2.Height || matrix1.Width != matrix2.Width) {
                return false;
            }

            if (matrix1.blankPosition != matrix2.blankPosition) {
                return false;
            }

            for (int y = 0; y < matrix1.Height; y++) {
                for (int x = 0; x < matrix1.Width; x++) {
                    if (matrix1.hooks[y, x].Order != matrix2.hooks[y, x].Order) { return false; }
                    if (matrix1.hooks[y, x].Element != null) {
                        if (!matrix1.hooks[y, x].Element.Equals(matrix2.hooks[y, x].Element)) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Matrix"/> and
        /// whether it contains the same elements as this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Matrix"/> and
        /// contains the same elements as this <see cref="Matrix"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix)) {
                return false;
            }

            return Equals(this, (Matrix) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Matrix"/> with this <see cref="Matrix"/>
        /// for equality.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Matrix"/> contains the 
        /// same elements as this <see cref="Matrix"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Matrix matrix)
        {
            return Equals(this, matrix);
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="MatrixEnumerator"/> for the <see cref="Matrix"/>.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return new MatrixEnumerator(this);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// The hash code of this <see cref="Matrix"/>.
        /// </returns>
        public override int GetHashCode()
        {
            if (this.hashCode == -1) {
                int hashCode = 17;

                for (int y = 0; y < Height; y++) {
                    for (int x = 0; x < Width; x++) {
                        hashCode = 31 * hashCode + (hooks[y, x].Order + 1);
                    }
                }

                this.hashCode = hashCode;
            }

            return this.hashCode;
        }

        /// <summary>
        /// Returns the number of moves starting from the specified
        /// <see cref="Position"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="Position"/> from which to count the number of moves.
        /// </param>
        /// <returns>The number of moves.</returns>
        public int GetMoveCountFrom(Position position)
        {
            int moveCount = 0;

            if (position.X == blankPosition.X && position.Y != blankPosition.Y) {
                if (position.Y < blankPosition.Y) {
                    moveCount = blankPosition.Y - position.Y;
                } else {
                    moveCount = position.Y - blankPosition.Y;
                }
            } else if (position.Y == blankPosition.Y && position.X != blankPosition.X) {
                if (position.X < blankPosition.X) {
                    moveCount = blankPosition.X - position.X;
                } else {
                    moveCount = position.X - blankPosition.X;
                }
            }

            return moveCount;
        }

        /// <summary>
        /// Returns the order of the element at the specified <see cref="Position"/>.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> of the element.</param>
        /// <returns>The order of the element at the specified <see cref="Position"/>.</returns>
        public int GetOrder(Position position)
        {
            return hooks[position.Y, position.X].Order;
        }

        /// <summary>
        /// Returns the order of the element at the specified zero-based coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the element.</param>
        /// <param name="y">The y-coordinate of the element.</param>
        /// <returns>The order of the element at the specified coordinates.</returns>
        public int GetOrder(int x, int y)
        {
            return hooks[y, x].Order;
        }

        /// <summary>
        /// Returns the <see cref="Position"/> of the element with the specified order.
        /// </summary>
        /// <param name="order">The order of the element.</param>
        /// <returns>The <see cref="Position"/> of the element with the specified order.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="order"/> is less than 0 or equal to or greather than
        /// the number of elements in this <see cref="Matrix"/>.
        /// </exception>
        public Position GetPosition(int order)
        {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    if (hooks[y, x].Order == order) return new Position(x, y);
                }
            }

            throw new ArgumentOutOfRangeException("order");
        }

        /// <summary>
        /// Gets the possible starting positions according to the current state
        /// of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The possible starting positions.</returns>
        public List<Position> GetPossibleStartPositions()
        {
            return GetPossibleStartPositions(false);
        }

        /// <summary>
        /// Gets the possible starting positions according to the current state
        /// of this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="adjacentOnly">
        /// A boolean value indicating whether or not to only get starting
        /// positions adjacent to <see cref="BlankPosition"/>.
        /// </param>
        /// <returns>The possible starting positions.</returns>
        public List<Position> GetPossibleStartPositions(bool adjacentOnly)
        {
            List<Position> startPositions = new List<Position>();

            if (blankPosition.X > 0) {
                for (int x = blankPosition.X; x > 0; x--) {
                    startPositions.Add(new Position(x - 1, blankPosition.Y));
                    if (adjacentOnly) { break; }
                }
            }

            if (blankPosition.X < Width - 1) {
                for (int x = blankPosition.X; x < Width - 1; x++) {
                    startPositions.Add(new Position(x + 1, blankPosition.Y));
                    if (adjacentOnly) { break; }
                }
            }

            if (blankPosition.Y > 0) {
                for (int y = blankPosition.Y; y > 0; y--) {
                    startPositions.Add(new Position(blankPosition.X, y - 1));
                    if (adjacentOnly) { break; }
                }
            }

            if (blankPosition.Y < Height - 1) {
                for (int y = blankPosition.Y; y < Height - 1; y++) {
                    startPositions.Add(new Position(blankPosition.X, y + 1));
                    if (adjacentOnly) { break; }
                }
            }

            return startPositions;
        }

        /// <summary>
        /// Re-initializes this <see cref="Matrix"/> with the current width and height.
        /// </summary>
        public void Reset()
        {
            Reset(Width, Height);
        }

        /// <summary>
        /// Re-initializes this <see cref="Matrix"/> with the specified width and height.
        /// </summary>
        /// <param name="width">The new width of the <see cref="Matrix"/>.</param>
        /// <param name="height">The new height of the <see cref="Matrix"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> is less than
        /// <see cref="MinDimensionLength"/> or greater than <see cref="MaxDimensionLength"/>.
        /// </exception>
        public void Reset(int width, int height)
        {
            if (width < Matrix.MinDimensionLength || width > Matrix.MaxDimensionLength) {
                throw new ArgumentOutOfRangeException("width");
            }

            if (height < Matrix.MinDimensionLength || height > Matrix.MaxDimensionLength) {
                throw new ArgumentOutOfRangeException("height");
            }

            if (hooks == null || Width != width || Height != height) {
                hooks = new Matrix.Hook[height, width];
            }

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    hooks[y, x].Order = x + (y * width);
                }
            }

            blankPosition.X = width - 1;
            blankPosition.Y = height - 1;
            currentCondition = CalculateSolvingCondition();

            OnInstanceReset(new EventArgs());
        }

        /// <summary>
        /// Arranges the elements of this <see cref="Matrix"/> in a random but
        /// solvable order, with the Blank in the last position.
        /// </summary>
        public void Scramble()
        {
            Scramble(Matrix.DefaultScramblingMagnitude);
        }

        /// <summary>
        /// Arranges the elements of this <see cref="Matrix"/> in a random but
        /// solvable order, with the Blank in the last position.
        /// </summary>
        /// <param name="magnitude">
        /// The scrambling magnitude. The higher the its value, the more complex
        /// the resulting element order.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="magnitude"/> is less than 1.0.
        /// </exception>
        public void Scramble(double magnitude)
        {
            if (magnitude < 1.0) { throw new ArgumentOutOfRangeException("magnitude"); }

            OnScrambling(new EventArgs());

            List<Position> startPositions;
            Position lastBlankPosition = blankPosition;

            ulong initialCondition = currentCondition;
            int log = (int) (Math.Log(100.0, Math.Max(Width, Height) / 2.0) * magnitude);

            while (currentCondition == initialCondition) {
                for (int i = 0, j = 0; i < log; i++) {
                    startPositions = GetPossibleStartPositions();
                    j = RandomUtility.GetRandomInt(startPositions.Count);
                    if (startPositions[j] == lastBlankPosition) {
                        if (j == 0) { j = 1; } else { j--; }
                    }

                    MoveFrom(startPositions[j].X, startPositions[j].Y, true);
                    lastBlankPosition = blankPosition;
                }
            }

            OnScrambled(new EventArgs());
        }

        /// <summary>
        /// Moves the elements starting from the specified <see cref="Position"/> toward
        /// the Blank.
        /// </summary>
        /// <param name="position">The starting <see cref="Position"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the elements could be moved; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool MoveFrom(Position position)
        {
            return MoveFrom(position.X, position.Y, false);
        }

        /// <summary>
        /// Moves the elements starting from the specified zero-based coordinates toward
        /// the Blank.
        /// </summary>
        /// <param name="x">The x-coordinate of the starting position.</param>
        /// <param name="y">The y-coordinate of the starting position.</param>
        /// <returns>
        /// <see langword="true"/> if the elements could be moved; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool MoveFrom(int x, int y)
        {
            return MoveFrom(x, y, false);
        }

        /// <summary>
        /// Moves the elements starting from the specified zero-based coordinates
        /// toward the Blank.
        /// </summary>
        /// <param name="x">The x-coordinate of the starting position.</param>
        /// <param name="y">The y-coordinate of the starting position.</param>
        /// <param name="isScrambling">
        /// A value indicating whether this method has been called while scrambling.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the elements could be moved; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Moving occurs if and only if the Blank has either the same
        /// x-coordinate or the same y-coordinate as the starting position.
        /// </remarks>
        private bool MoveFrom(int x, int y, bool isScrambling)
        {
            if (x == blankPosition.X && y != blankPosition.Y) {
                // Move vertically
                if (y < blankPosition.Y) {
                    MoveDown(x, y, isScrambling);
                } else {
                    MoveUp(x, y, isScrambling);
                }
            } else if (y == blankPosition.Y && x != blankPosition.X) {
                // Move horizontally
                if (x < blankPosition.X) {
                    MoveRight(x, y, isScrambling);
                } else {
                    MoveLeft(x, y, isScrambling);
                }
            } else {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Moves down the elements starting from the specified zero-based
        /// coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the starting position.</param>
        /// <param name="y">The y-coordinate of the starting position.</param>
        /// <param name="isScrambling">
        /// A value indicating whether this method has been called while scrambling.
        /// </param>
        private void MoveDown(int x, int y, bool isScrambling)
        {
            Hook blankHook;

            for (int i = blankPosition.Y; i > y; i--) {
                blankHook = hooks[i, x];
                hooks[i, x] = hooks[i - 1, x];
                hooks[i - 1, x] = blankHook;

                blankPosition.Y = i - 1;
                blankPosition.X = x;

                OnElementMoved(new ElementMovedEventArgs(new Position(x, i), new Position(x, i - 1),
                    i == blankPosition.Y, i == y + 1, isScrambling));
            }
        }

        /// <summary>
        /// Moves up the elements starting from the specified zero-based
        /// coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the starting position.</param>
        /// <param name="y">The y-coordinate of the starting position.</param>
        /// <param name="isScrambling">
        /// A value indicating whether this method has been called while scrambling.
        /// </param>
        private void MoveUp(int x, int y, bool isScrambling)
        {
            Hook blankHook;

            for (int i = blankPosition.Y; i < y; i++) {
                blankHook = hooks[i, x];
                hooks[i, x] = hooks[i + 1, x];
                hooks[i + 1, x] = blankHook;

                blankPosition.Y = i + 1;
                blankPosition.X = x;

                OnElementMoved(new ElementMovedEventArgs(new Position(x, i), new Position(x, i + 1),
                    i == blankPosition.Y, i == y - 1, isScrambling));
            }
        }

        /// <summary>
        /// Moves right the elements starting from the specified zero-based
        /// coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the starting position.</param>
        /// <param name="y">The y-coordinate of the starting position.</param>
        /// <param name="isScrambling">
        /// A value indicating whether this method has been called while scrambling.
        /// </param>
        private void MoveRight(int x, int y, bool isScrambling)
        {
            Hook blankHook;

            for (int i = blankPosition.X; i > x; i--) {
                blankHook = hooks[y, i];
                hooks[y, i] = hooks[y, i - 1];
                hooks[y, i - 1] = blankHook;

                blankPosition.Y = y;
                blankPosition.X = i - 1;

                OnElementMoved(new ElementMovedEventArgs(new Position(i, y), new Position(i - 1, y),
                    i == blankPosition.X, i == x + 1, isScrambling));
            }
        }

        /// <summary>
        /// Moves left the elements starting from the specified zero-based
        /// coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the starting position.</param>
        /// <param name="y">The y-coordinate of the starting position.</param>
        /// <param name="isScrambling">
        /// A value indicating whether this method has been called while scrambling.
        /// </param>
        private void MoveLeft(int x, int y, bool isScrambling)
        {
            Hook blankHook;

            for (int i = blankPosition.X; i < x; i++) {
                blankHook = hooks[y, i];
                hooks[y, i] = hooks[y, i + 1];
                hooks[y, i + 1] = blankHook;

                blankPosition.Y = y;
                blankPosition.X = i + 1;

                OnElementMoved(new ElementMovedEventArgs(new Position(i, y), new Position(i + 1, y),
                    i == blankPosition.X, i == x - 1, isScrambling));
            }
        }

        /// <summary>
        /// Calculates the solving condition for this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// A numeric value that represents the solving condition for this
        /// <see cref="Matrix"/>
        /// </returns>
        private ulong CalculateSolvingCondition()
        {
            solvingCondition = 0UL;
            int length = Width * Height;
            for (int i = 0; i < length; i++) { solvingCondition |= 1UL << i; }

            return solvingCondition;
        }

        /// <summary>
        /// Calculates the current condition of this <see cref="Matrix"/> according to
        /// the order of the element at the specified <see cref="Position"/>.
        /// </summary>
        /// <param name="position">
        /// The <see cref="Position"/> of the element whose order is used to calculate
        /// the current condition.
        /// </param>
        /// <returns>
        /// A numeric value that represents the current condition for this
        /// <see cref="Matrix"/>
        /// </returns>
        private ulong CalculateCurrentCondition(Position position)
        {
            ulong bit = 1UL << hooks[position.Y, position.X].Order;

            currentCondition = Position.ToOffset(position, Width) == hooks[position.Y, position.X].Order
                ? currentCondition | bit
                : currentCondition & ~bit;

            return currentCondition;
        }

        /// <summary>
        /// Absorbes the specified <see cref="Matrix"/> into this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to absorb.</param>
        /// <remarks>
        /// <see cref="Matrix.Absorb"/> is used internally by <see cref="GameSession"/>
        /// to preserve user-defined data and event handlers after a clone.
        /// </remarks>
        internal void Absorb(Matrix matrix)
        {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    hooks[y, x].Element = matrix[matrix.GetPosition(hooks[y, x].Order)];
                }
            }

            ElementMoved = matrix.ElementMoved;
            Scrambling = matrix.Scrambling;
            Scrambled = matrix.Scrambled;
            InstanceReset = matrix.InstanceReset;

            matrix.ElementMoved = null;
            matrix.Scrambling = null;
            matrix.Scrambled = null;
            matrix.InstanceReset = null;
        }

        /// <summary>
        /// Raises the <see cref="Matrix.Scrambled"/> event.
        /// </summary>
        /// <remarks>
        /// <see cref="Matrix.RaiseScrambledEvent"/> is used internally by
        /// <see cref="GameSession"/> to propagate the <see cref="Matrix.Scrambled"/>
        /// event to <see cref="Gamer"/> puzzles when <see cref="GameSession.Puzzle"/>
        /// has been scrambled.
        /// </remarks>
        internal void RaiseScrambledEvent()
        {
            OnScrambled (new EventArgs());
        }

        /// <summary>
        /// Raises the <see cref="Matrix.Scrambling"/> event.
        /// </summary>
        /// <remarks>
        /// <see cref="Matrix.RaiseScramblingEvent"/> is used internally by
        /// <see cref="GameSession"/> to propagate the <see cref="Matrix.Scrambling"/>
        /// event to <see cref="Gamer"/> puzzles when <see cref="GameSession.Puzzle"/>
        /// is about to be scrambled.
        /// </remarks>
        internal void RaiseScramblingEvent()
        {
            OnScrambling(new EventArgs());
        }

        /// <summary>
        /// Returns a string that represents this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="Matrix"/>.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int y = 0; y < Height; y++) {
                stringBuilder.Append("[");
                for (int x = 0; x < Width; x++) {
                    stringBuilder.Append(hooks[y, x].Order);
                    stringBuilder.Append(":");
                    stringBuilder.Append(hooks[y, x].Element);
                    if (x < Width - 1) stringBuilder.Append(", ");
                }
                stringBuilder.Append("]");
                if (y < Height - 1) stringBuilder.Append(", ");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Raises the <see cref="Matrix.Scrambling"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Matrix.OnScrambling"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnScrambling(EventArgs args)
        {
            if (Scrambling != null) { Scrambling(this, args); }
        }

        /// <summary>
        /// Raises the <see cref="Matrix.Scrambled"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Matrix.OnScrambled"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnScrambled(EventArgs args)
        {
            if (Scrambled != null) { Scrambled(this, args); }
        }

        /// <summary>
        /// Raises the <see cref="Matrix.InstanceReset"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Matrix.InstanceReset"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnInstanceReset(EventArgs args)
        {
            if (InstanceReset != null) {
                InstanceReset(this, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="Matrix.ElementMoved"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Matrix.OnElementMoved"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnElementMoved(ElementMovedEventArgs args)
        {
            CalculateCurrentCondition(args.OldPosition);
            CalculateCurrentCondition(args.NewPosition);

            hashCode = -1;
            if (ElementMoved != null) { ElementMoved(this, args); }
        }
        #endregion
    }
}
