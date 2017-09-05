Feature: TodoMVC AngularJS
	
@AddNewItems
Scenario: Add items to list 
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
Then  I see checkbox in front of every todo item in the list
And   I see number of items left, All button, Active button, Completed button
And   the number of items left count should match the number of Active items
And   I see All button in selected state 
 
@MarkItemAsCompleted
Scenario: Mark an Item as completed
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
And  I click on checkbox of completed item
| Completeditem  |
| BDD Scenarios  |   
Then I see the completed item striked out
And  I see the checkbox ticked in green for completed item
And  I see number of items left, All button, Active button, Completed button
And  I see clear completed button populated
And  the number of items left count should match the number of Active items
And  I see Completed button in selected state

@DisplayAllItems 
Scenario: Display All todo items from the list
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
And   I click on All button to see both completed and Active items
Then  I see All items from the list
And   I see checkbox in front of every todo item in the list
And   I see number of items left, All button, Active button, Completed button
And   the number of items left count should match the number of Active items
And   I see All button in selected state


@DisplayActiveItems 
Scenario: Display Active todo items from the list
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
And   I click on Active button to see all active items
Then  I see only Active items from the list
And   I see checkbox in front of every todo item in the list
And   I see number of items left, All button, Active button, Completed button
And   the number of items left count should match the number of Active items
And   I see Active button in selected state
When  I click on Active item checkbox
Then  the item disappears from the list


@DisplayCompletedItems 
Scenario: Mark and display completed todo items from the list
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
And  I click on checkbox of completed item
| Completeditem  |
| BDD Scenarios |
And   I click on Completed button to see all completed items
Then  I see only completed items from the list
Then  I see the completed item striked out
And   I see the checkbox ticked in green for completed item
And   I see number of items left, All button, Active button, Completed button
And   I see clear completed button populated
And   the number of items left count should match the number of Active items
And   I see Completed button in selected state
When  I click on completed item checkbox
Then  the item disappears from the list


@ClearCompletedItems 
Scenario: Clear completed todo items from the list
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
And   I click on checkbox of completed item
| Completeditem |
| BDD Scenarios  |
Then  I see clear completed button populated
When  I click on Clear completed button populated
Then  I see completed items checkbox unticked
And   I see number of items left, All button, Active button, Completed button
And   the number of items left count should match the number of Active items
And   I do not see clear completed button
And   I see All button in selected state


@EditTodoItem
Scenario: Edit a todo item from the list
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
And   I double click on a todo item from the list
Then  I can edit a todo item 
And   I see number of items left, All button, Active button, Completed button
And   the number of items left count should match the number of Active items


@RemoveTodoItem
Scenario: Remove a todo item from the list
Given I am on todomvc website
When  I add items to the list
| todoitems     |
| Automation    |
| BDD Scenarios |
| Learn C#      |
And   I see New item added to the list
And   I tick completed items in the checkbox
And   I click on "x" next to todo item
Then  I see the todo item removed from the list 
And   I see number of items left, All button, Active button, Completed button
And   the number of items left count should match the number of Active items 


	
