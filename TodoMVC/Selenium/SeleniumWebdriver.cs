using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.IO;
using NUnit.Framework;
using System.Drawing.Imaging;

namespace TodoMVC.Selenium
{
  
    public class SeleniumWebdriver
    {
        private static IWebDriver _driver;
        protected static IWebDriver WebDriver
        {
            get
            {
                string _executionMode = null;
                if (_driver == null)
                {
                    string driverConfig = GetAppSettings("browser");


                    _driver = InitLocalWebDriver(driverConfig);
                    _executionMode = " Local WebDriver";

                    Console.WriteLine("Test Execution Mode : {0} ", _executionMode);
                    ConfigureWebDriver(driverConfig);

                }
                return _driver;
            }
        }


        private static IWebDriver InitLocalWebDriver(string driverConfig)
        {

            if (!String.IsNullOrEmpty(driverConfig))
            {
                switch (driverConfig)
                {
                    case "Firefox":
                        _driver = new FirefoxDriver();
                        break;
                    case "Chrome":
                        _driver = new ChromeDriver();
                        break;
                    case "IE":
                        _driver = new InternetExplorerDriver();
                        break;

                    case "PortraitRespUAT":
                        _driver = new FirefoxDriver();
                        break;

                    case "ChromeResponsiveIphone6":
                        _driver = new ChromeDriver();
                        break;
                    default:
                        Console.WriteLine("Browser : {0} Not found ,Setting up Default browser(Firefox) for test execution", driverConfig);
                        _driver = new FirefoxDriver();
                        break;
                }


            }
            return _driver;
        }

        internal static void ConfigureWebDriver(string browser, int timeoutInSeconds = 30)
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeoutInSeconds);
            _driver.Manage().Cookies.DeleteAllCookies();           
                int widthBeforeMaximize = _driver.Manage().Window.Size.Width;
                _driver.Manage().Window.Maximize();
                if (_driver.Manage().Window.Size.Width < widthBeforeMaximize)
                {
                    _driver.Manage().Window.Maximize();
                }
           
        }


        public static string GetAppSettings(string key)
        {
            string value = null;
            try
            {
                value = ConfigurationManager.AppSettings[key];
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine(" Exception occured while reading key {0} from App.config file | Exception : {1}", key, e.StackTrace);
            }

            return value;
        }
 


  
        public static void CloseBrowser()
        {
            if (_driver != null) _driver.Close();
        }

       
        public static void QuitBrowser()
        {
            if (_driver != null)
                _driver.Dispose();
            _driver = null;

        }
    

        public static bool ElementExists(IWebElement element)
        {
            if (element == null)
            {
                return false;
            }
            return true;
        }


        public static void CaptureScreenshot()
        {
            try
            {
                ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile("c:/TodoMVCScreenshots", ScreenshotImageFormat.Png);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while taking screenshot: {e}");
            }

        }

   
        public IWebElement WaitUntilElementExists(By locatorStrategy, int waitTime = 30)
        {
            IWebElement element = WebDriverWait().Until<IWebElement>(
                (webDriver) =>
                {
                    return webDriver.FindElement(locatorStrategy);
                });
            return element;
        }

   
        /// To wait for page to load completely   
        public static void WaitForPageToLoad(int timeout)
        {
            string jsFunction = "return document.readyState";
            try
            {
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                WebDriverWait(timeout).Until(driver1 => ((IJavaScriptExecutor)_driver).ExecuteScript(jsFunction).Equals("complete"));
            }
            catch (Exception)
            {
                Assert.Fail("Page load is not complete after '{0}' seconds . Executed JS : {1} ", timeout, jsFunction);
            }

        }


  
        /// To Wait until an element is visible on the web page      
        public static void WaitUntilElementIsVisible(By locatorStrategy, int waitTime = 20)
        {
            try
            {
                WebDriverWait(waitTime).Until(ExpectedConditions.ElementIsVisible(locatorStrategy));
            }
            catch (Exception)
            {
                string msg = string.Concat(locatorStrategy, " not visible on the page after waiting for ", waitTime, "seconds");
                throw new Exception(msg);
            }


        }
    

        public static DefaultWait<IWebDriver> WebDriverWait(int waitTime = 20)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = TimeSpan.FromSeconds(waitTime);
            return wait;
        }
        private static void TurnOnImplicitWait()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }


    }


}
