#region Header
//+ <source name="ManhattanDistance.cs" language="C#" begin="04-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using M64.Engine;
#endregion

namespace M64.AI
{
    /// <summary>
    /// Implements the manhattan distance heuristic function.
    /// </summary>
    public class ManhattanDistance : IHeuristicFunction
    {
        private double divertFactor = 1.0;

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating how much the heuristic function
        /// diverts the most appropriate distance estimation.
        /// </summary>
        /// <value>
        /// A value indicating how much the heuristic function diverts the
        /// most appropriate distance estimation.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than 1.0.
        /// </exception>
        public double DivertFactor
        {
            get { return divertFactor; }
            set {
                if (value < 1.0)
                    throw new ArgumentOutOfRangeException();

                divertFactor = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Invokes the manhattan distance heuristic function with the specified
        /// initial <see cref="Node"/>.
        /// </summary>
        /// <param name="node">The initial <see cref="Node"/>.</param>
        /// <returns>
        /// <returns>
        /// An estimate of the minimum cost from <paramref name="node"/> to the
        /// goal <see cref="Node"/>.
        /// </returns>
        /// </returns>
        public int Invoke(Node node)
        {
            int heuristic = node.Heuristic;

            if (heuristic == -1) {
                heuristic = Calculate(node.State);
                node.Heuristic = heuristic;
            }

            return (int) Math.Ceiling((double) heuristic * divertFactor);
        }

        /// <summary>
        /// Calculates the the manhattan distance value for the specified <see cref="State"/>.
        /// </summary>
        /// <param name="state">
        /// The <see cref="State"/> for which to calculate the manhattan distance value.
        /// </param>
        /// <returns>The manhattan distance value.</returns>
        protected virtual int Calculate(State state)
        {
            int heuristic = 0 ,order;
            Matrix matrix = state.Matrix;

            for (int y = 0; y < matrix.Height; y++) {
                for (int x = 0; x < matrix.Width; x++) {
                    if (matrix.BlankPosition.X != x || matrix.BlankPosition.Y != y) {
                        order = matrix.GetOrder(x, y);
                        heuristic += (Math.Abs(y - (order / matrix.Height))
                                    + Math.Abs(x - (order % matrix.Width)));
                    }
                }
            }

            return heuristic;
        }
        #endregion
    }
}
