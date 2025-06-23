using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Satisfactory
{
    internal class SolverSimulator
    {
        public double Solve(List<Product> products, MachineGraph machines)
        {
            double l_result = 0;
            foreach (Product l_product in products)
            {
                machines.FindStart().GetDone().Add(l_product);
            }

            var l_last = machines.FindLast();
            while (l_last.GetInProduction().Count != products.Count)
            {
                Step(0.2, machines);
                l_result += 0.2;
            }

            return l_result;
        }

        public MachineGraph Step(double timeToSpend, MachineGraph machines)
        {
            //Ajouter 1 file tobedo et 1 liste inprod pour chaque machine
            //Ajouter au count le timeToSpend
            //Premier parcours on transfert les inprod vers les tobedo
            //Deuxième parcours on transfert les tobedo dans le inprod
            //On ajoute pour chaque produit un temps correspondant au step 

            //1er parcours
            foreach (Machines l_machine in machines.TheGraph.Vertices)
            {
                var l_listasupp = new List<Product>();
                //Pour chaque produit dans la production
                foreach (Product l_product in l_machine.GetInProduction())
                {
                    double l_currentTime = l_product.GetCurrTime();
                    //On ajoute le temps écoulé
                    l_product.SetCurrTime(l_currentTime - timeToSpend);
                    //Si le produit est terminé on le déplace et on augmente la capacité
                    if (l_product.GetCurrTime() <= 0)
                    {
                        l_listasupp.Add(l_product);
                        l_machine.SetCapacity(l_machine.GetCapacity() + 1);
                        l_machine.GetDone().Add(l_product);

                    }
                }

                foreach (Product l_product in l_listasupp)
                {
                    l_machine.GetInProduction().Remove(l_product);
                }

            }

            foreach (Machines l_machine in machines.TheGraph.Vertices)
            {
                //Pour chaque produit finI
                List<Product> l_listasupp = [];
                foreach (Product l_product in l_machine.GetDone())
                {
                    Type l_type = l_product.GetType();
                    //On trouve la prochaine étape
                    Step l_step = l_type.GetNextStep(l_machine.GetId());
                    //Si pas de prochaine étape alors fini
                    if (l_step == null)
                    {
                        l_listasupp.Add(l_product);
                        machines.FindMachine("end").GetInProduction().Add(l_product);
                    }
                    else
                    {
                        var l_nextMach = machines.FindMachine(l_step.GetId());
                        //Sinon on vérifie si on peut encore en mettre
                        if (l_nextMach.GetCapacity() != 0)
                        {
                            l_nextMach.SetCapacity(l_nextMach.GetCapacity() - 1);

                            l_listasupp.Add(l_product);
                            l_nextMach.GetInProduction().Add(l_product);
                            l_product.SetCurrTime(l_step.GetProductionTime());
                        }
                    }
                }

                foreach (Product l_product in l_listasupp)
                {
                    l_machine.GetDone().Remove(l_product);
                }
            }

            return machines;
        }



        public class _Step
        {
            public string Name { get; set; }
            public decimal ExecutionTime { get; set; }
            public List<_Step> Dependencies { get; set; } = new List<_Step>();
            public List<_Step> Dependents { get; set; } = new List<_Step>();
            public string RequiredMachineId { get; set; }
            public decimal StartTime { get; set; } // Temps de début de l'étape
            public decimal EndTime { get; set; }

            public _Step(string name, decimal executionTime, string requiredMachineId)
            {
                Name = name;
                ExecutionTime = executionTime;
                RequiredMachineId = requiredMachineId;
            }
        }

        public class _Product
        {
            public string Name { get; set; }
            public List<_Step> Steps { get; set; } = new List<_Step>();

            public _Product(string name)
            {
                Name = name;
            }
        }

        public class Processor
        {
            public string Id { get; set; }
            public decimal AvailableTime { get; set; } = 0;
            public List<_Step> ScheduledSteps { get; set; } = new List<_Step>();

            public decimal Exchange = 0;

            public Processor(String id, decimal exchange)
            {
                Id = id;
                this.Exchange = exchange;
            }
        }

        public decimal Solve2(List<Product> products, MachineGraph machines, bool print = false)
        {
            // Queue<_Step> étapes = new Queue<_Step>();
            PriorityQueue<_Step, (decimal, int)> étapes = new PriorityQueue<_Step, (decimal, int)>();
            int insertionCounter = 0;
            List<Processor> processors = newProcess(machines);
            Dictionary<_Step, bool> completedSteps = new Dictionary<_Step, bool>();
            decimal globalStartTime = decimal.MaxValue;
            decimal globalEndTime = 0;

            foreach (Product product in products)
            {
                _Product newProd = newProduct(product);
                foreach (_Step step in newProd.Steps)
                {
                    if (step.Dependencies.Count == 0)
                    {
                        if (print)
                        {
                            Console.WriteLine(product.GetName());
                        }
                        étapes.Enqueue(step, (0, insertionCounter++));
                    }
                    completedSteps[step] = false;
                }
            }

            while (étapes.Count > 0)
            {
                _Step step = étapes.Dequeue();

                Processor processor = FindProcessorWithEarliestAvailableTime(processors, step.RequiredMachineId);
                
                decimal earliestStartTime = processor.AvailableTime;
                foreach (_Step dependency in step.Dependencies)
                {
                    earliestStartTime = Math.Max(earliestStartTime, dependency.EndTime);
                }
                
                // Assigner l'étape à la machine et mettre à jour le temps disponible de la machine
                // step.StartTime = processor.AvailableTime;
                step.StartTime = earliestStartTime;
                // ajouter l'EXCHANGE time
                step.EndTime = step.StartTime + step.ExecutionTime + processor.Exchange;

                processor.ScheduledSteps.Add(step);
                processor.AvailableTime = step.EndTime - processor.Exchange;
                // processor.AvailableTime = step.EndTime;

                completedSteps[step] = true;
                
                // Mise à jour du temps de début global
                globalStartTime = Math.Min(globalStartTime, step.StartTime);
                // Mise à jour du temps de fin global
                globalEndTime = Math.Max(globalEndTime, step.EndTime);
                // globalEndTime = Math.Max(globalEndTime, step.StartTime + (int)step.ExecutionTime);

                foreach (_Step dependent in step.Dependents)
                {
                    bool allDependenciesCompleted = true;
                    foreach (_Step dependency  in dependent.Dependencies)
                    {
                        // dep.Dependents.Remove(step);
                        // Console.WriteLine(dep.Dependents[0]);
                        if (!completedSteps[dependency])
                        {
                            allDependenciesCompleted = false;
                            break;
                            étapes.Enqueue(dependent, (step.EndTime, insertionCounter++));
                        }
                    }
                    if (allDependenciesCompleted)
                    {
                        étapes.Enqueue(dependent, (step.EndTime, insertionCounter++));
                    }
                }
            }

            if (print)
            {
                foreach (Processor machine in processors)
                {
                    Console.WriteLine($"\n------------------------Machine {machine.Id}:, {machine.AvailableTime}\n");
                    foreach (_Step step in machine.ScheduledSteps)
                    {
                        Console.WriteLine($"{step.Name} Step {step.RequiredMachineId} starts at {step.StartTime}, ends at {step.EndTime}");
                    }
                }
            }
            return globalEndTime;
        }

        private Processor FindProcessorWithEarliestAvailableTime(List<Processor> processors, string requiredMachineId)
        {
            return processors.Where(m => m.Id == requiredMachineId).OrderBy(m => m.AvailableTime).First();
        }

        private _Product newProduct(Product product)
        {
            _Product newProduct = new _Product(product.GetId());
            
            foreach (Step step in product.GetType().GetSteps())
            {
                _Step newStep = new _Step(product.GetName(), (decimal)step.GetProductionTime(), step.GetId());
                newProduct.Steps.Add(newStep);
            }

            List<_Step> _steps = newProduct.Steps;
            for (int i = 0; i < _steps.Count; i++)
            {
                _Step currentStep = _steps[i];
    
                // Ajouter les étapes précédentes aux Dependencies
                for (int j = 0; j < i; j++)
                {
                    currentStep.Dependencies.Add(_steps[j]);
                }

                // Ajouter les étapes suivantes aux Dependents
                for (int k = i + 1; k < _steps.Count; k++)
                {
                    currentStep.Dependents.Add(_steps[k]);
                }
            }
            return newProduct;
        }

        private List<Processor> newProcess(MachineGraph machines)
        {
            List<Processor> processors = new List<Processor>();
            // Pour chaque machine, on crée un processeur pour chaque capacité de la machine
            foreach (Machines machine in machines.TheGraph.Vertices)
            {
                decimal exchange = 0;
                foreach (var ma in machine.GetNext())
                {
                    if (ma.Contains("EXCHANGE"))
                    {
                        exchange = (decimal)machines.FindMachine(ma).GetTime();
                    }
                }
                for (int i = 0; i < machine.GetCapacity(); i++)
                {
                    processors.Add(new Processor(machine.GetId(), exchange));
                }
            }
            return processors;
        }
    }
}
