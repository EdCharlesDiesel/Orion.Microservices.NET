// using JetBrains.Annotations;
// using Moq;
// using Orion.API.TradingEconomics.Controllers;
// using Orion.Domain.IRepositories;
// using Xunit;
//
// namespace Orion.API.TradingEcomomics.UnitTests.Controllers;
//
// [TestSubject(typeof(ComtradeController))]
// public class ComtradeControllerTest
// {
//     private readonly Mock<IComtradeServices> _serviceMock;
//     private readonly ComtradeController _controller;
//
//     public ComtradeControllerTest()
//     {
//         _serviceMock = new Mock<IComtradeServices>();
//         _controller = new ComtradeController(_serviceMock.Object);
//     }
//     //TODO: Fix this.
//     [Fact(Skip = "Fill fix just now")]
//     public async Task GetComCategories_ReturnsOkResult_WithData()
//     {
//         throw new NotImplementedException();
//         // Arrange
//         // var expected = new List<string> { "Category1", "Category2" };
//         // _serviceMock.Setup(s => s.GetCategories()).ReturnsAsync(expected);
//         //
//         // // Act
//         // var result = await _controller.GetComCategories();
//         //
//         // // Assert
//         // var ok = Assert.IsType<OkObjectResult>(result);
//         // Assert.Equal(expected, ok.Value);
//     }
//
//     //TODO: Fix this.
//     [Fact(Skip = "Fill fix just now")]
//     public async Task GetComCountries_ReturnsOkResult_WithData()
//     {
//         // var expected = new List<string> { "South Africa", "USA" };
//         // _serviceMock.Setup(s => s.GetCountries()).ReturnsAsync(expected);
//         //
//         // var result = await _controller.GetComCountries();
//         //
//         // var ok = Assert.IsType<OkObjectResult>(result);
//         // Assert.Equal(expected, ok.Value);
//    
//     }
//
//     //TODO: Fix this.
//     [Fact(Skip = "Fill fix just now")]
//     public async Task GetComCountryPage_ReturnsOkResult_WithData()
//     {
//         // var country = "South Africa";
//         // var page = 1;
//         // var expected = new List<string> { "Trade1", "Trade2" };
//         //
//         // _serviceMock.Setup(s => s.GetByCountryAndPage(country, page)).ReturnsAsync(expected);
//         //
//         // var result = await _controller.GetComCountryPage(country, page);
//         //
//         // var ok = Assert.IsType<OkObjectResult>(result);
//         // Assert.Equal(expected, ok.Value);
//     }
//
//     //TODO: Fix this.
//     [Fact(Skip = "Fill fix just now")]
//     public async Task GetComBetweenCountries_ReturnsOkResult_WithData()
//     {
//         // var country1 = "South Africa";
//         // var country2 = "China";
//         // var page = 2;
//         // var expected = new List<string> { "TradeX", "TradeY" };
//         //
//         // _serviceMock.Setup(s => s.GetBetweenCountries(country1, country2, page)).ReturnsAsync(expected);
//         //
//         // var result = await _controller.GetComBetweenCountries(country1, country2, page);
//         //
//         // var ok = Assert.IsType<OkObjectResult>(result);
//         // Assert.Equal(expected, ok.Value);
//     }
//
//     //TODO: Fix this.
//     [Fact(Skip = "Fill fix just now")]
//     public async Task GetComHistorical_ReturnsOkResult_WithData()
//     {
//         // var symbol = "ZARUSD";
//         // var expected = new List<string> { "2001", "2002", "2003" };
//         //
//         // _serviceMock.Setup(s => s.GetHistorical(symbol)).ReturnsAsync(expected);
//         //
//         // var result = await _controller.GetComHistorical(symbol);
//         //
//         // var ok = Assert.IsType<OkObjectResult>(result);
//         // Assert.Equal(expected, ok.Value);
//     }
// }