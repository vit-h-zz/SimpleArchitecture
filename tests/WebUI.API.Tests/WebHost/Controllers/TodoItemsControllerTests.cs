using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SimpleArchitecture.Application.Common.Models;
using SimpleArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using SimpleArchitecture.Application.TodoLists.Commands.CreateTodoList;
using SimpleArchitecture.Application.TodoLists.Queries.GetTodos;
using SimpleArchitecture.WebUI;
using Xunit;

namespace WebUI.API.Tests.WebHost.Controllers
{

    public class TodoItemsControllerTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public TodoItemsControllerTests(
            WebApplicationFactory<Startup> factory
            )
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ShouldReturnWeatherForecastData()
        {
            //Arrange 
            var client = _factory.CreateClient();

            //TODO: Implement authorization or comment Auth attributes 

            var listResponse = await client.PostAsJsonAsync("/api/TodoLists", new CreateTodoListCommand
            {
                Title = "New List " + Guid.NewGuid()
            });

            var listId = int.Parse(await listResponse.Content.ReadAsStringAsync());
            var command = new CreateTodoItemCommand
            {
                ListId = listId,
                Title = "Test Task " + Guid.NewGuid()
            };

            //Act
            var itemResponse = await client.PostAsJsonAsync("/api/TodoItems", command);

            //Assert
            itemResponse.IsSuccessStatusCode.Should().BeTrue();
            itemResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var itemsString = await client.GetStringAsync($"/api/TodoItems?ListId={listId}");
            var actual = JsonConvert.DeserializeObject<PaginatedList<TodoItemDto>>(itemsString);

            actual.Items.Should().NotBeEmpty();
            actual.Items.Should().HaveCount(1);
            actual.Items.Should().Contain(i => i.Title == command.Title);
        }
    }
}