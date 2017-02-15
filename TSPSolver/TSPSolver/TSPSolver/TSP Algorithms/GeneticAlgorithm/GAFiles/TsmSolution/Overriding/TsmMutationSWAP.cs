using System;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableTsmSolution.Overriding
{
    public class TsmMutationSWAP : IMutation
    {
        public IGenome Mutate(IGenome genome, double mutationProbability)
        {
            if (PortableGeneticAlgorithm.Helper.RandomGenerator.Next(100) > mutationProbability * 100)
            {
                return genome.Clone();
            }

            TsmGenome tsmGenome = genome as TsmGenome;

            string[] newPath = tsmGenome.GetPath();

            int firstIndex = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(newPath.Length);
            int secondIndex = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(newPath.Length - 1);

            if (firstIndex == secondIndex)
            {
                if (secondIndex == 0)
                {
                    secondIndex++;
                }
                else if (secondIndex == newPath.Length - 1)
                {
                    secondIndex--;
                }
                else
                {
                    if (PortableGeneticAlgorithm.Helper.RandomGenerator.Next(2) == 0)
                    {
                        secondIndex++;
                    }
                    else
                    {
                        secondIndex--;
                    }
                }
            }

            string buffer = newPath[firstIndex];
            newPath[firstIndex] = newPath[secondIndex];
            newPath[secondIndex] = buffer;

            TsmGenome newGenome = new TsmGenome(tsmGenome.GetStartCity(), newPath);

            return newGenome;
        }
    }
}