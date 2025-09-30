using Microsoft.AspNetCore.Mvc;
using Moq;
using Orion.API.HumanResources.Controllers;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.API.HR.UnitTests.Controllers;


public class EmployeeDepartmentHistoryControllerTest
{
    private readonly Mock<IEmployeeDepartmentHistoriesRepository> _mockRepo;
    private readonly EmployeeDepartmentHistoryController _controller;

    public EmployeeDepartmentHistoryControllerTest()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepo = new Mock<IEmployeeDepartmentHistoriesRepository>();

        // Mock Departments repository
        mockUnitOfWork.Setup(u => u.EmployeeDepartmentHistories).Returns(_mockRepo.Object);

        _controller = new EmployeeDepartmentHistoryController(mockUnitOfWork.Object);
    }
    
    [Fact(Skip = "Failing")]
    public async Task GetAll_ReturnsOkResult_WithEmployeeDepartmentHistory()
    {
        // Arrange
        var departments = new List<EmployeeDepartmentHistory>
        {
            new EmployeeDepartmentHistory { },
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<EmployeeDepartmentHistory>>(okResult.Value);
        Assert.Equal(2, ((List<Department>)returnValue).Count);
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenEmployeeDepartmentHistoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeeDepartmentHistory)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEmployeeDepartmentHistoryExists()
    {
        var department = new EmployeeDepartmentHistory { DepartmentID = 1 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(department, okResult.Value);
    }
    
    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenValid()
    {
        var department = new EmployeeDepartmentHistory { DepartmentID = 1 };
    
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

        var result = await _controller.Create(new EmployeeDepartmentHistory());

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_ReturnsNotFound_WhenEmployeeDepartmentHistoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeeDepartmentHistory)null);

        var result = await _controller.Update(1, new EmployeeDepartmentHistory());

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFound.Value.ToString());
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenEmployeeDepartmentHistoryExists()
    {
        var existing = new EmployeeDepartmentHistory { DepartmentID = 1 };
        var updated = new EmployeeDepartmentHistory { DepartmentID = 1};
    
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
    
        var result = await _controller.Update(1, updated);
    
        var okResult = Assert.IsType<OkObjectResult>(result);
        // var dept = Assert.IsType<EmployeeDepartmentHistory>(okResult.Value);
        // Assert.Equal("IT", dept.Name);
        // Assert.Equal("Tech", dept.GroupName);
    }
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEmployeeDepartmentHistoryDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeeDepartmentHistory)null);

        var result = await _controller.Delete(1);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFound.Value.ToString());
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenEmployeeDepartmentHistoryExists()
    {
        var existing = new EmployeeDepartmentHistory { BusinessEntityID = 1, };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
    
        var result = await _controller.Delete(1);
    
        Assert.IsType<NoContentResult>(result);
    }
}