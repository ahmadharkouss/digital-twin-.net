using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Satisfactory
{
    public enum State
    {
        AVAILABLE = 0,
        OCCUPIED = 1
    }
    
    internal class Machines
    {
        private string Id { get; }
        private int Capacity { get; set; }
        private string? Product { get; }
        private IEnumerable<string> IdNext { get; }
        private ImmutableArray<State> MachinesStates { get; }
        private float TimeS { get; set; }
        private List<Product> InProduction { get; }
        private List<Product> Done { get; }

        public Machines(string id, int capacity, string? product, IEnumerable<string> idNext, float timeS)
        {
            Id = id;
            Capacity = capacity;
            Product = product;
            IdNext = idNext;
            TimeS = timeS;
            MachinesStates = [..Enumerable.Repeat(State.AVAILABLE, Capacity).ToArray()];
            InProduction = new List<Product>();
            Done = new List<Product>();
        }

        public string GetId() => Id;
        public int GetCapacity() => Capacity;
        public void SetCapacity(int cap)
        {
            Capacity = cap;
        }
        public string? GetProduct() => Product;
        public IEnumerable<string> GetNext() => IdNext;
        public ImmutableArray<State> GetMachinesStates() => MachinesStates;
        public float GetTime() => TimeS;
        public List<Product> GetInProduction() => InProduction;
        public List<Product> GetDone() => Done;
        public override string ToString()
        {
            var l_sb = new StringBuilder();
            l_sb.AppendLine($"Machine Name: {Id}");
            l_sb.AppendLine($"Capacity: {Capacity}");
            l_sb.AppendLine($"Product: {Product}");
            l_sb.AppendLine("Next IDs: " + string.Join(", ", IdNext));
            l_sb.AppendLine($"Time: {TimeS}");
            return l_sb.ToString();
        }
    }
}
