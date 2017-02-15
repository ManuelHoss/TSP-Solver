using System;
using System.Collections.Generic;
using System.Linq;
using PortableTsmSolution.Helper;
using System.Net.Http;

namespace PortableTsmSolution.DataSets
{
    public class Fri26 : CityHelper.IDataSet
    {
        private static Dictionary<string, Dictionary<string, double>> _distancesDictionary;

        private static string[] _allCities;

        private static string DownloadDataSet(string url)
        {
            string contents = null;

            using (var wc = new HttpClient())
            {
                contents = wc.GetStringAsync(url).Result;
            }

            return contents;
        }

        public void LoadData()
        {
            string cities = "http://people.sc.fsu.edu/~jburkardt/datasets/tsp/fri26.tsp";
            cities = DownloadDataSet(cities);

            List<string> cityList = new List<string>();

            for (int i = 1; i <= 26; i++)
            {
                cityList.Add(i + "");
            }

            _allCities = cityList.ToArray();

            string intercityDistanceTable = "http://people.sc.fsu.edu/~jburkardt/datasets/tsp/fri26_d.txt";
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
                    rows[i].Replace("  ", " ").Trim().Split(new string[] { " " }, StringSplitOptions.None).ToList();

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