using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SeleniumFirst
{
    class Program
    {
        // Create reference for browser
        IWebDriver driver = new ChromeDriver();

        static void Main(string[] args)
        {

        }

        [SetUp]
        public void Initialize()
        {
            // Navigate to Google page
            driver.Navigate().GoToUrl("http://www.google.com");
            Console.WriteLine("Opened URL");
        }

        [Test]
        public void ExecuteTest()
        {
            // Find the text box element
            IWebElement element = driver.FindElement(By.Name("q"));

            // Perform ops
            element.SendKeys("executeautomation");
            Console.WriteLine("Executed Test");
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Close();
            Console.WriteLine("Closed the browser");
        }
    }   
}
