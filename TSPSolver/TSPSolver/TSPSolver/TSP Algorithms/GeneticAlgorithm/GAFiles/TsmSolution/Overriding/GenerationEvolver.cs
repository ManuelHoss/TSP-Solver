using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm;
using PortableGeneticAlgorithm.Interfaces;
using PortableGeneticAlgorithm.Predefined;
using PortableTsmSolution.Helper;

namespace PortableTsmSolution.Overriding
{
    public class GenerationEvolver : IGenerationEvolver
    {
        private IGenome Select(List<IGenome> array)
        {
            int random = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(3);

            if (random == 0)
                return new RouletteWheelSelection().SelectGenome(array);

            if (random == 1)
                return new TournamentSelection(GePrModel.GetTournamentSize()).SelectGenome(array);

            if (random == 2)
                return new ElitistSeletion().SelectGenome(array);

            return null;
        }

        private IList<IGenome> Crossover(List<IGenome> array)
        {
            IGenome parentOne = Select(array);
            IGenome parentTwo = Select(array);

            int random = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(5);

            if (random == 0)
                return new TsmCrossoverPMX().PerformCrossover(parentOne, parentTwo);

            if (random == 1)
                return new TsmCrossoverOrdinal().PerformCrossover(parentOne, parentTwo);

            if (random == 2)
                return new TsmCrossoverER().PerformCrossover(parentOne, parentTwo);

            if (random == 3)
                return new TsmCrossoverOX().PerformCrossover(parentOne, parentTwo);

            if (random == 4)
                return new TsmCrossoverAEX().PerformCrossover(parentOne, parentTwo);

            return null;
        }

        public List<IGenome> EvolveGeneration(List<IGenome> lastGenerationGenomes)
        {
            //Console.WriteLine(lastGenerationGenomes.Max(x => x.Fitness));

            List<IGenome> genomes = new List<IGenome>();

            genomes.Add(new ElitistSeletion().SelectGenome(lastGenerationGenomes));

            while (genomes.Count + 2 < GePrModel.GetPopulationSize())
            {
                int random = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(2);

                if (random == 0)
                {
                    genomes.AddRange(Crossover(lastGenerationGenomes));
                }
                else
                {
                    genomes.Add(Mutation(lastGenerationGenomes));
                    genomes.Add(Mutation(lastGenerationGenomes));
                }

            }

            if (genomes.Count < GePrModel.GetPopulationSize())
            {
                genomes.Add(Mutation(lastGenerationGenomes));
            }

            return genomes;
        }

        private IGenome Mutation(List<IGenome> array)
        {
            IGenome parent = Select(array);

            int random = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(1);

            if (random == 0)
                return new TsmMutationSWAP().Mutate(parent, GePrModel.GetMutationProbability());
            if (random == 1)
                return new TsmMutationScramble().Mutate(parent, GePrModel.GetMutationProbability());

            return null;
        }


        public List<IGenome> EvolveInitialGeneration(List<IGenome> preExistingGenomes)
        {
            List<IGenome> genomes = new List<IGenome>();

            for (int i = preExistingGenomes.Count; i < GePrModel.GetPopulationSize(); i++)
            {
                genomes.Add(CityHelper.GenerateRandomGenome());
            }

            return genomes;
        }
    }
}