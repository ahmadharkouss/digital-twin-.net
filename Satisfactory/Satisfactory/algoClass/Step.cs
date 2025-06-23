namespace Satisfactory;

internal class Step
{
    private string Id { get; }
    private string Description { get; }
    private double ProductionTime { get; set; }

    public Step(string id, string description, double productionTime)
    {
        Id = id;
        Description = description;
        ProductionTime = productionTime;
    }

    public string GetId() => Id;
    public double GetProductionTime() => ProductionTime;
    public override string ToString()
    {
           return $"Step ID: {Id}\nDescription: {Description}\nProduction Time: {ProductionTime}";
    }
}