using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ReCardle.Models
{
    public class CardleData
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


        public List<string> AcceptedMakes => AcceptedAnswers.Select(s => s.Replace("-", "").Split(" ").First()).ToList();

        [JsonProperty("answerImage")]
        public string AnswerImagePath { get; set; }

        public Image AnswerImage => new() { Path = AnswerImagePath };

        private Image[] images;
        public Image[] Images => images ??= new Image[6] { Image1, Image2, Image3, Image4, Image5, AnswerImage };
    }


}
