using System;
using System.Collections.Generic;
using System.Linq;

namespace ReCardle.Models
{
    /// <summary> Represents the collection of six guesses in a single game</summary>
    public class GuessCollection
    {

        /// <summary> The collection of Guesses. 
        /// Do not attempt to use this collection directly; it is only accessible for JSON deserialization.</summary>
        public Guess[] Collection { get; set; }
            = new Guess[6] { new(), new(), new(), new(), new(), new() };

        /// <summary>Get a single guess</summary>
        /// <param name="i"> The number guess to return</param>
        /// <returns> The guess at index <paramref name="i"/></returns>
        public Guess GetGuess(int i) => Collection[i];

        /// <summary> The best guess so far </summary>
        public GuessResult BestGuess => Collection.Max(g => g.Result);

        /// <summary> The number of guesses that have been attempted </summary>
        public int AttemptedCount => Collection.Count(g => g.IsAttempted);

        /// <summary> Whether any guesses remain unattempted</summary>
        public bool AnyRemaining => Collection.Any(g => !g.IsAttempted);

        /// <summary> The car models the user has guessed so far </summary>
        public IEnumerable<Guess> Guesses => Collection.Where(g => !string.IsNullOrEmpty(g.Answer));

        /// <summary> The string representing the user's score with <see cref="Guess.Emoji"/> </summary>
        public string ScoreShare => Collection.Select(g => g.Emoji).Aggregate((i, j) =>$"{i} {j}");

        /// <summary>
        /// Set the restult of the next (unattempted) guess
        /// </summary>
        /// <param name="result">The result of the guess</param>
        /// <param name="guess">The car model the user guessed</param>
        internal void SetCurrentGuessResult(GuessResult result, string guess) => Collection[AttemptedCount] = new() { Result = result, Answer = guess };
    }
}
