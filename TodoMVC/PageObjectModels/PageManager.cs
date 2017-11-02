using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;

namespace TodoMVC.PageObjectModels
{
    class PageManager : CBBase
    {
        public PageManager(string title = "")
            : base(title)
        {
            PageFactory.InitElements(Driver, this);
        }
        public TodoMVCPage todoMVCPage = new TodoMVCPage();
    }
}
