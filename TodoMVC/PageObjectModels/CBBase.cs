using System;
using System.Configuration;
using OpenQA.Selenium;
using TodoMVC.Selenium;
using OpenQA.Selenium.Support.UI;


namespace TodoMVC.PageObjectModels
{
    class CBBase : SeleniumWebdriver
    {

 
        internal IWebDriver Driver { get; set; }

        public CBBase(String urlOfPage)
        {
            Driver = WebDriver;
        }
     
        private const int DefaultWaitSeconds = 30;
        protected const bool NewImplementation = true;

        protected IWebElement WaitUntilElementIsVisible(By locator, bool isNewImplementation, int timeoutSeconds = DefaultWaitSeconds)
        {
            if (isNewImplementation)
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.Message = "Element " + locator + " did not become visible before timeout.";
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return FindElement(locator);
            }
            WaitUntilElementIsVisible(locator);
            return null;
        }

        protected IWebElement FindElement(By locator)
        {
            return Driver.FindElement(locator);
        }

        protected bool IsElementVisible(By locator, bool isNewImplementation, int timeoutSeconds = 10)
        {
            SetImplicitWaitTimeout(0);
            try
            {
                WaitUntilElementIsVisible(locator, timeoutSeconds);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                RestoreImplicitWaitTimeout();
            }
        }

        protected void SetImplicitWaitTimeout(int seconds)
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        protected void RestoreImplicitWaitTimeout()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["implicitWaitTimeoutSeconds"]));
        }
    }
}
