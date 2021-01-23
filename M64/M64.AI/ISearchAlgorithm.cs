#region Header
//+ <source name="ISearchAlgorithm.cs" language="C#" begin="04-Feb-2012">
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

namespace M64.AI
{
    /// <summary>
    /// Represents a search algorithm.
    /// </summary>
    public interface ISearchAlgorithm
    {
        #region Properties
        /// <summary>
        /// Gets or sets the heuristic function.
        /// </summary>
        /// <value>The heuristic function.</value>
        IHeuristicFunction HeuristicFunction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of time after which the algorithm stops
        /// searching for the solution.
        /// </summary>
        /// <value>The search timeout, in milliseconds.</value>
        /// <remarks>
        /// If <see cref="ISearchAlgorithm.Timeout"/> is zero, then the
        /// search algorithm never timeouts.
        /// </remarks>
        long Timeout
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Solves the specified initial <see cref="State"/> by finding the
        /// <see cref="Path"/> to reach the specified goal <see cref="State"/>.
        /// </summary>
        /// <param name="initialState">The initial <see cref="State"/>.</param>
        /// <param name="goalState">The goal <see cref="State"/>.</param>
        /// <returns>The <see cref="Solution"/> to the problem.</returns>
        /// <exception cref="TimeoutException">
        /// The algorithm was unable to find the solution within the amount of
        /// time specified by <see cref="ISearchAlgorithm.Timeout"/>.
        /// </exception>
        Solution Solve(State initialState, State goalState);
        #endregion
    }
}
