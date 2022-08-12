using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace ReCardle.Models
{
    public class GuessState
    {
        public int? Result { get; set; }
        public string Guess { get; set; }

    }

    public class State
    {
        public DateTime Date { get; set; }

        [JsonIgnore]
        public int ShowImage { get; set; }


        public void ShowDefaultImage() => ShowImage = (Result ?? 1) == 1 ? Guess : 5;
        public int Guess => Guesses.Count(g => g != null);

#nullable enable
        public GuessState?[] Guesses { get; set; } = new GuessState?[6] { null, null, null, null, null, null };
        public int? Result => Guesses.Any(g => g?.Result == 2) ? 2 : Guesses.Any(g => g?.Result == 1) ? 1 : Guesses.Contains(null) ? null : 0;
        //Guesses.All(a=>a is not null or 2) ? 0 //{ get; set; } = null;

        public State() => ShowDefaultImage();

        internal void Reset()
        {
            Date = DateTime.Today;
            Guesses = new GuessState?[6] { null, null, null, null, null, null };
            //Result = null;
        }
#nullable disable
        internal void SetGuessResult(int i, string guess)
        {
            Guesses[Guess] = new() { Result = i, Guess = guess };
            ShowDefaultImage();
        }
    }
}
