namespace ECA.DiamondKata.ConsoleApp;

using BusinessLayer;
using BusinessLayer.Abstract;
using NLog;

public class DiamondGeneratorExecutor(IDiamondKatanaService diamondKatanaService) : IDiamondGeneratorExecutor
{
    public void Execute(string[] args)
    {
        if (args.Length != 1 ||
            args[0].Length != 1)
        {
            Console.WriteLine("Usage: dotnet ECA.DiamondKata.ConsoleApp.dll [Uppercase character]");
            return;
        }

        var inputChar = args[0][0];

        List<string> diamond;

        try
        {
            diamond = diamondKatanaService.GenerateDiamond(inputChar);
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred");
            LogManager.GetCurrentClassLogger().Error(ex, "Unexpected Error");
            return;
        }

        foreach (string line in diamond)
        {
            Console.WriteLine(line);
        }
    }
}
