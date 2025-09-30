// using System.Text.Json;
// using JetBrains.Annotations;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Orion.API.TradingEconomics.Controllers;
// using Orion.Domain.IRepositories;
// using Shouldly;
// using Xunit;
//
// namespace Orion.API.TradingEcomomics.UnitTests.Controllers;
//
// [TestSubject(typeof(ForecastController))]
// public class ForecastControllerTest
// {
//
//     private readonly Mock<IForecastServices> _serviceMock;
//     private readonly ForecastController _controller;
//
//     public ForecastControllerTest()
//     {
//         _serviceMock = new Mock<IForecastServices>();
//         _controller = new ForecastController(_serviceMock.Object);
//     }
//
//     [Fact]
//     public async Task GetAllEvents_ReturnsOk_WhenDeserializationSuccessful()
//     {
//         // Arrange
//         var mockData = JsonSerializer.Serialize(new List<Forecast>
//         {
//             new Forecast { CreateBy = "Event 1", Country = "1" }
//         });
//
//         _serviceMock.Setup(s => s.GetForecasts()).ReturnsAsync(mockData);
//
//         // Act
//         var result = await _controller.GetAllForecasts();
//
//         // Assert
//         result.ShouldBeOfType<OkObjectResult>();
//         var okResult = result as OkObjectResult;
//         okResult!.Value.ShouldBe(mockData);
//     }
//
//     [Fact]
//     public async Task GetAllEvents_ReturnsBadRequest_OnInvalidJson()
//     {
//         // Arrange
//         var invalidJson = "INVALID_JSON";
//
//         _serviceMock.Setup(s => s.GetForecasts()).ReturnsAsync(invalidJson);
//
//         // Act
//         var result = await _controller.GetAllForecasts();
//
//         // Assert
//         result.ShouldBeOfType<BadRequestObjectResult>();
//     }
//
//     [Fact]
//     public async Task GetAllEvents_ReturnsBadRequest_WhenNoEvents()
//     {
//         // Arrange
//         var emptyListJson = JsonSerializer.Serialize(new List<Forecast>());
//         _serviceMock.Setup(s => s.GetForecasts()).ReturnsAsync(emptyListJson);
//
//         // Act
//         var result = await _controller.GetAllForecasts();
//
//         // Assert
//         result.ShouldBeOfType<BadRequestObjectResult>();
//     }
//
//     [Fact]
//     public async Task GetEventsByDate_ReturnsOk()
//     {
//         // Arrange
//         var expected = "sample json result";
//         var start = new DateTime(2025, 7, 1);
//         var end = new DateTime(2025, 7, 31);
//         _serviceMock.Setup(s => s.GetForecastsByDate(start, end)).ReturnsAsync(expected);
//
//         // Act
//         var result = await _controller.GetEventsByDate(start, end);
//
//         // Assert
//         result.ShouldBeOfType<OkObjectResult>().Value.ShouldBe(expected);
//     }
//
//     [Fact]
//     public async Task GetEventsByCountries_ReturnsOk()
//     {
//         var expected = "countries result";
//         var countries = new[] { "USA", "Germany" };
//         _serviceMock.Setup(s => s.GetForecastsByCountries(countries)).ReturnsAsync(expected);
//
//         var result = await _controller.GetEventsByCountries(countries);
//
//         result.ShouldBeOfType<OkObjectResult>().Value.ShouldBe(expected);
//     }
//
//     [Fact]
//     public async Task GetEventsByCountriesAndDates_ReturnsOk()
//     {
//         var expected = "combined result";
//         var countries = new[] { "South Africa", "Canada" };
//         var start = new DateTime(2025, 7, 1);
//         var end = new DateTime(2025, 7, 10);
//
//         _serviceMock
//             .Setup(s => s.GetForecastsByCountriesAndDates(start, end, countries))
//             .ReturnsAsync(expected);
//
//         var result = await _controller.GetEventsByCountriesAndDates(start, end, countries);
//
//         result.ShouldBeOfType<OkObjectResult>().Value.ShouldBe(expected);
//     }
//
//     [Fact]
//     public async Task GetEventsByIndicators_ReturnsOk()
//     {
//         var expected = "indicators result";
//         var indicators = new[] { "GDP", "Inflation" };
//
//         _serviceMock
//             .Setup(s => s.GetForecastsByIndicator(indicators))
//             .ReturnsAsync(expected);
//
//         var result = await _controller.GetEventsByIndicators(indicators);
//
//         result.ShouldBeOfType<OkObjectResult>().Value.ShouldBe(expected);
//     }
// }