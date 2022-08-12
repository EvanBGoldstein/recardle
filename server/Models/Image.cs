using Newtonsoft.Json;

namespace ReCardle.Models
{
    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("credit")]
        public string Credit { get; set; }
    }


}
