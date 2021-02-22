#region Header
//+ <source name="MatrixEnumerator.cs" language="C#" begin="24-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Collections;
#endregion;

namespace M64.Engine
{
    /// <summary>
    /// Provides a simple iteration mechanism over a <see cref="Matrix"/>.
    /// </summary>
    public class MatrixEnumerator : IEnumerator
    {
        #region Fields
        private readonly int Length;
        private Matrix matrix;
        private int position = -1;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixEnumerator"/>
        /// class with the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to enumerate.</param>
        public MatrixEnumerator(Matrix matrix)
        {
            this.matrix = matrix;
            Length = matrix.Width * matrix.Height;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the value of the current element in the <see cref="Matrix"/>.
        /// </summary>
        /// <value>
        /// The value of the current element in the <see cref="Matrix"/>.
        /// </value>
        public object Current
        {
            get { return matrix[Position.FromOffset(position, matrix.Width)]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Disposes of this <see cref="MatrixEnumerator"/> and releases all the
        /// associated resources.
        /// </summary>
        /// <remarks>
        /// The default implementation of <see cref="Dispose"/> does nothing.
        /// </remarks>
        void Dispose()
        {
        }

        /// <summary>
        /// Advances the enumerator to the next element of the <see cref="Matrix"/>.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the enumerator was successfully advanced
        /// to the next element; <see langword="false"/> if the enumerator has
        /// passed the end of the <see cref="Matrix"/>.
        /// </returns>
        public bool MoveNext()
        {
            position++;
            return (position < Length);
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the
        /// first element of the <see cref="Matrix"/>.
        /// </summary>
        public void Reset()
        {
            position = -1;
        }
        #endregion;
    }
}
