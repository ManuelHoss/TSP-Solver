using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableTsmSolution.Overriding
{
    public class TsmCrossoverPMX : ICrossover
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

            List<List<string>> maps = new List<List<string>>();

            for (int i = 0; i < indexOne; i++)
            {
                newPathOne[i] = pathOne.GetPath()[i];
                newPathTwo[i] = pathTwo.GetPath()[i];
            }

            for (int i = indexOne; i < indexTwo; i++)
            {
                List<string> mapOne = FindMap(maps, pathOne.GetPath()[i]);
                List<string> mapTwo = FindMap(maps, pathTwo.GetPath()[i]);

                if (mapOne != null && mapTwo != null)
                {
                    maps.Remove(mapTwo);

                    foreach (var s in mapTwo)
                    {
                        if (!mapOne.Contains(s))
                        {
                            mapOne.Add(s);
                        }
                    }

                    mapTwo = null;
                }

                List<string> map = mapOne ?? mapTwo;

                if (map == null)
                {
                    map = new List<string>(new[] { pathOne.GetPath()[i], pathTwo.GetPath()[i] });
                    maps.Add(map);
                }
                else
                {
                    if (!map.Contains(pathOne.GetPath()[i]))
                    {
                        map.Add(pathOne.GetPath()[i]);
                    }
                    if (!map.Contains(pathTwo.GetPath()[i]))
                    {
                        map.Add(pathTwo.GetPath()[i]);
                    }
                }

                newPathOne[i] = pathTwo.GetPath()[i];
                newPathTwo[i] = pathOne.GetPath()[i];
            }

            for (int i = indexTwo; i < length; i++)
            {
                newPathOne[i] = pathOne.GetPath()[i];
                newPathTwo[i] = pathTwo.GetPath()[i];
            }

            foreach (string[] path in new List<string[]> { newPathOne, newPathTwo })
            {
                for (int i = 0; i < length; i++)
                {
                    if (i >= indexOne && i < indexTwo)
                    {
                        continue;
                    }

                    if (path.Count(x => x == path[i]) == 1)
                    {
                        continue;
                    }

                    List<string> map = FindMap(maps, path[i]);
                    string newString = null;

                    foreach (string s in map)
                    {
                        if (path.Count(x => x == s) == 0)
                        {
                            newString = s;
                        }
                    }

                    path[i] = newString;
                }
            }

            TsmGenome newGenomeOne = new TsmGenome(TsmModel.GetStartCity(), newPathOne);
            TsmGenome newGenomeTwo = new TsmGenome(TsmModel.GetStartCity(), newPathTwo);

            return new List<IGenome>() { newGenomeOne, newGenomeTwo };
        }

        private List<string> FindMap(List<List<string>> maps, params string[] search)
        {
            foreach (string s in search)
            {
                foreach (List<string> map in maps)
                {
                    if (map.Contains(s))
                    {
                        return map;
                    }
                }
            }

            return null;
        }
    }
}