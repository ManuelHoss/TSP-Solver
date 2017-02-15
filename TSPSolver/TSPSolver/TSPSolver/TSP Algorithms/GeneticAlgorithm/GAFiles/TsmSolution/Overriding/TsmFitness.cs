using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;
using PortableTsmSolution.Helper;

namespace PortableTsmSolution.Overriding
{
    public class TsmFitness : IFitness
    {
        public Solution Evaluate(IGenome genome)
        {
            TsmGenome g = genome as TsmGenome;

            double fitness = 0;
            string[] path = g.GetPath();

            for (int i = 0; i < path.Length - 1; i++)
            {
                fitness += CityHelper.GetDistance(path[i], path[i + 1]);
            }

            fitness += CityHelper.GetDistance(g.GetStartCity(), path[0]);
            fitness += CityHelper.GetDistance(g.GetStartCity(), path[path.Length - 1]);

            // GA maximiert automatisch, hier soll aber minimiert werden
            fitness *= -1;

            TsmSolution solution = new TsmSolution(1, PortableGeneticAlgorithm.GeneticAlgorithm.Instance.Population.CurrentGeneration.GenerationIndex, fitness);
            return solution;
        }
    }
}