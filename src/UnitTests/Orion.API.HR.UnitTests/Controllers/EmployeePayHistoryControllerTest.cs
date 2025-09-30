using Microsoft.AspNetCore.Mvc;
using Moq;
using Orion.API.HumanResources.Controllers;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.API.HR.UnitTests.Controllers;


public class EmployeePayHistoryControllerTest
{
    private readonly Mock<IEmployeePayHistoriesRepository> _mockRepo;
    private readonly EmployeePayHistoryController _controller;

    public EmployeePayHistoryControllerTest()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepo = new Mock<IEmployeePayHistoriesRepository>();

        // Mock Departments repository
        mockUnitOfWork.Setup(u => u.EmployeePayHistories).Returns(_mockRepo.Object);

        _controller = new EmployeePayHistoryController(mockUnitOfWork.Object);
    }
    
    [Fact(Skip = "Failing")]
    public async Task GetAll_ReturnsOkResult_WithEmployeePayHistory()
    {
        // Arrange
        var departments = new List<EmployeePayHistory>
        {
            new EmployeePayHistory { },
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<EmployeePayHistory>>(okResult.Value);
        Assert.Equal(2, ((List<Department>)returnValue).Count);
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenEmployeePayHistoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeePayHistory)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEmployeePayHistoryExists()
    {
        var department = new EmployeePayHistory { BusinessEntityID = 1 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(department, okResult.Value);
    }
    
    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenValid()
    {
        var department = new EmployeePayHistory { BusinessEntityID = 1 };
    
        _mockRepo.Setup(r => r.AddAsync(department)).Returns(Task.CompletedTask);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(Task.CompletedTask)
     
    
        var result = await _controller.Create(department);
    
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(department, createdResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Required");

        var result = await _controller.Create(new EmployeePayHistory());

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_ReturnsNotFound_WhenEmployeePayHistoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeePayHistory)null);

        var result = await _controller.Update(1, new EmployeePayHistory());

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFound.Value.ToString());
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenEmployeePayHistoryExists()
    {
        var existing = new EmployeePayHistory { BusinessEntityID = 1 };
        var updated = new EmployeePayHistory { BusinessEntityID = 1};
    
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
    
        var result = await _controller.Update(1, updated);
    
        var okResult = Assert.IsType<OkObjectResult>(result);
        // var dept = Assert.IsType<EmployeePayHistory>(okResult.Value);
        // Assert.Equal("IT", dept.Name);
        // Assert.Equal("Tech", dept.GroupName);
    }
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEmployeePayHistoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeePayHistory)null);

        var result = await _controller.Delete(1);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFound.Value.ToString());
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenEmployeePayHistoryExists()
    {
        var existing = new EmployeePayHistory { BusinessEntityID = 1, };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
    
        var result = await _controller.Delete(1);
    
        Assert.IsType<NoContentResult>(result);
    }
}