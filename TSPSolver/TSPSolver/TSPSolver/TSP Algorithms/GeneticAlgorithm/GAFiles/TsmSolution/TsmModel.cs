using System;
using System.Linq;
using PortableTsmSolution.Helper;

namespace PortableTsmSolution
{
    public class TsmModel
    {
        private static int _numberOfCitiesToVisit;
        private static string _startCity;
        public static bool _isFinished;

        public static int GetNumberOfCitiesToVisit() { return _numberOfCitiesToVisit; }
        public static bool IsFinished() { return _isFinished; }
        public static string GetStartCity() { return _startCity; }


        public class Builder
        {
            public Builder()
            {
                _numberOfCitiesToVisit = 0;
                _isFinished = false;
            }

            public Builder SetNumberOfCitiesToVisit(int i)
            {
                if (i < 2 || i > CityHelper.GetAllCitiesWithoutStart().Length)
                {
                    throw new Exception("number of cities too low, at least 2 and a maximum of " + CityHelper.GetAllCitiesWithoutStart().Length);
                }

                if(_startCity == null)
                    throw new Exception("please set first your start city");

                _numberOfCitiesToVisit = i;
                return this;
            }

            public Builder SetStartCity(string s)
            {
                if (string.IsNullOrEmpty(s) || !CityHelper.GetAllCities().ToList().Contains(s))
                    throw new Exception("start city is not valid");

                if (!CityHelper.GetAllCities().Contains(s)) 
                    throw new Exception("start city is not in your dataset");

                _startCity = s;
                return this;
            }

            public void Build()
            {
                _isFinished = true;
            }

        }
    }
}