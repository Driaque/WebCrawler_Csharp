using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace webCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            StartCrawler("https://sedna.com"); //Initiate Crawler for Sedna.com
            Console.ReadLine();
        }

        private static async Task StartCrawler(string domainUrl)
        {
            var url = domainUrl;
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var siteMap = new List<TopLink>();
            var topLevelLinks = htmlDocument.DocumentNode
                 .Descendants("a").Where(node => node.GetAttributeValue("class", "")
                 .Equals("top-level-link")).ToList();
            foreach (var link in topLevelLinks)
            {
                var childLevelLinks = link.NextSibling.Descendants("a").ToList();
                var childlinks = new List<ChildLink>();

                foreach (var childLink in childLevelLinks)
                {
                    var href = childLink.GetAttributes("href", "").FirstOrDefault().Value.Contains("https://sedna.com/") ? childLink.GetAttributes("href", "").FirstOrDefault().Value : "";
                    var ChildLink = new ChildLink
                    {
                        Link = href,
                        Name = childLink.InnerText,
                    };                   
                    ImgCrawler(ref ChildLink);
                    childlinks.Add(ChildLink);
                }
                var toplink = new TopLink
                {
                    Link = link.GetAttributes("href", "").FirstOrDefault().Value,
                    Name = link.InnerText,
                    ChildLinks = childlinks
                };
                siteMap.Add(toplink);
            }
            Console.WriteLine($"{JsonConvert.SerializeObject(siteMap)}"); // Print Serialized Object of Sitemap

            string siteMapJson = JsonConvert.SerializeObject(siteMap);
            string siteMapFormatted = JValue.Parse(siteMapJson).ToString(Formatting.Indented);
            //write string to file
            File.WriteAllText(@"C:\Users\Daniel.Onyeani-Nwosu\Desktop\Sand\sandbox\webCrawlerC#\webCrawler\siteMap.json", siteMapFormatted);
        }

        private static void ImgCrawler(ref ChildLink childLink)
        {
            var link = childLink.Link;
            var url = link;
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var staticAssetsList = new List<string>();
            var staticAssets = htmlDocument.DocumentNode.Descendants("img").ToList();
            foreach (var asset in staticAssets)
            {
                var src = asset.GetAttributes("src", "").FirstOrDefault().Value;
                staticAssetsList.Add(src);
            }
            childLink.StaticAssests = staticAssetsList;
        }
    }
}
