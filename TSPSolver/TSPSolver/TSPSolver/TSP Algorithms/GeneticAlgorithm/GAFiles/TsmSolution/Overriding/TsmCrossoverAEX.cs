using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;
using PortableTsmSolution.Helper;

namespace PortableTsmSolution.Overriding
{
    public class TsmCrossoverAEX : ICrossover
    {
        private string Follower(string s, TsmGenome genome)
        {
            int length = genome.GetPath().Length;
            int index = genome.GetPath().ToList().IndexOf(s);

            if (index + 1 < length)
            {
                return genome.GetPath()[index + 1];
            }
            else
            {
                return TsmModel.GetStartCity();
            }
        }

        public IList<IGenome> PerformCrossover(IGenome parentOne, IGenome parentTwo)
        {
            TsmGenome pathOne = parentOne as TsmGenome;
            TsmGenome pathTwo = parentTwo as TsmGenome;

            string[][] newPaths = new string[2][];
            int toggle;

            for (int i = 0; i < newPaths.Length; i++)
            {
                toggle = i;
                string city = TsmModel.GetStartCity();
                newPaths[i] = new string[TsmModel.GetNumberOfCitiesToVisit()];

                for (int j = 0; j < newPaths[i].Length; j++)
                {
                    string newCity;

                    if (toggle == 0)
                    {
                        newCity = Follower(city, pathOne);
                    }
                    else
                    {
                        newCity = Follower(city, pathTwo);
                    }

                    if (newPaths[i].Contains(newCity))
                    {
                        List<string> cities = CityHelper.GetAllCitiesWithoutStart().ToList();
                        cities.RemoveAll(x => newPaths[i].ToList().Contains(x));
                        int random = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(cities.Count);
                        newCity = cities[random];
                    }

                    newPaths[i][j] = newCity;
                }
            }

            TsmGenome newGenomeOne = new TsmGenome(TsmModel.GetStartCity(), newPaths[0]);
            TsmGenome newGenomeTwo = new TsmGenome(TsmModel.GetStartCity(), newPaths[1]);

            return new List<IGenome>() { newGenomeOne, newGenomeTwo };
        }
    }
}