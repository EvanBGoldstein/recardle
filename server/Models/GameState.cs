using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static ReCardle.Models.GameStatus;
using static ReCardle.Models.GuessResult;
namespace ReCardle.Models
{

    /// <summary>Represents the overall state of a single game </summary>
    public class GameState
    {

        /// <summary> The date of this game </summary>
        public DateTime Date { get; set; }

        /// <summary> The index of the image to show, between 0 and 5 </summary>
        [JsonIgnore]
        public int ShowImage { get; set; }

        /// <summary> Set the <see cref="ShowImage"/> to the default value: Either the first unguessed image, or the very last (answer) image if the game has ended. </summary>
        public void ShowDefaultImage() => ShowImage = Result is INCOMPLETE or MAKE ? Guesses.AttemptedCount : 5;

        /// <summary> The collection of <see cref="Guess"/>es in this game.</summary>
        public GuessCollection Guesses { get; set; } = new GuessCollection();

        /// <summary> This game's status </summary>
        public GameStatus? Result => Guesses.BestGuess switch
        {
            UNATTEMPTED => INCOMPLETE,
            INCORRECT => Guesses.AnyRemaining ? INCOMPLETE : LOST,
            PARTIAL => Guesses.AnyRemaining ? MAKE : LOST,
            CORRECT => WON,
            _ => throw new NotImplementedException(),
        };

        /// <summary> Whether the game is over </summary>
        public bool GameEnded => Result is WON or LOST;

        /// <summary> The string representing this game's score, 
        /// using <see cref="GuessCollection.ScoreShare"/> and <see cref="Guess.Emoji"/></summary>
        public string ScoreShare => Guesses.ScoreShare;

        /// <summary> Create and initialize a new game</summary>
        public GameState() => ShowDefaultImage();

        /// <summary> Reset the guesses for a new game ; 
        /// Use when the current date is different from that of the previously saved game</summary>
        internal void Reset()
        {
            Date = DateTime.Today;
            Guesses = new();
        }

        /// <summary> The list of car models the user has guessed during this game</summary>
        internal IEnumerable<Guess> GuessList() => Guesses.Guesses;

        /// <summary>
        /// Set the restult of the next (unattempted) guess; 
        /// See <see cref="GuessCollection.SetCurrentGuessResult(GuessResult, string)"/>
        /// </summary>
        /// <param name="result">The result of the guess</param>
        /// <param name="guess">The car model the user guessed</param>
        internal void SetCurrentGuessResult(GuessResult i, string guess)
        {
            Guesses.SetCurrentGuessResult(i, guess);
            ShowDefaultImage();
        }
    }
}
