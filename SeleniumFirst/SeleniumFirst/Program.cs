using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create reference for browser
            IWebDriver driver = new ChromeDriver();

            // Navigate to Google page
            driver.Navigate().GoToUrl("http://www.google.com");

            // Find the text box element
            IWebElement element = driver.FindElement(By.Name("q"));

            // Perform ops
            element.SendKeys("executeautomation");

            driver.Close();
        }
    }
}
