#region Header
//+ <source name="GameSession.cs" language="C#" begin="12-Feb-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
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
    /// Represents the period of time one or more gamers interface with a <see cref="Game"/>.
    /// A <see cref="GameSession"/> begins when one of the gamers issue the first move after
    /// puzzle scrambling and ends when at least one gamer has solved the puzzle.
    /// </summary>
    /// <seealso cref="Game"/>
    /// <example>
    /// The following example shows how to instantiate a <c>GameSession</c> and
    /// interact with it using a <c>Gamer</c>.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using M64.Engine;
    /// 
    /// public class MyGame
    /// {
    ///     private UserInterface userInterface;
    ///     private GameSession gameSession;
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
    ///         // Create a new game session.
    ///         gameSession = new GameSession(puzzle);
    ///         
    ///         // Create two gamers.
    ///         MyGamer gamer1 = userInterface.GetGamer("Gamer 1")
    ///         MyGamer gamer2 = userInterface.GetGamer("Gamer 2")
    ///         
    ///         // Let gamers join the game session.
    ///         gameSession.Join(gamer1);
    ///         gameSession.Join(gamer2);
    ///         
    ///         // Hook event handlers to gamer1.
    ///         gamer1.OnCheatsChanged = OnCheatsChanged1;
    ///         gamer1.OnCheatStateChanged = OnCheatStateChanged1;
    ///         gamer1.OnPuzzleElementMoved = OnPuzzleElementMoved1;
    ///         gamer1.OnGameOver = OnGameOver1;
    ///         
    ///         // Hook event handlers to gamer1.
    ///         gamer2.OnCheatsChanged = OnCheatsChanged2;
    ///         gamer2.OnCheatStateChanged = OnCheatStateChanged2;
    ///         gamer2.OnPuzzleElementMoved = OnPuzzleElementMoved2;
    ///         gamer2.OnGameOver = OnGameOver2;
    ///         
    ///         // Load available cheats.
    ///         foreach (Game gameInstance in gameSession.GameInstances) {
    ///             foreach (Cheat cheat in userInterface.GetCheats()) {
    ///                 gameInstance.Cheats.Add(cheat.Id, cheat);
    ///             }
    ///         }
    ///     }
    ///     
    ///     private void Run()
    ///     {
    ///         gameSession.Puzzle.Scramble();
    ///         
    ///         // Game loop.
    ///         while (gameSession.IsActive) {
    ///             gameSession.Update()
    ///         }
    ///     }
    ///     
    ///     private void OnCheatsChanged1(object sender, DictionaryChangedEventArgs<int, Cheat> args)
    ///     {
    ///         // Handle cheats for gamer1 here.
    ///     }
    ///  
    ///     private void OnCheatStateChanged1(object sender, CheatStateChangedEventArgs args)
    ///     {
    ///         // Handle cheat activation/deactivation for gamer1 here.
    ///     }
    /// 
    ///     private void OnPuzzleElementMoved1(object sender, ElementMovedEventArgs args)
    ///     {
    ///         // Handle puzzle moves for gamer1 here.
    ///     }
    /// 
    ///     private void OnGameOver1(object sender, GameOverEventArgs args)
    ///     {
    ///         // Handle game over condition for gamer1 here.
    ///     }
    ///     
    ///     private void OnCheatsChanged2(object sender, DictionaryChangedEventArgs<int, Cheat> args)
    ///     {
    ///         // Handle cheats for gamer2 here.
    ///     }
    ///  
    ///     private void OnCheatStateChanged2(object sender, CheatStateChangedEventArgs args)
    ///     {
    ///         // Handle cheat activation/deactivation for gamer1 here.
    ///     }
    /// 
    ///     private void OnPuzzleElementMoved2(object sender, ElementMovedEventArgs args)
    ///     {
    ///         // Handle puzzle moves for gamer2 here.
    ///     }
    /// 
    ///     private void OnGameOver2(object sender, GameOverEventArgs args)
    ///     {
    ///         // Handle game over condition for gamer2 here.
    ///     }
    /// }
    /// 
    /// public class MyGamer : Gamer
    /// {
    ///     private userInterface;
    ///     
    ///     public MyGamer(string gamertag, UserInterface userInterface) : base(gamertag)
    ///     {
    ///         userInterface = userInterface;
    ///     }
    ///     
    ///     public Position NextMove
    ///     {
    ///         get { return userInterface.GetNextMove(); }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class GameSession
    {
        #region Types
        private struct GameInstance
        {
            public Gamer Gamer;
            public Game Game;
        }
        #endregion

        #region Fields
        private long startTime;
        private long endTime;
        private IDictionary<Guid, GameInstance> gameInstances;
        private int solvedCount;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a <see cref="Gamer"/> has joined this <see cref="GameSession"/>.
        /// </summary>
        public event EventHandler<GamerJoinedEventArgs> GamerJoined;

        /// <summary>
        /// Occurs when a <see cref="Gamer"/> has left this <see cref="GameSession"/>.
        /// </summary>
        public event EventHandler<GamerLeftEventArgs> GamerLeft;

        /// <summary>
        /// Occurs when the <see cref="GameSession"/> has been reset.
        /// </summary>
        public event EventHandler InstanceReset;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameSession"/> class
        /// with the specified puzzle.
        /// </summary>
        /// <param name="puzzle">
        /// The puzzle to be solved in this <see cref="GameSession"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="puzzle"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>Event <c>GameOver</c> is not raised by default.</remarks>
        public GameSession(Matrix puzzle) : this(puzzle, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSession"/> class with
        /// the specified puzzle and boolean value indicating whether or not to
        /// raise a <c>GameOver</c> event when an <see cref="Gamer"/> first
        /// solves the puzzle.
        /// </summary>
        /// <param name="puzzle">
        /// The puzzle to be solved in this <see cref="GameSession"/>.
        /// </param>
        /// <param name="raiseGameOverEvent">
        /// A boolean value indicating whether or not to raise a <c>GameOver</c>
        /// event when an <see cref="Gamer"/> first solves the puzzle.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="puzzle"/> is <see langword="null"/>.
        /// </exception>
        public GameSession(Matrix puzzle, bool raiseGameOverEvent)
        {
            if (puzzle == null) { throw new ArgumentNullException("puzzle"); }

            Reset(puzzle, raiseGameOverEvent);
            Id = Guid.NewGuid();
            gameInstances = new Dictionary<Guid, GameInstance>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Game"/> owned by the specified <see cref="Gamer"/>.
        /// </summary>
        /// <param name="gamer">The owner of the <see cref="Game"/> to get.</param>
        /// <value>The <see cref="Game"/> owned by <paramref name="gamer"/>.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamer"/> is <see langword="null"/>.
        /// </exception>
        public Game this[Gamer gamer]
        {
            get {
                if (gamer == null) { throw new ArgumentNullException(); }
                return gameInstances[gamer.Id].Game;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="GameSession"/>
        /// is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="GameSession"/> is active;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsActive
        {
            get { return startTime != 0L && endTime == 0L; }
        }

        /// <summary>
        /// Gets a globally unique identifier that identifies this <see cref="GameSession"/>.
        /// </summary>
        /// <value>
        /// A globally unique identifier that identifies this <see cref="GameSession"/>.
        /// </value>
        public Guid Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the puzzle to be solved in this <see cref="GameSession"/>.
        /// </summary>
        /// <value>
        /// The puzzle to be solved in this <see cref="GameSession"/>.
        /// </value>
        public Matrix Puzzle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Game"/> instances in this <see cref="GameSession"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Game"/> instances in this <see cref="GameSession"/>.
        /// </value>
        public Game[] GameInstances
        {
            get {
                int i = 0;
                Game[] gameInstances = new Game[this.gameInstances.Count];

                foreach (KeyValuePair<Guid, GameInstance> gameInstance in this.gameInstances) {
                    gameInstances[i++] = gameInstance.Value.Game;
                }

                return gameInstances;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether or not to raise a
        /// <c>GameOver</c> event when an <see cref="Gamer"/> first solves the
        /// puzzle.
        /// </summary>
        public bool RaiseGameOverEvent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the start time of this <see cref="GameSession"/>.
        /// </summary>
        /// <value>
        /// The start time of this <see cref="GameSession"/>, or <code>DateTime.MinValue</code>
        /// if not started yet.
        /// </value>
        public DateTime StartTime
        {
            get { return new DateTime(startTime); }
        }

        /// <summary>
        /// Gets the end time of this <see cref="GameSession"/>.
        /// </summary>
        /// <value>
        /// The end time of this <see cref="GameSession"/>, or <code>DateTime.MinValue</code>
        /// if not ended yet.
        /// </value>
        public DateTime EndTime
        {
            get { return new DateTime(endTime); }
        }

        /// <summary>
        /// Gets the duration of this <see cref="GameSession"/>.
        /// </summary>
        /// <value>
        /// The duration of this <see cref="GameSession"/>.
        /// </value>
        public TimeSpan Duration
        {
            get {
                if (startTime == 0L) { return TimeSpan.Zero; }

                long time = endTime != 0L
                    ? endTime
                    : DateTime.UtcNow.Ticks;

                return new TimeSpan(time - startTime);
            }
        }

        /// <summary>
        /// Gets the winner of this <see cref="GameSession"/>.
        /// </summary>
        /// <value>The <see cref="Gamer"/> that won this <see cref="GameSession"/>,
        /// or <see langword="null"/> if there is no winner yet.
        /// </value>
        public Gamer Winner
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new instance of the <see cref="Game"/> class and assigns
        /// it to the specified <see cref="Gamer"/>.
        /// </summary>
        /// <param name="gamer">
        /// The <see cref="Gamer"/> that joins this <see cref="GameSession"/>.
        /// </param>
        /// <returns>A new instance of the <see cref="Game"/> class.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamer"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>Game logging is not activated by default.</remarks>
        public Game Join(Gamer gamer)
        {
            return Join(gamer, false);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Game"/> class and assigns
        /// it to the specified <see cref="Gamer"/>.
        /// </summary>
        /// <param name="gamer">
        /// The <see cref="Gamer"/> that joins this <see cref="GameSession"/>.
        /// </param>
        /// <param name="activateLogging">
        /// A boolean value indicating whether or not to activate game logging.
        /// </param>
        /// <returns>A new instance of the <see cref="Game"/> class.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="GameSession"/> has already started.
        /// - or -
        /// <paramref name="gamer"/> already joined another session.
        /// </exception>
        public Game Join(Gamer gamer, bool activateLogging)
        {
            if (gamer == null) { throw new ArgumentNullException("gamer"); }

            if (startTime != 0L) {
                string message = String.Format(
                    "Gamer {0} tried to join session {1}, which has already started.",
                    gamer.Gamertag, Id
                );
                throw new InvalidOperationException(message);
            }

            if (gamer.GameSession != null) {
                string message = String.Format(
                    "Gamer {0} already joined session {1}.",
                    gamer.Gamertag, gamer.GameSession.Id
                );
                throw new InvalidOperationException(message);
            }

            GameInstance gameInstance = new GameInstance();
            gameInstance.Gamer = gamer;
            gameInstance.Game = new Game(Puzzle.Clone(), activateLogging);
            gameInstance.Game.Puzzle.ElementMoved += gamer._OnPuzzleElementMoved;
            gameInstance.Game.Puzzle.Scrambling += gamer._OnPuzzleScrambling;
            gameInstance.Game.Puzzle.Scrambled += gamer._OnPuzzleScrambled;
            (gameInstance.Game.Cheats as ObservableDictionary<int, Cheat>).DictionaryChanged += gamer._OnCheatsChanged;
            gameInstances.Add(gamer.Id, gameInstance);

            gamer.GameSession = this;
            OnGamerJoined(new GamerJoinedEventArgs(gamer));

            return gameInstance.Game;
        }

        /// <summary>
        /// Removes the <see cref="Game"/> instance owned by the specified
        /// <see cref="Gamer"/> from this <see cref="GameSession"/>.
        /// </summary>
        /// <param name="gamer">
        /// The <see cref="Gamer"/> that leaves this <see cref="GameSession"/>.
        /// </param>
        /// <returns>The <see cref="Game"/> instance owned by <paramref name="gamer"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="gamer"/> is <see langword="null"/>.
        /// </exception>
        public Game Leave(Gamer gamer)
        {
            if (gamer == null) { throw new ArgumentNullException("gamer"); }

            GameInstance gameInstance = gameInstances[gamer.Id];
            gameInstances.Remove(gamer.Id);

            gameInstance.Game.Puzzle.ElementMoved -= gamer._OnPuzzleElementMoved;
            gameInstance.Game.Puzzle.Scrambling -= gamer._OnPuzzleScrambling;
            gameInstance.Game.Puzzle.Scrambled -= gamer._OnPuzzleScrambled;
            (gameInstance.Game.Cheats as ObservableDictionary<int, Cheat>).DictionaryChanged -= gamer._OnCheatsChanged;
            foreach (KeyValuePair<int, Cheat> cheat in gameInstance.Game.Cheats) {
                cheat.Value.StateChanged -= gamer._OnCheatStateChanged;
            }

            OnGamerLeft(new GamerLeftEventArgs(gamer));
            gamer.GameSession = null;

            return gameInstance.Game;
        }

        /// <summary>
        /// Re-initializes this <see cref="GameSession"/> with the specified puzzle.
        /// </summary>
        /// <param name="puzzle">
        /// The puzzle to be solved in this <see cref="GameSession"/>, or
        /// <see langword="null"/> to reset the current <see cref="GameSession.Puzzle"/>.
        /// </param>
        /// <remarks>Event <c>GameOver</c> is not raised by default.</remarks>
        public void Reset(Matrix puzzle)
        {
            Reset(puzzle, false);
        }

        /// <summary>
        /// Re-initializes this <see cref="GameSession"/> with the specified puzzle and
        /// boolean value indicating whether or not to raise a <c>GameOver</c> event
        /// when an <see cref="Gamer"/> first solves the puzzle.
        /// </summary>
        /// <param name="puzzle">
        /// The puzzle to be solved in this <see cref="GameSession"/>, or
        /// <see langword="null"/> to reset the current <see cref="GameSession.Puzzle"/>.
        /// </param>
        /// <param name="raiseGameOverEvent">
        /// A boolean value indicating whether or not to raise a <c>GameOver</c>
        /// event when an <see cref="Gamer"/> first solves the puzzle.
        /// </param>
        public void Reset(Matrix puzzle, bool raiseGameOverEvent)
        {
            startTime = 0L;
            endTime = 0L;
            RaiseGameOverEvent = raiseGameOverEvent;
            Winner = null;
            solvedCount = 0;

            if (puzzle == null) {
                Puzzle.Reset();
            } else if ((puzzle as object) != (Puzzle as object)) {
                if (Puzzle != null) { Puzzle.Scrambled -= OnScrambled; }
                Puzzle = puzzle;
                Puzzle.Scrambled += OnScrambled;
            }

            if (gameInstances != null) {
                Gamer gamer = null;
                Game game = null;

                foreach (KeyValuePair<Guid, GameInstance> gameInstance in gameInstances) {
                    gamer = gameInstance.Value.Gamer;
                    game = gameInstance.Value.Game;

                    game.Puzzle.ElementMoved -= gamer._OnPuzzleElementMoved;
                    game.Puzzle.Scrambling -= gamer._OnPuzzleScrambling;
                    game.Puzzle.Scrambled -= gamer._OnPuzzleScrambled;

                    game.Reset(Puzzle.Clone(), game.IsLoggingActive);

                    game.Puzzle.ElementMoved += gamer._OnPuzzleElementMoved;
                    game.Puzzle.Scrambling += gamer._OnPuzzleScrambling;
                    game.Puzzle.Scrambled += gamer._OnPuzzleScrambled;
                }
            }

            OnInstanceReset(new EventArgs());
        }

        /// <summary>
        /// Forwards gamers input to <see cref="Game"/> instances and then notifies
        /// back the updated <see cref="Game"/> state to each <see cref="Gamer"/>.
        /// </summary>
        public void Update()
        {
            if (endTime > 0L || Puzzle.IsSolved) { return; }

            Gamer gamer = null;
            Game game = null;
            Position position;
            double bestScore = 0.0;

            foreach (KeyValuePair<Guid, GameInstance> gameInstance in gameInstances) {
                game = gameInstance.Value.Game;
                if (game.Puzzle.IsSolved) { continue; }

                gamer = gameInstance.Value.Gamer;
                position = gamer.GetNextMove();

                if (position != Position.Undefined) { 
                    if (startTime == 0L) { startTime = DateTime.UtcNow.Ticks; }

                    game.Puzzle.MoveFrom(position);
                    if (!game.Puzzle.IsSolved) { continue; }

                    solvedCount++;
                    if (game.Score > bestScore) {
                        bestScore = game.Score;
                        Winner = gamer;
                    }
                }
            }

            if (Winner != null && RaiseGameOverEvent || solvedCount == gameInstances.Count) {
                endTime = DateTime.UtcNow.Ticks;

                if (RaiseGameOverEvent) {
                    foreach (KeyValuePair<Guid, GameInstance> gameInstance in gameInstances) {
                        gamer = gameInstance.Value.Gamer;
                        if (gamer != Winner) {
                            gamer._OnGameOver(this, (new GameOverEventArgs(Winner)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="Matrix.Scrambled"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="Matrix"/> that generated the event.</param>
        /// <param name="args">The event data.</param>
        private void OnScrambled(object sender, EventArgs args)
        {
            if (gameInstances != null) {
                Gamer gamer = null;
                Game game = null;

                foreach (KeyValuePair<Guid, GameInstance> gameInstance in gameInstances) {
                    gamer = gameInstance.Value.Gamer;
                    game = gameInstance.Value.Game;

                    Matrix puzzle = game.Puzzle;
                    game.SoftReset((sender as Matrix).Clone());
                    game.Puzzle.Absorb(puzzle);
                    game.Puzzle.RaiseScrambledEvent();
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="GameSession.GamerJoined"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="GameSession.OnGamerJoined"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnGamerJoined(GamerJoinedEventArgs args)
        {
            if (GamerJoined != null) {
                GamerJoined(this, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="GameSession.GamerLeft"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="GameSession.OnGamerLeft"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnGamerLeft(GamerLeftEventArgs args)
        {
            if (GamerLeft != null) {
                GamerLeft(this, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="GameSession.InstanceReset"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="GameSession.InstanceReset"/> method also allows derived
        /// classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnInstanceReset(EventArgs args)
        {
            if (InstanceReset != null) {
                InstanceReset(this, args);
            }
        }
        #endregion
    }
}