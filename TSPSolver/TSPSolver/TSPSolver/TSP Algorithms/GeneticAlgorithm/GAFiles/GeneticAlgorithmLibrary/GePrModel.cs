using System;
using System.Linq;
using PortableGeneticAlgorithm.Analytics;
using PortableGeneticAlgorithm.Interfaces;
using PortableGeneticAlgorithm.Predefined;
using System.Collections.Generic;

namespace PortableGeneticAlgorithm
{
    public static class GePrModel
    {
        private static Type _fitness;
        private static Type _solutionType;
        private static IGenerationEvolver _generationEvolver;
        private static TerminationCombiner _termination;
        private static IGenome _initialGenome;
        private static IAnalytics _additionalAnalytics;
        private static double _crossoverProbability;
        private static int _populationSize;
        private static double _mutationProbability;
        private static int _maximumNumberOfGenerations;
        private static int _seed;
        private static bool _useParalell;
        private static bool _enableAnalytics;
        private static int _tournamentSize;
        private static bool _isFinished;

        public static Type GetSolutionType()
        {
            return _solutionType;
        }

        public static IAnalytics GetAdditionalAnalytics()
        {
            return _additionalAnalytics;
        }

        public static Type GetFitness()
        {
            return _fitness;
        }

        public static ITermination GetTermination()
        {
            return _termination;
        }

        public static int GetPopulationSize()
        {
            return _populationSize;
        }

        public static IGenerationEvolver GetGenerationEvolver()
        {
            return _generationEvolver;
        }

        public static IGenome GetInitialGenome()
        {
            return _initialGenome;
        }

        public static int GetMaximumNumberOfGenerations()
        {
            return _maximumNumberOfGenerations;
        }

        public static bool GetUseParalell()
        {
            return _useParalell;
        }

        public static int GetSeed()
        {
            return _seed;
        }

        public static int GetTournamentSize()
        {
            return _tournamentSize;
        }

        public static double GetMutationProbability()
        {
            return _mutationProbability;
        }

        public static double GetCrossoverProbability()
        {
            return _crossoverProbability;
        }

        public static bool IsFinished()
        {
            return _isFinished;
        }

        public static bool AnalyticsEnabled()
        {
            return _enableAnalytics;
        }

        public class Builder
        {
            public Builder()
            {
                Clear();
            }

            public void Clear()
            {
                _fitness = null;
                _generationEvolver = null;
                _termination = null;
                _initialGenome = null;
                _additionalAnalytics = null;
                _solutionType = null;

                _crossoverProbability = 0.0;
                _mutationProbability = 0.0;
                _maximumNumberOfGenerations = 1;
                _tournamentSize = 2;
                _populationSize = 10;

                _seed = new Random().Next();

                _useParalell = false;
                _isFinished = false;
                _enableAnalytics = false;
            }

            public Builder SetMaximumNumberOfGenerations(int i)
            {
                if (i < 1)
                    throw new Exception("maximum number of generations too low, at least 1");

                _maximumNumberOfGenerations = i;
                return this;
            }

            public Builder SetCrossoverProbability(double d)
            {
                if (d < 0)
                    throw new Exception("crossover probability too low, at least 0");

                if (d > 1)
                    throw new Exception("crossover probability too high, maximum is 1");

                _crossoverProbability = d;
                return this;
            }

            public Builder SetMutationProbability(double d)
            {
                if (d < 0)
                    throw new Exception("mutation probability too low, at least 0");

                if (d > 1)
                    throw new Exception("mutation probability too high, maximum is 1");

                _mutationProbability = d;
                return this;
            }

            public Builder SetUseParalell(bool b)
            {
                _useParalell = b;
                return this;
            }

            public Builder SetEnableAnalytics(bool b)
            {
                _enableAnalytics = b;
                return this;
            }

            public Builder SetFitness(Type t)
            {
                if (!(Activator.CreateInstance(t) is IFitness))
                    throw new Exception("fitness does not implement IFitness interface");

                try
                {
                    var dump = (IFitness)Activator.CreateInstance(t);
                }
                catch (Exception)
                {
                    throw new Exception("not possible to create a fitness object (test failed)");
                }

                _fitness = t;
                return this;
            }

            public Builder SetSeed(int i)
            {
                _seed = i;
                return this;
            }

            public Builder SetTournamentSize(int i)
            {
                if (i < 2)
                    throw new Exception("turnament size too small, at least 2");

                _tournamentSize = i;
                return this;
            }

            public Builder SetGenerationEvolver(IGenerationEvolver g)
            {
                if (g == null)
                    throw new Exception("generation evolver is null");

                _generationEvolver = g;
                return this;
            }

            public Builder SetAdditionalAnalytics(IAnalytics a)
            {
                _additionalAnalytics = a;
                return this;
            }

            public Builder SetPopulationSize(int i)
            {
                if (i < 1)
                    throw new Exception("population size too small");

                _populationSize = i;
                return this;
            }

            public Builder SetTermination(params ITermination[] parameters)
            {
                if (parameters.Length == 0)
                    throw new Exception("no termination provided");

                if (_termination == null)
                {
                    _termination = new TerminationCombiner(new IterationTermination(GetMaximumNumberOfGenerations()));
                }

                foreach (ITermination termination in parameters)
                {
                    _termination.AddTermination(termination);
                }

                return this;
            }

            public Builder SetSolutionType(Type t)
            {
                if (t == null)
                    throw new Exception("solution type is null");

                if (!(Activator.CreateInstance(t) is Solution))
                    throw new Exception("solution type is not subclass of solution");

                _solutionType = t;
                return this;
            }

            public Builder SetInitialGenome(IGenome g)
            {
                _initialGenome = g;
                return this;
            }

            public void Build()
            {
                if (_generationEvolver == null
                    || _fitness == null
                    || _termination == null
                    || _solutionType == null)
                    throw new Exception("Not all input delivered...");

                _isFinished = true;
            }
        }
    }
}