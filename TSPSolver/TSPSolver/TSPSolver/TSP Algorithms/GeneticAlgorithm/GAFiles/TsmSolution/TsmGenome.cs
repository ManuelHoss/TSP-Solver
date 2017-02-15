using System;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;
using System.Collections.Generic;

namespace PortableTsmSolution
{
    public class TsmGenome : IGenome
    {
        public double? Fitness { get; set; }
        public int NumberOfGenes { get; }
        public string Id { get; }

        private readonly string[] _path;
        private readonly string _startCity;

        public TsmGenome(string startCity, string[] cities)
        {
            foreach (var city in cities)
            {
                if (cities.Count(x => x == city) > 1)
                {
                    throw new Exception("it is not possible to visit the same city twice");
                }
            }

            if (cities.Contains(startCity))
                throw new Exception("start city not allowed within the path");


            _startCity = startCity;
            _path = cities;
        }

        public string[] GetPath()
        {
            return _path;
        }

        public override string ToString()
        {
            return _startCity + ", " + string.Join(", ", _path) + ", " + _startCity;
        }

        public IGenome Clone()
        {
            return new TsmGenome(_startCity, (string[])_path.Clone());
        }

        public string GetStartCity()
        {
            return _startCity;
        }

        public string[] GetFullPath()
        {
            List<string> list = new List<string>();
            list.Add(GetStartCity());
            list.AddRange(GetPath());
            list.Add(GetStartCity());

            return list.ToArray();
        }
    }
}