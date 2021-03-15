Feature: TodoList
Todo lists is a key functionality for this domain and used for storing new and compleated tasks

Link to a feature: [TodoList](Application.Specs/Features/TodoList.feature)

@listCreation
Scenario: Should create new TodoList
	Given the particular user logged in
	When the user creates a new list with name 'Test List'
	Then the system should have new list created

@listCreation
Scenario: Should require mandatory fields for TodoList creation
	Given the particular user logged in
	When the user creates a new list with name ''
	Then the system should respond with the validation exception for list

@listCreation
Scenario: Should require unique title for TodoList creation
	Given the particular user logged in
	And the user creates a new list with name 'Duplicate'
	When the user creates a new list with name 'Duplicate'
	Then the system should respond with the validation exception for list

@listDeletion
Scenario: Should delete TodoList
	Given the particular user logged in
	And the user creates a new list with name 'Test List'
	When the user deletes an existing list
	Then the deleted list must not exist in the system

@listDeletion
Scenario: Should require existing TodoList to be deleted
	Given the particular user logged in
	When the user deletes not existing list
	Then the system should respond with the not found exception

@ItemCreation
Scenario: Should create new TodoItem in the TodoList
	Given the list with name 'Test List' exists
	When the user creates a new item in the list with the name 'Test title'
	Then the system should have new item created in the list

@ItemCreation
Scenario: Should require mandatory fields for TodoItem creation
	Then the system should respond with the validation exception for empty item creation

@ItemDeletion
Scenario: Should delete TodoItem
	Given the list with name 'Test List' exists
	And the user creates a new item in the list with the name 'Test title'
	When the user deletes an existing item
	Then the item must not exist in the system

@ItemCreation
Scenario: Should update TodoItem
	Given the particular user logged in
	And the user creates a new list with name 'Test List'
	And the system have new list created
	And the user creates a new item in the list with the name 'Test Item'
	When the user updates an existing item with the name 'New title'
	Then the system should have item correctly updated