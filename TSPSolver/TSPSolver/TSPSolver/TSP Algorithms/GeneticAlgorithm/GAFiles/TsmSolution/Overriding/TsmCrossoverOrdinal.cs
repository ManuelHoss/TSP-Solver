using System;
using System.Collections.Generic;
using PortableGeneticAlgorithm.Interfaces;
using PortableTsmSolution.Helper;

namespace PortableTsmSolution.Overriding
{
    public class TsmCrossoverOrdinal : ICrossover
    {
        public IList<IGenome> PerformCrossover(IGenome parentOne, IGenome parentTwo)
        {
            TsmGenome oldGenomeOne = parentOne as TsmGenome;
            TsmGenome oldGenomeTwo = parentTwo as TsmGenome;

            int[] oldOrdinalOne = CityHelper.GetOrdinalRepresentation(oldGenomeOne.GetPath());
            int[] oldOrdinalTwo = CityHelper.GetOrdinalRepresentation(oldGenomeTwo.GetPath());

            int length = oldGenomeOne.GetPath().Length;
            int crossoverIndex = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(
                0, length);

            int[] newOrdinalOne = new int[length];
            int[] newOrdinalTwo = new int[length];

            for (int i = 0; i < crossoverIndex; i++)
            {
                newOrdinalOne[i] = oldOrdinalOne[i];
                newOrdinalTwo[i] = oldOrdinalTwo[i];
            }

            for (int i = crossoverIndex; i < length; i++)
            {
                newOrdinalOne[i] = oldOrdinalTwo[i];
                newOrdinalTwo[i] = oldOrdinalOne[i];
            }

            string[] newPathOne = CityHelper.GetPath(newOrdinalOne);
            string[] newPathTwo = CityHelper.GetPath(newOrdinalTwo);

            TsmGenome newGenomeOne = new TsmGenome(TsmModel.GetStartCity(), newPathOne);
            TsmGenome newGenomeTwo = new TsmGenome(TsmModel.GetStartCity(), newPathTwo);

            return new List<IGenome>() { newGenomeOne, newGenomeTwo };
        }
    }
}