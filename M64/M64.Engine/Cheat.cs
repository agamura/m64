#region Header
//+ <source name="Cheat.cs" language="C#" begin="21-Nov-2011">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2011">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using PlaXore;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Represents an advantage beyond normal gameplay.
    /// </summary>
    public class Cheat
    {
        #region Fields
        private bool isActive;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the <see cref="Cheat"/> state has been changed.
        /// </summary>
        public event EventHandler<CheatStateChangedEventArgs> StateChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Cheat"/> class.
        /// </summary>
        public Cheat()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cheat"/> class with the
        /// specified id, name, and hint.
        /// </summary>
        /// <param name="id">The id of the <see cref="Cheat"/>.</param>
        /// <param name="name">The name of the <see cref="Cheat"/>.</param>
        /// <param name="hint">A short text that describes what the <see cref="Cheat"/> does.</param>
        public Cheat(int id, string name, string hint)
        {
            Id = id;
            Name = name;
            Hint = hint;
            ElapseFactor = Stopwatch.DefaultElapseFactor;
            IncrementFactor = Counter.DefaultIncrementFactor;
            DecrementFactor = Counter.DefaultDecrementFactor;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether or not this <see cref="Cheat"/>
        /// is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Cheat"/> is active; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public virtual bool IsActive
        {
            get { return isActive; }
            set {
                bool isActive = this.isActive;
                this.isActive = value;

                if (isActive != value) {
                    CheatStateChangedType cheatStateChangedType = value
                        ? CheatStateChangedType.Activated
                        : CheatStateChangedType.Deactivated;

                    OnStateChanged(new CheatStateChangedEventArgs(cheatStateChangedType));
                }
            }
        }

        /// <summary>
        /// Gets or sets the id of this <see cref="Cheat"/>.
        /// </summary>
        /// <value>The id of this <see cref="Cheat"/>.</value>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of this <see cref="Cheat"/>.
        /// </summary>
        /// <value>The name <see cref="Cheat"/>.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hint message for this <see cref="Cheat"/>.
        /// </summary>
        /// <value>The hint message for this <see cref="Cheat"/>.</value>
        public string Hint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the elapse factor associated with this <see cref="Cheat"/>.
        /// </summary>
        /// <value>The elapse factor.</value>
        /// <seealso cref="PlaXore.Stopwatch.ElapseFactor"/>
        public double ElapseFactor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the increment factor associated with this <see cref="Cheat"/>.
        /// </summary>
        /// <value>The increment factor.</value>
        /// <seealso cref="PlaXore.Counter.IncrementFactor"/>
        public int IncrementFactor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the decrement factor associated with this <see cref="Cheat"/>.
        /// </summary>
        /// <value>The decrement factor.</value>
        /// <seealso cref="PlaXore.Counter.DecrementFactor"/>
        public int DecrementFactor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Game"/> this <see cref="Cheat"/> is
        /// associated with.
        /// </summary>
        /// <value>
        /// The <see cref="Game"/> this <see cref="Cheat"/> is associated with, or
        /// <see langword="null"/> if not associated with any <see cref="Game"/>.
        /// </value>
        public Game Game
        {
            get;
            internal set;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Compares the specified <see cref="Cheat"/> classes for equality.
        /// </summary>
        /// <param name="cheat1">The first <see cref="Cheat"/> to compare.</param>
        /// <param name="cheat2">The Second <see cref="Cheat"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the properties of <paramref name="cheat1"/> and
        /// <paramref name="cheat2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Cheat cheat1, Cheat cheat2)
        {
            return Equals(cheat1, cheat2);
        }

        /// <summary>
        /// Compares the specified <see cref="Cheat"/> classes for inequality.
        /// </summary>
        /// <param name="cheat1">The first <see cref="Cheat"/> to compare.</param>
        /// <param name="cheat2">The Second <see cref="Cheat"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the properties of <paramref name="cheat1"/> and
        /// <paramref name="cheat2"/> are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Cheat cheat1, Cheat cheat2)
        {
            return !Equals(cheat1, cheat2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a shallow copy of this <see cref="Cheat"/>.
        /// </summary>
        /// <returns>
        /// A shallow copy of this <see cref="Cheat"/>.
        /// </returns>
        public Cheat Clone()
        {
            Cheat cheat = new Cheat(Id, Name, Hint);
            cheat.ElapseFactor = ElapseFactor;
            cheat.IncrementFactor = IncrementFactor;
            cheat.DecrementFactor = DecrementFactor;

            return cheat;
        }

        /// <summary>
        /// Compares the specified <see cref="Cheat"/> classes for equality.
        /// </summary>
        /// <param name="cheat1">The first <see cref="Cheat"/> to compare.</param>
        /// <param name="cheat2">The second <see cref="Cheat"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the properties of <paramref name="cheat1"/> and
        /// <paramref name="cheat2"/> are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals(Cheat cheat1, Cheat cheat2)
        {
            if ((cheat1 as object) == (cheat2 as object)) {
                return true;
            }

            if (cheat1 as object == null || cheat2 as object == null) {
                return false;
            }

            return cheat1.Id == cheat2.Id
                && cheat1.Name == cheat2.Name
                && cheat1.Hint == cheat2.Hint
                && cheat1.ElapseFactor == cheat2.ElapseFactor
                && cheat1.IncrementFactor == cheat2.IncrementFactor
                && cheat1.DecrementFactor == cheat2.DecrementFactor;
        }

        /// <summary>
        /// Determines whether the specified object is a <see cref="Cheat"/> and
        /// whether it contains the same properties as this <see cref="Cheat"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="Cheat"/>
        /// and contains the same property values as this <see cref="Cheat"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Cheat)) {
                return false;
            }

            return Equals(this, (Cheat) obj);
        }

        /// <summary>
        /// Compares the specified <see cref="Cheat"/> with this <see cref="Cheat"/>
        /// for equality.
        /// </summary>
        /// <param name="cheat">The <see cref="Cheat"/> to compare.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="Cheat"/> contains the 
        /// same property values as this <see cref="Cheat"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(Cheat cheat)
        {
            return Equals(this, cheat);
        }

        /// <summary>
        /// Returns the hash code of this <see cref="Cheat"/>.
        /// </summary>
        /// <returns>The hash code of this <see cref="Cheat"/>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Raises the <see cref="Cheat.StateChanged"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Cheat.StateChanged"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnStateChanged(CheatStateChangedEventArgs args)
        {
            if (StateChanged != null) { StateChanged(this, args); }
        }
        #endregion
    }
}
