#region Header
//+ <source name="Solution.cs" language="C#" begin="04-Feb-2021">
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
    /// Represents the solution to a pathfinding problem.
    /// </summary>
    public struct Solution
    {
        #region Fields
        private Path path;
        private long expandedNodeCount;
        private long time;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Solution"/> class with
        /// the specified <see cref="Path"/>, expanded <see cref="Node"/> count,
        /// and time.
        /// </summary>
        /// <param name="path">
        /// The <see cref="Path"/> from the initial <see cref="State"/> to the
        /// goal <see cref="State"/>.
        /// </param>
        /// <param name="expandedNodeCount">
        /// The number of nodes expanded to find the <see cref="Solution"/>.
        /// </param>
        /// <param name="time">
        /// The time it took to find the <see cref="Solution"/>, in nanoseconds.
        /// </param>
        internal Solution(Path path, long expandedNodeCount, long time)
        {
            this.path = path;
            this.expandedNodeCount = expandedNodeCount;
            this.time = time;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Path"/> from the initial <see cref="State"/> to the
        /// goal <see cref="State"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Path"/> from the initial <see cref="State"/> to the
        /// goal <see cref="State"/>.
        /// </value>
        public Path Path
        {
            get { return path; }
        }

        /// <summary>
        /// Gets the time it took to find the <see cref="Solution"/>.
        /// </summary>
        /// <value>
        /// The time it took to find the <see cref="Solution"/>, in nanoseconds.
        /// </value>
        public long Time
        {
            get { return time; }
        }

        /// <summary>
        /// Gets the number of nodes expanded to find the <see cref="Solution"/>.
        /// </summary>
        /// <value>
        /// The number of nodes expanded to find the <see cref="Solution"/>
        /// .</value>
        public long ExpandedNodeCount
        {
            get { return expandedNodeCount; }
        }
        #endregion
    }
}
