using System;
using System.Collections.Generic;
using System.Linq;

namespace PortableTsmSolution.Helper
{
    public class Example : CityHelper.IDataSet
    {
        public string[] GetAllCities()
        {
            return new string[] { "a", "b", "c", "d", "e" };
        }

        public Dictionary<string, Dictionary<string, double>> GetDirections()
        {/*
            Dictionary<Tuple<string, string>, double> _data = new Dictionary<Tuple<string, string>, double>();
            _data.Add(new Tuple<string, string>("a", "b"), 2);
            _data.Add(new Tuple<string, string>("a", "c"), 2);
            _data.Add(new Tuple<string, string>("a", "d"), 1);
            _data.Add(new Tuple<string, string>("a", "e"), 4);

            _data.Add(new Tuple<string, string>("b", "c"), 3);
            _data.Add(new Tuple<string, string>("b", "d"), 2);
            _data.Add(new Tuple<string, string>("b", "e"), 3);

            _data.Add(new Tuple<string, string>("c", "d"), 2);
            _data.Add(new Tuple<string, string>("c", "e"), 2);

            _data.Add(new Tuple<string, string>("d", "e"), 4);

            return _data;*/
            return null;
        }

        public void LoadData()
        {
            // nothing to do...
        }
    }
}