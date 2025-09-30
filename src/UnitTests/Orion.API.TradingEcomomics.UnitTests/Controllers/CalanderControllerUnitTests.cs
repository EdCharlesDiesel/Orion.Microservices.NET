// using System.Text.Json;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Orion.API.BuildVersion.Controllers;
// using Orion.DataAccess.Postgres.Entities;
// using Orion.DataAccess.Postgres.Entities.Common;
// using Shouldly;
// using Xunit;
//
// namespace Orion.API.TradingEconomics.UnitTests.Controllers;
//
// public class CalendarControllerTests
// {
//     private readonly Mock<ICalendarServices> _serviceMock;
//     private readonly CalendarController _controller;
//
//     public CalendarControllerTests()
//     {
//         _serviceMock = new Mock<ICalendarServices>();
//         _controller = new CalendarController(_serviceMock.Object);
//     }
//
//     [Fact]
//     public async Task GetAllEvents_ReturnsOk_WhenDeserializationSuccessful()
//     {
//         // Arrange
//         var mockData = JsonSerializer.Serialize(new List<TradingEconomicsCalendar>
//         {
//             new TradingEconomicsCalendar { CalendarId = "Event 1", Importance = 1 }
//         });
//
//         _serviceMock.Setup(s => s.GetCalendarEvents()).ReturnsAsync(mockData);
//
//         // Act
//         var result = await _controller.GetAllEvents();
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
//         _serviceMock.Setup(s => s.GetCalendarEvents()).ReturnsAsync(invalidJson);
//
//         // Act
//         var result = await _controller.GetAllEvents();
//
//         // Assert
//         result.ShouldBeOfType<BadRequestObjectResult>();
//     }
//
//     [Fact]
//     public async Task GetAllEvents_ReturnsBadRequest_WhenNoEvents()
//     {
//         // Arrange
//         var emptyListJson = JsonSerializer.Serialize(new List<TradingEconomicsCalendar>());
//         _serviceMock.Setup(s => s.GetCalendarEvents()).ReturnsAsync(emptyListJson);
//
//         // Act
//         var result = await _controller.GetAllEvents();
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
//         _serviceMock.Setup(s => s.GetCalendarEventsByDate(start, end)).ReturnsAsync(expected);
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
//         _serviceMock.Setup(s => s.GetCalendarEventsByCountries(countries)).ReturnsAsync(expected);
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
//             .Setup(s => s.GetCalendarEventsByCountriesAndDates(start, end, countries))
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
//             .Setup(s => s.GetCalendarEventsByIndicator(indicators))
//             .Equals(expected);
//     
//         var result = await _controller.GetEventsByIndicators(indicators);
//     
//         result.ShouldBeOfType<OkObjectResult>().Value.ShouldBe(expected);
//     }
// }
//
