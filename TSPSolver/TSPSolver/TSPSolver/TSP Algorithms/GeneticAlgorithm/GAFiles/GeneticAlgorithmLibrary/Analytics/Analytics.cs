using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PortableGeneticAlgorithm.Interfaces;
using Newtonsoft.Json;
using Windows.System.Threading;
using System.Threading.Tasks;

namespace PortableGeneticAlgorithm.Analytics
{
    /// <summary>
    ///     Provides functionality to read the streaming log file
    /// </summary>
    public class Analytics
    {
        /// <summary>
        ///     Information on different aggregation values
        /// </summary>
        public enum AggInfoType
        {
            NoOfActivitiesAvg,
            NoOfActivitiesMin,
            NoOfActivitiesMax,
            FitnessAvg,
            FitnessMin,
            FitnessMax,
            MueAvg,
            MueMin,
            MueMax,
            Sigma2Avg,
            Sigma2Min,
            Sigma2Max,
            AvgFitnessAvg,
            AvgFitnessMin,
            AvgFitnessMax,
            AvgCount,
            ValidGenomesPercentage,
            FitnessCalculationTimeAvg,
            FitnessCalculationTimeMin,
            FitnessCalculationTimeMax
        }

        public static Queue<Solution> SolutionQueue;
        private static string _fileName;
        private static bool _stopRequested;

        // [Generation][Genome][Information]
        private static Dictionary<int, List<Solution>> _allSolutions;

        // [Generation][Information]
        private static Dictionary<int, Dictionary<AggInfoType, double>> _allSolutionsAgg;
        private static Dictionary<AggInfoType, double> _aggOver;

        private static List<string> _logfile;
        private static bool _isFinished;
        private static int _currentGeneration = 0;

        /// <summary>
        ///     A list of all solutions, concurrent for thread safe
        /// </summary>
//        private static readonly ConcurrentBag<Solution> ConcurrentSolutions = new ConcurrentBag<Solution>();
        /// <summary>
        ///     Creates an Analytics object noticing on the analytics file
        /// </summary>
        public Analytics()
        {
            _isFinished = false;
            _fileName = Log.PathAnalytics;
            _stopRequested = false;

            SolutionQueue = new Queue<Solution>();

            _allSolutions = null;
            _allSolutions = new Dictionary<int, List<Solution>>();

            TopSolutions.Reset();

            _allSolutionsAgg = null;
            _allSolutionsAgg = new Dictionary<int, Dictionary<AggInfoType, double>>();

            _aggOver = null;
            _aggOver = new Dictionary<AggInfoType, double>();

            _logfile = null;
            _logfile = new List<string>();
        }

        /// <summary>
        ///     Start analysing
        /// </summary>
        public void Start()
        {
            Task.Run(() => { PickItemsFromConcurrentQueue(); });
        }

        public static int GetCurrentGeneration()
        {
            return _currentGeneration;
        }

        private static List<Solution> Dequeue(int maxItems = 100)
        {
            List<Solution> solutions = new List<Solution>();

            for (int i = 0; i < maxItems; i++)
            {
                if (SolutionQueue.Count > 0)
                {
                    Solution s = SolutionQueue.Dequeue();

                    /*   if (!SolutionQueue.Dequeue(out s))
                       {
                           i = maxItems;
                       }
                       else
                       {
                           solutions.Add(s);
                       }*/
                    solutions.Add(s);
                }
            }

            return solutions;
        }

        private static void PickItemsFromConcurrentQueue()
        {
            while (_stopRequested == false && _isFinished == false)
            {
                List<Solution> solutions = Dequeue(200);

                if (solutions.Count == 0)
                {
                    Task.Delay(100);
                    continue;
                }

                foreach (Solution s in solutions)
                {
                    if (s != null)
                    {
                        Reading(s);
                    }
                }

            }
        }

        /// <summary>
        ///     Returns the fitness average/minimum/maximum values for the specified generation
        /// </summary>
        /// <param name="lastGeneration">name of generation</param>
        /// <returns>list of all developement values</returns>
        public List<Dictionary<AggInfoType, double>> GetGenerationFitnessDevelopement(int lastGeneration)
        {
            var list = new List<Dictionary<AggInfoType, double>>();
            AggInfoType[] infos =
            {
                AggInfoType.FitnessAvg,
                AggInfoType.FitnessMin,
                AggInfoType.FitnessMax
            };

            for (var i = lastGeneration; i < _allSolutionsAgg.Count; i++)
            {
                var currentList = _allSolutionsAgg.ElementAt(i).Value.Keys.ToList();

                if (infos.Any(x => !currentList.Contains(x)))
                    break;

                var series = new Dictionary<AggInfoType, double>
                {
                    {AggInfoType.FitnessAvg, _allSolutionsAgg.ElementAt(i).Value[AggInfoType.FitnessAvg]},
                    {AggInfoType.FitnessMin, _allSolutionsAgg.ElementAt(i).Value[AggInfoType.FitnessMin]},
                    {AggInfoType.FitnessMax, _allSolutionsAgg.ElementAt(i).Value[AggInfoType.FitnessMax]}
                };

                list.Add(series);
            }

            return list;
        }

        /// <summary>
        ///     Returns a list of percentage of valid genomes
        /// </summary>
        /// <param name="generation"></param>
        /// <returns></returns>
        public List<double> GetGenerationValidGenomes(int generation)
        {
            var series = new List<double>();

            for (var i = generation; i < _allSolutionsAgg.Count; i++)
            {
                if (!_allSolutionsAgg.ElementAt(i).Value.Keys.ToList().Contains(AggInfoType.ValidGenomesPercentage))
                    break;

                series.Add(_allSolutionsAgg.ElementAt(i).Value[AggInfoType.ValidGenomesPercentage]);
            }

            return series;
        }

        /// <summary>
        ///     Returns the current average fitness values for Average, minimum and maximum
        /// </summary>
        /// <returns>list of all average values</returns>
        public Dictionary<AggInfoType, double> GetCurrentFitnessAverage()
        {
            var series = new Dictionary<AggInfoType, double>();

            if (!(_aggOver.ContainsKey(AggInfoType.AvgFitnessAvg)
                  && _aggOver.ContainsKey(AggInfoType.AvgFitnessMin)
                  && _aggOver.ContainsKey(AggInfoType.AvgFitnessMax)))
                return series;

            series.Add(AggInfoType.AvgFitnessAvg, _aggOver[AggInfoType.AvgFitnessAvg]);
            series.Add(AggInfoType.AvgFitnessMin, _aggOver[AggInfoType.AvgFitnessMin]);
            series.Add(AggInfoType.AvgFitnessMax, _aggOver[AggInfoType.AvgFitnessMax]);

            return series;
        }

        private static void Reading(Solution s)
        {
            //_logfile.Add(JsonConvert.SerializeObject(s));


            if (_currentGeneration != 0 && _currentGeneration != s.Generation)
            {
                AggregateHistory(_currentGeneration);
            }

            if (_currentGeneration != s.Generation)
            {
                _currentGeneration = s.Generation;
            }

            if (s is FinishedSolution)
            {
                _isFinished = true;
                return;
            }

            if (!_allSolutions.ContainsKey(s.Generation))
            {
                _allSolutions.Add(s.Generation, new List<Solution>());
            }
            _allSolutions[s.Generation].Add(s);

            TopSolutions.Add(s);
        }

        private static void AggregateHistory(int generation)
        {
            if (_allSolutions.Count <= 0)
                return;

            if (_allSolutions.Count > 11)
            {
                int minIndex = _allSolutions.Keys.Min();

                List<Solution> toSave = new List<Solution>();

                for (int i = 0; i < 10; i++)
                {
                    foreach (Solution s in _allSolutions[i + minIndex])
                    {
                        toSave.Add(s);
                    }

                    _allSolutions.Remove(i + minIndex);
                }
                /*
                using (LiteDatabase db = new LiteDatabase(@"Solutions.db"))
                {
                    LiteCollection<Solution> dbSolutions = null;

                    try
                    {
                        dbSolutions = db.GetCollection<Solution>("solutions");
                    }
                    catch (Exception)
                    {
                        db.DropCollection("solutions");
                        dbSolutions = db.GetCollection<Solution>("solutions");
                    }

                    foreach (Solution s in toSave)
                    {
                        dbSolutions.Insert(s);
                    }
                }
                */
            }

            Dictionary<AggInfoType, double> dict = null;

            if (!_allSolutionsAgg.ContainsKey(generation))
            {
                dict = new Dictionary<AggInfoType, double>();
                _allSolutionsAgg.Add(generation, dict);
            }
            else
            {
                dict = _allSolutionsAgg[generation];
            }

            // Calculates the avg/min and max for the fitness per generation
            var avg = _allSolutions[generation].Average(x => x.Fitness);
            var min = _allSolutions[generation].Min(x => x.Fitness);
            var max = _allSolutions[generation].Max(x => x.Fitness);
            dict.Add(AggInfoType.FitnessAvg, avg);
            dict.Add(AggInfoType.FitnessMin, min);
            dict.Add(AggInfoType.FitnessMax, max);

            // Calculates the avg/min and max for the time to calculate per generation
            avg = _allSolutions[generation].Average(x => x.EvaluationTime);
            min = _allSolutions[generation].Min(x => x.EvaluationTime);
            max = _allSolutions[generation].Max(x => x.EvaluationTime);
            dict.Add(AggInfoType.FitnessCalculationTimeAvg, avg);
            dict.Add(AggInfoType.FitnessCalculationTimeMin, min);
            dict.Add(AggInfoType.FitnessCalculationTimeMax, max);

            IAnalytics additionalAnalytics = GePrModel.GetAdditionalAnalytics();

            if (additionalAnalytics != null)
            {
                Tuple<AggInfoType, double> info = additionalAnalytics.EvaluateGeneration(_allSolutions[generation]);
                dict.Add(info.Item1, info.Item2);
            }

            if (!_aggOver.ContainsKey(AggInfoType.AvgCount))
            {
                // first generation, initialize average numbers
                _aggOver.Add(AggInfoType.AvgCount, 1);
                _aggOver.Add(AggInfoType.AvgFitnessAvg, dict[AggInfoType.FitnessAvg]);
                _aggOver.Add(AggInfoType.AvgFitnessMin, dict[AggInfoType.FitnessMin]);
                _aggOver.Add(AggInfoType.AvgFitnessMax, dict[AggInfoType.FitnessMax]);

                return;
            }

            // Fitness average anpassen
            _aggOver[AggInfoType.AvgFitnessAvg] *= _aggOver[AggInfoType.AvgCount];
            _aggOver[AggInfoType.AvgCount] += 1;
            _aggOver[AggInfoType.AvgFitnessAvg] += dict[AggInfoType.FitnessAvg];
            _aggOver[AggInfoType.AvgFitnessAvg] /= _aggOver[AggInfoType.AvgCount];
        }

        /// <summary>
        ///     Returns as a list (of max 1000 items) of all logfiles, except the first #items
        /// </summary>
        /// <param name="itemsCount">number of items to skip</param>
        /// <returns>list of log lines</returns>
        public List<string> GetLog(int itemsCount)
        {
            return _logfile.GetRange(itemsCount, Math.Min(500, _logfile.Count - itemsCount));
        }

        /// <summary>
        ///     Returns information if analytics is finished or not
        /// </summary>
        /// <returns></returns>
        public static bool IsFinished()
        {
            return _isFinished;
        }

        /// <summary>
        ///     Returns a list of the best solutions
        /// </summary>
        /// <returns></returns>
        public List<Solution> GetTopSolutions()
        {
            return TopSolutions.Get();
        }

        /// <summary>
        ///     Returns a list of avg, min and max-fitness-calculation-time-developement
        /// </summary>
        /// <param name="lastGeneration"></param>
        /// <returns></returns>
        public List<Dictionary<AggInfoType, double>> GetFitnessCalculationDevelopement(int lastGeneration)
        {
            var list = new List<Dictionary<AggInfoType, double>>();

            AggInfoType[] infos =
            {
                AggInfoType.FitnessCalculationTimeAvg,
                AggInfoType.FitnessCalculationTimeMin,
                AggInfoType.FitnessCalculationTimeMax
            };

            for (var i = lastGeneration; i < _allSolutionsAgg.Count; i++)
            {
                if (infos.Any(x => !_allSolutionsAgg.ElementAt(i).Value.ContainsKey(x)))
                    break;

                var series = new Dictionary<AggInfoType, double>
                {
                    {
                        AggInfoType.FitnessCalculationTimeAvg,
                        _allSolutionsAgg.ElementAt(i).Value[AggInfoType.FitnessCalculationTimeAvg]
                    },
                    {
                        AggInfoType.FitnessCalculationTimeMin,
                        _allSolutionsAgg.ElementAt(i).Value[AggInfoType.FitnessCalculationTimeMin]
                    },
                    {
                        AggInfoType.FitnessCalculationTimeMax,
                        _allSolutionsAgg.ElementAt(i).Value[AggInfoType.FitnessCalculationTimeMax]
                    }
                };

                list.Add(series);
            }

            return list;
        }

        private static class TopSolutions
        {
            private static List<Solution> _top100 = new List<Solution>();

            public static List<Solution> Get()
            {
                return new List<Solution>(_top100);
            }

            public static void Add(Solution s)
            {
                if (_top100.Count < 100 || _top100.Last().Fitness < s.Fitness)
                {
                    _top100.Add(s);
                    _top100.OrderByDescending(x => x.Fitness);
                }
            }

            public static void Reset()
            {
                _top100 = new List<Solution>();
            }
        }
    }
}