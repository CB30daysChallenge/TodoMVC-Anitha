using System;
using System.IO;
using TechTalk.SpecFlow;
using TodoMVC.Selenium;
using System.Configuration;
using Microsoft.Win32;
using System.Diagnostics;


namespace TodoMVC.TestCases.SpecflowHooks
{
    [Binding]
    class BeforeAfterTestRun
    {
        [BeforeTestRun]
        public static void BeforeFeature()
        {
            Console.WriteLine("[BeforeTestRun]");        
            Console.WriteLine("Application URL  : {0}", ConfigurationManager.AppSettings["TestEnv"]);        
            Console.WriteLine("Browser          : {0}", ConfigurationManager.AppSettings["browser"]);

        }

        [AfterStep]
        public static void AfterStep()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                var err = ScenarioContext.Current.TestError;
                Console.WriteLine("[After Step]");
                Console.WriteLine("An Error occured : {0}", err.Message);
                Console.WriteLine("Error Type       : {0}", err.GetType().Name);
                SeleniumWebdriver.CaptureScreenshot();
            }

        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Console.WriteLine("[AfterScenario]");
            CleanUp();
        }

        [AfterTestRun]
        public static void AfterFeature()
        {
            Console.WriteLine("[AfterTestRun]");
            CleanUp();
        }

        private static void CleanUp()
        {
            SeleniumWebdriver.QuitBrowser();
            Console.WriteLine("** Browser is Closed! ***");

        }
    }
}
