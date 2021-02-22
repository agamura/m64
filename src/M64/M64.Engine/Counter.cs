#region Header
//+ <source name="Counter.cs" language="C#" begin="18-Nov-2020">
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
    /// Represents a counter and provides functionality for initializing,
    /// incrementing, and decrementing its value.
    /// </summary>
    public class Counter
    {
        #region Fields
        /// <summary>
        /// Specifies the default increment factor.
        /// </summary>
        public const int DefaultIncrementFactor = 1;

        /// <summary>
        /// Specifies the default decrement factor.
        /// </summary>
        public const int DefaultDecrementFactor = 1;

        private int incrementFactor = Counter.DefaultIncrementFactor;
        private int decrementFactor = Counter.DefaultDecrementFactor;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Counter"/> class.
        /// </summary>
        public Counter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter"/> class
        /// with the specified initial value.
        /// </summary>
        /// <param name="value">The initial <see cref="Counter"/> value.</param>
        public Counter(int value)
        {
            Current = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current <see cref="Counter"/> value.
        /// </summary>
        /// <value>The current <see cref="Counter"/> value.</value>
        public int Current
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the increment factor.
        /// </summary>
        /// <value>The increment factor.</value>
        public int IncrementFactor
        {
            get { return incrementFactor; }
            set { incrementFactor = Math.Abs(value); }
        }

        /// <summary>
        /// Gets or sets the decrement factor.
        /// </summary>
        /// <value>The decrement factor.</value>
        public int DecrementFactor
        {
            get { return decrementFactor; }
            set { decrementFactor = Math.Abs(value); }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Counter"/> instances for equality.
        /// </summary>
        /// <param name="counter1">The first <see cref="Counter"/> to compare.</param>
        /// <param name="counter2">The Second <see cref="Counter"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="counter1"/> and
        /// <paramref name="counter2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Counter counter1, Counter counter2)
        {
            return Equals(counter1, counter2);
        }

        /// <summary>
        /// Compares the specified <see cref="Counter"/> instances for inequality.
        /// </summary>
        /// <param name="counter1">The first <see cref="Counter"/> to compare.</param>
        /// <param name="counter2">The Second <see cref="Counter"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the values of <paramref name="counter1"/> and
        /// <paramref name="counter2"/> are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Counter counter1, Counter counter2)
        {
            return !Equals(counter1, counter2);
        }

        /// <summary>
        /// Increments the specified <see cref="Counter"/> by <see cref="Counter.IncrementFactor"/>.
        /// </summary>
        /// <param name="counter">The <see cref="Counter"/> to increment.</param>
        /// <returns><paramref name="counter"/>.</returns>
        public static Counter operator ++(Counter counter)
        {
            counter.Increment();
            return counter;
        }

        /// <summary>
        /// Decrements the specified <see cref="Counter"/> by <see cref="Counter.DecrementFactor"/>.
        /// </summary>
        /// <param name="counter">The <see cref="Counter"/> to decrement.</param>
        /// <returns><paramref name="counter"/>.</returns>
        public static Counter operator --(Counter counter)
        {
            counter.Decrement();
            return counter;
        }

        /// <summary>
        /// Adds the value of one <see cref="Counter"/> to the value of another <see cref="Counter"/>.
        /// </summary>
        /// <param name="counter1">The <see cref="Counter"/> on the left side of the operator.</param>
        /// <param name="counter2">The <see cref="Counter"/> on the right side of the operator.</param>
        /// <returns>A <see cref="Counter"/> that is the result of the operation.</returns>
        public static Counter operator +(Counter counter1, Counter counter2)
        {
            Counter counter = new Counter();

            counter.Current = counter1.Current + counter2.Current;
            counter.decrementFactor = counter1.decrementFactor;
            counter.incrementFactor = counter1.incrementFactor;

            return counter;
        }

        /// <summary>
        /// Subtracts the value of one <see cref="Counter"/> from the value of another <see cref="Counter"/>.
        /// </summary>
        /// <param name="counter1">The <see cref="Counter"/> on the left side of the operator.</param>
        /// <param name="counter2">The <see cref="Counter"/> on the right side of the operator.</param>
        /// <returns>A <see cref="Counter"/> that is the result of the operation.</returns>
        public static Counter operator -(Counter counter1, Counter counter2)
        {
            Counter counter = new Counter();

            counter.decrementFactor = counter1.decrementFactor;
            counter.incrementFactor = counter1.incrementFactor;

            if (counter1.Current < counter2.decrementFactor) {
                counter.Current = 0;
            } else {
                counter.Current = counter1.Current - counter2.Current;
            }

            return counter;
        }

        /// <summary>
        /// Determines whether the value of a <see cref="Counter"/> is greater than
        /// the value of another <see cref="Counter"/>.
        /// </summary>
        /// <param name="counter1">The <see cref="Counter"/> on the left side of the operator.</param>
        /// <param name="counter2">The <see cref="Counter"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="counter1"/> is greater than
        /// <paramref name="counter2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(Counter counter1, Counter counter2)
        {
            return counter1.Current > counter2.Current;
        }

        /// <summary>
        /// Determines whether the value of a <see cref="Counter"/> is less than
        /// the value of another <see cref="Counter"/>.
        /// </summary>
        /// <param name="counter1">The <see cref="Counter"/> on the left side of the operator.</param>
        /// <param name="counter2">The <see cref="Counter"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="counter1"/> is less than
        /// <paramref name="counter2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(Counter counter1, Counter counter2)
        {
            return counter1.Current < counter2.Current;
        }

        /// <summary>
        /// Determines whether the value of a <see cref="Counter"/> is greater than or equal to
        /// the value of another <see cref="Counter"/>.
        /// </summary>
        /// <param name="counter1">The <see cref="Counter"/> on the left side of the operator.</param>
        /// <param name="counter2">The <see cref="Counter"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="counter1"/> is greater than or equal to
        /// <paramref name="counter2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(Counter counter1, Counter counter2)
        {
            return counter1.Current >= counter2.Current;
        }

        /// <summary>
        /// Determines whether the value of a <see cref="Counter"/> is less than or equal to
        /// the value of another <see cref="Counter"/>.
        /// </summary>
        /// <param name="counter1">The <see cref="Counter"/> on the left side of the operator.</param>
        /// <param name="counter2">The <see cref="Counter"/> on the right side of the operator.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="counter1"/> is less than or equal to
        /// <paramref name="counter2"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(Counter counter1, Counter counter2)
        {
            return counter1.Current <= counter2.Current;
        }

        /// <summary>
        /// Converts the specified <see cref="Counter"/> implicitly to an integer.
        /// </summary>
        /// <param name="counter">The <see cref="Counter"/> to convert.</param>
        /// <returns>The resulting integer.</returns>
        public static implicit operator int(Counter counter)
        {
            return counter.Current;
        }

        /// <summary>
        /// Converts the specified integer explicitly to a <see cref="Counter"/>.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>The resulting <see cref="Counter"/>.</returns>
        public static explicit operator Counter(int value)
        {
            return new Counter(value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a shallow copy of the this <see cref="Counter"/>.
        /// </summary>
        /// <returns>
        /// A shallow copy of this <see cref="Counter"/>.
        /// </returns>
        public virtual Counter Clone()
        {
            Counter counter = new Counter();
            counter.decrementFactor = decrementFactor;
            counter.incrementFactor = incrementFactor;
            counter.Current = Current;

            return counter;
        }

        /// <summary>
        /// Compares the specified <see cref="Counter"/> instances for equality.
        /// </summary>
        /// <param name="counter1">The first <see cref="Counter"/> to compare.</param>
        /// <param name="counter2">The second <see cref="Counter"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both <paramref name="counter1"/> and <paramref name="counter2"/>
        /// have the same values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Counter counter1, Counter counter2)
        {
            if ((counter1 as object) == (counter2 as object)) {
                return true;
            }

            if (counter1 as object == null || counter2 as object == null) {
                return false;
            }

            return counter1.Current == counter2.Current;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Counter"/> and
        /// whether it has the same value as this <see cref="Counter"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Counter"/> and
        /// has the same value as this <see cref="Counter"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Counter)) {
                return false;
            }

            return Equals(this, (Counter)obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Counter"/> with this <see cref="Counter"/>
        /// for equality.
        /// </summary>
        /// <param name="counter">The <see cref="Counter"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="counter"/> has the same value as this
        /// <see cref="Counter"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Counter counter)
        {
            return Equals(this, counter);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Counter"/>.
        /// </summary>
        /// <returns>
        /// The hash code of this <see cref="Counter"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Resets this <see cref="Counter"/> value to 0.
        /// </summary>
        public void Reset()
        {
            Current = 0;
        }

        /// <summary>
        /// Increments this <see cref="Counter"/> by <see cref="Counter.IncrementFactor"/>.
        /// </summary>
        /// <returns>The incremented <see cref="Counter"/> value.</returns>
        public int Increment()
        {
            return Current += incrementFactor;
        }

        /// <summary>
        /// Decrements this <see cref="Counter"/> by <see cref="Counter.DecrementFactor"/>.
        /// </summary>
        /// <returns>The decremented <see cref="Counter"/> value.</returns>
        public int Decrement()
        {
            if (Current < decrementFactor) {
                Current = 0;
            } else {
                Current -= decrementFactor;
            }

            return Current;
        }
        #endregion
    }
}
