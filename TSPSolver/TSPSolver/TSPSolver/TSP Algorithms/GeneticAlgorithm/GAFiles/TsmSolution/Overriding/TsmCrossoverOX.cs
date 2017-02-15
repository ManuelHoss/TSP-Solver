using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableTsmSolution.Overriding
{
    public class TsmCrossoverOX : ICrossover
    {
        public IList<IGenome> PerformCrossover(IGenome parentOne, IGenome parentTwo)
        {
            TsmGenome pathOne = parentOne as TsmGenome;
            TsmGenome pathTwo = parentTwo as TsmGenome;

            int length = pathOne.GetPath().Length;
            int crossoverIndexOne = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(
                0, length);
            int crossoverIndexTwo = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(
                0, length);

            if (crossoverIndexOne == crossoverIndexTwo && crossoverIndexTwo == 0)
                crossoverIndexTwo++;

            if (crossoverIndexOne == crossoverIndexTwo && crossoverIndexTwo == length - 1)
                crossoverIndexTwo--;

            int indexOne = Math.Min(crossoverIndexOne, crossoverIndexTwo);
            int indexTwo = Math.Max(crossoverIndexOne, crossoverIndexTwo);

            string[] newPathOne = new string[length];
            string[] newPathTwo = new string[length];

            for (int i = indexOne; i < indexTwo; i++)
            {
                newPathOne[i] = pathOne.GetPath()[i];
                newPathTwo[i] = pathTwo.GetPath()[i];
            }

            // first genome
            for (int i = indexTwo; i < indexTwo + length; i++)
            {
                for (int j = indexTwo; j < indexTwo + length; j++)
                {
                    if (!newPathOne.Contains(pathTwo.GetPath()[j % length]))
                    {
                        newPathOne[i % length] = pathTwo.GetPath()[j % length];
                        break;
                    }
                }
            }

            // second genome
            for (int i = indexTwo; i < indexTwo + length; i++)
            {
                for (int j = indexTwo; j < indexTwo + length; j++)
                {
                    if (!newPathTwo.Contains(pathOne.GetPath()[j % length]))
                    {
                        newPathTwo[i % length] = pathOne.GetPath()[j % length];
                        break;
                    }
                }
            }

            TsmGenome newGenomeOne = new TsmGenome(TsmModel.GetStartCity(), newPathOne);
            TsmGenome newGenomeTwo = new TsmGenome(TsmModel.GetStartCity(), newPathTwo);

            return new List<IGenome> { newGenomeOne, newGenomeTwo };
        }
    }
}