using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using QuickGraph;

namespace Satisfactory;

internal class MachineGraph
{
    [NotNull]
    public readonly AdjacencyGraph<Machines, Edge<Machines>>? TheGraph;

    public MachineGraph()
    {
        TheGraph = new AdjacencyGraph<Machines, Edge<Machines>>();
    }
    
    public MachineGraph(AdjacencyGraph<Machines, Edge<Machines>> graph)
    { 
        TheGraph = graph.Clone();
    }

    public void PopulateGraph(IEnumerable<Machines> machinesEnumerable)
    {
        if (TheGraph is null)
        {
            throw new NullReferenceException();
        }
        TheGraph.AddVertexRange(machinesEnumerable);
        TheGraph.AddVertex(new Machines("end", 1, "", new string[]{}, 0));
        foreach (var l_vertex in TheGraph.Vertices)
        {
            var l_edgeRange = l_vertex.GetNext()
                .Select(x => new Edge<Machines>(l_vertex, FindMachine(x)));
            TheGraph.AddEdgeRange(l_edgeRange);
        }
    }

    public override string ToString()
    {
        var l_sb = new StringBuilder();
        l_sb.Append($"Number of vertices : {TheGraph.VertexCount}\n");
        l_sb.Append($"Number of edges : {TheGraph.EdgeCount}\n");
        foreach (var l_vertex in TheGraph.Vertices)
        {
            l_sb.Append($"{l_vertex.GetId()} {(!l_vertex.GetNext().Any() ? "" : ":")}\n");
            foreach (var l_edge in TheGraph.Edges.Where(x => x.Source.GetId() == l_vertex.GetId()))
            {
                l_sb.Append($"\t--> {l_edge.Target.GetId()}\n");
            }
        }

        return l_sb.ToString();
    }

    public MachineGraph Clone()
    {
        return new MachineGraph(TheGraph);
    }

    public Machines FindMachine(string id)
    {
        return TheGraph.Vertices.First(x => x.GetId() == id);
    }

    public Machines FindLast()
    {
        return TheGraph.Vertices.First(x => x.GetId() == "end");
    }

    public Machines FindStart()
    {
        return TheGraph.Vertices.First(x => x.GetId() == "start");
    }
}