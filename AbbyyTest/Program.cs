using System;
using System.Diagnostics;

public class Program
{
    private static readonly Stopwatch _stopwatch = new Stopwatch();

    public static void Main()
    {
        Console.WriteLine("Hello, World!");

        // MeasureTime(() =>
        //     {
        //         using (var loader = new ManualLoadAbbyy())
        //         {
        //             // AbbyyFunctions.ProcessDocument(loader.Engine);
        //             AbbyyFunctions.ProcessAndParseDocument(loader.Engine);
        //         }
        //     }, "ManualLoader"
        // );


        MeasureTime(() =>
            {
                using (var loader = new InprocLoadAbbyy())
                {
                    // AbbyyFunctions.ProcessDocument(loader.Engine);
                    AbbyyFunctions.ProcessAndParseDocument(loader.Engine);
                }
            }, "InprocLoader"
        );
    }

    public static void MeasureTime(Action action, string processTag)
    {
        _stopwatch.Restart();
        action();
        _stopwatch.Stop();
        Console.WriteLine($"Process \"{processTag}\" took {_stopwatch.Elapsed}");
    }
}