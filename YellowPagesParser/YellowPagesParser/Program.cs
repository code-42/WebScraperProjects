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
            // eBayParser();

            yellowPagesParser();

            //yellowPagesParserPage2();

            //StartCrawlerAsync();

            Console.ReadLine();
        }


        private static async void eBayParser()
        {
            // Example from .https://youtu.be/B4x4pnLYMWI

            var url = "https://www.ebay.com/sch/i.html?_from=R40&_nkw=xbox+one&_in_kw=1&_ex_kw=&_sacat=0&LH_Complete=1&_udlo=&_udhi=&_samilow=&_samihi=&_sadis=15&_stpos=02886&_sargn=-1%26saslc%3D1&_salic=1&_sop=12&_dmd=1&_ipg=200";

            // This method call using System.Net.Http;
            // Requires method to be async
            // Need to add await to method call
            var httpclient = new HttpClient();
            var html = await httpclient.GetStringAsync(url);

            // This call using HtmlAgilityPack package
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // Use HtlmAgilityPack to parse out data
            var ProductsHtml = htmlDocument.DocumentNode.Descendants("ul")
                .Where(node2 => node2.GetAttributeValue("id", "")
                    .Equals("ListViewInner")).ToList();

            var ProductListItems = ProductsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("id", "")
                    .Contains("item")).ToList();

            Console.WriteLine(ProductListItems.Count());
            Console.WriteLine();

            foreach (var ProductListItem in ProductListItems)
            {
                // id
                Console.WriteLine(ProductListItem.GetAttributeValue("listingid", ""));

                // ProductName
                Console.WriteLine(ProductListItem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                        .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                );

                // Price
                Console.WriteLine(
                    Regex.Match(
                    ProductListItem.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("lvprice prc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    , @"\d+.\d+")
                    );

                // ListingType lvformat
                Console.WriteLine(ProductListItem.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                        .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                        );

                // Url
                Console.WriteLine(ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "").Trim('\r', '\n', '\t')
                    );

                Console.WriteLine();
            }
            Console.WriteLine("Press Control + C to exit");
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

