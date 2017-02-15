using System;
using System.Collections.Generic;
using System.Linq;

namespace PortableTsmSolution.Helper
{
    public class CityHelper
    {
        public interface IDataSet
        {
            void LoadData();
            string[] GetAllCities();
            Dictionary<string, Dictionary<string, double>> GetDirections();
        }

        public static string GetRandomCity()
        {
            int random = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(_dataSet.GetAllCities().Length);
            return _dataSet.GetAllCities().ElementAt(random);
        }

        private static IDataSet _dataSet = null;
        private static string[] _allCities = null;
        private static Dictionary<string, Dictionary<string, double>> _distancesDictionary = new Dictionary<string, Dictionary<string, double>>();

        public static void SetDataSet(IDataSet dataSet)
        {
            _dataSet = dataSet;
        }

        public static void LoadDataSet()
        {
            _dataSet.LoadData();

            _distancesDictionary = _dataSet.GetDirections();
            _allCities = _dataSet.GetAllCities();
        }

        public static string[] GetAllCities()
        {
            return _allCities;
        }

        public static bool IsFinished()
        {
            return _dataSet != null && _allCities != null;
        }

        public static string[] GetAllCitiesWithoutStart()
        {
            List<string> allCities = GetAllCities().ToList();
            allCities.Remove(TsmModel.GetStartCity());
            return allCities.ToArray();
        }

        public static double GetDistance(string a, string b)
        {
            if (_distancesDictionary.Keys.Contains(a))
            {
                return _distancesDictionary[a][b];
            }
            else
            {
                return _distancesDictionary[b][a];
            }
        }

        public static TsmGenome GenerateRandomGenome()
        {
            string[] path = new string[TsmModel.GetNumberOfCitiesToVisit()];
            string startCity = TsmModel.GetStartCity();

            List<string> availableCities = GetAllCitiesWithoutStart().ToList();

            for (int i = 0; i < path.Length; i++)
            {
                int randomIndex = PortableGeneticAlgorithm.Helper.RandomGenerator.Next(availableCities.Count);
                path[i] = availableCities.ElementAt(randomIndex);
                availableCities.RemoveAt(randomIndex);
            }

            return new TsmGenome(startCity, path);
        }

        public static int[] GetOrdinalRepresentation(string[] path)
        {
            int[] ordinal = new int[path.Length];
            List<string> canonicList = GetAllCitiesWithoutStart().ToList();

            for (int i = 0; i < path.Length; i++)
            {
                ordinal[i] = canonicList.IndexOf(path[i]) + 1;
                canonicList.RemoveAt(canonicList.IndexOf(path[i]));
            }

            return ordinal;
        }

        public static string[] GetPath(int[] ordinal)
        {
            string[] path = new string[ordinal.Length];
            List<string> canonicList = GetAllCitiesWithoutStart().ToList();

            for (int i = 0; i < ordinal.Length; i++)
            {
                path[i] = canonicList.ElementAt(ordinal[i] - 1);
                canonicList.RemoveAt(ordinal[i] - 1);
            }

            return path;
        }
    }
}