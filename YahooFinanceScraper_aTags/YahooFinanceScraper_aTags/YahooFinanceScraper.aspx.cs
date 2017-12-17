using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
//using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;


// This project modeled largely after the tutorial on codeproject.com
// .https://www.codeproject.com/Articles/659019/Scraping-HTML-DOM-elements-using-HtmlAgilityPack-H
// but refactoring has changed it quite a lot
// TODO: need to fix page keeps reloading and adding more and more data - it needs to stop after first pass


namespace YahooFinanceScraper
{
    public partial class YahooFinanceScraper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ClickMeButton_Click(object sender, EventArgs e)
        {
            // Get stock symbol from user
            var stockSymbol = getStocksymbol();

            var page = LoadPage1(stockSymbol);
            // var page = LoadPage2(stockSymbol);

            GetAtags(page);
        }

        public string getStocksymbol()
        {
            return InputTextBox.Text;
        }

        public HtmlDocument LoadPage1(string stockSymbol)
        {
            // There are several ways to load a web page - heres one
            // .https://www.codeproject.com/Articles/659019/Scraping-HTML-DOM-elements-using-HtmlAgilityPack-H

            var getHtmlWeb = new HtmlWeb();

            var document = "https://finance.yahoo.com/quote/";
            document += stockSymbol;
            document += "/history?p=";
            document += stockSymbol;

            HtmlDocument page = getHtmlWeb.Load(document);

            return page;
        }

        public HtmlDocument LoadPage2(string stockSymbol)
        {
            // There are several ways to load a web page - heres another
            // .http://blog.adrian-thomas.com/2013/02/starters-guide-to-web-scraping-with.html

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            var document = "https://finance.yahoo.com/quote/";
            document += stockSymbol;
            document += "/history?p=";
            document += stockSymbol;

            using (var webClient = new System.Net.WebClient())
            {
                using (var stream = webClient.OpenRead(document))
                {
                    htmlDocument.Load(stream);
                }
            }

            return htmlDocument;
        }

        public void GetAtags(HtmlDocument page)
        {
            var aTags = page.DocumentNode.SelectNodes("//a");
            int counter = 1;
            if (aTags != null)
            {
                foreach (var aTag in aTags)
                {
                    OutputLabel.Text += counter + ". " + aTag.InnerHtml + " #-# " + aTag.Attributes["href"].Value + "\t" + "<br />";
                    counter++;
                }
            }
        }
    }
}
