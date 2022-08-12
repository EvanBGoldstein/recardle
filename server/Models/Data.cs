using Newtonsoft.Json;
using System.Collections.Generic;

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

        [JsonProperty("answerImage")]
        public string AnswerImageUrl { get; set; }

        public Image AnswerImage => new() { Url = AnswerImageUrl };

        private Image[] images;
        public Image[] Images => images ??= new Image[5] { Image1, Image2, Image3, Image4, Image5 };
    }


}
