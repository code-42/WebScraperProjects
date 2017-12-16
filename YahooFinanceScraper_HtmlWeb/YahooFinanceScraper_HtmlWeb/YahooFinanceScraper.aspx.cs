﻿using System;
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
            // var page = LoadPage2(stockSymbol)

            // Get stock symbol from user then get web page
            GetYahooFinanceHistoricalData(page);

            //YahooScraper();

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
            using (SqlConnection conn = new SqlConnection())
            {
                // Create connection string
                conn.ConnectionString =
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\MeAdmin\Documents\YahooData.mdf;Integrated Security=True;Connect Timeout=30"
                    ;

                try
                {
                    conn.Open();
                    //DataBaseLabel.Text = "Database connection opened!" + "<br /><br />";
                }
                catch (Exception e)
                {
                    //DataBaseLabel.Text = "My Bad: " + e + "<br /><br />";
                }

                //SqlCommand insertCommand = new SqlCommand(
                //    "INSERT INTO HistoricalData (Date, Open, High, Low, Close, AdjClose, Volume) VALUES (@0, @1, @2, @3, @4, @5, @6)",
                //    conn);

                //string statement =
                //    "INSERT INTO HistoricalData (Date, Open, High, Low, Close, AdjClose, Volume) VALUES (@0, @1, @2, @3, @4, @5, @6)";
                //SqlCommand insertCommand = new SqlCommand(statement, conn);

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

                //List<HtmlNode> Data = new List<HtmlNode>();
                //List<string> data = new List<string>();
                var dataArr = new string[8];
                var Date = DateTime.Today;
                var Open = 0.0f;
                var High = 0.0f;
                var Low = 0.0f;
                var Close = 0.0f;
                var AdjClose = 0.0f;
                Int32 Volume = 0;

                // Loop through the data rows
                for (var row = 0; row < trCounter; row++)
                {
                    // Select all tr that do not have the specified class in their descendants .//
                    var trTags =
                        page.DocumentNode.SelectNodes("//tr[" + row +
                                                      "][not(.//@class='Ta(c) Py(10px) Pstart(10px)')]");
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
                                        //OutputLabel.Text += tdTag.InnerText + " \t ";
                                        //Array.add(dataArr, tdTag.InnerText);
                                        dataArr[col] = tdTag.InnerText;
                                    }

                                }
                                // DataArrLabel1.Text += dataArr[col] + " \t ";

                                switch (col)
                                {
                                    case 1:
                                        Date = Convert.ToDateTime(dataArr[col]);
                                        DataArrLabel1.Text += dataArr[col] + " \t ";
                                        break;
                                    case 2:
                                        Open = float.Parse(dataArr[col]);
                                        DataArrLabel1.Text += dataArr[col] + " \t ";
                                        break;
                                    case 3:
                                        High = float.Parse(dataArr[col]);
                                        // DataArrLabel1.Text += dataArr[col] + " \t ";
                                        break;
                                    case 4:
                                        Low = float.Parse(dataArr[col]);
                                        // DataArrLabel1.Text += dataArr[col] + " \t ";
                                        break;
                                    case 5:
                                        Close = float.Parse(dataArr[col]);
                                        // DataArrLabel1.Text += dataArr[col] + " \t ";
                                        break;
                                    case 6:
                                        AdjClose = float.Parse(dataArr[col]);
                                        // DataArrLabel1.Text += dataArr[col] + " \t ";
                                        break;
                                    case 7:
                                        Volume = int.Parse((dataArr[col]).Replace(",", ""));
                                        // DataArrLabel1.Text += Volume + " \t ";
                                        break;
                                }

                                // DataArrLabel1.Text += Date + " " + Open + " " + Volume + "<br />";

                            }

                            string statement =
                                "INSERT INTO HistoricalData (Date, Open, High, Low, Close, AdjClose, Volume) VALUES (@0, @1, @2, @3, @4, @5, @6)";
                            SqlCommand insertCommand = new SqlCommand(statement, conn);

                            //DataArrLabel1.Text += Date + " " + Open + " " + Volume + "<br />";

                            insertCommand.Parameters.Add(new SqlParameter("0", Date));
                            insertCommand.Parameters.Add(new SqlParameter("1", Open));
                            insertCommand.Parameters.Add(new SqlParameter("2", High));
                            insertCommand.Parameters.Add(new SqlParameter("3", Low));
                            insertCommand.Parameters.Add(new SqlParameter("4", Close));
                            insertCommand.Parameters.Add(new SqlParameter("5", AdjClose));
                            insertCommand.Parameters.Add(new SqlParameter("6", Volume));

                            // DataArrLabel1.Text += "Commands executed! Total rows affected are " + insertCommand.ExecuteNonQuery();
                            //int RowsInserted = insertCommand.ExecuteNonQuery();
                            //DataLabel1.Text += RowsInserted;



                            // Print a line break after each tr
                            //OutputLabel.Text += "<br />";
                            //DataArrLabel1.Text += "<br />";

                            //foreach (string item in data)
                            //{
                            //    DataLabel1.Text += item;
                            //}

                            //DataLabel1.Text += Date + " " + Open + " " + Volume + "<br />";

                            //SqlCommand insertCommand = new SqlCommand(
                            //    "INSERT INTO HistoricalData (Date, Open, High, Low, Close, AdjClose, Volume) VALUES (@0, @1, @2, @3, @4, @5, @6)",
                            //    conn);


                            //insertCommand.Parameters.Add(new SqlParameter("0", Date));
                            //insertCommand.Parameters.Add(new SqlParameter("1", Open));
                            //insertCommand.Parameters.Add(new SqlParameter("2", High));
                            //insertCommand.Parameters.Add(new SqlParameter("3", Low));
                            //insertCommand.Parameters.Add(new SqlParameter("4", Close));
                            //insertCommand.Parameters.Add(new SqlParameter("5", AdjClose));
                            //insertCommand.Parameters.Add(new SqlParameter("6", Volume));

                            //Console.WriteLine("Commands executed! Total rows affected are " + insertCommand.ExecuteNonQuery());


                            //Array.Clear(dataArr, 0, 8);

                        }

                    }

                }
                //Console.WriteLine("Commands executed! Total rows affected are " + insertCommand.ExecuteNonQuery());

                // Query the databsase to see the records that were inserted
                // SqlCommand queryCommand = new SqlCommand("SELECT * FROM HistoricalData", conn);

                // Return a count of the records just inserted into the databse
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM HistoricalData", conn);
                try
                {
                    DataLabel1.Text = "Records count: " + countCommand.ExecuteNonQuery() + "<br />";
                }
                catch (Exception e)
                {
                    DataLabel1.Text = "OOPs " + e;
                }


            }
        }


        public static void YahooScraper()
        {
            // Establish database connection
            using (SqlConnection conn = new SqlConnection())
            {
                // Create connection string
                conn.ConnectionString =
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\MeAdmin\Documents\YahooData.mdf;Integrated Security=True;Connect Timeout=30"
                    ;

                try
                {
                    conn.Open();
                    //DataBaseLabel.Text = "Database connection opened!" + "<br /><br />";
                }
                catch (Exception e)
                {
                    //DataBaseLabel.Text = "My Bad: " + e + "<br /><br />";
                }

                SqlCommand insertCommand = new SqlCommand(
                    "INSERT INTO SQLCodeProject (Date, Open, High, Low, Close, AdjClose, Volume) VALUES (@0, @1, @2, @3, @4, @5, @6)",
                    conn);

                var url = "https://finance.yahoo.com/quote/HD/history?p=HD";

                // This method call using System.Net.Http;
                // Requires method to be async
                // Need to add await to method call
                //var httpclient = new HttpClient();
                //var html = await httpclient.GetStringAsync(url);

                var getHtmlWeb = new HtmlWeb();
                HtmlDocument page = getHtmlWeb.Load(url);

                // This call using HtmlAgilityPack package
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(url);

                // Scraping modeled after example from .https://youtu.be/B4x4pnLYMWI
                // Use HtmlAgilityPack to parse out data
                var tr = page.DocumentNode.Descendants("tr")
                    .Where(trNode => trNode.GetAttributeValue("class", "")
                        .Contains("BdT Bdc($c-fuji-grey-c)")).ToList();

                var td = page.DocumentNode.Descendants("td")
                    .Where(tdNode => tdNode.HasAttributes.Equals("data-reactid")).ToList();

                foreach (var span in td)
                {
                    // Date
                    var yDate = (span.GetAttributeValue("data-reactid", ""));


                    //DataBuiderLabel.Text += "test" + yDate;
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
