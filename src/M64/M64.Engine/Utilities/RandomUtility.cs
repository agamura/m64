#region Header
//+ <source name="RandomUtility.cs" language="C#" begin="25-Mar-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#endregion

namespace M64.Engine.Utilities
{
    /// <summary>
    /// Helper class for generating pseudo-random numbers.
    /// </summary>
    public class RandomUtility
    {
        #region Fields
        private static Random random;
        #endregion

        #region properties
        private static Random Random
        {
            get {
                if (RandomUtility.random == null) {
                    RandomUtility.random = new Random((int) DateTime.Now.Ticks);
                }

                return RandomUtility.random;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>
        /// An integer greater than or equal to zero, and less than <b>Int32.MaxValue</b>.
        /// </returns>
        public static int GetRandomInt()
        {
            return GetRandomInt(0, Int32.MaxValue);
        }

        /// <summary>
        /// Returns a non-negative random integer less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random integer to be generated. <paramref name="maxValue"/>
        /// must be greater than or equal to zero. 
        /// </param>
        /// <returns>
        /// An integer greater than or equal to zero, and less than <paramref name="maxValue"/>.
        /// </returns>
        public static int GetRandomInt(int maxValue)
        {
            return GetRandomInt(0, maxValue);
        }

        /// <summary>
        /// Returns a random integer within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random integer returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random integer returned.</param>
        /// <returns>
        /// An integer greater than or equal to <paramref name="minValue"/> and less
        /// than <paramref name="maxValue"/>.
        /// </returns>
        public static int GetRandomInt(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a non-negative random float.
        /// </summary>
        /// <returns>
        /// A float greater than or equal to zero, and less than <b>Single.MaxValue</b>.
        /// </returns>
        public static float GetRandomFloat()
        {
            return GetRandomFloat(0f, Single.MaxValue);
        }

        /// <summary>
        /// Returns a non-negative random float less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random float to be generated. <paramref name="maxValue"/>
        /// must be greater than or equal to zero. 
        /// </param>
        /// <returns>
        /// A float greater than or equal to zero, and less than <paramref name="maxValue"/>.
        /// </returns>
        public static float GetRandomFloat(float maxValue)
        {
            return GetRandomFloat(0f, maxValue);
        }

        /// <summary>
        /// Returns a random float within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random float returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random float returned.</param>
        /// <returns>
        /// A float greater than or equal to <paramref name="minValue"/> and less
        /// than <paramref name="maxValue"/>.
        /// </returns>
        public static float GetRandomFloat(float minValue, float maxValue)
        {
            return (float)Random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        /// <returns>
        /// A byte greater than or equal to zero, and less than <b>Byte.MaxValue</b>.
        /// </returns>
        public static byte GetRandomByte()
        {
            return GetRandomByte(0, Byte.MaxValue);
        }

        /// <summary>
        /// Returns a random byte less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random byte to be generated. <paramref name="maxValue"/>
        /// must be greater than or equal to zero. 
        /// </param>
        /// <returns>
        /// A byte greater than or equal to zero, and less than <paramref name="maxValue"/>.
        /// </returns>
        public static byte GetRandomByte(byte maxValue)
        {
            return (byte)GetRandomInt(0, maxValue);
        }

        /// <summary>
        /// Returns a random byte within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random byte returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random byte returned.</param>
        /// <returns>
        /// A byte greater than or equal to <paramref name="minValue"/> and less
        /// than <paramref name="maxValue"/>.
        /// </returns>
        public static byte GetRandomByte(byte minValue, byte maxValue)
        {
            return (byte)GetRandomInt(minValue, maxValue);
        }
        #endregion
    }
}
