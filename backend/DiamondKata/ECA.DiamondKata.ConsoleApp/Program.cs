namespace ECA.DiamondKata.ConsoleApp;

using BusinessLayer.Concrete;
using NLog;
using NLog.Config;
using NLog.Targets;

public static class Program
{
    public static void Main(string[] args)
    {
        ConfigureLogging();

        var diamondService = new DiamondKatanaService();
        var executor = new DiamondGeneratorExecutor(diamondService);

        executor.Execute(args);
    }

    static void ConfigureLogging()
    {
        var config = new LoggingConfiguration();

        // Define file target
        var fileTarget = new FileTarget("file")
        {
            FileName = "${basedir}/logs/${shortdate}.log",
            ArchiveFileName = "${basedir}/logs/archive/{#}.log",
            ArchiveEvery = FileArchivePeriod.Day,
            ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
            MaxArchiveFiles = 7 // Keep up to 7 days of logs
        };
        config.AddTarget(fileTarget);

        config.AddRule(LogLevel.Error, LogLevel.Fatal, fileTarget);

        LogManager.Configuration = config;
    }

}
