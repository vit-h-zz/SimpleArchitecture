using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using SimpleArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using SimpleArchitecture.WebUI;
using Xunit;

namespace WebUI.API.Tests.WebHost.Controllers
{

    public class WeatherForecastControllerTests
        //: IClassFixture<WebApplicationFactory<Startup>>
        : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public WeatherForecastControllerTests(
            //WebApplicationFactory<Startup> factory
            TestWebApplicationFactory<Startup> factory
            )
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ShouldReturnWeatherForecastData()
        {
            //Arrange 
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync("/api/WeatherForecast");

            //Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var actual = JsonConvert.DeserializeObject<List<WeatherForecast>>(await response.Content.ReadAsStringAsync());
            actual.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Get_ShouldReturnWeatherForecastData_V2()
        {
            //Arrange 
            var client = _factory.WithWebHostBuilder((builder) =>
            {
                builder.ConfigureTestServices(services =>
                {
                    //services.AddScoped<INotificationGateway, FakeNotificationGateway>();
                });
            }).CreateClient();

            //Act
            var response = await client.GetAsync("/api/WeatherForecast");

            //Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var actual = JsonConvert.DeserializeObject<List<WeatherForecast>>(await response.Content.ReadAsStringAsync());
            actual.Should().NotBeEmpty();
        }
    }
}