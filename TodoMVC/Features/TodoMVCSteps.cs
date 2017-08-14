using System;
using TechTalk.SpecFlow;

namespace TodoMVC.Features
{
    [Binding]
    public class TodoMVCSteps
    {
        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            SQLServer.ExecuteQuery("", "");
        }
        
        [When(@"I press add")]
        public void WhenIPressAdd()
        {
          
        }
        
        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            
        }
    }
}
