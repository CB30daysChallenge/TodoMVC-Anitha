using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoMVC.PageObjectModels
{
    class CommonPageObjects : CBBase
    {
        public CommonPageObjects(String title = "")
                    : base(title)
        {
            PageFactory.InitElements(Driver, this);
        }

    }
}
