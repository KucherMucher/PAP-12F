using System;
using System.IO;
using System.Linq;
using Dynastream.Fit;
using FitDecoderSource;
using FitInterpreterSource;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: FitDecoder <fit file>");
            return;
        }
     
        // decode
        FileStream fileStream = new FileStream(args[0], FileMode.Open, FileAccess.Read);

        FitDecoder fitDecoder = new FitDecoder(fileStream);
        FitInterpreter fitInterpreter = new FitInterpreter(fitDecoder.dataTable);
        fitInterpreter.graph.ExportGraph(@"fitfolder\graph.png");

        /*foreach(var record in fitDecoder.dataTable)
        {
            foreach(var field in record)
            {
                Console.Write(field + ",");
            }
            Console.WriteLine();
        }*/

        fitInterpreter.info.ShowFitInfo();
        fitInterpreter.score.ShowFitScore();

    }
}
