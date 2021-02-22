#region Header
//+ <source name="Node.cs" language="C#" begin="04-Feb-2021">
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
    /// Represents a node in the problem space.
    /// </summary>
    public class Node
    {
        #region Fields
        private Node parent;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class with the
        /// specified <see cref="State"/>.
        /// </summary>
        /// <param name="state">The <see cref="State"/> of the <see cref="Node"/>.</param>
        internal Node(State state)
        {
            State = state;
            Heuristic = -1;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the actual cost from this <see cref="Node"/> to the goal
        /// <see cref="Node"/>.
        /// </summary>
        /// <value>
        /// The actual cost from this <see cref="Node"/> to the goal <see cref="Node"/>.
        /// </value>
        public int Cost
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets an estimated cost from this <see cref="Node"/> to the goal
        /// <see cref="Node"/>.
        /// </summary>
        /// <value>
        /// An estimated cost from this <see cref="Node"/> to the goal <see cref="Node"/>.
        /// </value>
        public int Heuristic
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Move"/> associated with this <see cref="Node"/>.
        /// </summary>
        /// <value>The <see cref="Move"/> associated with this <see cref="Node"/>.</value>
        public Move Move
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets  the parent <see cref="Node"/> of this <see cref="Node"/>.
        /// </summary>
        /// <value>The parent <see cref="Node"/> of this <see cref="Node"/>.</value>
        public Node Parent
        {
            get { return parent; }
            internal set {
                parent = value;
                Cost = value.Cost + 1;
            }
        }

        /// <summary>
        /// Gets the <see cref="State"/> of this <see cref="Node"/>.
        /// </summary>
        /// <value>The <see cref="State"/> of this <see cref="Node"/>.</value>
        public State State
        {
            get;
            private set;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Node"/> classes for equality.
        /// </summary>
        /// <param name="node1">The first <see cref="Node"/> to compare.</param>
        /// <param name="node2">The Second <see cref="Node"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="Node.State"/> of <paramref name="node1"/>
        /// and <paramref name="node2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Node node1, Node node2)
        {
            return Equals(node1, node2);
        }

        /// <summary>
        /// Compares the specified <see cref="Node"/> classes for inequality.
        /// </summary>
        /// <param name="node1">The first <see cref="Node"/> to compare.</param>
        /// <param name="node2">The Second <see cref="Node"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="node1"/> and <paramref name="node2"/>
        /// have different <see cref="Node.State"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Node node1, Node node2)
        {
            return !Equals(node1, node2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="Node"/> classes for equality.
        /// </summary>
        /// <param name="node1">The first <see cref="Node"/> to compare.</param>
        /// <param name="node2">The second <see cref="Node"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="Node.State"/> of <paramref name="node1"/>
        /// and <paramref name="node2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Node node1, Node node2)
        {
            if ((node1 as object) == (node2 as object)) {
                return true;
            }

            if (node1 as object == null || node2 as object == null) {
                return false;
            }

            return node1.State == node2.State;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Node"/> and
        /// whether it contains the <see cref="State"/> as this <see cref="Node"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Node"/> and
        /// contains the same <see cref="Node.State"/> as this <see cref="Node"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Node)) {
                return false;
            }

            return Equals(this, (Node) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Node"/> with this <see cref="Node"/>
        /// for equality.
        /// </summary>
        /// <param name="node">The <see cref="Node"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Node"/> contains
        /// the same <see cref="Node.State"/> as this <see cref="Node"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Node node)
        {
            return Equals(this, node);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Node"/>.
        /// </summary>
        /// <returns>The hash code of this <see cref="Node"/>.</returns>
        public override int GetHashCode()
        {
            return State.GetHashCode();
        }
        #endregion
    }
}
