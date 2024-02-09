namespace ECA.DiamondKata.BusinessLayerTests;

using BusinessLayer;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using FluentAssertions;

public class DiamondServiceTests
{
    private readonly IDiamondKatanaService _service = new DiamondKatanaService();

    [Theory]
    [InlineData(' ', "Input must be a letter and non empty.")]
    [InlineData(null, "Input must be a letter and non empty.")]
    [InlineData('1', "Input must be a letter and non empty.")]
    [InlineData('a', "Input must be an uppercase letter.")]
    [InlineData('h', "Input must be an uppercase letter.")]
    [InlineData('z', "Input must be an uppercase letter.")]
    public void GenerateDiamond_WithInvalidInput_ThrowsValidationException(char input, string expectedMessage)
    {
        // Arrange is already done

        // Act
        Action act = () => _service.GenerateDiamond(input);

        // Assert
        act.Should().Throw<ValidationException>().WithMessage(expectedMessage);
    }
    
    
    [Fact]
    public void GenerateDiamond_WithA_GeneratesSingleLineDiamond()
    {
        // Arrange
        var expected = new List<string> { "A" };

        // Act
        var result = _service.GenerateDiamond('A');

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }
    
    [Fact]
    public void GenerateDiamond_WithValidInputC_GeneratesCorrectDiamond()
    {
        // Arrange
        var expectedDiamond = new List<string>
        {
            "__A__",
            "_B_B_", 
            "C___C",
            "_B_B_",
            "__A__"
        };

        // Act
        var result = _service.GenerateDiamond('C');

        // Assert
        result.Should().BeEquivalentTo(expectedDiamond, options => options.WithStrictOrdering());
    }
    
    [Fact]
    public void GenerateDiamond_WithValidInputK_GeneratesCorrectDiamond()
    {
        // Arrange
        var expectedDiamond = new List<string>
        {
            "__________A__________",
            "_________B_B_________",
            "________C___C________",
            "_______D_____D_______",
            "______E_______E______",
            "_____F_________F_____",
            "____G___________G____",
            "___H_____________H___",
            "__I_______________I__",
            "_J_________________J_",
            "K___________________K",
            "_J_________________J_",
            "__I_______________I__",
            "___H_____________H___",
            "____G___________G____",
            "_____F_________F_____",
            "______E_______E______",
            "_______D_____D_______",
            "________C___C________",
            "_________B_B_________",
            "__________A__________"
        };

        // Act
        var result = _service.GenerateDiamond('K');

        // Assert
        result.Should().BeEquivalentTo(expectedDiamond, options => options.WithStrictOrdering());
    }
}
