#region Header
//+ <source name="IDAStar.cs" language="C#" begin="04-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
#endregion

namespace M64.AI
{
    /// <summary>
    /// Implements the iterative-deepening-A* (IDA*) algorithm.
    /// </summary>
    /// <remarks>
    /// The IDA* algorithm is designed to solve problems with infinite search
    /// trees or for which it is difficult to construct a low-cost solution,
    /// like the slinding tile puzzles.
    /// </remarks>
    public class IDAStar : ISearchAlgorithm
    {
        #region Fields
        private long startTime;
        private long expandedNodeCount;
        private IHeuristicFunction heuristicFunction;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IDAStar"/> class.
        /// </summary>
        public IDAStar() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IDAStar"/> class with
        /// the specified <see cref="IHeuristicFunction"/>.
        /// </summary>
        /// <param name="heuristicFunction">The heuristic function.</param>
        public IDAStar(IHeuristicFunction heuristicFunction)
        {
            HeuristicFunction = heuristicFunction;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the heuristic function.
        /// </summary>
        /// <value>The heuristic function.</value>
        public IHeuristicFunction HeuristicFunction
        {
            get { return heuristicFunction; }
            set {
                heuristicFunction = value != null ? value : new ManhattanDistance();
            }
        }

        /// <summary>
        /// Gets or sets the amount of time after which the algorithm stops
        /// searching for the solution.
        /// </summary>
        /// <value>The search timeout, in milliseconds.</value>
        /// <remarks>
        /// If <see cref="IDAStar.Timeout"/> is zero, then the search algorithm
        /// never timeouts.
        /// </remarks>
        public long Timeout
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
        /// <returns>The <see cref="Solution"/> data.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="initialState"/> or <paramref name="goalState"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// The algorithm was unable to find the solution within the amount of
        /// time specified by <see cref="IDAStar.Timeout"/>.
        /// </exception>
        public Solution Solve(State initialState, State goalState)
        {
            if (initialState == null) { throw new ArgumentNullException("initialState"); }
            if (goalState == null) { throw new ArgumentNullException("goalState"); }

            startTime = DateTime.UtcNow.Ticks * 100;
            expandedNodeCount = 0;

            Node initialNode = new Node(initialState);
            Node goalNode = null;

            int costBound = heuristicFunction.Invoke(initialNode);

            while (goalNode == null) {
                goalNode = DepthFirstSearch(initialNode, costBound, goalState);
                costBound += 2;
            }

            long elapesed = (DateTime.Now.Ticks * 100) - startTime;
            return new Solution(Path.Create(goalNode), expandedNodeCount, elapesed);
        }

        /// <summary>
        /// Returns the next child <see cref="Node"/> of the deepest unexpanded
        /// <see cref="Node"/>, starting from the specified <see cref="Node"/>.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> to start searching from.</param>
        /// <param name="costBound">The current cost bound.</param>
        /// <param name="goalState">The goal <see cref="State"/>.</param>
        /// <returns>
        /// The next child <see cref="Node"/> of the deepest unexpanded <see cref="Node"/>.
        /// </returns>
        /// <exception cref="TimeoutException">
        /// The algorithm was unable to find the solution within the amount of
        /// time specified by <see cref="IDAStar.Timeout"/>.
        /// </exception>
        private Node DepthFirstSearch(Node node, int costBound, State goalState)
        {
            if (node.State == goalState) { return node; }
            expandedNodeCount++;

            Node[] children = GetChildren(node);

            int nextCostBound;
            foreach (Node child in children) {
                if (Timeout != 0L) {
                    if (((DateTime.UtcNow.Ticks * 100) - startTime) > (Timeout * 10e6)) {
                        throw new TimeoutException();
                    }
                }

                nextCostBound = child.Cost + heuristicFunction.Invoke(child);

                if (nextCostBound <= costBound) {
                    Node result = DepthFirstSearch(child, costBound, goalState);
                    if (result != null) { return result; }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the child nodes of the specified <see cref="Node"/>.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> for which to get the child nodes.</param>
        /// <returns>The child nodes of <paramref name="node"/></returns>
        private Node[] GetChildren(Node node)
        {
            State state = node.State;
            List<Move> possibleMoves = state.PossibleMoves;
            Move lastMove = node.Move;

            int childrenSize = lastMove == null ? possibleMoves.Count : possibleMoves.Count - 1;
            Node[] children = new Node[childrenSize];

            int i = 0;
            Node child;
            foreach (Move move in possibleMoves) {
                if (!move.IsInverse(lastMove)) {
                    child = new Node(move.ApplyTo(state));
                    child.Parent = node;
                    child.Move = move;
                    children[i++] = child;
                }
            }

            return children;
        }
        #endregion
    }
}
