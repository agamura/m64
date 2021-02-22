#region Header
//+ <source name="BufferUtility.cs" language="C#" begin="26-Jun-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region Reference
using System;
using System.Linq;
#endregion

namespace M64.Engine.Utilities
{
    /// <summary>
    /// Provides functionality for manipulating arrays of bytes.
    /// </summary>
    public static class BufferUtility
    {
        #region Methods
        /// <summary>
        /// Combines the specified arrays into one array.
        /// </summary>
        /// <param name="arrays">The array to combine.</param>
        /// <returns>The combined array.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arrays"/> is <see langword="null"/>.
        /// </exception>
        public static byte[] Combine(params byte[][] arrays)
        {
            if (arrays == null) { throw new ArgumentNullException("arrays"); }

            byte[] combined = new byte[arrays.Sum(a => a != null ? a.Length : 0)];
            int offset = 0;

            foreach (byte[] array in arrays) {
                if (array != null) {
                    Buffer.BlockCopy(array, 0, combined, offset, array.Length);
                    offset += array.Length;
                }
            }

            return combined;
        }

        /// <summary>
        /// Determines whether the specified byte arrays are equal.
        /// </summary>
        /// <param name="array1">
        /// The byte array to compare with <paramref name="array2"/>.
        /// </param>
        /// <param name="array2">
        /// The byte array to compare with <paramref name="array1"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="array1"/> is equal to
        /// <paramref name="array2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(byte[] array1, byte[] array2)
        {
            if (array1 == array2) { return true; }
            if (array1 == null || array2 == null) { return false; }
            if (array1.Length != array2.Length) { return false; }

            unsafe {
                fixed (byte* pArray1 = array1, pArray2 = array2) {
                    byte* p1 = pArray1, p2 = pArray2;

                    // Loop over the count in blocks of 8 bytes, checking a
                    // long integer (8 bytes) at a time
                    int count = array1.Length;
                    for (int i = 0; i < count / sizeof(long); i++, p1 += sizeof(long), p2 += sizeof(long)) {
                        if (*((long*) p1) != *((long*) p2)) { return false; }
                    }

                    // Complete the check by comparing any bytes that weren't
                    // compared in blocks of 8
                    if ((count & sizeof(int)) != 0) {
                        if (*((int*) p1) != *((int*) p2)) { return false; }
                        p1 += sizeof(int); p2 += sizeof(int);
                    }
                    if ((count & sizeof(short)) != 0) {
                        if (*((short*) p1) != *((short*) p2)) { return false; }
                        p1 += sizeof(short); p2 += sizeof(short);
                    }
                    if ((count & sizeof(byte)) != 0) {
                        if (*((byte*) p1) != *((byte*) p2)) { return false; }
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
