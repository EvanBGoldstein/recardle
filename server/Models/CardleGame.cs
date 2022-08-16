using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.StringComparison;

namespace ReCardle.Models
{
    public class CardleGame
    {
        [JsonProperty("image1")]
        public Image Image1 { get; set; }

        [JsonProperty("image2")]
        public Image Image2 { get; set; }

        [JsonProperty("image3")]
        public Image Image3 { get; set; }

        [JsonProperty("image4")]
        public Image Image4 { get; set; }

        [JsonProperty("image5")]
        public Image Image5 { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("acceptedAnswers")]
        public List<string> AcceptedAnswers { get; set; }

        public List<string> AcceptedAnswersCleaned => AcceptedAnswers.Select(a => a.Replace("-", "")).ToList();


        public List<string> AcceptedMakes =>
            AcceptedAnswers.Select(s => s.Replace("-", "").Split(" ").First()).ToList();

        public string AcceptedMake => AcceptedMakes[0];

        [JsonProperty("answerImage")]
        public string AnswerImagePath { get; set; }

        public Image AnswerImage => new() { Path = AnswerImagePath };

        private Image[] images;
        public Image[] Images => images ??= new Image[6] { Image1, Image2, Image3, Image4, Image5, AnswerImage };

        internal bool ValidateGuess(string guess) 
            => AcceptedAnswers.Any(a => guess.Contains(a.Replace("-", ""), CurrentCultureIgnoreCase));

        internal bool ValidateMake(string guess)
            => AcceptedMakes.Any(a => guess.Contains(a.Replace("-", ""), CurrentCultureIgnoreCase));
    }


}
