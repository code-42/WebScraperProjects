using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
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
            driver.Navigate().GoToUrl("http://executeautomation.com/demosite/index.html?UserName=&Password=&Login=Login");
            Console.WriteLine("Opened URL");
        }

        [Test]
        public void ExecuteTest()
        {
            // Title
            SeleniumSetMethods.SelectDropDown(driver, "TitleId", "Mr.", "Id");

            // Initial
            SeleniumSetMethods.EnterText(driver, "Initial", "executeautomation", "Name");

            // Click
            SeleniumSetMethods.Click(driver, "Save", "Name");
        }

        [Test]
        public void NextTest()
        {
            Console.WriteLine("Next method");
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Close();
            Console.WriteLine("Closed the browser");
        }
    }   
}
