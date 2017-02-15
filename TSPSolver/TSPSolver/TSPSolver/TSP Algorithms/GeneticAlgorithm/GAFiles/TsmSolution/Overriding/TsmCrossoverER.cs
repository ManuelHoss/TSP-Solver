using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;
using PortableTsmSolution.Helper;

namespace PortableTsmSolution.Overriding
{
    public class TsmCrossoverER : ICrossover
    {
        private Dictionary<string, HashSet<string>> CalculateAdjacentMatrix(TsmGenome pathOne, TsmGenome pathTwo)
        {
            Dictionary<string, HashSet<string>> adjacentMatrix = new Dictionary<string, HashSet<string>>();

            adjacentMatrix.Add(TsmModel.GetStartCity(), new HashSet<string> { pathOne.GetPath().First(), pathOne.GetPath().Last(), pathTwo.GetPath().First(), pathTwo.GetPath().Last() });

            foreach (TsmGenome g in new[] { pathOne, pathTwo })
            {
                string[] path = g.GetPath();

                for (int i = 0; i < path.Length; i++)
                {
                    string city = path[i];

                    if (!adjacentMatrix.ContainsKey(city))
                    {
                        adjacentMatrix.Add(city, new HashSet<string>());
                    }

                    if (i == 0)
                    {
                        adjacentMatrix[city].Add(TsmModel.GetStartCity());
                    }
                    else
                    {
                        adjacentMatrix[city].Add(path[i - 1]);
                    }

                    if (i == path.Length - 1)
                    {
                        adjacentMatrix[city].Add(TsmModel.GetStartCity());
                    }
                    else
                    {
                        adjacentMatrix[city].Add(path[i + 1]);
                    }
                }
            }

            return adjacentMatrix;
        }

        public IList<IGenome> PerformCrossover(IGenome parentOne, IGenome parentTwo)
        {
            TsmGenome pathOne = parentOne as TsmGenome;
            TsmGenome pathTwo = parentTwo as TsmGenome;

            string[][] newPaths = new string[2][];

            for (int i = 0; i < newPaths.Length; i++)
            {
                Dictionary<string, HashSet<string>> adjacentMatrix = CalculateAdjacentMatrix(pathOne, pathTwo);

                string[] newPath = new string[TsmModel.GetNumberOfCitiesToVisit()];
                newPaths[i] = newPath;

                string city = TsmModel.GetStartCity();

                for (int j = 0; j < newPath.Length; j++)
                {
                    HashSet<string> currentEdges = adjacentMatrix[city];

                    adjacentMatrix.Remove(city);

                    foreach (var keyValuePair in adjacentMatrix)
                    {
                        keyValuePair.Value.Remove(city);
                    }

                    List<KeyValuePair<string, HashSet<string>>> activeEdges = adjacentMatrix
                        .Where(x => currentEdges.Contains(x.Key))
                        .ToList();

                    if (activeEdges.Count == 0)
                    {
                        // TODO check
                        // choose random node
                        int index = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(adjacentMatrix.Count);
                        city = adjacentMatrix.Keys.ElementAt(index);
                    }
                    else
                    {
                        int minActiveEdges = activeEdges
                            .Min(z => z.Value.Count);

                        activeEdges.RemoveAll(x => x.Value.Count > minActiveEdges);

                        int index = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(activeEdges.Count);
                        city = activeEdges[index].Key;
                    }

                    newPath[j] = city;
                }
            }

            TsmGenome newGenomeOne = new TsmGenome(TsmModel.GetStartCity(), newPaths[0]);
            TsmGenome newGenomeTwo = new TsmGenome(TsmModel.GetStartCity(), newPaths[1]);

            return new List<IGenome>() { newGenomeOne, newGenomeTwo
};
        }
    }
}