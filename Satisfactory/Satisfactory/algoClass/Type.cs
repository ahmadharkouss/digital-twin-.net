using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Satisfactory;

internal class Type
{
    private string Id { get; }
    private string Name { get; }
    private IEnumerable<Step> Steps { get; }

    public Type(string id, string name, IEnumerable<Step> steps)
    {
        Id = id;
        Name = name;
        Steps = steps.ToList();
    }

    public string GetId() => Id;
    public string GetName() => Name;
    public IEnumerable<Step> GetSteps() => Steps;

    public Step GetNextStep(string name)
    {
        if (name=="start")
            return Steps.ElementAt(0);
        for (var i = 0; i < Steps.Count();i++)
        {
            if (Steps.ElementAt(i).GetId() == name && i != Steps.Count() - 1)
            {
                return Steps.ElementAt(i + 1);
            }
        }
        return null;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Type ID: {Id}");
        sb.AppendLine($"Type Name: {Name}");
        sb.AppendLine("Steps:");
        foreach (var step in Steps)
        {
            sb.AppendLine(step.ToString());
        }
        return sb.ToString();
    }
}