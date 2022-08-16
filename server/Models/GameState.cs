using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static ReCardle.Models.GameStatus;
using static ReCardle.Models.GuessResult;
namespace ReCardle.Models
{

    public class GameState
    {
        public DateTime Date { get; set; }

        [JsonIgnore]
        public int ShowImage { get; set; }

        public void ShowDefaultImage() => ShowImage = Result is INCOMPLETE or MAKE ? Guesses.AttemptedCount : 5;

        public GuessCollection Guesses { get; set; } = new GuessCollection();

        public GameStatus? Result => Guesses.BestGuess switch
        {
            UNATTEMPTED => INCOMPLETE,
            INCORRECT => Guesses.AnyRemaining ? INCOMPLETE : LOST,
            PARTIAL => MAKE,
            CORRECT => WON,
            _ => throw new NotImplementedException(),
        };

        public bool GameEnded => Result is WON or LOST;

        public string ScoreShare => Guesses.ScoreShare;

        public GameState() => ShowDefaultImage();

        internal void Reset()
        {
            Date = DateTime.Today;
            Guesses = new();
        }

        internal IEnumerable<Guess> GuessList() => Guesses.Guesses;

        internal void SetGuessResult(GuessResult i, string guess)
        {
            Guesses.SetCurrentGuessResult(i, guess);
            ShowDefaultImage();
        }
    }
}
