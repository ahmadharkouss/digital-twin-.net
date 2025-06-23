using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Satisfactory
{
    internal class Solver
    {
        private List<List<Product>> initial_solutions;
        
        // Génére la liste des solutions voisine à "écart" écart
        // actual_solutions: solution actuelle
        // écart: nombre d'écart entre la solution actuelle et les voisins qui vont être généré
        public List<List<Product>> Generate_Solution_Street(List<Product> actual_solutions, int écart)
        {
            List<List<Product>> new_solutions = new List<List<Product>>();
            if (actual_solutions == null || actual_solutions.Count < 2)
            {
                Console.WriteLine("Solutions empty!");
                // Si la liste est null ou contient moins de deux éléments, retourner une liste vide
                return new_solutions;
            }
            for (int i = 0; i < actual_solutions.Count - écart; i++)
            {
                // Crée une copie de la liste actuelle avec des copies profondes des produits
                List<Product> new_solution = new List<Product>();
                foreach (var product in actual_solutions)
                {
                    new_solution.Add((Product) product.Clone());
                }
                if (new_solution[i].GetId() != new_solution[i + écart].GetId())
                {
                    // Échange les éléments adjacents
                    (new_solution[i], new_solution[i + écart]) = (new_solution[i + écart], new_solution[i]);

                    // Ajoute la nouvelle permutation à la liste des solutions
                    new_solutions.Add(new_solution);
                }
            }
            return new_solutions;
        }
        
        
        // initial_solutions: la list de product de base à optimiser
        // l_listMach: la liste des machines
        // temp: la température initiale
        // cooling: le taux de réduction de la température
        // ReadMe pour la théorie
        public async Task<(List<Product>, double)> SimulatedAnnealingSolution(List<Product> initialSolutions, IEnumerable<Machines> lListMach, double temp, double cooling)
        {
            List<Product> current_solution = new List<Product>(initialSolutions);
            List<Product> best_solution = new List<Product>(initialSolutions);
            double current_time, best_time;

            var solver = new SolverSimulator();
            MachineGraph l_machineGraph = new MachineGraph();
            l_machineGraph.PopulateGraph(lListMach);
            current_time = best_time = (double)solver.Solve2(current_solution, l_machineGraph);
            Console.WriteLine("Initial best time: " + best_time);

            var temperature = temp; // Initial temperature, example 10000
            var coolingRate = cooling; // Cooling rate, example 0.003
            var kB = 1.3807e-23; // Boltzmann constant

            Random random = new Random();

            while (temperature > 1)
            {
                List<Product> new_solution = Generate_Neighbor_Solution(current_solution).ToList();

                solver = new SolverSimulator();
                l_machineGraph = new MachineGraph();
                l_machineGraph.PopulateGraph(lListMach);
                double new_time = (double)solver.Solve2(new_solution, l_machineGraph);

                if (AcceptanceProbability(current_time, new_time, temperature, kB) > random.NextDouble())
                {
                    current_solution = new_solution;
                    current_time = new_time;
                    Console.Write("New time " + current_time + "   temp: " + temperature + "\r");
                }

                if (new_time < best_time)
                {
                    best_solution = new_solution;
                    best_time = new_time;
                    Console.WriteLine("New best time: " + best_time + "   temp: " + temperature);
                }
                temperature *= 1 - coolingRate; // Decrease the temperature
            }

            Console.WriteLine("\nBest time get :" + current_time);
            return (best_solution, best_time);
        }
        
        // Création du voisin de la solution à 1 écart
        public IEnumerable<Product> Generate_Neighbor_Solution(List<Product> solution)
        {
            List<Product> new_solution = new List<Product>();
            foreach (var product in solution)
            {
                new_solution.Add((Product)product.Clone());
            }

            int pos1 = new Random().Next(new_solution.Count);
            int pos2 = new Random().Next(new_solution.Count);
            while (pos1 == pos2)
            {
                pos2 = new Random().Next(new_solution.Count);
            }

            (new_solution[pos1], new_solution[pos2]) = (new_solution[pos2], new_solution[pos1]);
    
            return new_solution;
        }
        
        // Fonction pour savoir si on a accepte une solution (même moins bien)
        public double AcceptanceProbability(double current_time, double new_time, double temperature, double kB)
        {
            if (new_time < current_time)
            {
                return 1.0;
            }
            return Math.Exp((current_time - new_time) / (temperature * kB));
        }

    }
}
