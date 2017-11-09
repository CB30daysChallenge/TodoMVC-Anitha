using Framework.DBhelper;
using System;
using TechTalk.SpecFlow;
using TodoMVC.PageObjectModels;


namespace TodoMVC.Features
{
    [Binding]
    public class TodoMVCSteps
    {
        TodoMVCPage todoMVCPage = new TodoMVCPage();
        [Given(@"I am on todomvc website")]
        public void GivenIAmOnTodomvcWebsite()
        {
            
            todoMVCPage.LaunchTodoMVCPage();
        
        }

        [When(@"I add items to the list")]
        public void WhenIAddItemsToTheList(Table table)
        {
            todoMVCPage.AddItemsToTheList(table);
        }

        [When(@"I click on checkbox of completed item")]
        public void WhenIClickOnCheckboxOfCompletedItem(Table table)
        {

        }

        [When(@"I click on All button to see both completed and Active items")]
        public void WhenIClickOnAllButtonToSeeBothCompletedAndActiveItems()
        {

        }

        [When(@"I click on Active button to see all active items")]
        public void WhenIClickOnActiveButtonToSeeAllActiveItems()
        {

        }

        [When(@"I click on Active item checkbox")]
        public void WhenIClickOnActiveItemCheckbox()
        {

        }

        [When(@"I click on Completed button to see all completed items")]
        public void WhenIClickOnCompletedButtonToSeeAllCompletedItems()
        {

        }

        [When(@"I click on completed item checkbox")]
        public void WhenIClickOnCompletedItemCheckbox()
        {

        }

        [When(@"I click on Clear completed button populated")]
        public void WhenIClickOnClearCompletedButtonPopulated()
        {

        }

        [When(@"I double click on a todo item from the list")]
        public void WhenIDoubleClickOnATodoItemFromTheList()
        {

        }

        [When(@"I see New item added to the list")]
        public void WhenISeeNewItemAddedToTheList()
        {

        }

        [When(@"I tick completed items in the checkbox")]
        public void WhenITickCompletedItemsInTheCheckbox()
        {

        }

        [When(@"I click on ""(.*)"" next to todo item")]
        public void WhenIClickOnNextToTodoItem(string p0)
        {

        }

        [Then(@"I see checkbox in front of every todo item in the list")]
        public void ThenISeeCheckboxInFrontOfEveryTodoItemInTheList()
        {
            todoMVCPage.VerifyCheckboxExistsForEveryTodoItem();
        }

        [Then(@"I see the completed item striked out")]
        public void ThenISeeTheCompletedItemStrikedOut()
        {
          
        }

        [Then(@"I see number of items left, All button, Active button, Completed button")]
        public void ThenISeeNumberOfItemsLeftAllButtonActiveButtonCompletedButton()
        {
            todoMVCPage.VerifyButtonsExistInThePage();
        }

        [Then(@"the number of items left count should match the number of Active items")]
        public void ThenTheNumberOfItemsLeftCountShouldMatchTheNumberOfActiveItems()
        {
            todoMVCPage.VerifyNoOfItemsCountMatchesNoOfActiveItems();
        }

        [Then(@"I see All button in selected state")]
        public void ThenISeeAllButtonInSelectedState()
        {
            todoMVCPage.VerifyAllButtonIsInSelectedState();
        }

        [Then(@"I see the checkbox ticked in green for completed item")]
        public void ThenISeeTheCheckboxTickedInGreenForCompletedItem()
        {
           
        }

        [Then(@"I see clear completed button populated")]
        public void ThenISeeClearCompletedButtonPopulated()
        {
           
        }

        [Then(@"I see Completed button in selected state")]
        public void ThenISeeCompletedButtonInSelectedState()
        {

        }

        [Then(@"I see All items from the list")]
        public void ThenISeeAllItemsFromTheList()
        {
            
        }

        [Then(@"I see only Active items from the list")]
        public void ThenISeeOnlyActiveItemsFromTheList()
        {
           
        }

        [Then(@"I see Active button in selected state")]
        public void ThenISeeActiveButtonInSelectedState()
        {
           
        }

        [Then(@"the item disappears from the list")]
        public void ThenTheItemDisappearsFromTheList()
        {
          
        }

        [Then(@"I see only completed items from the list")]
        public void ThenISeeOnlyCompletedItemsFromTheList()
        {
            
        }

        [Then(@"I see completed items checkbox unticked")]
        public void ThenISeeCompletedItemsCheckboxUnticked()
        {
           
        }

        [Then(@"I do not see clear completed button")]
        public void ThenIDoNotSeeClearCompletedButton()
        {
           
        }

        [Then(@"I can edit a todo item")]
        public void ThenICanEditATodoItem()
        {
          
        }

        [Then(@"I see the todo item removed from the list")]
        public void ThenISeeTheTodoItemRemovedFromTheList()
        {
          
        }

    }
}
