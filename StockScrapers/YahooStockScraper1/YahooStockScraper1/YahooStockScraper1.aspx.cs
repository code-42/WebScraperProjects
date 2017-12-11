using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
// using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;


// This project modeled largely after the tutorial on codeproject.com
// .https://www.codeproject.com/Articles/659019/Scraping-HTML-DOM-elements-using-HtmlAgilityPack-H
// but refactoring has changed it quite a lot
// TODO: need to fix page keeps reloading and adding more and more data - it needs to stop after first pass


namespace YahooStockScraper1
{
    public partial class YahooStockScraper1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ClickMeButton_Click(object sender, EventArgs e)
        {
            // Get stock symbol from user
            var stockSymbol = getStocksymbol();

            var page = LoadPage1(stockSymbol);
            // var page = LoadPage2(stockSymbol)

            // Get stock symbol from user then get web page
            GetYahooFinanceHistoricalData(page);

            //getAtags(page);
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

        public void GetYahooFinanceHistoricalData(HtmlDocument page)
        {
             // Establish database connection
            using (SqlConnection conn = new SqlConnection())
            {
                // Create connection string
                conn.ConnectionString =
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\MeAdmin\Documents\YahooData.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    conn.Open();
                    DataBaseLabel.Text = "Database connection opened!" + "<br /><br />";
                }
                catch (Exception e)
                {
                    DataBaseLabel.Text = "My Bad: " + e + "<br /><br />";
                }
            }


            // Count the number of rows to retrieve
            var trRows = page.DocumentNode.SelectNodes("//tr");
            int trCounter = 1;
            if (trRows != null)
            {
                foreach (var trRow in trRows)
                {
                    trCounter++;
                }
                RowCount.Text = "Total rows: " + trCounter.ToString() + "<br /><br />";
            }

            // Loop through the data rows
            for (var row = 0; row < trCounter; row++)
            {
                // Select all tr that do not have the specified class in their descendants .//
                var trTags = page.DocumentNode.SelectNodes("//tr[" + row + "][not(.//@class='Ta(c) Py(10px) Pstart(10px)')]");
                if (trTags != null)
                {
                    foreach (var trTag in trTags)
                    {
                        // Loop through the columns to select the data
                        for (var col = 0; col <= 7; col++)
                        {
                            // Select the data in the col
                            var selectNodes = "//tbody//tr[" + row + "]//td[" + col + "]/span";
                            var tdTags = page.DocumentNode.SelectNodes(selectNodes);
                            if (tdTags != null)
                            {
                                foreach (var tdTag in tdTags)
                                {
                                    {
                                        OutputLabel.Text += tdTag.InnerText + " \t "; // + "\t" + "<br />";
                                    }
                                }
                            }
                        }
                        // Print a line break after each tr
                        OutputLabel.Text += "<br />";
                    }
                }
            }
        }


        public void getAtags(HtmlDocument page)
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
