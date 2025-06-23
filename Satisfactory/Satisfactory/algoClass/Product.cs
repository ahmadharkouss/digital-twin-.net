using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Satisfactory
{
    internal class Product
    {
        private string Id { get; }
        private string Name { get; }
        
        private string MachineName { get; set; }
        
        private Type Type { get; }
        
        private double CurrTime { get; set; }
        public Product(string id, string name, Type type)
        {
            Id = id;
            Name = name;
            Type = type;
            CurrTime = 0;
            MachineName = "MISE_EN_PLACE_POT";
        }
        public Type GetType() => Type;

        public double GetCurrTime() => CurrTime;
        public void SetCurrTime(double curr)
        { 
            CurrTime = curr;
        }

        public override string ToString()
        {
            return $"Product ID: {Id}\nName: {Name}";
        }
        
        
        public object Clone()
        {
            return new Product(Id, Name, Type);
        }
        
        public string GetId() => Id;
        
        public string GetName() => Name;
        
        public string GetMachineName() => MachineName;

        public void SetMachineName(string machineName) {MachineName = machineName;}
    }
}
