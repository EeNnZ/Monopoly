using Microsoft.AspNetCore.Mvc;
using Monopoly.API;
using Monopoly.API.Controllers;
using Monopoly.API.Data.DTOs;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;
using Moq;

namespace Monopoly.TestsAPI;

public class BoxesControllerTests
{
    private readonly BoxesController         _controller;
    private readonly Mock<IDataService<Box>> _mockDataService;

    public BoxesControllerTests()
    {
        _mockDataService = new Mock<IDataService<Box>>();
        _controller      = new BoxesController(new MainDbContext(), _mockDataService.Object);
    }

    [Fact]
    public async Task GetBoxes_ReturnsOkResult_WhenBoxesExist()
    {
        // Arrange
        var boxes = new List<Box> { new() { Id = 1 }, new() { Id = 2 } };
        _mockDataService.Setup(service => service.GetAllAsync()).ReturnsAsync(boxes);

        // Act
        var result = await _controller.GetBoxes();

        // Assert
        var okResult = Assert.IsType<List<BoxDto>>(result.Value);
        Assert.Equal(2, okResult.Count);
    }

    [Fact]
    public async Task GetBoxes_ReturnsNotFound_WhenNoBoxesExist()
    {
        // Arrange
        _mockDataService.Setup(service => service.GetAllAsync()).ReturnsAsync((List<Box>?)null);

        // Act
        var result = await _controller.GetBoxes();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetBox_ReturnsOkResult_WhenBoxExists()
    {
        // Arrange
        var box = new Box { Id = 1 };
        _mockDataService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(box);

        // Act
        var result = await _controller.GetBox(1);

        // Assert
        var okResult = Assert.IsType<BoxDto>(result.Value);
        Assert.Equal(1, okResult.Id);
    }

    [Fact]
    public async Task GetBox_ReturnsNotFound_WhenBoxDoesNotExist()
    {
        // Arrange
        _mockDataService.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Box)null);

        // Act
        var result = await _controller.GetBox(10);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostBox_ReturnsCreatedResult_WhenBoxIsCreated()
    {
        // Arrange

        var newBoxDto = new BoxDto { Id = 1, DateCreated = DateOnly.FromDateTime(DateTime.Today) };
        var newBox    = new Box(newBoxDto);
        _mockDataService.Setup(service => service.Create(It.IsAny<Box>())).ReturnsAsync(newBox);

        // Act
        var result = await _controller.PostBox(newBoxDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetBox", createdResult.ActionName);
        Assert.Equal(1,        (long)(createdResult.RouteValues?["id"] ?? throw new InvalidOperationException()));
    }

    [Fact]
    public async Task PostBox_ReturnsProblemResult_WhenCreationFails()
    {
        // Arrange
        var newBoxDto = new BoxDto { Id = 1, DateCreated = DateOnly.FromDateTime(DateTime.Today) };
        _mockDataService.Setup(service => service.Create(It.IsAny<Box>())).ReturnsAsync((Box)null);

        // Act
        var result = await _controller.PostBox(newBoxDto);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, problemResult.StatusCode);
    }

    [Fact]
    public async Task DeleteBox_ReturnsNoContent_WhenBoxIsDeleted()
    {
        // Arrange
        _mockDataService.Setup(service => service.DeleteByIdAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteBox(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBox_ReturnsNotFound_WhenBoxDoesNotExist()
    {
        // Arrange
        _mockDataService.Setup(service => service.DeleteByIdAsync(It.IsAny<int>())).ReturnsAsync((bool?)null);

        // Act
        var result = await _controller.DeleteBox(10);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}