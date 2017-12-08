using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace eBayScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            eBayScraper();
            Console.ReadLine();
        }

        private static async void eBayScraper()
        {
            // Scraping example from .https://youtu.be/B4x4pnLYMWI
            // Database example from .

            // Establish database connection
            using (SqlConnection conn = new SqlConnection())
            {
                // Create the connectionString
                // Trusted_Connection is used to denote the connection uses Windows Authentication
                conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\MyDox\C_sharp_projects\WebScraperProjects\eBayScraper\eBayScraper\eBayDatabase1.mdf;Integrated Security=True;Connect Timeout = 30";

                try
                {
                    conn.Open();
                    Console.WriteLine("connection opened!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("My Bad: " + e);
                }

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
                    var Id = (ProductListItem.GetAttributeValue("listingid", ""));
                    Console.WriteLine(Id);


                    // ProductName
                    var ProductName = (ProductListItem.Descendants("h3")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    );
                    Console.WriteLine(ProductName);
                
                    // Price
                    var xPrice = (
                        Regex.Match(
                        ProductListItem.Descendants("li")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Equals("lvprice prc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                        , @"\d+.\d+")
                        );
                    var Price = xPrice.ToString();
                    Console.WriteLine(Price);
                    
                    // ListingType
                    var ListingType = (ProductListItem.Descendants("li")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                            );
                    Console.WriteLine(ListingType);
                    

                    // Url
                    var Url = (ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "").Trim('\r', '\n', '\t')
                        );
                    Console.WriteLine(Url);

                    Console.WriteLine();

                    // Add data to database
                    SqlCommand insertCommand = new SqlCommand("INSERT INTO dbo.eBayData1 (ListingId, ProductName, Price, ListingType, Url) VALUES (@0, @1, @2, @3, @4)", conn);

                    insertCommand.Parameters.Add(new SqlParameter("0", Id));
                    insertCommand.Parameters.Add(new SqlParameter("1", ProductName));
                    insertCommand.Parameters.Add(new SqlParameter("2", Price));
                    insertCommand.Parameters.Add(new SqlParameter("3", ListingType));
                    insertCommand.Parameters.Add(new SqlParameter("4", Url));

                    try
                    {
                        Console.WriteLine("Commands executed! Total rows affected are " + insertCommand.ExecuteNonQuery());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("OOPs " + e);
                    }

                    //Console.WriteLine();
                }

                // Query the databsase to see the records that were inserted
                SqlCommand queryCommand = new SqlCommand("SELECT * FROM eBayData1", conn);

                Console.WriteLine();
                try
                {
                    using (SqlDataReader reader = queryCommand.ExecuteReader())
                    {
                        Console.WriteLine("ListingId\t\tProductName\t\tPrice\t\tListingType\t\tUrl");
                        while (reader.Read())
                        {
                            Console.WriteLine(String.Format("{0} \n | {1} \n | {2} \n | {3} \n | {4}",
                                reader[0], reader[1], reader[2], reader[3], reader[4]));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("MyBad: " + e);
                }
            }

            //Console.WriteLine("Press Control + C to exit");
        }

    }
}
