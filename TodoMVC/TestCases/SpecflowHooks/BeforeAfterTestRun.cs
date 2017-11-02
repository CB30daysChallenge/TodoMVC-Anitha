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
         
            if (ConfigurationManager.AppSettings["browser"].Contains("Firefox"))
            {
                string _firefoxPath = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe";
                var path2 = Registry.GetValue(_firefoxPath, "", null);
                Console.WriteLine("Firefox Version is : {0} ", FileVersionInfo.GetVersionInfo(path2.ToString()).FileVersion);
                Console.WriteLine("*****************************************************************");
            }
            else if (ConfigurationManager.AppSettings["browser"].Contains("Chrome"))
            {
                string _chromePath = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";
                var path2 = Registry.GetValue(_chromePath, "", null);
                Console.WriteLine("Chrome Ver {0}: ", FileVersionInfo.GetVersionInfo(path2.ToString()).FileVersion);
                Console.WriteLine("*****************************************************************");

            }


            //if (ConfigurationManager.AppSettings["isGridExecution"].ToString().Trim() == "true")
            //{
            //    string _hubUrl = "http://" + ConfigurationManager.AppSettings["hub"] + ":" + ConfigurationManager.AppSettings["port"] + "/grid/console";
            //    Console.WriteLine("Test Executing on Selenium Grid | HUB Console URL : {0}", _hubUrl);
            //}
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
