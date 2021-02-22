#region Header
//+ <source name="IHeuristicFunction.cs" language="C#" begin="04-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace M64.AI
{
    /// <summary>
    /// Represents the heuristic function for a <see cref="ISearchAlgorithm"/>.
    /// </summary>
    public interface IHeuristicFunction
    {
        #region Methods
        /// <summary>
        /// Invokes the heuristic function with the specified initial <see cref="Node"/>.
        /// </summary>
        /// <param name="node">The initial <see cref="Node"/>.</param>
        /// <returns>
        /// An estimate of the minimum cost from <paramref name="node"/> to the
        /// goal <see cref="Node"/>.
        /// </returns>
        int Invoke(Node node);
        #endregion
    }
}
