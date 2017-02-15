using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableTsmSolution.Overriding
{
    public class TsmCrossover : ICrossover
    {
        public IList<IGenome> PerformCrossover(IGenome parentOne, IGenome parentTwo)
        {
            TsmGenome pathOne = parentOne as TsmGenome;
            TsmGenome pathTwo = parentTwo as TsmGenome;

            int length = pathOne.GetPath().Length;
            int crossoverIndex = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(
                0, length);

            string[] newPathOne = new string[length];
            string[] newPathTwo = new string[length];

            for (int i = 0; i < crossoverIndex; i++)
            {
                newPathOne[i] = pathOne.GetPath()[i];
                newPathTwo[i] = pathTwo.GetPath()[i];
            }

            for (int i = crossoverIndex; i < length; i++)
            {
                newPathOne[i] = pathTwo.GetPath()[i];
                newPathTwo[i] = pathOne.GetPath()[i];
            }

            TsmGenome newGenomeOne = new TsmGenome(TsmModel.GetStartCity(), newPathOne);
            TsmGenome newGenomeTwo = new TsmGenome(TsmModel.GetStartCity(), newPathTwo);

            return new List<IGenome>() { newGenomeOne, newGenomeTwo };
        }
    }
}