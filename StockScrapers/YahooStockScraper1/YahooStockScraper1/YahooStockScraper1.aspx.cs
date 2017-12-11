using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
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
            // Get stock symbol from user then get web page
            var page = getYahooFinanceHistoricalData();

            //getAtags(page);

            // Get the data
            getTDtags(page);

        }
        
        public HtmlDocument getYahooFinanceHistoricalData()
        {
            var getHtmlWeb = new HtmlWeb();
            var document = "https://finance.yahoo.com/quote/";
            document += InputTextBox.Text;
                document += "/history?p=";
                document += InputTextBox.Text;
                HtmlDocument page = getHtmlWeb.Load(document);

            return page;
        }

        public void getTDtags(HtmlDocument page)
        {
            /*
             /html[@id='atomic']/body/div[@id='app']/div/div/div[@id='render-target-default']/div[@class='Bgc($bg-body) Mih(100%) W(100%) US']/div[@class='Pos(r) Bgc($bg-content) Miw(1007px) Maw(1260px) tablet_Miw(600px)--noRightRail Bxz(bb) Bdstartc(t) Bdstartw(20px) Bdendc(t) Bdends(s) Bdendw(20px) Bdstarts(s) Mx(a)']/div[@id='YDC-Col1']/div[@id='Main']/div[2]/div[@id='mrt-node-Col1-1-HistoricalDataTable']/div[@id='Col1-1-HistoricalDataTable-Proxy']/section[@class='smartphone_Px(20px)']/div[@class='Pb(10px) Ovx(a) W(100%)']/table[@class='W(100%) M(0)']/tbody/tr[@class='BdT Bdc($c-fuji-grey-c) Ta(end) Fz(s) Whs(nw)'][1]/td[@class='Py(10px) Ta(start) Pend(10px)']/span
             */

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

                // want to include "//tbody//tr[" + row + "]"
                // want to exclude "//tr[" + row + "][.//@class='Ta(c) Py(10px) Pstart(10px)']"
                // try this //tr[" + row + "][not(.//@class='Ta(c) Py(10px) Pstart(10px)')]
                // this will select only Dividend rows //tr[9][.//@class='Ta(c) Py(10px) Pstart(10px)']

                var trTags = page.DocumentNode.SelectNodes("//tr[" + row + "][not(.//@class='Ta(c) Py(10px) Pstart(10px)')]");
                if (trTags != null)
                {
                    foreach (var trTag in trTags)
                    {
                        //html[@id='atomic']/body/div[@id='app']/div/div/div[@id='render-target-default']/div[@class='Bgc($bg-body) Mih(100%) W(100%) US']/div[@class='Pos(r) Bgc($bg-content) Miw(1007px) Maw(1260px) tablet_Miw(600px)--noRightRail Bxz(bb) Bdstartc(t) Bdstartw(20px) Bdendc(t) Bdends(s) Bdendw(20px) Bdstarts(s) Mx(a)']/div[@id='YDC-Col1']/div[@id='Main']/div[2]/div[@id='mrt-node-Col1-1-HistoricalDataTable']/div[@id='Col1-1-HistoricalDataTable-Proxy']/section[@class='smartphone_Px(20px)']/div[@class='Pb(10px) Ovx(a) W(100%)']/table[@class='W(100%) M(0)']/tbody/tr[@class='BdT Bdc($c-fuji-grey-c) Ta(end) Fz(s) Whs(nw)'][ROW]/td[@class='Py(10px) Pstart(10px)'][COL]/span // replace ROW and COL
                        for (var col = 0; col <= 7; col++)
                        {

                            //var tdTags = page.DocumentNode.SelectNodes(@"/html[@id='atomic']/body/div[@id='app']/div/div/div[@id='render-target-default']/div[@class='Bgc($bg-body) Mih(100%) W(100%) US']/div[@class='Pos(r) Bgc($bg-content) Miw(1007px) Maw(1260px) tablet_Miw(600px)--noRightRail Bxz(bb) Bdstartc(t) Bdstartw(20px) Bdendc(t) Bdends(s) Bdendw(20px) Bdstarts(s) Mx(a)']/div[@id='YDC-Col1']/div[@id='Main']/div[2]/div[@id='mrt-node-Col1-1-HistoricalDataTable']/div[@id='Col1-1-HistoricalDataTable-Proxy']/section[@class='smartphone_Px(20px)']/div[@class='Pb(10px) Ovx(a) W(100%)']/table[@class='W(100%) M(0)']/tbody/tr[@class='BdT Bdc($c-fuji-grey-c) Ta(end) Fz(s) Whs(nw)'][1]/td[@class='Py(10px) Pstart(10px)'][" + col + "]/span");
                            var selectNodes = "//tbody//tr[" + row + "]//td[" + col + "]/span";
                            //    " | not //tr[" + row + "][.//@class='Ta(c) Py(10px) Pstart(10px)']";
                            //var selectNodes = "//tr[.//*[@class='Ta(c) Py(10px) Pstart(10px)']]";

                            var tdTags = page.DocumentNode.SelectNodes(selectNodes);
                            if (tdTags != null)
                            {
                                foreach (var tdTag in tdTags)
                                {
                                    {
                                        OutputLabel.Text += tdTag.InnerText + " \t "; // + "\t" + "<br />";
                                    }
                                    //OutputLabel.Text += tdTag.InnerText + " \t "; // + "\t" + "<br />";
                                }
                            }
                            //OutputLabel.Text += "<br />";
                        }
                        OutputLabel.Text += "<br />";
                    }
                }
                //OutputLabel.Text += "<br />";
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
