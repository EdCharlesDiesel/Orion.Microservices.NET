using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orion.API.Person.Controllers;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;
using Xunit;

namespace Orion.API.Person.UnitTests.Controllers;

[TestSubject(typeof(AddressController))]
public class AddressControllerTests
{

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAddressesRepository> _mockRepo;
    private readonly AddressController _controller;

    public AddressControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepo = new Mock<IAddressesRepository>();

        // Mock Addresss repository
        _mockUnitOfWork.Setup(u => u.Addresses).Returns(_mockRepo.Object);

        _controller = new AddressController(_mockUnitOfWork.Object);
    }
    
    [Fact]
    public async Task GetAll_ReturnsOkResult_WithAddresss()
    {
        // Arrange
        var Addresss = new List<Address>
        {
            new Address { AddressID = 1, },
            new Address { AddressID = 2, }
        };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(Addresss);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Address>>(okResult.Value);
        Assert.Equal(2, ((List<Address>)returnValue).Count);
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Address)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenAddressExists()
    {
        var Address = new Address { AddressID = 1};
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(Address);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(Address, okResult.Value);
    }
    
    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenValid()
    {
        var Address = new Address { AddressID = 1,};
    
        _mockRepo.Setup(r => r.AddAsync(Address)).Returns(Task.CompletedTask);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(Task.CompletedTask)
     
    
        var result = await _controller.Create(Address);
    
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(Address, createdResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelInvalid()
    {
        _controller.ModelState.AddModelError("Name", "Required");

        var result = await _controller.Create(new Address());

        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Address)null);

        var result = await _controller.Update(1, new Address());

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFound.Value.ToString());
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenAddressExists()
    {
        var existing = new Address { AddressID = 1, };
        var updated = new Address { AddressID = 1, };
    
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
    
        var result = await _controller.Update(1, updated);
    
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dept = Assert.IsType<Address>(okResult.Value);
        // Assert.Equal("IT", dept.Name);
        // Assert.Equal("Tech", dept.GroupName);
    }
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Address)null);

        var result = await _controller.Delete(1);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFound.Value.ToString());
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenAddressExists()
    {
        var existing = new Address { AddressID = 1};
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        // _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
    
        var result = await _controller.Delete(1);
    
        Assert.IsType<NoContentResult>(result);
    }
}