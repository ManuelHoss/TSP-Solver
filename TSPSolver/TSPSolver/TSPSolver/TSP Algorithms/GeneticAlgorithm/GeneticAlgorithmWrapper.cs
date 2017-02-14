using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPSolver.Interfaces;
using TSPSolver.Model;
using Windows.System.Threading;
using PortableGeneticAlgorithm;
using PortableTsmSolution;
using PortableTsmSolution.Helper;
using PortableGeneticAlgorithm.Interfaces;
using PortableTsmSolution.Overriding;
using PortableGeneticAlgorithm.Predefined;
using System.Diagnostics;

namespace TSPSolver.TSP_Algorithms.GeneticAlgorithm
{
    public class TspDataSet : CityHelper.IDataSet
    {
        private string[] _allCities;
        private Dictionary<string, Dictionary<string, double>> _matrix;
        private Dictionary<Address, Dictionary<Address, double>> _source;

        public string[] GetAllCities()
        {
            return _allCities;
        }

        public Dictionary<string, Dictionary<string, double>> GetDirections()
        {
            return _matrix;
        }

        public void LoadData()
        {
            if (_source == null)
                throw new Exception("please load dataset in advance");
        }

        public Address StringToAddress(string s)
        {
            return _source.Select(x => x.Key).Where(x => x.Id.ToString().Equals(s)).First();
        }

        public void LoadData(Dictionary<Address, Dictionary<Address, double>> source)
        {
            _source = source;

            _allCities = source.Keys.Select(x => x.Id.ToString()).ToArray();

            _matrix = new Dictionary<string, Dictionary<string, double>>();

            foreach (Address keyA in source.Keys)
            {
                Dictionary<string, double> d = new Dictionary<string, double>();
                _matrix.Add(keyA.Id.ToString(), d);

                foreach (Address keyB in source.Keys)
                {
                    d.Add(keyB.Id.ToString(), keyA.Equals(keyB) ? 0 : source[keyA][keyB]);
                }
            }
        }
    }

    class GeneticAlgorithmWrapper : ITspSolver
    {
        private Route GenomeToRoute(TsmGenome genome, TspDataSet dataSet)
        {
            var list = genome.GetFullPath().ToList();

            List<Address> addresses = new List<Address>();
            foreach (string s in list)
            {
                addresses.Add(dataSet.StringToAddress(s));
            }

            double distance = genome.Fitness.Value * (-1);

            Route r = new Route()
            {
                Addresses = addresses,
                Distance = distance
                //DistanceMatrix = distanceMatrix,
                //Duration = duration
                //DurationMatrix = durationMatrix,
            };

            return r;
        }

        public string GetName()
        {
            return "Genetic Algorithm";
        }

        public Dictionary<string, string> GetOptimizationInformation()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("Number of Iterations", GePrModel.GetMaximumNumberOfGenerations() + "");

            return dict;
        }

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> adjacencyMatrix, List<Address> addresses, Address depotAddress)
        {
            TspDataSet dataSet = new TspDataSet();
            dataSet.LoadData(adjacencyMatrix);

            CityHelper.SetDataSet(dataSet);
            CityHelper.LoadDataSet();

            new GePrModel.Builder()
                .SetCrossoverProbability(0)
                .SetMutationProbability(1)
                .SetFitness(typeof(TsmFitness))
                .SetGenerationEvolver(new GenerationEvolver())
                .SetMaximumNumberOfGenerations(500)
                .SetTermination(new IterationTermination(500), new FitnessUnchangedTermination(20))
                .SetSeed(42)
                .SetTournamentSize(5)
                .SetPopulationSize(100)
                .SetUseParalell(true)
                .SetSolutionType(typeof(TsmSolution))
                .Build();

            new TsmModel.Builder()
                .SetStartCity(depotAddress.Id.ToString())
                .SetNumberOfCitiesToVisit(CityHelper.GetAllCitiesWithoutStart().Length)
                .Build();

            var GaStopwatch = Stopwatch.StartNew();
            var geneticAlgorithm = new PortableGeneticAlgorithm.GeneticAlgorithm();
            PortableGeneticAlgorithm.GeneticAlgorithm.StartInNewTask();

            while (geneticAlgorithm.IsStopped() == false)
            {
                Task.Delay(100);
            }

            GaStopwatch.Stop();

            IGenome g = geneticAlgorithm.BestGenome;
            Route r = GenomeToRoute((TsmGenome)g, dataSet);
            return r;
        }
    }
}
