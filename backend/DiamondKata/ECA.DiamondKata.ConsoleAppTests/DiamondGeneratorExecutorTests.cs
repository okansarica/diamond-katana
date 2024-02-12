namespace ECA.DiamondKata.ConsoleAppTests;

using BusinessLayer;
using BusinessLayer.Abstract;
using ConsoleApp;
using FluentAssertions;
using Moq;
using ViewModel.Response;

public class DiamondGeneratorExecutorTests
{
    private readonly Mock<IDiamondKatanaService> _mockDiamondKatanaService;
    private readonly IDiamondGeneratorExecutor _executor;

    public DiamondGeneratorExecutorTests()
    {
        _mockDiamondKatanaService = new Mock<IDiamondKatanaService>();
        _executor = new DiamondGeneratorExecutor(_mockDiamondKatanaService.Object);
    }
    
    [Fact]
    public void Execute_WithIncorrectNumberOfArgs_PrintsUsageMessage()
    {
        // Arrange
        var input = new[] { "A", "B" };
        
        // Redirect console output to capture the print
        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        _executor.Execute(input);

        // Assert
        consoleOutput.ToString().Should().Be("Usage: dotnet ECA.DiamondKata.ConsoleApp.dll [Uppercase character]" + Environment.NewLine);
    }
    
    [Fact]
    public void Execute_WithIncorrectArgsItemLength_PrintsUsageMessage()
    {
        // Arrange
        var input = new[] { "ABC" };
        
        // Redirect console output to capture the print
        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        _executor.Execute(input);

        // Assert
        consoleOutput.ToString().Should().Be("Usage: dotnet ECA.DiamondKata.ConsoleApp.dll [Uppercase character]" + Environment.NewLine);
    }

    
    [Fact]
    public void Execute_InputThrowsValidationException_PrintsValidationMessage()
    {
        // Arrange
        var input = new[] { "o" };
        _mockDiamondKatanaService.Setup(p => p.GenerateDiamond(It.IsAny<char>())).Throws(new ValidationException("Invalid_Input"));
        
        // Redirect console output to capture the print
        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        // Act
        _executor.Execute(input);
        
        //Assert
        consoleOutput.ToString().Should().Be("Invalid_Input"+ Environment.NewLine);
    }
    
    [Fact]
    public void Execute_InputThrowsException_PrintsValidationMessage()
    {
        // Arrange
        var input = new[] { "o" };
        _mockDiamondKatanaService.Setup(p => p.GenerateDiamond(It.IsAny<char>())).Throws(new Exception("Error_Occured"));
        
        // Redirect console output to capture the print
        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        // Act
        _executor.Execute(input);
        
        //Assert
        consoleOutput.ToString().Should().Be("An unexpected error occurred"+ Environment.NewLine);
    }

    [Fact]
    public void Execute_ValidInput_PrintsDiamond()
    {
        // Arrange
        var input = new[] { "B" };
        _mockDiamondKatanaService.Setup(p => p.GenerateDiamond(It.IsAny<char>())).Returns(new List<DiamondResponseViewModel>
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
                SortOrder = 0
            },
            new DiamondResponseViewModel
            {
                Character = "A",
                MiddleSpaceQuantity = 0,
                SideSpaceQuantity = 1,
                SortOrder = 1
            },
        });
        
        // Redirect console output to capture the print
        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        // Act
        _executor.Execute(input);
        
        //Assert
        consoleOutput.ToString().Should().Be($"_A_{Environment.NewLine}B_B{Environment.NewLine}_A_{Environment.NewLine}");
    }
}
