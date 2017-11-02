using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoMVC.PageObjectModels
{
        class TodoMVCPage : CBBase
        {
            //public object driver;
            //public IWebDriver driver;
            public TodoMVCPage(string title = "") : base(title)
            {
                PageFactory.InitElements(Driver, this);
            }
        public const string ToDoMvcHomePage = @"http://todomvc.com/examples/angularjs/#/";

        //[FindsBy(How = How.XPath, Using = "//span[@class='place-bet-copy']")]
        //public IWebElement placeBetButton { get; set; }
        public void LaunchTodoMVCPage()
            {

                PageManager pageManager = new PageManager();
                try
                {
                    Driver.Navigate().GoToUrl(ToDoMvcHomePage);
                //WaitUntilElementIsVisible

                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Element does not exist");
                }
            }
        }
}
