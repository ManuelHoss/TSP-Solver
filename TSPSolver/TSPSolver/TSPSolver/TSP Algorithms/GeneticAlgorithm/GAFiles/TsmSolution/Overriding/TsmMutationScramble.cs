using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableTsmSolution.Overriding
{
    public class TsmMutationScramble : IMutation
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

            int count = Math.Max(firstIndex, secondIndex) - Math.Min(firstIndex, secondIndex);

            if (count > 2)
            {
                int startIndex = Math.Min(firstIndex, secondIndex);
                List<string> permutationList = new List<string>();

                for (int i = startIndex; i < count; i++)
                {
                    permutationList.Add(newPath[i]);
                }

                for (int i = startIndex; i < count; i++)
                {
                    int randomIndex = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(permutationList.Count);
                    newPath[i] = permutationList.ElementAt(randomIndex);
                    permutationList.RemoveAt(randomIndex);
                }

            }

            TsmGenome newGenome = new TsmGenome(tsmGenome.GetStartCity(), newPath);

            return newGenome;
        }
    }
}