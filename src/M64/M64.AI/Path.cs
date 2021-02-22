#region Header
//+ <source name="Path.cs" language="C#" begin="04-Feb-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System.Collections.Generic;
#endregion

namespace M64.AI
{
    /// <summary>
    /// Represents the path from the initial <see cref="State"/> to the goal
    /// <see cref="State"/>.
    /// </summary>
    public class Path
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class with the
        /// specified length.
        /// </summary>
        /// <param name="length">The length of the <see cref="Path"/>.</param>
        private Path(int length)
        {
            Nodes = new List<Node>(length);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the nodes of this <see cref="Path"/>.
        /// </summary>
        /// <value>The nodes of this <see cref="Path"/>.</value>
        public List<Node> Nodes
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the <see cref="Path"/> from the initial <see cref="Node"/>
        /// to the specified goal <see cref="Node"/>.
        /// </summary>
        /// <param name="goalNode">The goal <see cref="Node"/>.</param>
        /// <returns>
        /// The <see cref="Path"/> from the initial <see cref="Node"/> to
        /// <paramref name="goalNode"/>.
        /// </returns>
        internal static Path Create(Node goalNode)
        {
            if (goalNode == null) { return null; }

            Path path = new Path(goalNode.Cost + 1);
            path.Nodes.Insert(0, goalNode);
            
            Node parent = goalNode.Parent;
            Node current = null;

            while (parent != null) {
                current = parent;
                path.Nodes.Insert(0, current);
                parent = current.Parent;
            }

            return path;
        }
        #endregion
    }
}
