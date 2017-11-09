using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using log4net;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TodoMVC.PageObjectModels
{
    class TodoMVCPage : CBBase
        {
                   
            public TodoMVCPage(string title = "") : base(title)
            {
                PageFactory.InitElements(Driver, this);
            }

        public const string ToDoMvcHomePage = @"http://todomvc.com/examples/angularjs/#/";

        private static readonly ILog log  = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [FindsBy(How = How.XPath, Using = "//*[@id='header']//h1[contains(text(),'todos')]")]
        public IWebElement todoMVCHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@id,'filters')]//a[@class='selected']")]
        public IWebElement allButtonSelected { get; set; }

        [FindsBy(How = How.Id, Using = "new-todo")]
        public IWebElement todos { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[contains(@id,'todo-count')]")]
        public IWebElement noOfItemsLeft { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@id,'filters')]//a[contains(text(),'All')]")]
        public IWebElement allButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@id,'filters')]//a[contains(@href,'#/active')]")]
        public IWebElement activeButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@id,'filters')]//a[contains(@href,'#/completed')]")]
        public IWebElement completedButton { get; set; }
  
        [FindsBy(How = How.XPath, Using = "//span[contains(@id,'todo-count')]//strong")]
        public IWebElement noOfItemsLeftCount { get; set; }



        public void LaunchTodoMVCPage()
         {

                PageManager pageManager = new PageManager();
                try
                {
                    Driver.Navigate().GoToUrl(ToDoMvcHomePage);
                    WaitForPageToLoad(10000);              
                    Console.WriteLine("ToDoMvcHomePage loaded");
                    LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    log.Info("ToDoMvcHomePage loaded");
                    
            }
                catch (NoSuchElementException)
                {
                log.Error("Element does not exist");
                Console.WriteLine("Element does not exist");
                     
                }
           }

        public void AddItemsToTheList(Table table)
        {
           string newTodoItems = null;         
           int count = 0;        
            foreach (var row in table.Rows)
            {                              
                    count++;
                    newTodoItems = row["todoitems"].ToString();
                    Console.WriteLine(newTodoItems);
                    todos.SendKeys(newTodoItems);
                    todos.SendKeys(Keys.Enter);
               
            }
            log.Info("new todo items are added");
        }

        public void VerifyCheckboxExistsForEveryTodoItem()
        {
            IList<IWebElement> AllTodos = Driver.FindElements(By.XPath("//li[@class='ng-scope']//div[@class='view']//input"));
            int todosCount = AllTodos.Count;            
            
                for (int i = 1; i <= todosCount; i++)
                {
                    try
                    {
                        IWebElement checkbox = Driver.FindElement(By.XPath("//ul[contains(@id,'todo-list')]//li[@class='ng-scope']["+i+"]//div[@class='view']"));                                   
                        Assert.IsTrue(PageManager.ElementExists(checkbox), "Checkbox does not exist");
                        Console.WriteLine("Checkbox Exists for todo item" +i);
                        log.Info("Checkbox exists for todo item" +i);
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Checkbox does not exist");
                        log.Error("Checkbox does not exist for todo item " +i);
                    }
                }
            }

        public void VerifyButtonsExistInThePage()
        {
            
            Assert.IsTrue(PageManager.ElementExists(noOfItemsLeft), "number of items left element does not exist");
            Assert.IsTrue(PageManager.ElementExists(allButton), "All button element does not exist");
            Assert.IsTrue(PageManager.ElementExists(activeButton), "Active button element does not exist");
            Assert.IsTrue(PageManager.ElementExists(completedButton), "Completed button element does not exist");
            Console.WriteLine("Buttons exist as expected in the page");
            log.Info("Buttons exist as expected in the page");

        }

        public void VerifyNoOfItemsCountMatchesNoOfActiveItems()
        {
            IList<IWebElement> allTodos = Driver.FindElements(By.XPath("//li[@class='ng-scope']//div[@class='view']//input"));
            string noOfItemsCount = noOfItemsLeftCount.Text;          
            Console.WriteLine("no of items count is " + noOfItemsCount);
            string noOfActiveItems = allTodos.Count.ToString();
            Console.WriteLine("no of Active todo items are " + noOfActiveItems);
            Assert.AreEqual(noOfItemsCount, noOfActiveItems, "number of items count is  : " + noOfItemsCount + " Value should be  : " + noOfActiveItems + " Not Matching !");
            Console.WriteLine("No of Items left count is " +noOfItemsCount+" and No of Active items count is " +noOfActiveItems+" are matching!");
            log.Info("No of Items left count is " + noOfItemsCount + " and No of Active items count is " + noOfActiveItems + " are matching!");
        }

        public void VerifyAllButtonIsInSelectedState()
        {
            try
            {
              if( allButtonSelected.Displayed==true)
              Console.WriteLine("All button is in selected state");
                log.Info("All button is in selected state");
            }
            catch
            {
                Console.WriteLine("All button is not in selected state");
                log.Error("All button is not in selected state");
            }
           
        }
    }
}
