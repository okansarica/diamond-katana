namespace ECA.DiamondKata.ConsoleApp;

using BusinessLayer;
using BusinessLayer.Abstract;
using NLog;
using System.Text;
using ViewModel.Response;

public class DiamondGeneratorExecutor(IDiamondKatanaService diamondKatanaService) : IDiamondGeneratorExecutor
{
    public void Execute(string[] args)
    {
        //The input should be a single character
        if (args.Length != 1 ||
            args[0].Length != 1)
        {
            Console.WriteLine("Usage: dotnet ECA.DiamondKata.ConsoleApp.dll [Uppercase character]");
            return;
        }

        //We verified that the input is a single character
        var inputChar = args[0][0];

        List<DiamondResponseViewModel> diamond;

        try
        {
            diamond = diamondKatanaService.GenerateDiamond(inputChar);
        }
        catch (ValidationException ex)
        {
            //The service throws validation exception when the input is not valid, so we are directly showing the message
            Console.WriteLine(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred");
            LogManager.GetCurrentClassLogger().Error(ex, "Unexpected Error");
            return;
        }
        PrintDiamond(diamond);
    }

    private void PrintDiamond(List<DiamondResponseViewModel> diamond)
    {
        const char underscore = '_';
        
        foreach (var diamondRow in diamond)
        {
            var builder = new StringBuilder();
            builder.Append(underscore, diamondRow.SideSpaceQuantity);
            builder.Append(diamondRow.Character);

            if (diamondRow.MiddleSpaceQuantity > 0)
            {
                builder.Append(underscore, diamondRow.MiddleSpaceQuantity);
                builder.Append(diamondRow.Character);
            }
            builder.Append(underscore, diamondRow.SideSpaceQuantity);
            Console.WriteLine(builder);
        }
    }
}
