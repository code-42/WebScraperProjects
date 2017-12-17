using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
//using ScrapySharp.Network;


namespace YellowPagesParser
{
    class Program
    {
        static void Main(string[] args)
        {
            // yellowPagesParser();

            yellowPagesParserPage2();

            //StartCrawlerAsync();

            Console.ReadLine();
        }

        private static async Task yellowPagesParser()
        {
            // Example from .http://html-agility-pack.net/from-web

            var url = @"https://www.yellowpages.com/search?search_terms=programmer&geo_location_terms=Warwick%2C+RI";

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.yellowpages.com/search?search_terms=programmer&geo_location_terms=Warwick%2C+RI");

            var headerNames = doc.DocumentNode.SelectNodes("//a[@class='business-name']").ToList();

            foreach (var item in headerNames)
            {
                Console.WriteLine(item.InnerText);
            }
        }


        private static async Task yellowPagesParserPage2()
        {
            // Example from .http://html-agility-pack.net/from-web

            // var url = @"https://www.yellowpages.com/search?search_terms=programmer&geo_location_terms=Warwick%2C+RI";
            var url = @"https://www.yellowpages.com/search?search_terms=programmer&geo_location_terms=Warwick%2C%20RI&page=2";


            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);

            var headerNames = doc.DocumentNode.SelectNodes("//a[@class='business-name']").ToList();

            foreach (var item in headerNames)
            {
                Console.WriteLine(item.InnerText);
            }
        }

        private static async Task StartCrawlerAsync()
        {
            // Example from .https://youtu.be/oeuvL1_5UIQ

            var url = "https://www.cars.com/shopping/lexus/";

            Console.WriteLine("29. " + url);

            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(html);

            //var divs = htmlDocument.DocumentNode.Descendants("div")
            //    .Where(node => node.GetAttributeValue("class", "")
            //        .Equals("shrunk")).ToList();

            //foreach (var div in divs)
            //{
            //    string name = div.Descendants("ul").FirstOrDefault().InnerText;
            //    Console.WriteLine(name);
            //}
        }
    }
}

