namespace ECA.DiamondKata.ApiTests;

using Api.Controllers;
using BusinessLayer;
using BusinessLayer.Abstract;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ViewModel.Request;
using ViewModel.Response;

public class DiamondKataControllerTests
{
    private readonly Mock<IDiamondKatanaService> _mockDiamondKatanaService;
    private readonly DiamondKataController _diamondKataController;
    public DiamondKataControllerTests()
    {
        _mockDiamondKatanaService = new Mock<IDiamondKatanaService>();
        _diamondKataController = new DiamondKataController(_mockDiamondKatanaService.Object);
    }

    [Fact]
    public void Generate_OnNullRequest_ReturnsStatusCode400()
    {
        //Arrange 
        
        //Act
        var act = () => _diamondKataController.GenerateDiamond(null);
        
        //Assert
        act.Should().Throw<ValidationException>().WithMessage("Request is not valid.");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("AA")]
    public void Generate_OnRequestWithInvalidCharacter_ReturnsStatusCode400(string? character)
    {
        //Arrange 
        var request = new GenerateRequestViewModel
        {
            Character = character
        };
        
        //Act
        var act = () => _diamondKataController.GenerateDiamond(request);
        
        //Assert
        act.Should().Throw<ValidationException>().WithMessage("Request is not valid.");
    }


    [Fact]
    public void Generate_Onsuccess_ReturnsStatusCode200()
    {
        //Arrange 
        var request = new GenerateRequestViewModel { Character = "A" };
        
        //Act
        var result = (OkObjectResult)_diamondKataController.GenerateDiamond(request);
        
        //Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public void Generate_Onsuccess_CallsService()
    {
        //Arrange 
        var request = new GenerateRequestViewModel { Character = "A" };
        _mockDiamondKatanaService.Setup(x => x.GenerateDiamond(It.IsAny<char>())).Verifiable();
        
        //Act
        _diamondKataController.GenerateDiamond(request);
        
        //Assert
        _mockDiamondKatanaService.Verify(p=>p.GenerateDiamond(request.Character[0]),Times.Once());
    }
    
    [Fact]
    public void Generate_OnValidInput_ReturnsOkWithData()
    {
        //Arrange 
        var request = new GenerateRequestViewModel { Character = "B" };
        var expected = new List<DiamondResponseViewModel>
        {
            new DiamondResponseViewModel
            {
                Character = "A",
                MiddleSpaceQuantity = 0,
                SideSpaceQuantity = 1,
                SortOrder = -1
            },
            new DiamondResponseViewModel
            {
                Character = "B",
                MiddleSpaceQuantity = 1,
                SideSpaceQuantity = 0,
                SortOrder = -1
            },
            new DiamondResponseViewModel
            {
                Character = "A",
                MiddleSpaceQuantity = 0,
                SideSpaceQuantity = 1,
                SortOrder = 1
            },
        };
        _mockDiamondKatanaService.Setup(p => p.GenerateDiamond(It.IsAny<char>())).Returns(expected);
        
        //Act
        var result = (OkObjectResult)_diamondKataController.GenerateDiamond(request);
        
        //Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(expected);
    }
}
