using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace WhizDomSelenimExample1
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://www.facebook.com/";

            //driver.Manage().Timeouts().ImplicitWait;

            driver.FindElement(By.Id("email")).SendKeys("edwd42@gmail.com");
            driver.FindElement(By.Id("pass")).SendKeys("*");

        }
    }
}
