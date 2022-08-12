using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ReCardle.Models
{
    public class Image
    {
        [JsonProperty("url")]
        public string Path { get; set; }

        public string Url => Path.Replace("./", "https://cardle.uk/");

        [JsonProperty("credit")]
        public string Credit { get; set; }

        [JsonIgnore]
        private HtmlNode creditHtml => CreditHtmlNode().Descendants("a").ElementAt(0);

        public string CreditText => Credit is null ? "" : creditHtml.InnerHtml + ", ";
        public string CreditUrl => Credit is null ? "" : creditHtml.GetAttributes("href").First().Value;


        [JsonIgnore]
        private HtmlNode licenseHtml => CreditHtmlNode().Descendants("a").ElementAt(1);
        public string LicenseText => Credit is null ? "" : licenseHtml.InnerHtml + ", ";
        public string LicenseUrl => Credit is null ? "" : licenseHtml.GetAttributes("href").First().Value;

        [JsonIgnore]
        public string Source => Credit?.Split(",").Last().Trim() ?? "";

        //[JsonIgnore]
        //private HtmlNode CreditText => CreditHtmlNode().Descendants("a").ElementAt(0);

        private HtmlNode creditHtmlNode;
        private HtmlNode CreditHtmlNode()
        {
            if (Credit is null) return null;
            if (creditHtmlNode != null) return creditHtmlNode;
            var doc = new HtmlDocument();
            doc.LoadHtml(Credit); //.Split(",")[0]
            return (creditHtmlNode = doc.DocumentNode);
        }
    }


}
