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

        // We want to compare computation time between an engine loaded manually, and one loaded using InprocLoader

        MeasureTime(() =>
            {
                // using (var loader = new InprocLoadAbbyy())
                using (var loader = new ManualLoadAbbyy())
                {
                    AbbyyFunctions.ProcessDocument(loader.Engine);      // Process the document and return without extracting the result
                    // AbbyyFunctions.ProcessAndParseDocument(loader.Engine);      // Process the document and parse it for the information we need
                }
            }
        );
    }

    /// Results:
    // ManualLoader, Process: 4.44s
    // InprocLoader, Process: 4.44s
    // ManualLoader, Process and Parse: 4.45s
    // InprocLoader, Process and Parse: 7.57.s

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