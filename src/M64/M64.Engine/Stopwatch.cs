#region Header
//+ <source name="Stopwatch.cs" language="C#" begin="17-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Provides functionality for measuring elapsed time.
    /// </summary>
    public class Stopwatch
    {
        #region Fields
        private static readonly double tickFrequency;

        private long elapsed;
        private long started;
        private bool isRunning;
        private double elapseFactor;

        /// <summary>
        /// Gets the default elapse factor.
        /// </summary>
        public const double DefaultElapseFactor = 1.0;

        /// <summary>
        /// Gets the frequency of the timer as the number of ticks per second.
        /// </summary>
        public static readonly long Frequency;

        /// <summary>
        /// Indicates whether the timer is based on a high-resolution performance counter. 
        /// </summary>
        public static readonly bool IsHighResolution;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes read-only members.
        /// </summary>
        static Stopwatch()
        {
            IsHighResolution = System.Diagnostics.Stopwatch.IsHighResolution;
            Frequency = System.Diagnostics.Stopwatch.Frequency;
            tickFrequency = IsHighResolution ? TimeSpan.TicksPerSecond / Frequency : 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stopwatch"/> class.
        /// </summary>
        public Stopwatch()
        {
            Reset();
            elapseFactor = DefaultElapseFactor;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating how fast this <see cref="Stopwatch"/>
        /// runs.
        /// </summary>
        /// <value>The elapse factor.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than <see cref="Double.Epsilon"/>.
        /// </exception>
        public double ElapseFactor
        {
            get { return elapseFactor; }
            set {
                if (value < Double.Epsilon) {
                    throw new ArgumentOutOfRangeException();
                }

                if (isRunning) {
                    elapsed = GetElapsedTicks();
                    started = GetTimestamp();
                }

                elapseFactor = value;
            }
        }

        /// <summary>
        /// Gets the total elapsed time measured by this <see cref="Stopwatch"/>.
        /// </summary>
        /// <value>A <see cref="TimeSpan"/> that represents the total elapsed time.</value>
        /// <remarks>
        /// The elapsed time is adjusted according to the current
        /// <see cref="Stopwatch.ElapseFactor"/>.
        /// </remarks>
        public TimeSpan Elapsed
        {
            get { return new TimeSpan(GetElapsedDateTimeTicks()); }
        }

        /// <summary>
        /// Gets the total elapsed time measured by this <see cref="Stopwatch"/>.
        /// </summary>
        /// <value>The total elapsed time, in milliseconds.</value>
        /// <remarks>
        /// The elapsed time is adjusted according to the current
        /// <see cref="Stopwatch.ElapseFactor"/>.
        /// </remarks>
        public long ElapsedMilliseconds
        {
            get { return GetElapsedDateTimeTicks() / TimeSpan.TicksPerMillisecond; }
        }

        /// <summary>
        /// Gets the total elapsed time measured by this <see cref="Stopwatch"/>.
        /// </summary>
        /// <value>The total elapsed time, in timer ticks.</value>
        /// <remarks>The elapsed time is adjusted according to the current
        /// <see cref="Stopwatch.ElapseFactor"/>.
        /// </remarks>
        public long ElapsedTicks
        {
            get { return GetElapsedTicks(); }
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="Stopwatch"/> is running.
        /// </summary>
        /// <value>
        /// <see langword="true"/> it this <see cref="Stopwatch"/> is running;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsRunning
        {
            get { return isRunning; }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Stopwatch"/> instances for equality.
        /// </summary>
        /// <param name="stopwatch1">The first <see cref="Stopwatch"/> to compare.</param>
        /// <param name="stopwatch2">The Second <see cref="Stopwatch"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the elapsed time of <paramref name="stopwatch1"/> and
        /// <paramref name="stopwatch2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Stopwatch stopwatch1, Stopwatch stopwatch2)
        {
            return Equals(stopwatch1, stopwatch2);
        }

        /// <summary>
        /// Compares the specified <see cref="Stopwatch"/> instances for inequality.
        /// </summary>
        /// <param name="stopwatch1">The first <see cref="Stopwatch"/> to compare.</param>
        /// <param name="stopwatch2">The Second <see cref="Stopwatch"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the elapsed time of <paramref name="stopwatch1"/> and
        /// <paramref name="stopwatch2"/> are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Stopwatch stopwatch1, Stopwatch stopwatch2)
        {
            return !Equals(stopwatch1, stopwatch2);
        }

        /// <summary>
        /// Determines whether the elapsed time of a <see cref="Stopwatch"/> is greater than
        /// the elapsed time of another <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="stopwatch1">The <see cref="Stopwatch"/> on the left side of the operator.</param>
        /// <param name="stopwatch2">The <see cref="Stopwatch"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="stopwatch1"/> is greater than
        /// <paramref name="stopwatch2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(Stopwatch stopwatch1, Stopwatch stopwatch2)
        {
            return stopwatch1.elapsed > stopwatch2.elapsed;
        }

        /// <summary>
        /// Determines whether the elapsed time of a <see cref="Stopwatch"/> is less than
        /// the elapsed time of another <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="stopwatch1">The <see cref="Stopwatch"/> on the left side of the operator.</param>
        /// <param name="stopwatch2">The <see cref="Stopwatch"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="stopwatch1"/> is less than
        /// <paramref name="stopwatch2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(Stopwatch stopwatch1, Stopwatch stopwatch2)
        {
            return stopwatch1.elapsed < stopwatch2.elapsed;
        }

        /// <summary>
        /// Determines whether the elapsed time of a <see cref="Stopwatch"/> is greater than or equal to
        /// the elapsed time of another <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="stopwatch1">The <see cref="Stopwatch"/> on the left side of the operator.</param>
        /// <param name="stopwatch2">The <see cref="Stopwatch"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="stopwatch1"/> is greater than or equal to
        /// <paramref name="stopwatch2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(Stopwatch stopwatch1, Stopwatch stopwatch2)
        {
            return stopwatch1.elapsed >= stopwatch2.elapsed;
        }

        /// <summary>
        /// Determines whether the elapsed time of a <see cref="Stopwatch"/> is less than or equal
        /// the elapsed time of another <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="stopwatch1">The <see cref="Stopwatch"/> on the left side of the operator.</param>
        /// <param name="stopwatch2">The <see cref="Stopwatch"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="stopwatch1"/> is less than or equal to
        /// <paramref name="stopwatch2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(Stopwatch stopwatch1, Stopwatch stopwatch2)
        {
            return stopwatch1.elapsed <= stopwatch2.elapsed;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Compares the specified <see cref="Stopwatch"/> instances for equality.
        /// </summary>
        /// <param name="stopwatch1">The first <see cref="Stopwatch"/> to compare.</param>
        /// <param name="stopwatch2">The second <see cref="Stopwatch"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the elapsed time of <paramref name="stopwatch1"/> and
        /// <paramref name="stopwatch2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Stopwatch stopwatch1, Stopwatch stopwatch2)
        {
            if ((stopwatch1 as object) == (stopwatch2 as object)) {
                return true;
            }

            if (stopwatch1 as object == null || stopwatch2 as object == null) {
                return false;
            }

            return stopwatch1.elapsed == stopwatch2.elapsed;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Stopwatch"/> and
        /// whether it has the same elapsed time as this <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Stopwatch"/>
        /// and has the same elapsed time as this <see cref="Stopwatch"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Stopwatch)) {
                return false;
            }

            return Equals(this, (Stopwatch)obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Stopwatch"/> with this
        /// <see cref="Stopwatch"/> for equality.
        /// </summary>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="stopwatch"/> has the same value as this
        /// <see cref="Stopwatch"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Stopwatch stopwatch)
        {
            return Equals(this, stopwatch);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Stopwatch"/>.
        /// </summary>
        /// <returns>
        /// The hash code of this <see cref="Stopwatch"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the number of ticks that represent the current date and time.
        /// </summary>
        /// <returns>
        /// The current date and time, in ticks.
        /// </returns>
        public static long GetTimestamp()
        {
            return IsHighResolution
                ? System.Diagnostics.Stopwatch.GetTimestamp()
                : DateTime.UtcNow.Ticks;
        }

        /// <summary>
        /// Stops time interval measurement and resets the elapsed time to 0.
        /// </summary>
        public void Reset()
        {
            elapsed = 0;
            started = 0;
            isRunning = false;
        }

        /// <summary>
        /// Starts measuring elapsed time for an interval.
        /// </summary>
        public void Start()
        {
            if (!isRunning) {
                started = GetTimestamp();
                isRunning = true;
            }
        }

        /// <summary>
        /// Stops measuring elapsed time for an interval.
        /// </summary>
        public void Stop()
        {
            if (isRunning) {
                elapsed = GetElapsedTicks();
                isRunning = false;
            }
        }

        /// <summary>
        /// Returns a string that represents this <see cref="Stopwatch"/>.
        /// </summary>
        /// <returns>A string that represents this <see cref="Stopwatch"/>.</returns>
        public override string ToString()
        {
            TimeSpan timeSpan = Elapsed;

            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        /// <summary>
        /// Returns the elapsed timer ticks.
        /// </summary>
        /// <returns>The elapsed timer ticks.</returns>
        private long GetElapsedTicks()
        {
            if (isRunning) {
                long elapsedTicks = GetTimestamp() - started;
                return elapsed + (long)((double)elapsedTicks * elapseFactor);
            }

            return elapsed;
        }

        /// <summary>
        /// Returns the elapsed timer ticks converted into DateTime ticks. 
        /// </summary>
        /// <returns>The elapsed DateTime ticks.</returns>
        private long GetElapsedDateTimeTicks()
        {
            long elapsedTicks = GetElapsedTicks();
            return IsHighResolution ? unchecked((long)((double)elapsedTicks * tickFrequency)) : elapsedTicks;
        }
        #endregion
    }
}
