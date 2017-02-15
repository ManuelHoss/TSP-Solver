using System;
using System.Collections.Generic;
using System.Linq;
using PortableTsmSolution.Helper;

namespace PortableTsmSolution.DataSets
{
    public class Att48 : CityHelper.IDataSet
    {
        private static Dictionary<string, Dictionary<string, double>> _distancesDictionary;

        private static string[] _allCities;

        private static string DownloadDataSet(string url)
        {
            string contents = null;

            /*   using (var wc = new System.Net.WebClient())
               {
                   contents = wc.DownloadString(url);
               }
               */
            return contents;
        }

        public void LoadData()
        {
            string cities = "http://people.sc.fsu.edu/~jburkardt/datasets/tsp/att48.tsp";
            cities = DownloadDataSet(cities);

            List<string> cityList = null;
            cityList = cities.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Skip(6).Take(48).ToList();

            _allCities = new string[cityList.Count];

            for (int i = 0; i < _allCities.Length; i++)
                _allCities[i] =
                    cityList.ElementAt(i).Split(new string[] { " " }, StringSplitOptions.None).FirstOrDefault();

            string intercityDistanceTable = "http://people.sc.fsu.edu/~jburkardt/datasets/tsp/att48_d.txt";
            string distances = DownloadDataSet(intercityDistanceTable);

            List<string> rows = null;
            rows = distances.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            _distancesDictionary = new Dictionary<string, Dictionary<string, double>>();

            for (int i = 0; i < rows.Count - 1; i++)
            {
                _distancesDictionary.Add(_allCities[i], new Dictionary<string, double>());

                while (true)
                {
                    int oldLength = rows[i].Length;
                    rows[i] = rows[i].Replace("  ", " ");
                    int newLength = rows[i].Length;

                    if (oldLength == newLength)
                    {
                        break;
                    }
                }

                List<string> columns =
                    rows[i].Replace("  ", " ").Split(new string[] { " " }, StringSplitOptions.None).Skip(1).ToList();

                for (int j = i; j < columns.Count; j++)
                {
                    _distancesDictionary[_allCities[i]].Add(_allCities[j], double.Parse(columns[j]));
                }
            }
        }

        public string[] GetAllCities()
        {
            return _allCities;
        }

        public Dictionary<string, Dictionary<string, double>> GetDirections()
        {
            return _distancesDictionary;
        }
    }
}