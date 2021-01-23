#region Header
//+ <source name="Gamer.cs" language="C#" begin="11-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
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
    /// Represents a human being or artificial game player that takes part in a
    /// <see cref="GameSession"/>.
    /// </summary>
    public abstract class Gamer
    {
        #region Event Handlers
        private EventHandler<EventArgs>
            onPuzzleScrambling = delegate(object sender, EventArgs args)
            {
            };
        private EventHandler<EventArgs>
            onPuzzleScrambled = delegate(object sender, EventArgs args)
            {
            };
        private EventHandler<ElementMovedEventArgs>
            onPuzzleElementMoved = delegate(object sender, ElementMovedEventArgs args)
            {
            };
        private EventHandler<DictionaryChangedEventArgs<int, Cheat>>
            onCheatsChanged = delegate(object sender, DictionaryChangedEventArgs<int, Cheat> args)
            {
            };
        private EventHandler<CheatStateChangedEventArgs>
            onCheatStateChanged = delegate(object sender, CheatStateChangedEventArgs args)
            {
            
            };
        private EventHandler<GameOverEventArgs>
            onGameOver = delegate(object sender, GameOverEventArgs args)
            {
            };
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Gamer"/> class with the
        /// specified gamertag.
        /// </summary>
        /// <param name="gamertag">
        /// A string that uniquely identifies the <see cref="Gamer"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamertag"/> is <see langword="null"/> or empty.
        /// </exception>
        public Gamer(string gamertag) : this(Guid.NewGuid(), gamertag)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamer"/> class with the
        /// specified globally unique identifier and gamertag.
        /// </summary>
        /// <param name="id">
        /// A globally unique identifier that identifies this <see cref="Gamer"/>.
        /// </param>
        /// <param name="gamertag">
        /// A string that uniquely identifies the <see cref="Gamer"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamertag"/> is <see langword="null"/> or empty.
        /// </exception>
        public Gamer(Guid id, string gamertag)
        {
            if (String.IsNullOrEmpty(gamertag)) {
                throw new ArgumentNullException("gamertag");
            }

            Id = id;
            Gamertag = gamertag;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the delegate to be invoked when the <see cref="Game.Puzzle"/>
        /// is about to be scrambled.
        /// </summary>
        /// <value>
        /// The delegate to be invoked when the <see cref="Game.Puzzle"/> is
        /// about to be scrambled.
        /// </value>
        public virtual EventHandler<EventArgs> OnPuzzleScrambling
        {
            get { return onPuzzleScrambling; }
            set { onPuzzleScrambling = value; }
        }

        /// <summary>
        /// Gets or sets the delegate to be invoked when the <see cref="Game.Puzzle"/>
        /// has finished scrambling.
        /// </summary>
        /// <value>
        /// The delegate to be invoked when the <see cref="Game.Puzzle"/> has
        /// finished scrambling.
        /// </value>
        public virtual EventHandler<EventArgs> OnPuzzleScrambled
        {
            get { return onPuzzleScrambled; }
            set { onPuzzleScrambled = value; }
        }

        /// <summary>
        /// Gets or sets the delegate to be invoked when an element in the
        /// <see cref="Game.Puzzle"/> is moved.
        /// </summary>
        /// <value>
        /// The delegate to be invoked when an element in <see cref="Game.Puzzle"/>
        /// is moved.
        /// </value>
        /// <remarks>
        /// <see cref="Gamer.OnPuzzleElementMoved"/> is also invoked for scrambling
        /// moves; if necessary, use <see cref="ElementMovedEventArgs.IsScrambling"/>
        /// to filter them out.
        /// </remarks>
        public virtual EventHandler<ElementMovedEventArgs> OnPuzzleElementMoved
        {
            get { return onPuzzleElementMoved; }
            set { onPuzzleElementMoved = value; }
        }

        /// <summary>
        /// Gets or sets the delegate to be invoked when a <see cref="Cheat"/>
        /// is added to or removed from <see cref="Game.Cheats"/>
        /// </summary>
        /// <value>
        /// The delegate to be invoked when a <see cref="Cheat"/> is added to
        /// or removed from <see cref="Game.Cheats"/>
        /// </value>
        public virtual EventHandler<DictionaryChangedEventArgs<int, Cheat>> OnCheatsChanged
        {
            get { return onCheatsChanged; }
            set { onCheatsChanged = value; }
        }

        /// <summary>
        /// Gets or sets the delegate to be invoked when a <see cref="Cheat"/>
        /// in <see cref="Game.Cheats"/> changes its state.
        /// </summary>
        /// <value>
        /// The delegate to be invoked when a <see cref="Cheat"/> in
        /// <see cref="Game.Cheats"/> changes its state.
        /// </value>
        public virtual EventHandler<CheatStateChangedEventArgs> OnCheatStateChanged
        {
            get { return onCheatStateChanged; }
            set { onCheatStateChanged = value; }
        }

        /// <summary>
        /// Gets or sets the delegate to be invoked when the <see cref="Game"/>
        /// is over.
        /// </summary>
        /// <value>
        /// The delegate to be invoked when the <see cref="Game"/> is over.
        /// </value>
        /// <remarks>
        /// This delegate is invoked for losers only.
        /// </remarks>
        public virtual EventHandler<GameOverEventArgs> OnGameOver
        {
            get { return onGameOver; }
            set { onGameOver = value; }
        }

        /// <summary>
        /// Gets a globally unique identifier that identifies this <see cref="Gamer"/>.
        /// </summary>
        /// <value>
        /// A globally unique identifier that identifies this <see cref="Gamer"/>.
        /// </value>
        public Guid Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a string that uniquely identifies this <see cref="Gamer"/>.
        /// </summary>
        /// <value>
        /// A string that uniquely identifies this <see cref="Gamer"/>.
        /// </value>
        public string Gamertag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="GameSession"/> joined by this <see cref="Gamer"/>.
        /// </summary>
        /// <value>
        /// The <see cref="GameSession"/> joined by this <see cref="Gamer"/>, or
        /// <see langword="null"/> if no <see cref="GameSession"/> was joined.
        /// </value>
        public GameSession GameSession
        {
            get;
            internal set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the start <see cref="Position"/> of the next move issued by this
        /// <see cref="Gamer"/>.
        /// </summary>
        /// <value>
        /// The start <see cref="Position"/> of the next move, or <see cref="Position.Undefined"/>
        /// if no moves are available.
        /// </value>
        public abstract Position GetNextMove();

        /// <summary>
        /// Handles the <see cref="Matrix.Scrambling"/> event.
        /// </summary>
        /// <param name="sender">The puzzle <see cref="Matrix"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        internal void _OnPuzzleScrambling(object sender, EventArgs args)
        {
            OnPuzzleScrambling(sender, args);
        }

        /// <summary>
        /// Handles the <see cref="Matrix.Scrambled"/> event.
        /// </summary>
        /// <param name="sender">The puzzle <see cref="Matrix"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        internal void _OnPuzzleScrambled(object sender, EventArgs args)
        {
            OnPuzzleScrambled(sender, args);
        }

        /// <summary>
        /// Handles the <see cref="Matrix.ElementMoved"/> event.
        /// </summary>
        /// <param name="sender">The puzzle <see cref="Matrix"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        internal void _OnPuzzleElementMoved(object sender, ElementMovedEventArgs args)
        {
            OnPuzzleElementMoved(sender, args);
        }

        /// <summary>
        /// Handles the <see cref="ObservableDictionary&lt;TKey, TValue&gt;.DictionaryChanged"/>
        /// event.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> that
        /// generated the event.
        /// </param>
        /// <param name="args">The event data.</param>
        internal void _OnCheatsChanged(object sender, DictionaryChangedEventArgs<int, Cheat> args)
        {
            switch (args.ListChangedType) {
                case ListChangedType.ItemAdded:
                    args.NewValue.StateChanged += OnCheatStateChanged;
                    break;
                case ListChangedType.ItemRemoved:
                    args.OldValue.StateChanged -= OnCheatStateChanged;
                    break;
                default:
                    break;
            }

            OnCheatsChanged(sender, args);
        }

        /// <summary>
        /// Handles the <see cref="Cheat.StateChanged"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="Cheat"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        internal void _OnCheatStateChanged(object sender, CheatStateChangedEventArgs args)
        {
            OnCheatStateChanged(sender, args);
        }

        /// <summary>
        /// Handles the <c>GameOver</c> event.
        /// </summary>
        /// <param name="sender">The <see cref="GameSession"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        internal void _OnGameOver(object sender, GameOverEventArgs args)
        {
            OnGameOver(sender, args);
        }
        #endregion
    }
}
