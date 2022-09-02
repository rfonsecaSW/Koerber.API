using FluentAssertions;
using Koerber.API.Contracts;
using Koerber.API.Controlers;
using Koerber.API.Models;
using Koerber.UnitTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Koerber.UnitTests.Systems.Controllers;

public class TestTopZonesController
{
    #region Public Methods

    [Fact]
    public async Task GetTopZones_OnBadRequest_ReturnStatusCode400()
    {
        //Arrange
        Mock<IKoerberServices> mockKoerberServices = new Mock<IKoerberServices>();

        mockKoerberServices
            .Setup(service => service.GetTopZones(true))
            .ReturnsAsync(TopZonesFixtures.GetTestTopZones());

        TopZonesController topZonesController = new TopZonesController(mockKoerberServices.Object, new LoggerFactory().CreateLogger<TopZonesController>());

        //Act
        var result = (BadRequestObjectResult)await topZonesController.GetTopZones(String.Empty);

        //Assert
        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetTopZones_OnNotFound_ReturnStatusCode404()
    {
        //Arrange
        Mock<IKoerberServices> mockKoerberServices = new Mock<IKoerberServices>();

        mockKoerberServices
            .Setup(service => service.GetTopZones(true))
            .ReturnsAsync(new TopZonesOutput());

        TopZonesController topZonesController = new TopZonesController(mockKoerberServices.Object, new LoggerFactory().CreateLogger<TopZonesController>());

        //Act
        var result = (NotFoundResult)await topZonesController.GetTopZones("pickups");

        //Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetTopZones_OnSuccess_ReturnsListOfTopZones()
    {
        //Arrange
        Mock<IKoerberServices> mockKoerberServices = new Mock<IKoerberServices>();

        mockKoerberServices
            .Setup(service => service.GetTopZones(true))
            .ReturnsAsync(TopZonesFixtures.GetTestTopZones());

        TopZonesController topZonesController = new TopZonesController(mockKoerberServices.Object, new LoggerFactory().CreateLogger<TopZonesController>());

        //Act
        var result = (OkObjectResult)await topZonesController.GetTopZones("pickups");

        //Assert
        result.Value.Should().BeOfType<TopZonesOutput>();
    }

    [Fact]
    public async Task GetTopZones_OnSuccess_ReturnStatusCode200()
    {
        //Arrange
        Mock<IKoerberServices> mockKoerberServices = new Mock<IKoerberServices>();

        mockKoerberServices
            .Setup(service => service.GetTopZones(true))
            .ReturnsAsync(TopZonesFixtures.GetTestTopZones());

        TopZonesController topZonesController = new TopZonesController(mockKoerberServices.Object, new LoggerFactory().CreateLogger<TopZonesController>());

        //Act
        var result = (OkObjectResult)await topZonesController.GetTopZones("pickups");

        //Assert
        result.StatusCode.Should().Be(200);
    }

    #endregion Public Methods
}