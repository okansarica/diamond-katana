namespace ECA.DiamondKata.BusinessLayer.Abstract;

using ViewModel.Response;

public interface IDiamondKatanaService
{
    /// <summary>
    /// Generates the diamond in list of strings for every line with given input
    /// </summary>
    /// <param name="input">The character input which will be at the middle line of the diamond</param>
    /// <returns></returns>
    List<DiamondResponseViewModel> GenerateDiamond(char input);
}
