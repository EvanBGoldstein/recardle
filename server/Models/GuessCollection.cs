using System;
using System.Collections.Generic;
using System.Linq;

namespace ReCardle.Models
{
    public class GuessCollection
    {
        public Guess[] Collection { get; set; }
            = new Guess[6] { new(), new(), new(), new(), new(), new() };

        public Guess GetGuess(int i) => Collection[i];

        public GuessResult BestGuess => Collection.Max(g => g.Result);
        public int AttemptedCount => Collection.Count(g => !g.IsUnattempted);

        public bool AnyRemaining => Collection.Any(g => g.IsUnattempted);

        public IEnumerable<Guess> Guesses => Collection.Where(g => !string.IsNullOrEmpty(g.Answer));

        public string ScoreShare => Collection.Select(g => g.Emoji).Aggregate((i, j) =>$"{i} {j}");

        public static implicit operator Guess[](GuessCollection gC) => gC.Collection;

        internal void SetCurrentGuessResult(GuessResult res, string guess) => Collection[AttemptedCount] = new() { Result = res, Answer = guess };
    }
}
