#region Header
//+ <source name="IQProfile.cs" language="C#" begin="15-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace M64.AI
{
    /// <summary>
    /// Describes the IQ profile of an <see cref="ArtificialGamer"/>.
    /// </summary>
    public struct IQProfile
    {
        #region Fields
        /// <summary>
        /// Gets or sets a value indicating how much AI should divert the most
        /// appropriate distance estimation.
        /// </summary>
        /// <value>
        /// A value indicating how much AI should divert the most appropriate
        /// distance estimation.
        /// </value>
        public double DivertFactor;

        /// <summary>
        /// Gets or sets the maximum solve time.
        /// </summary>
        /// <value>The maximum solve time, in milliseconds.</value>
        public int MaxSolveTime;

        /// <summary>
        /// Gets or sets the minimum think time.
        /// </summary>
        /// <value>The minimum think time, in milliseconds.</value>
        public int MinThinkTime;

        /// <summary>
        /// Gets or sets the maximum think time.
        /// </summary>
        /// <value>The maximum think time, in milliseconds.</value>
        public int MaxThinkTime;
        #endregion

        #region
        /// <summary>
        /// Initializes a new instance of the <see cref="IQProfile"/> structure
        /// with the specified divert factor, maximum solve time, and minimum
        /// and maximum think time.
        /// </summary>
        /// <param name="divertFactor">
        /// A value indicating how much AI should divert the most appropriate
        /// distance estimation.
        /// </param>
        /// <param name="maxSolveTime">The maximum solve time, in milliseconds.</param>
        /// <param name="minThinkTime">The minimum think time, in milliseconds.</param>
        /// <param name="maxThinkTime">The maximum think time, in milliseconds.</param>
        public IQProfile(double divertFactor, int maxSolveTime, int minThinkTime, int maxThinkTime)
        {
            DivertFactor = divertFactor;
            MaxSolveTime = maxSolveTime;
            MinThinkTime = minThinkTime;
            MaxThinkTime = maxThinkTime;
        }
        #endregion
    }
}
