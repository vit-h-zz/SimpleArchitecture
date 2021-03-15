using System;
using Application.Specs.Hooks;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using SimpleArchitecture.Application.Common.Exceptions;
using SimpleArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using SimpleArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using SimpleArchitecture.Application.TodoItems.Commands.UpdateTodoItem;
using SimpleArchitecture.Application.TodoLists.Commands.CreateTodoList;
using SimpleArchitecture.Application.TodoLists.Commands.DeleteTodoList;
using SimpleArchitecture.Domain.Entities;
using TechTalk.SpecFlow;

namespace Application.Specs.Steps
{
    [Binding]
    public class TodoListSteps
    {
        private string _userId;
        private int _listId;
        private int _itemId;
        private CreateTodoListCommand _createTodoListCommand;
        private CreateTodoItemCommand _createTodoItemCommand;
        private Func<Task<Unit>> _listDeletionAction;
        private Task<int> _listCreationTask;
        private UpdateTodoItemCommand _updateTodoItemCommand;

        [Given(@"the particular user logged in")]
        public async Task GivenTheParticularUserLoggedIn()
        {
            _userId = await AppHooks.RunAsDefaultUserAsync();
        }

        [Given(@"the user creates a new list with name '(.*)'")]
        [When(@"the user creates a new list with name '(.*)'")]
        public void WhenTheUserCreatesANewListWithName(string listTitle)
        {
            _createTodoListCommand = new CreateTodoListCommand
            {
                Title = listTitle
            };

            _listCreationTask = AppHooks.SendAsync(_createTodoListCommand);
        }

        [When(@"the user deletes an existing list")]
        public async Task WhenTheUserDeletesAnExistingList()
        {
            _listId = await _listCreationTask;
            await AppHooks.SendAsync(new DeleteTodoListCommand
            {
                Id = _listId
            });
        }

        [When(@"the user deletes not existing list")]
        public async Task WhenTheUserDeletesNotExistingList()
        {
            _listId = _listCreationTask == null ? 0 : (await _listCreationTask);
            _listDeletionAction = async () => await AppHooks.SendAsync(new DeleteTodoListCommand
            {
                Id = _listId
            });
        }

        [Given("the system have new list created")]
        [Then(@"the system should have new list created")]
        public async Task ThenTheSystemShouldHaveNewListCreated()
        {
            _listId = await _listCreationTask;
            var list = await AppHooks.FindAsync<TodoList>(_listId);

            list.Should().NotBeNull();
            list.Title.Should().Be(_createTodoListCommand.Title);
            list.CreatedBy.Should().Be(_userId);
            list.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }

        [Then(@"the system should respond with the validation exception for list")]
        public void ThenTheSystemShouldRespondWithTheValidationExceptionForList()
        {
            FluentActions.Invoking(async () => await _listCreationTask)
                .Should().Throw<ValidationException>();
        }

        [Then(@"the deleted list must not exist in the system")]
        public async Task ThenTheDeletedListMustNotExistInTheSystem()
        {
            var list = await AppHooks.FindAsync<TodoList>(_listId);

            list.Should().BeNull();
        }

        [Then(@"the system should respond with the not found exception")]
        public void ThenTheSystemShouldRespondWithTheNotFoundException()
        {
            FluentActions.Invoking(_listDeletionAction)
                .Should().Throw<NotFoundException>();
        }

        [Given(@"the list with name '(.*)' exists")]
        public async Task GivenTheListWithNameExists(string listTitle)
        {
            await GivenTheParticularUserLoggedIn();
            WhenTheUserCreatesANewListWithName(listTitle);
            await ThenTheSystemShouldHaveNewListCreated();
        }

        [Given(@"the user creates a new item in the list with the name '(.*)'")]
        [When(@"the user creates a new item in the list with the name '(.*)'")]
        public async Task WhenTheUserCreatesANewItemInTheList(string itemTitle)
        {
            _createTodoItemCommand = new CreateTodoItemCommand
            {
                ListId = _listId,
                Title = itemTitle
            };

            _itemId = await AppHooks.SendAsync(_createTodoItemCommand);
        }

        [When(@"the user deletes an existing item")]
        public async Task WhenTheUserDeletesAnExistingItem()
        {
            await AppHooks.SendAsync(new DeleteTodoItemCommand
            {
                Id = _itemId
            });
        }


        [Then(@"the system should have new item created in the list")]
        public async Task ThenTheSystemShouldHaveNewItemCreatedInTheList()
        {
            var item = await AppHooks.FindAsync<TodoItem>(_itemId);

            item.Should().NotBeNull();
            item.ListId.Should().Be(_createTodoItemCommand.ListId);
            item.Title.Should().Be(_createTodoItemCommand.Title);
            item.CreatedBy.Should().Be(_userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 10000);
            item.LastModifiedBy.Should().BeNull();
            item.LastModified.Should().BeNull();
        }

        [Then(@"the system should respond with the validation exception for empty item creation")]
        public void ThenTheSystemShouldRespondWithTheValidationExceptionForEmptyItemCreation()
        {
            FluentActions.Invoking(async () => await AppHooks.SendAsync(new CreateTodoItemCommand()))
                .Should().Throw<ValidationException>();
        }

        [Then(@"the item must not exist in the system")]
        public async Task ThenTheItemMustNotExistInTheSystem()
        {
            var list = await AppHooks.FindAsync<TodoItem>(_itemId);

            list.Should().BeNull();
        }

        [When(@"the user updates an existing item with the name '(.*)'")]
        public async Task WhenTheUserUpdatesAnExistingItemWithTheName(string p0)
        {
            _updateTodoItemCommand = new UpdateTodoItemCommand
            {
                Id = _itemId,
                Title = "Updated Item Title"
            };

            await AppHooks.SendAsync(_updateTodoItemCommand);
        }

        [Then(@"the system should have item correctly updated")]
        public async Task ThenTheSystemShouldHaveItemCorrectlyUpdated()
        {
            var item = await AppHooks.FindAsync<TodoItem>(_itemId);

            item.Title.Should().Be(_updateTodoItemCommand.Title);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(_userId);
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
