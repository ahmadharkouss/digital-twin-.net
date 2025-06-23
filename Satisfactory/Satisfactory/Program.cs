using System;
using CommandLine;
using System.Collections.Generic;

namespace Satisfactory;

class Program
{
    private class Options
    {
        [Option("input", Required = false, HelpText = "List of items to manufacture.")]
        public string InputPath { get; }
        
        [Option("topology", Required = true, HelpText = "Architecture of the manufacturing line within the factory.")]
        public string TopologyPath { get; }
        
        [Option("types", Required = false, HelpText = "Definition of all the manufacturable types.")]
        public string TypesPath { get; }
        
        [Option("output", Required = false, HelpText = "Output file.")]
        public string OutputPath { get; }
    }
    public static int Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(RunErrors);
        return 0;
    }


    private static async void RunOptions(Options opt)
    {
        /// Parse Input files
        var l_listMach = ParseMachinesXml.ParseMachines(opt.TopologyPath);
        var l_types = ParseTypesXml.ParseTypes(opt.TypesPath);
        Products products = Input.ParseInput(opt.InputPath, l_types);
        
        Output output = new Output(opt.OutputPath);

        /// Generate the graph
        MachineGraph l_machineGraph = new MachineGraph();
        l_machineGraph.PopulateGraph(l_listMach);

        /// Product list of product to craft
        var l_listprod = products.GenerateProductionList();


        var solversol = new Solver();
        var (l_optiprod, best_time) = await solversol.SimulatedAnnealingSolution(l_listprod, l_listMach, 10000, 0.003);
        
        output.FromProductList(l_optiprod);
        output.SetBestTime(best_time);
        output.WriteToFile();
    }

    private static void RunErrors(IEnumerable<Error> errs)
    {
        // Do something
    }
}