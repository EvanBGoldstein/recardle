using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static System.StringComparison;

namespace ReCardle.Models
{
    /// <summary>
    /// Stores the data for a single game. Use when deserializing data.json
    /// </summary>
    public class CardleGame
    {
        [JsonProperty("image1")]
        public CardleImageInfo Image1 { get; set; }

        [JsonProperty("image2")]
        public CardleImageInfo Image2 { get; set; }

        [JsonProperty("image3")]
        public CardleImageInfo Image3 { get; set; }

        [JsonProperty("image4")]
        public CardleImageInfo Image4 { get; set; }

        [JsonProperty("image5")]
        public CardleImageInfo Image5 { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("acceptedAnswers")]
        public List<string> AcceptedAnswers { get; set; }

        public List<string> AcceptedMakes =>
            AcceptedAnswers.Select(s => s.Replace("-", "").Split(" ").First()).ToList();

        public string AcceptedMake => AcceptedMakes[0];

        [JsonProperty("answerImage")]
        public string AnswerImagePath { get; set; }


        public CardleImageInfo AnswerImage => new() { Path = AnswerImagePath };

        private CardleImageInfo[] images;
        public CardleImageInfo[] Images => images ??= new CardleImageInfo[6] { Image1, Image2, Image3, Image4, Image5, AnswerImage };

        internal bool ValidateGuess(string guess) 
            => AcceptedAnswers.Any(a => guess.Contains(a.Replace("-", ""), CurrentCultureIgnoreCase));

        internal bool ValidateMake(string guess)
            => AcceptedMakes.Any(a => guess.Contains(a.Replace("-", ""), CurrentCultureIgnoreCase));
    }


}
