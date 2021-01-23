#region Header
//+ <source name="Game.cs" language="C#" begin="21-Nov-2011">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2011">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
using PlaXore;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Implements <c>Game</c> core logic and provides functionality for
    /// interacting with it.
    /// </summary>
    /// <example>
    /// The following example shows how to instantiate a <c>Game</c> and
    /// play with it.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using M64.Engine;
    /// 
    /// public class MyGame
    /// {
    ///     private UserInterface userInterface;
    ///     private Game game;
    ///     
    ///     public static void Main(string[] args)
    ///     {
    ///         // Get the puzzle dimension from the command-line arguments.
    ///         int width = Int32.Parse(args[0]);
    ///         int height = Int32.Parse(args[1]);
    ///         
    ///         // Initialize and run the game.
    ///         MyGame myGame = new MyGame(Width, Height);
    ///         myGame.Run();
    ///     }
    ///     
    ///     MyGame(int width, int height)
    ///     {
    ///         Matrix puzzle = new Matrix(Width, Height);
    ///         userInterface = new UserInterface();
    ///         
    ///         // Hook the graphical representation of the puzzle
    ///         // to the game engine.
    ///         PuzzleTile[] puzzleTiles = userInterface.GetPuzzleTiles();
    ///         for (int i = 0; i < puzzleTiles.Length ; i++) {
    ///             puzzle[i] = puzzleTiles[i];
    ///         }
    ///         
    ///         game = new Game(puzzle);
    ///         
    ///         // Load available cheats.
    ///         foreach (Cheat cheat in userInterface.GetCheats()) {
    ///             game.Cheats.Add(cheat.Id, cheat);
    ///         }
    ///     }
    ///     
    ///     private void Run()
    ///     {
    ///         game.Puzzle.Scramble();
    ///         
    ///         // Game loop.
    ///         while (!game.Puzzle.IsSolved) {
    ///             switch (userInterface.GetCommand()) {
    ///                 case Command.Quit:
    ///                     return;
    ///                 case Command.Pause:
    ///                     game.Stopwatch.Stop();
    ///                     break;
    ///                 case Command.Resume:
    ///                     // No action required. The stopwatch is automatically
    ///                     // restarted on the next moving event.
    ///                     break;
    ///                 case Command.MoveFrom:
    ///                     game.Puzzle.MoveFrom(userInterface.GetNextMove());
    ///                     break;
    ///                 case Command.ActivateCheat:
    ///                     game.Cheats[userInterface.GetCheatId()].IsActive = true;
    ///                     break;
    ///                 case Command.DeactivateCheat:
    ///                     game.Cheats[userInterface.GetCheatId()].IsActive = false;
    ///                     break;
    ///                 default:
    ///                     break;
    ///             }
    ///             
    ///             userInterface.UpdatePuzzle(game.Puzzle);
    ///             userInterface.UpdateTime(game.Stopwatch.Elapsed);
    ///             userInterface.UpdateCounter(game.Counter.Current);
    ///             userInterface.UpdateScore(game.Score);
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class Game
    {
        #region Fields
        private Matrix puzzle;
        private double complexity;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the <see cref="Game"/> has been reset.
        /// </summary>
        public event EventHandler InstanceReset;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class with the
        /// specified puzzle.
        /// </summary>
        /// <param name="puzzle">The <see cref="Game"/> puzzle.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="puzzle"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>Game logging is not activated by default.</remarks>
        public Game(Matrix puzzle) : this(puzzle, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class with the
        /// specified puzzle and boolean value indicating whether or not to
        /// activate game logging.
        /// </summary>
        /// <param name="puzzle">The <see cref="Game"/> puzzle.</param>
        /// <param name="activateLogging">
        /// A boolean value indicating whether or not to activate game logging.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="puzzle"/> is <see langword="null"/>.
        /// </exception>
        public Game(Matrix puzzle, bool activateLogging)
        {
            if (puzzle == null) { throw new ArgumentNullException("puzzle"); }

            Puzzle = puzzle;
            Stopwatch = new Stopwatch();
            Counter = new Counter();
            Cheats = new ObservableDictionary<int, Cheat>();
            IsLoggingActive = activateLogging;
            GameLog = activateLogging ? new GameLog() : null;

            // Register to puzzle events.
            Puzzle.Scrambled += OnPuzzleScrambled;
            Puzzle.ElementMoved += OnPuzzleElementMoved;

            // Register to cheat dictionary change events.
            (Cheats as ObservableDictionary<int, Cheat>).DictionaryChanged += OnCheatsChanged;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets the dictionary that holds the <see cref="Game"/> cheats.
        /// </summary>
        /// <value>The dictionary that holds the <see cref="Game"/> cheats.</value>
        /// <remarks>
        /// Use this property to add, remove, or modify <see cref="Game"/> cheats.
        /// </remarks>
        public IDictionary<int, Cheat> Cheats
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Counter"/> that counts the <see cref="Game"/> moves.
        /// </summary>
        /// <value> 
        /// The <see cref="Counter"/> that counts the <see cref="Game"/> moves.
        /// </value>
        public Counter Counter
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not game logging is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if game logging is active; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsLoggingActive
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Matrix"/> that controls the <see cref="Game"/> puzzle.
        /// </summary>
        /// <value>
        /// The <see cref="Matrix"/> that controls the <see cref="Game"/> puzzle.
        /// </value>
        public Matrix Puzzle
        {
            get { return puzzle; }
            private set {
                puzzle = value;
                complexity = puzzle.Width * puzzle.Height;
            }
        }

        /// <summary>
        /// Gets the current game score.
        /// </summary>
        /// <value>The current game score.</value>
        /// <remarks>
        /// The game <see cref="Game.Score"/> is calculated by considering the
        /// <see cref="Game.Puzzle"/> complexity and by correlating elapsed time
        /// with move count.
        /// </remarks>
        public double Score
        {
            get {
                if (Stopwatch.ElapsedMilliseconds == 0) {
                    return 0.0;
                }

                double timeMoves = Math.Max(Stopwatch.ElapsedMilliseconds, 1) * Math.Max(Counter.Current, 1);
                double score = (complexity / timeMoves) * 10e9;
                return score < Double.Epsilon ? 0.0 : score;
            }
        }

        /// <summary>
        /// Gets the <see cref="GameLog"/> of this <see cref="Game"/>.
        /// </summary>
        /// <value>
        /// The current <see cref="GameLog"/> if logging is active; otherwise, <see lanword="null"/>.
        /// </value>
        public GameLog GameLog
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Stopwatch"/> that measures the elapsed <see cref="Game"/> time.
        /// </summary>
        /// <value>
        /// The <see cref="Stopwatch"/> that measures the elapsed <see cref="Game"/> time.
        /// </value>
        public Stopwatch Stopwatch
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Re-initializes this <see cref="Game"/> with the specified puzzle.
        /// </summary>
        /// <param name="puzzle">The new <see cref="Game"/> puzzle.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="puzzle"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// <see cref="Game.Reset(Matrix)"/> also re-initializes the <see cref="Game.Stopwatch"/> and the
        /// <see cref="Game.Counter"/>, and deactivates any active <see cref="Cheat"/>. Game logging is
        /// not activated by default.
        /// </remarks>
        public void Reset(Matrix puzzle)
        {
            Reset(puzzle, false);
        }

        /// <summary>
        /// Re-initializes this <see cref="Game"/> with the specified puzzle and
        /// a boolean value indicating whether or not to activate game logging.
        /// </summary>
        /// <param name="puzzle">The new <see cref="Game"/> puzzle.</param>
        /// <param name="activateLogging">
        /// A boolean value indicating whether or not to activate game logging.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="puzzle"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// <see cref="Game.Reset(Matrix, bool)"/> also re-initializes the <see cref="Game.Stopwatch"/>
        /// and the <see cref="Game.Counter"/>, and deactivates any active <see cref="Game.Cheats"/>.
        /// </remarks>
        public void Reset(Matrix puzzle, bool activateLogging)
        {
            if (puzzle == null) { throw new ArgumentNullException("puzzle"); }

            SoftReset(puzzle);
            Stopwatch.Reset();
            Counter.Reset();
            IsLoggingActive = activateLogging;

            if (activateLogging) {
                if (GameLog != null) { GameLog.Reset(); }
                else { GameLog = new GameLog(); }
            } else {
                GameLog = null;
            }

            foreach (KeyValuePair<int, Cheat> cheat in Cheats) { cheat.Value.IsActive = false; }
            OnInstanceReset(new EventArgs());
        }

        /// <summary>
        /// Re-initializes this <see cref="Game"/> with the specified puzzle.
        /// </summary>
        /// <param name="puzzle">The new <see cref="Game"/> puzzle.</param>
        internal void SoftReset(Matrix puzzle)
        {
            if ((puzzle as object) != (Puzzle as object)) {
                Puzzle = puzzle;
                Puzzle.Scrambled += OnPuzzleScrambled;
                Puzzle.ElementMoved += OnPuzzleElementMoved;
            }
        }

        /// <summary>
        /// Raises the <see cref="Game.InstanceReset"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="Game.InstanceReset"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnInstanceReset(EventArgs args)
        {
            if (InstanceReset != null) {
                InstanceReset(this, args);
            }
        }

        /// <summary>
        /// Handles the <see cref="Matrix.Scrambled"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="Matrix"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        private void OnPuzzleScrambled(object sender, EventArgs args)
        {
            if (IsLoggingActive) {
                GameLog.Reset(puzzle);
            }
        }

        /// <summary>
        /// Handels the <see cref="Matrix.ElementMoved"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="Matrix"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        private void OnPuzzleElementMoved(object sender, ElementMovedEventArgs args)
        {
            if (args.IsScrambling) { return; }

            if (puzzle.IsSolved) { Stopwatch.Stop(); }
            else if (!Stopwatch.IsRunning) { Stopwatch.Start(); }

            Counter++;

            if (IsLoggingActive) {
                GameLogEntry logEntry = new GameLogEntry();
                logEntry.ElapsedTime = Stopwatch.ElapsedMilliseconds;
                logEntry.MoveCount = Counter.Current;
                logEntry.StartPosition = args.OldPosition;

                foreach (KeyValuePair<int, Cheat> cheat in Cheats) {
                    if (cheat.Value.IsActive) { logEntry.ActiveCheats.Add(cheat.Value); }
                }

                GameLog.Entries.Add(logEntry);
            }
        }

        /// <summary>
        /// Handles the <see cref="ObservableDictionary&lt;TKey, TValue&gt;.DictionaryChanged"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        private void OnCheatsChanged(object sender, DictionaryChangedEventArgs<int, Cheat> args)
        {
            switch (args.ListChangedType) {
                case ListChangedType.Cleared:
                    Stopwatch.ElapseFactor = Stopwatch.DefaultElapseFactor;
                    Counter.IncrementFactor = Counter.DefaultIncrementFactor;
                    Counter.DecrementFactor = Counter.DefaultDecrementFactor;
                    break;
                case ListChangedType.ItemAdded:
                    EnableCheat(args.Value);
                    break;
                case ListChangedType.ItemChanged:
                    DisableCheat(args.OldValue);
                    EnableCheat(args.NewValue);
                    break;
                case ListChangedType.ItemRemoved:
                    DisableCheat(args.Value);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles the <see cref="Cheat.StateChanged"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="Cheat"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        private void OnCheatStateChanged(object sender, CheatStateChangedEventArgs args)
        {
            switch (args.CheatStateChangedType) {
                case CheatStateChangedType.Activated:
                    ApplyCheat(sender as Cheat);
                    break;
                case CheatStateChangedType.Deactivated:
                    UnapplyCheat(sender as Cheat);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Enables the specified <see cref="Cheat"/>.
        /// </summary>
        /// <param name="cheat">The <see cref="Cheat"/> to enable.</param>
        private void EnableCheat(Cheat cheat)
        {
            cheat.Game = this;
            cheat.StateChanged += OnCheatStateChanged;
            if (cheat.IsActive) { ApplyCheat(cheat); }
        }

        /// <summary>
        /// Disables the specified <see cref="Cheat"/>.
        /// </summary>
        /// <param name="cheat">The <see cref="Cheat"/> to disable.</param>
        private void DisableCheat(Cheat cheat)
        {
            cheat.Game = null;
            cheat.StateChanged -= OnCheatStateChanged;
            if (cheat.IsActive) { UnapplyCheat(cheat); }
        }

        /// <summary>
        /// Applies the specified <see cref="Cheat"/> so that possible
        /// penalties or advantages it defines are considered during gameplay.
        /// </summary>
        /// <param name="cheat">The <see cref="Cheat"/> to apply.</param>
        private void ApplyCheat(Cheat cheat)
        {
            Stopwatch.ElapseFactor += cheat.ElapseFactor - Stopwatch.DefaultElapseFactor;
            Counter.IncrementFactor += cheat.IncrementFactor - Counter.DefaultIncrementFactor;
            Counter.DecrementFactor += cheat.DecrementFactor - Counter.DefaultDecrementFactor;
        }

        /// <summary>
        /// Unapplies the specified <see cref="Cheat"/> so that possible
        /// penalties or advantages it defines are no longer considered during gameplay.
        /// </summary>
        /// <param name="cheat">The <see cref="Cheat"/> to unapply.</param>
        private void UnapplyCheat(Cheat cheat)
        {
            Stopwatch.ElapseFactor -= cheat.ElapseFactor - Stopwatch.DefaultElapseFactor;
            Counter.IncrementFactor -= cheat.IncrementFactor - Counter.DefaultIncrementFactor;
            Counter.DecrementFactor -= cheat.DecrementFactor - Counter.DefaultDecrementFactor;
        }
        #endregion
    }
}
