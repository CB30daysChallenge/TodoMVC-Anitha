using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Drawing.Imaging;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TodoMVC.PageObjectModels;
using System.Drawing;
using OpenQA.Selenium.Interactions;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using System.Linq;
using RestSharp;

namespace TodoMVC.Selenium
{
    //TODO: refactor this class to improve robustness for selenium usage
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

        private static IWebDriver InitRemoteWebDriver(string driverConfig)
        {
            ThreadLocal<IWebDriver> _thread = null;
            DriverOptions options;
            string hubUrl = string.Concat("http://", GetAppSettings("hub"), ":", GetAppSettings("port"), "/wd/hub").Trim();

            if (!String.IsNullOrEmpty(driverConfig))
            {
                switch (driverConfig)
                {
                    case "Firefox":
                        options = new FirefoxOptions();
                        break;
                    case "Chrome":
                        options = new ChromeOptions();
                        break;                 
                    case "IE":
                        options = new InternetExplorerOptions();
                        break;
                    default:
                        Console.WriteLine("Browser : {0} Not found ,Initializing Default browser(Firefox) for test execution", driverConfig);
                        options = new FirefoxOptions();
                        break;
                }

                RemoteWebDriver webDriver = new RemoteWebDriver(new Uri(hubUrl), options);
                _thread = new ThreadLocal<IWebDriver>(() => webDriver);
                string session = webDriver.SessionId.ToString();
                //GetNodeIp(GetAppSettings("hub"), GetAppSettings("port"), session);

            }
            return _thread.Value;
        }


        private static IWebDriver InitLocalWebDriver(string driverConfig)
        {
            // FirefoxProfile profile;
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

            //Configuration settings for Responsive tests
            if (browser.Contains("Resp"))
            {
                int width = int.Parse(GetAppSettings("Responsivewidth").Trim());
                int height = int.Parse(GetAppSettings("Responsiveheight").Trim());
                _driver.Manage().Window.Size = new Size(width, height);

                Console.WriteLine("Device       : {0}", GetAppSettings("Device"));
                Console.WriteLine("Device Width : {0} , Height : {1}", GetAppSettings("Responsivewidth"), GetAppSettings("Responsiveheight"));

            }
            else
            {
                int widthBeforeMaximize = _driver.Manage().Window.Size.Width;
                _driver.Manage().Window.Maximize();
                if (_driver.Manage().Window.Size.Width < widthBeforeMaximize)
                {
                    _driver.Manage().Window.Maximize();
                }
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
        public static void ClickByJS(IWebElement element, string name)
        {
            try
            {
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].click();", element);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to click " + name, ex);
            }
        }

        public static void ClickByJS(By locator, int retries = 10)
        {
            try
            {
                IWebElement element = GetElement(locator);
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].click();", element);
                return;
            }
            catch (Exception)
            { }

            if (retries > 0)
            {
                System.Threading.Thread.Sleep(200);
                ClickByJS(locator, --retries);
            }
            else
            {
                throw new ApplicationException("Unable to click by JS " + locator.ToString());
            }
        }


        public static void Click(By locator, int retries = 10)
        {
            try
            {
                IWebElement element = GetElement(locator);
                element.Click();
                return;
            }
            catch (Exception)
            { }

            if (retries > 0)
            {
                System.Threading.Thread.Sleep(200);
                Click(locator, --retries);
            }
            else
            {
                throw new ApplicationException("Unable to click " + locator.ToString());
            }
        }

        public static void Click(By locator, string elementName, int retries = 10)
        {
            try
            {
                Console.WriteLine($"Click on {elementName}");
                IWebElement element = GetElement(locator);
                element.Click();
                return;
            }
            catch (Exception)
            { }

            if (retries > 0)
            {
                System.Threading.Thread.Sleep(200);
                Click(locator, --retries);
            }
            else
            {
                throw new ApplicationException("Unable to click " + locator.ToString());
            }
        }



        public static void SendByJS(IWebElement element, string name, string value)
        {
            try
            {
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].setAttribute('value',arguments[1])", element, value);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to click " + name, ex);
            }
        }

        public static void sendKeysByJS(By locator, string value, int retries = 10)
        {
            try
            {
                IWebElement element = GetElement(locator);
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].setAttribute('value',arguments[1])", element, value);
                return;
            }
            catch (Exception)
            {
            }

            if (retries > 0)
            {
                System.Threading.Thread.Sleep(200);
                sendKeysByJS(locator, value, --retries);
            }
            else
            {
                throw new ApplicationException("Unable to click element " + locator.ToString());
            }
        }

        public static String getTextByJS(By locator, int retries = 10)
        {
            try
            {
                IWebElement element = GetElement(locator);
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                return (String)js.ExecuteScript("return arguments[0].innerHTML", element);
            }
            catch (Exception)
            {
            }

            if (retries > 0)
            {
                System.Threading.Thread.Sleep(200);
                return getTextByJS(locator, --retries);
            }
            else
            {
                throw new ApplicationException("Unable to get value for element " + locator.ToString());
            }
        }


        public static IWebElement WaitAndGetElement(By locator, int timeoutInSeconds = 30)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IWebElement element = wait.Until<IWebElement>((d) => { return _driver.FindElement(locator); });
            return element;                //var el = new WebDriverWait();
        }

        public static IWebElement GetElement(By selector, int tries = 5)
        {
            for (int i = 1; i <= tries; i++)
            {
                try
                {
                    return _driver.FindElement(selector);
                }
                catch (WebDriverException)
                {
                    System.Threading.Thread.Sleep(200);
                }
            }
            return null;
        }

        public static void CloseBrowser()
        {
            if (_driver != null) _driver.Close();
        }

        /// <summary>
        /// Dispose driver and set to null as _driver is declared as static
        /// </summary>
        public static void QuitBrowser()
        {
            if (_driver != null)
                _driver.Dispose();
            _driver = null;

        }
        public static void RefreshBrowser()
        {
            if (_driver != null) _driver.Navigate().Refresh();
        }

        public static IWebElement FindElementSafe(By by)
        {
            IWebElement element = null;
            try
            {
                ReduceImplicitWaitTime(5);
                element = _driver.FindElement(by);
                TurnOnImplicitWait();

            }
            catch (NoSuchElementException)
            {
                TurnOnImplicitWait();

            }
            return element;


        }

        public bool IsElementPresent(By locator)
        {
            return _driver.FindElements(locator).Count >= 1 ? true : false;
        }
        public static bool ElementExists(IWebElement element)
        {
            if (element == null)
            {
                return false;
            }
            return true;
        }

        //Displayed
        public static bool IsElementDisplayed(By element)
        {
            if (_driver.FindElements(element).Count > 0)
            {
                if (_driver.FindElement(element).Displayed)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        //Enabled
        public static bool IsElementEnabled(By element)
        {
            if (_driver.FindElements(element).Count > 0)
            {
                if (_driver.FindElement(element).Enabled)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }


        public void VerifyFieldswithAttribute(By locator, string p1)
        {
            IWebElement element = _driver.FindElement((locator));
            string x = element.GetAttribute("value");

            try
            {
                if (x.ToString().Equals(p1))
                {
                    Console.WriteLine("Value in the Text box : " + x + " is Same as Value Entered which was : " + p1);
                }
                else
                {
                    Assert.Fail("Something is Wrong and values in the text box is " + x + " but value entered while Update was " + p1);
                    CBBase.CaptureScreenshot();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void VerifyFieldswithText(By locator, string p1)
        {
            IWebElement element = _driver.FindElement((locator));
            Thread.Sleep(1000);
            string x = element.Text;

            try
            {
                if (x.ToString().Equals(p1))
                {
                    Console.WriteLine("Expected Value : " + x + " , is Same as actual Value : " + p1 + " ");
                }
                else
                {
                    Assert.Fail("Something went Wrong as Actual value is : " + x + " , but Expected Value was : " + p1);
                    CBBase.CaptureScreenshot();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool CompareLists2(List<string> list1, List<string> list2)
        {
            foreach (var item in list1)
            {
                if (!list2.Contains(item))
                {
                    Console.WriteLine("Value : " + item.ToString().Trim() + " is not matching !");
                    return false;
                }
            }
            return true;
        }

        public static void CaptureScreenshot()
        {
            try
            {
                ((ITakesScreenshot)_driver).GetScreenshot().SaveAsFile(GetFileNameForScreenshot("png"), ScreenshotImageFormat.Png);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while taking screenshot: {e}");
            }

        }


        private static string GetFileNameForScreenshot(string format)
        {
            string fileName = string.Format("Screenshot_" + DateTime.Now.ToString("dd-MM-yyyy-hhmm-ss") + "." + format);

            var artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Screenshot");
            if (!Directory.Exists(artifactDirectory))
            {
                Directory.CreateDirectory(artifactDirectory);
            }

            string screenshotFilePath = Path.Combine(artifactDirectory, fileName);
            Console.WriteLine($"Screenshot : {new Uri(screenshotFilePath)}");

            return screenshotFilePath;
        }

        public static void ScrollIntoView(IWebElement element)
        {
            try
            {
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                Thread.Sleep(2000);
                js.ExecuteScript("arguments[0].scrollIntoView();", element);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to click " + "scrollIntoView", ex);
            }
        }

        public static void scrollIntoView(By locator, int retries = 10)
        {
            try
            {
                IWebElement element = GetElement(locator);
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].scrollIntoView();", element);
                return;
            }
            catch (Exception)
            {
            }

            if (retries > 0)
            {
                System.Threading.Thread.Sleep(200);
                scrollIntoView(locator, --retries);
            }
            else
            {
                throw new ApplicationException("Unable to scrollIntoView for " + locator.ToString());
            }
        }

        public static DefaultWait<IWebDriver> WebDriverWait(int waitTime = 20)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = TimeSpan.FromSeconds(waitTime);
            return wait;
        }

        public void WaitUntilTextPresent(By locatorStrategy, String expectedText, int waitTime = 20)
        {
            /* Commenting below Method implementation as downgrading Selenium Webdriver to 2.45 and below is supported for 2.48.1.0 */
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
            try
            {
                wait.Until<bool>(ExpectedConditions.TextToBePresentInElement(_driver.FindElement(locatorStrategy), expectedText));

            }
            catch (Exception)
            {

                throw new ApplicationException($"Text  {expectedText} not present in the locator {locatorStrategy.ToString()}");
            }
        }


        public static void LongwaitUntilElementIsVisible(By locatorStrategy, int waitTime = 40)
        {
            WebDriverWait().Until<IWebElement>(ExpectedConditions.ElementIsVisible(locatorStrategy));

        }

        public void WaitUntilElementIsClickable(By locatorStrategy, int waitTime = 20)
        {
            WebDriverWait().Until<IWebElement>(ExpectedConditions.ElementToBeClickable(locatorStrategy));
        }

        // Scroll the page to required x and y position of the screen.
        public void Scroll(int xPos, int yPos)
        {
            try
            {
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("scroll(arguments[0], arguments[1]);", xPos, yPos);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to Scroll to location " + xPos + ":" + yPos, ex);
            }
        }
        /// <summary>
        /// To Perform scroll to a visible element
        /// </summary>
        /// <param name="element"></param>
        public void ScrollToVisibleElement(By element)
        {
            if (IsElementVisible(element))
            {
                Point p = _driver.FindElement(element).Location;
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("window.scrollTo(0," + p.Y + ")");
            }
            else
            {
                throw new ApplicationException("Unable to Scroll to element as this is not visible on the page");

            }
        }

        /// <summary>
        /// To Perform scroll to a specified coordinate
        /// </summary>
        /// <param name="element"></param>
        public static void ScrollByCoordinates(int x, int y)
        {
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
            js.ExecuteScript("window.scrollTo(" + x + "," + y + ")");

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

        //Method for Converting Color code RGBA to HEX !
        public static string ConvertRgbaToHex(string rgba)
        {
            var matches = Regex.Matches(rgba, @"\d+");
            StringBuilder hexaString = new StringBuilder("#");

            for (int i = 0; i < matches.Count - 1; i++)
            {
                int value = Int32.Parse(matches[i].Value);

                hexaString.Append(value.ToString("X"));
            }

            return hexaString.ToString();
        }

        //Method for Making string First Letter Capital . Ex American Football
        public string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }


        //Method to get parent background color as sometimes color has been inherited from parent. Specifically useful if GetCSSValue returns value as transperent
        public String GetParentBackgroundColor(IWebElement element)
        {
            IWebElement current = element;
            //Ugly while true loop, fix this
            while (true)
            {
                //Get the current elements parent
                IWebElement parent = element.FindElement(By.XPath(".."));

                //If the parent is null then doesn't have a parent so we should stop (change to seleniums default if element doesn't have a parent)
                if (parent == null)
                {
                    throw new Exception("Sorry, no parent elements had a background-color set");
                }
                else
                {
                    //Otherwise get the parents color
                    String color = parent.GetCssValue("background-color");
                    //If the color is transparent then set the parent as the current and continue the loop to try and get the parents parents color
                    if (color == "transparent")
                    {
                        current = parent;
                    }
                    else
                    {
                        //If we found a color return it
                        return color;
                    }
                }
            }
        }

        //Method to invoke date from date time picker       
        public void DateTimePicker(String Date, IList<IWebElement> allDates, IWebElement nextMonthArrow)
        {
            try
            {
                Thread.Sleep(500);
                int day = 0;
                int month = 0;
                int year = 0;
                string minutestemp = "00";

                //to open calendar
                /*Claender opens on selecting the textbox for date time*/
                //Split the Date
                if (Date.Contains('/'))
                {
                    String[] DateArray = Date.Split('/');
                    month = Int32.Parse(DateArray[0]);
                    day = Int32.Parse(DateArray[1]);
                    year = Int32.Parse(DateArray[2]);
                }
                else if (Date.Contains('.'))
                {
                    String[] DateArray = Date.Split('.');
                    day = Int32.Parse(DateArray[0]);
                    month = Int32.Parse(DateArray[1]);
                    year = Int32.Parse(DateArray[2]);
                }
                else if (Date.Contains(':'))
                {
                    String[] DateArray = Date.Split(':');
                    day = Int32.Parse(DateArray[0]);
                    month = Int32.Parse(DateArray[1]);
                    year = Int32.Parse(DateArray[2]);
                }
                else if (Date.Contains(' '))
                {
                    String[] DateArray = Date.Split(' ');
                    day = Int32.Parse(DateArray[0]);
                    month = Int32.Parse(DateArray[1]);
                    year = Int32.Parse(DateArray[2]);
                }
                else
                {
                    Assert.Fail("Invalid date format!!");
                }

                //get the year difference between current year and year to set in calander
                DateTime oldDate = DateTime.Now;

                DateTime newDate = new DateTime(year, month, day);

                String T_time = Convert.ToString(newDate);
                String[] T_timeArray1 = T_time.Split(' ');
                String[] T_timeArray2 = T_timeArray1[1].Split(':');
                String input_time = T_timeArray2[0] + ':' + minutestemp + ' ' + T_timeArray1[2];
                String input_time_detail = T_timeArray2[0] + ':' + T_timeArray2[1] + ' ' + T_timeArray1[2];

                // Difference in days, hours, and minutes.
                TimeSpan ts = newDate - oldDate;

                int differenceInDays = ts.Days;
                if (differenceInDays < 0)
                {
                    Assert.Fail("Start date and End Date should be greater than or equal to current date!!");
                }

                // Click on next arrow till we reach the correct month we want to select
                if (oldDate.Month != newDate.Month)
                {
                    DateTime oldDatetemp = DateTime.Now;

                    while (oldDatetemp.Month != newDate.Month)
                    {
                        nextMonthArrow.Click();
                        oldDatetemp = oldDatetemp.AddMonths(1);
                    }
                }

                //get all dates from calendar to select correct one                
                int size = allDates.Count;

                foreach (IWebElement _day in allDates)
                {
                    string daytmp = _day.Text;
                    if (daytmp.Equals(""))
                    {
                        continue;
                    }
                    int daySelect = Int32.Parse(daytmp);
                    if (daySelect == day)
                    {
                        _day.Click();
                        break;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in timepicker:" + e.ToString());
            }
        }


        ///<summary>
        ///This method will highlight a web element
        /// </summary>
        public static void HighlightUIControl(By element, string color = "red")
        {
            try
            {
                IWebElement uiControl = _driver.FindElement(element);
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                string formattedJs = $"arguments[0].style.border='1px {color}'";
                js.ExecuteScript(formattedJs, element);
            }
            catch (Exception)
            {

                Console.WriteLine("Couldn't highlight webelement");
            }

        }

        public static void ScrollTo(By element)
        {
            try
            {
                Actions action = new Actions(_driver);
                action.MoveToElement(_driver.FindElement(element)).Build().Perform();
            }
            catch (Exception)
            {

                Console.WriteLine("Mouse over operation failed on element {0}", element);
            }

        }

        ///<summary>
        ///This method will highlight a web element
        /// </summary>
        public static void Blink(By element, int timeout = 5)
        {
            try
            {
                IWebElement uiControl = _driver.FindElement(element);
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                for (int i = 0; i < timeout; i++)
                {
                    js.ExecuteScript("arguments[0].style.border='4px red'", uiControl);
                    js.ExecuteScript("arguments[0].style.backgroundColor='yellow'", uiControl);
                    Thread.Sleep(TimeSpan.FromMilliseconds(200));
                    js.ExecuteScript("arguments[0].style.border='4px red'", uiControl);
                    js.ExecuteScript("arguments[0].style.backgroundColor='lime'", uiControl);

                }

            }
            catch (Exception)
            {

                Console.WriteLine("Couldn't highlight webelement");
            }

        }


        ///<summary>
        ///This method will highlight and then perform click on a web element
        /// </summary>
        /// 
        public static void ClickAndHighlight(By locator, int retries = 5)
        {
            try
            {
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript("arguments[0].style.border='4px dotted green'", locator);
            }
            catch (Exception)
            {

                Console.WriteLine("Unable to highlight {0}", locator.ToString());
            }


            try
            {
                IWebElement element = GetElement(locator);
                element.Click();
                return;
            }
            catch (Exception)
            { }

            if (retries > 0)
            {
                System.Threading.Thread.Sleep(200);
                Click(locator, --retries);
            }
            else
            {
                throw new ApplicationException("Unable to click " + locator.ToString());
            }

        }

        public static void SendKeys(By locator, string text, bool clearBeforeType = true)
        {
            try
            {
                IWebElement element = _driver.FindElement(locator);
                if (clearBeforeType)
                {
                    element.Clear();
                }
                element.SendKeys(text);
            }
            catch (InvalidElementStateException)
            {
                throw new ApplicationException("Invalid Element State " + locator.ToString());
            }
            catch (ElementNotVisibleException)
            {
                throw new ApplicationException("Element " + locator + " is not visible on the page");

            }
            catch (StaleElementReferenceException)
            {
                throw new ApplicationException("StaleElement Exception on element " + locator);

            }
        }

        public static void SelectFromDropDownByText(By locator, string text)
        {
            int attempts = 0;
            while (attempts <= 5)
            {
                try
                {
                    IWebElement optionText = _driver.FindElement(locator);
                    SelectElement selectElement = new SelectElement(optionText);
                    selectElement.SelectByText(text);
                }
                catch (StaleElementReferenceException)
                {
                    if (attempts == 5)
                    {
                        throw new ApplicationException("StaleElementReferenceException on Locator : " + locator);

                    }
                }
                attempts++;
            }


        }


        public static void SelectFromDropDownByValue(By locator, string value)
        {
            int attempts = 0;
            while (attempts <= 5)
            {
                try
                {
                    IWebElement element = _driver.FindElement(locator);
                    SelectElement selectElement = new SelectElement(element);
                    selectElement.SelectByValue(value);
                }
                catch (StaleElementReferenceException)
                {
                    if (attempts == 5)
                    {
                        throw new ApplicationException("StaleElementReferenceException on Locator : " + locator);

                    }
                }
                attempts++;
            }


        }


        //To prevent Stale element exception and get the attribute of a web element
        public static string GetLocatorAttribute(IWebElement element, string attribute)
        {
            string result = null;
            int attempts = 0;
            while (attempts < 6)
            {
                try
                {
                    result = element.GetAttribute(attribute);
                    break;
                }
                catch (StaleElementReferenceException s)
                {
                    Console.WriteLine($"Stale element reference {s}");
                }
                attempts++;

            }

            return result;
        }


        public static void SetAttributeUsingJS(By locator, string attributeName, string attributeValue)
        {
            string jsFunction = "arguments[0].setAttribute(arguments[1], arguments[2]);";
            try
            {
                IWebElement element = _driver.FindElement(locator);
                IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
                js.ExecuteScript(jsFunction, element, attributeName, attributeValue);
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// To wait for page to load completely
        /// </summary>
        /// <param name="timeout"></param>
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

        /// <summary>
        /// To Wait until an element is visible on the web page
        /// </summary>
        /// <param name="locatorStrategy"></param>
        /// <param name="waitTime"></param>
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

        /// <summary>
        /// To Wait until an element is visible on the web page
        /// </summary>
        /// <param name="locatorStrategy"></param>
        /// <param name="waitTime"></param>
        public static void WaitUntilElementIsVisible(By locatorStrategy, string message, int waitTime = 20)
        {
            try
            {
                WebDriverWait(waitTime).Until(ExpectedConditions.ElementIsVisible(locatorStrategy));
            }
            catch (Exception)
            {
                string msg = string.Concat(message, "after waiting for ", waitTime, "seconds");
                throw new Exception(msg);
            }


        }

        public static void WaitForElementToDisappear(By locatorStrategy, int waitTime = 20)
        {
            try
            {
                TurnOffImplicitWait();
                WebDriverWait(waitTime).Until(
                                          ExpectedConditions.InvisibilityOfElementLocated(locatorStrategy));
                TurnOnImplicitWait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                if (!e.Message.Contains("The frame that contains this element was removed"))

                {
                    Assert.Fail("Element is still visible after waiting for '{0}' seconds", waitTime);
                }
            }


        }

        private static void TurnOffImplicitWait()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(01);
        }

        private static void ReduceImplicitWaitTime(int seconds)
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }


        private static void TurnOnImplicitWait()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        public static bool IsElementVisible(By locatorStrategy, int waitTime = 10)
        {
            bool flag = true;
            try
            {

                TurnOffImplicitWait();
                WebDriverWait(waitTime).Until(ExpectedConditions.ElementIsVisible(locatorStrategy));
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            TurnOnImplicitWait();
            return flag;

        }

        public static bool IsElementPresent(By locatorStrategy, int waitTime = 5)
        {
            bool flag = true;
            try
            {

                TurnOffImplicitWait();
                WebDriverWait(waitTime).Until(ExpectedConditions.ElementExists(locatorStrategy));
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            TurnOnImplicitWait();
            return flag;

        }

        public static string GetText(By locator)
        {
            string text = string.Empty;
            try
            {
                IWebElement element = _driver.FindElement(locator);
                text = element.Text;
            }
            catch (Exception)
            {
                text = null;
            }

            return text;
        }

        public static void ExecuteJavaScript(string script)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript(script);
            }
            catch (Exception)
            {

                throw new ApplicationException("Javascript execution failed . Script : " + script);
            }
        }

        public static void ToggleElementDisplayUsingId(string idOfElement, bool visibility = true)
        {
            string val = null;
            string flag = null;
            if (visibility)
            {
                val = "inline";
                flag = "Display";
            }
            else
            {
                val = "none";
                flag = "Hide";
            }
            string script = " document.getElementById(\"" + idOfElement + "\").style.display =\"" + val + "\" ";
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript(script);
            }
            catch (Exception)
            {

                throw new ApplicationException("Cannot" + flag + "the element with id " + idOfElement + "using java script");
            }
        }

        public static void ToggleElementDisplayUsingClassName(string className, bool visibility = true)
        {
            string val = null;
            string flag = null;
            if (visibility)
            {
                val = "inline";
                flag = "Display";
            }
            else
            {
                val = "none";
                flag = "Hide";
            }
            // string script = " document.getElementById(\"" + idOfElement + "\").style.display =\"" + val + "\" ";
            string script = "var x = document.getElementsByClassName('" + className + "'); x[0].style.display =\"" + val + "\" ";

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript(script);
            }
            catch (Exception)
            {

                throw new ApplicationException("Cannot" + flag + "the element with classname " + className + "using java script");
            }
        }


        public static void CaptureWebElement(By element)
        {
            string _fileName = string.Format("Screenshot_" + DateTime.Now.ToString("dd-MM-yyyy-hhmm-ss") + ".Jpeg");
            try
            {
                string _filePath = GetFileNameForScreenshot("jpg");

                TurnOffImplicitWait();
                IWebElement _element = _driver.FindElement(element);
                TurnOnImplicitWait();

                Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();
                var img = Image.FromStream(new MemoryStream(ss.AsByteArray)) as Bitmap;

                using (MemoryStream memory = new MemoryStream(ss.AsByteArray))
                {
                    var _imageAsBmp = Image.FromStream(memory) as Bitmap;
                    var _croppedImage = img.Clone(new Rectangle(_element.Location, _element.Size), img.PixelFormat);

                    _croppedImage.Save(_filePath, ImageFormat.Jpeg);
                    Console.WriteLine("Screenshot of Web Element: {0}", new Uri(_filePath));
                }
            }
            catch (ElementNotVisibleException)
            {
                Console.WriteLine("Can't capture screenshot of the element using : {0} as it is not visible in DOM", element);
            }
            catch (Exception)
            {
                Console.WriteLine("Can't capture screenshot of the element using : {0}", element);
            }
        }

        //private static void GetNodeIp(string hostName, string port, string sessionId)
        //{
        //    try
        //    {
        //        RestClient client = new RestClient("http://" + hostName + ":" + port + "/grid/api/testsession?session=" + sessionId);
        //        RestRequest req = new RestRequest(Method.GET);
        //        var resp = client.Execute(req);
        //        dynamic jsonData = JsonConvert.DeserializeObject(resp.Content);
        //        Console.WriteLine("Test Will run on Machine : {0}", new Uri(jsonData.proxyId.ToString()));
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        public static void SendKeysOnVisibleElement(By locator, string text, string elementName = "")
        {
            Console.WriteLine($"Wait for {elementName} to be visible and enter value : {text}");
            WaitUntilElementIsVisible(locator);
            Click(locator);
            SendKeys(locator, text);
        }

        public static void SendKeysByCharacter(By locator, string text, string locatorName)
        {
            Console.WriteLine($"Typing {text} character by character into {locatorName}");
            foreach (char character in text)
            {
                _driver.FindElement(locator).SendKeys(character.ToString());
            }
        }



    }


}
