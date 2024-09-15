using Microsoft.AspNetCore.Mvc;
using Monopoly.API.Controllers;
using Monopoly.API.Data.DTOs;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;
using Moq;

namespace Monopoly.API.Tests;

public class PalletsControllerTests
{
    private readonly PalletsController          _controller;
    private readonly Mock<IDataService<Pallet>> _palletServiceMock;

    public PalletsControllerTests()
    {
        _palletServiceMock = new Mock<IDataService<Pallet>>();
        _controller        = new PalletsController(Mock.Of<MainDbContext>(), _palletServiceMock.Object);
    }

    [Fact]
    public async Task GetPallets_ReturnsPallets_WhenPalletsExist()
    {
        // Arrange
        var pallets = new List<Pallet>
        {
            new() { Id = 1 /* other properties */ },
            new() { Id = 2 /* other properties */ }
        };
        _palletServiceMock.Setup(service => service.GetAllAsync())
                          .ReturnsAsync(pallets);

        // Act
        var result = await _controller.GetPallets();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<PalletDto>>>(result);
        var okResult     = Assert.IsType<List<PalletDto>>(actionResult.Value);
        Assert.Equal(2, okResult.Count);
    }

    [Fact]
    public async Task GetPallets_ReturnsNotFound_WhenNoPalletsExist()
    {
        // Arrange
        _palletServiceMock.Setup(service => service.GetAllAsync())
                          .ReturnsAsync((IEnumerable<Pallet>?)null);

        // Act
        var result = await _controller.GetPallets();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPallet_ReturnsPallet_WhenPalletExists()
    {
        // Arrange
        var pallet = new Pallet { Id = 1 /* other properties */ };
        _palletServiceMock.Setup(service => service.GetByIdAsync(1))
                          .ReturnsAsync(pallet);

        // Act
        var result = await _controller.GetPallet(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PalletDto>>(result);
        var okResult     = Assert.IsType<PalletDto>(actionResult.Value);
        Assert.Equal(1, okResult.Id);
    }

    [Fact]
    public async Task GetPallet_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Arrange
        _palletServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                          .ReturnsAsync((Pallet?)null);

        // Act
        var result = await _controller.GetPallet(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostPallet_ReturnsCreated_WhenPalletIsCreated()
    {
        // Arrange
        var newPalletDto = new PalletDto { Id = 1 /* other properties */ };
        _palletServiceMock.Setup(service => service.Create(It.IsAny<Pallet>()))
                          .ReturnsAsync(new Pallet { Id = 1 /* other properties */ });

        // Act
        var result = await _controller.PostPallet(newPalletDto);

        // Assert
        var actionResult  = Assert.IsType<ActionResult<PalletDto>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal("GetPallet", createdResult.ActionName);
        Assert.Equal(1,           (long)(createdResult.RouteValues?["id"] ?? throw new InvalidOperationException()));
    }

    [Fact]
    public async Task PostPallet_ReturnsProblem_WhenEntitySetIsNull()
    {
        // Arrange
        var newPalletDto = new PalletDto { Id = 1 /* other properties */ };
        _palletServiceMock.Setup(service => service.Create(It.IsAny<Pallet>()))
                          .ReturnsAsync((Pallet?)null);

        // Act
        var result = await _controller.PostPallet(newPalletDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PalletDto>>(result);
        Assert.IsType<ObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task DeletePallet_ReturnsNoContent_WhenSuccessfullyDeleted()
    {
        // Arrange
        _palletServiceMock.Setup(service => service.DeleteByIdAsync(1))
                          .ReturnsAsync(true);

        // Act
        var result = await _controller.DeletePallet(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePallet_ReturnsNotFound_WhenPalletDoesNotExist()
    {
        // Arrange
        _palletServiceMock.Setup(service => service.DeleteByIdAsync(1))
                          .ReturnsAsync((bool?)null);

        // Act
        var result = await _controller.DeletePallet(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}