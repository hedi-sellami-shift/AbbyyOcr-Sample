using System;
using System.Diagnostics;

public class Program
{
    private static readonly Stopwatch _stopwatch = new Stopwatch();

    public static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException(
                $"Run with the following arguments: {nameof(CustomProjectId)} {nameof(LicensePassword)}");
        }

        CustomProjectId = args[0];
        LicensePassword = args[1];

        Console.WriteLine("Starting computation");

        MeasureTime(() =>
            {
                using (var loader = new InprocLoadAbbyy())
                // using (var loader = new ManualLoadAbbyy())
                {
                    // AbbyyFunctions.ProcessDocument(loader.Engine);
                    AbbyyFunctions.ProcessAndParseDocument(loader.Engine);
                }
            }
        );
    }

    public static void MeasureTime(Action action)
    {
        _stopwatch.Restart();
        action();
        _stopwatch.Stop();
        Console.WriteLine($"Process took {_stopwatch.Elapsed}");
    }

    public static string CustomProjectId = "";
    public static string LicensePassword = "";
}