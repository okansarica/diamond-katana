namespace ECA.DiamondKata.BusinessLayerTests;

using BusinessLayer;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using FluentAssertions;
using ViewModel.Response;

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
        var act = () => _service.GenerateDiamond(input);

        // Assert
        act.Should().Throw<ValidationException>().WithMessage(expectedMessage);
    }


    [Fact]
    public void GenerateDiamond_WithA_GeneratesSingleLineDiamond()
    {
        // Arrange
        var expected = new List<DiamondResponseViewModel>
        {
            new DiamondResponseViewModel
            {
                Character = "A",
                SideSpaceQuantity = 0,
                MiddleSpaceQuantity = 0,
                SortOrder = 0
            }
        };

        // Act
        var result = _service.GenerateDiamond('A');

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GenerateDiamond_WithValidInputC_GeneratesCorrectDiamond()
    {
        // Arrange
        var expected = new List<DiamondResponseViewModel>
        {
            new DiamondResponseViewModel
            {
                Character = "A",
                SideSpaceQuantity = 2,
                MiddleSpaceQuantity = 0,
                SortOrder = -2
            },
            new DiamondResponseViewModel
            {
                Character = "B",
                SideSpaceQuantity = 1,
                MiddleSpaceQuantity = 1,
                SortOrder = -1
            },
            new DiamondResponseViewModel
            {
                Character = "C",
                SideSpaceQuantity = 0,
                MiddleSpaceQuantity = 3,
                SortOrder = 0
            },
            new DiamondResponseViewModel
            {
                Character = "B",
                SideSpaceQuantity = 1,
                MiddleSpaceQuantity = 1,
                SortOrder = 1
            },
            new DiamondResponseViewModel
            {
                Character = "A",
                SideSpaceQuantity = 2,
                MiddleSpaceQuantity = 0,
                SortOrder = 2
            },
        };

        // Act
        var result = _service.GenerateDiamond('C');

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GenerateDiamond_WithValidInputK_GeneratesCorrectDiamond()
    {
        // Arrange
        var expected = new List<DiamondResponseViewModel>
        {
            new DiamondResponseViewModel { Character = "A", SideSpaceQuantity = 10, MiddleSpaceQuantity = 0, SortOrder = -10 },
            new DiamondResponseViewModel { Character = "B", SideSpaceQuantity = 9, MiddleSpaceQuantity = 1, SortOrder = -9 },
            new DiamondResponseViewModel { Character = "C", SideSpaceQuantity = 8, MiddleSpaceQuantity = 3, SortOrder = -8 },
            new DiamondResponseViewModel { Character = "D", SideSpaceQuantity = 7, MiddleSpaceQuantity = 5, SortOrder = -7 },
            new DiamondResponseViewModel { Character = "E", SideSpaceQuantity = 6, MiddleSpaceQuantity = 7, SortOrder = -6 },
            new DiamondResponseViewModel { Character = "F", SideSpaceQuantity = 5, MiddleSpaceQuantity = 9, SortOrder = -5 },
            new DiamondResponseViewModel { Character = "G", SideSpaceQuantity = 4, MiddleSpaceQuantity = 11, SortOrder = -4 },
            new DiamondResponseViewModel { Character = "H", SideSpaceQuantity = 3, MiddleSpaceQuantity = 13, SortOrder = -3 },
            new DiamondResponseViewModel { Character = "I", SideSpaceQuantity = 2, MiddleSpaceQuantity = 15, SortOrder = -2 },
            new DiamondResponseViewModel { Character = "J", SideSpaceQuantity = 1, MiddleSpaceQuantity = 17, SortOrder = -1 },
            new DiamondResponseViewModel { Character = "K", SideSpaceQuantity = 0, MiddleSpaceQuantity = 19, SortOrder = 0 },
            new DiamondResponseViewModel { Character = "J", SideSpaceQuantity = 1, MiddleSpaceQuantity = 17, SortOrder = 1 },
            new DiamondResponseViewModel { Character = "I", SideSpaceQuantity = 2, MiddleSpaceQuantity = 15, SortOrder = 2 },
            new DiamondResponseViewModel { Character = "H", SideSpaceQuantity = 3, MiddleSpaceQuantity = 13, SortOrder = 3 },
            new DiamondResponseViewModel { Character = "G", SideSpaceQuantity = 4, MiddleSpaceQuantity = 11, SortOrder = 4 },
            new DiamondResponseViewModel { Character = "F", SideSpaceQuantity = 5, MiddleSpaceQuantity = 9, SortOrder = 5 },
            new DiamondResponseViewModel { Character = "E", SideSpaceQuantity = 6, MiddleSpaceQuantity = 7, SortOrder = 6 },
            new DiamondResponseViewModel { Character = "D", SideSpaceQuantity = 7, MiddleSpaceQuantity = 5, SortOrder = 7 },
            new DiamondResponseViewModel { Character = "C", SideSpaceQuantity = 8, MiddleSpaceQuantity = 3, SortOrder = 8 },
            new DiamondResponseViewModel { Character = "B", SideSpaceQuantity = 9, MiddleSpaceQuantity = 1, SortOrder = 9 },
            new DiamondResponseViewModel { Character = "A", SideSpaceQuantity = 10, MiddleSpaceQuantity = 0, SortOrder = 10 }
        };


        // Act
        var result = _service.GenerateDiamond('K');

        // Assert
        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }
}
