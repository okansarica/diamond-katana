namespace ECA.DiamondKata.BusinessLayer.Concrete;

using Abstract;
using System.Text;
using ViewModel.Response;

public class DiamondKatanaService : IDiamondKatanaService
{
    private const char StartingCharacter = 'A';
    
    /// <inheritdoc/>
    public List<DiamondResponseViewModel> GenerateDiamond(char input)
    {
        if (!char.IsLetter(input))
        {
            throw new ValidationException("Input must be a letter and non empty.");
        }

        if (!char.IsUpper(input))
        {
            throw new ValidationException("Input must be an uppercase letter.");
        }

        //We are calculating the distance by considering ASCII codes of the characters
        var characterDistance = input - StartingCharacter;

        //When we think the first row as we know that the number for spaces around is equal to characterDistance we can multiply the distance by 2 and add 1 for the character itself
        var totalRowLength = characterDistance * 2 + 1;
        var diamond = new List<DiamondResponseViewModel>();
        for (int i = -characterDistance; i <= characterDistance; i++)
        {
            diamond.Add(CreateLine(i, characterDistance, totalRowLength));
        }
        return diamond.OrderBy(p=>p.SortOrder).ToList();
    }

    /// <summary>
    /// This function creates the line content
    /// </summary>
    /// <param name="currentLineIndex">The line index</param>
    /// <param name="characterDistance">The distance of the input character from starting character</param>
    /// <param name="totalRowLength">Total number of characters in a row</param>
    /// <returns></returns>
    private DiamondResponseViewModel CreateLine(int currentLineIndex, int characterDistance, int totalRowLength)
    {
        var result = new DiamondResponseViewModel
        {
            SortOrder = currentLineIndex
        };
        
        //const char underScore = '_';

        //This is the quantity of blank (_) characters count in the left and the right side. We are using Abs to get the positive value since we started from negative distance in the loop above
        var blankCharactersCountAround = Math.Abs(currentLineIndex);

        //This is the builder that will store the whole line
        //var builder = new StringBuilder();

        //We are adding the spaces to the left side of the character
        //builder.Append(underScore, blankCharactersCountAround);
        result.SideSpaceQuantity = blankCharactersCountAround;

        //We are converting the ascii code to character by adding the distance to the starting character and subtracting the abs(currentLineIndex) which is blankCharactersCountAround
        result.Character = ((char)(StartingCharacter + characterDistance - blankCharactersCountAround)).ToString();

        //We are adding the first character for the line.
        //builder.Append(currentCharacter);

        //This condition is used to check if we are at the first or at the last line
        if (blankCharactersCountAround != characterDistance)
        {
            //We need to add the empty spaces between the characters. The quantity of spaces are calculated by reducing the spaces between and the occurrences of the  char which is 2 from total row length
            var spacesBetweenCount = totalRowLength - 2 - 2 * blankCharactersCountAround;
            //builder.Append(underScore, spacesBetweenCount);
            result.MiddleSpaceQuantity = spacesBetweenCount;
            //builder.Append(currentCharacter);
        }

        //We are adding the spaces to the right side of the character
        //builder.Append(underScore, blankCharactersCountAround);

        //return builder.ToString();
        return result;
    }
}
