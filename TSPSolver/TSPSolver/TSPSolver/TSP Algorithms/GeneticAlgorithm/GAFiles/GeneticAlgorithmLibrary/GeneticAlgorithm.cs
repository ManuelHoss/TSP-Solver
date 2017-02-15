using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PortableGeneticAlgorithm.Interfaces;
using Newtonsoft.Json;
using Windows.System.Threading;
using System.Collections.Generic;
using PortableGeneticAlgorithm.Analytics;

namespace PortableGeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        #region Constructor

        public GeneticAlgorithm()
        {
            if (!GePrModel.IsFinished())
                throw new Exception("Model not finished...");

            Population = new Population(GePrModel.GetInitialGenome());
            CrossoverProbability = GePrModel.GetCrossoverProbability();
            MutationProbability = GePrModel.GetMutationProbability();
            Instance = this;
            TimeEvolving = TimeSpan.Zero;
            Analytics = new Analytics.Analytics();
            _isStopped = false;
            _stopRequested = false;
        }

        #endregion //Constructor

        #region Fields

        private readonly object _lock = new object();
        private bool _stopRequested;
        private static bool _isStopped;

        #endregion //Fields

        #region Constants

        /// <summary>
        ///     The default crossover probability.
        /// </summary>
        public const float DefaultCrossoverProbability = 0f;

        /// <summary>
        ///     The default mutation probability.
        /// </summary>
        public const float DefaultMutationProbability = 1f;

        #endregion

        #region Properties

        public IGenome BestGenome => Population.BestGenome;

        public IPopulation Population { get; set; }
        private Analytics.Analytics Analytics { get; }
        private double CrossoverProbability { get; set; }
        private double MutationProbability { get; set; }
        public TimeSpan TimeEvolving { get; set; }
        public static GeneticAlgorithm Instance { get; set; }

        #endregion //Properties

        #region Events

        /// <summary>
        ///     Occurs when generation is finished.
        /// </summary>
        public event EventHandler GenerationRan;

        /// <summary>
        ///     Occurs when termination is reached.
        /// </summary>
        public event EventHandler TerminationReached;

        #endregion //Events

        #region Methods

        public static void StartInNewTask()
        {
            Task.Run(new Action(Start));
        }

        public static void Start()
        {
            Instance.Analytics.Start();

            lock (Instance._lock)
            {
                // State = GeneticAlgorithmState.Started;
                var startDateTime = DateTime.Now;
                Instance.Population.CreateInitialGeneration(Instance.Population.InitialGenome);
                Instance.TimeEvolving = DateTime.Now - startDateTime;
            }

            Instance.Resume();
        }

        public bool IsStopped()
        {
            return _isStopped;
        }

        public void RequestStop()
        {
            _stopRequested = true;
        }

        /// <summary>
        ///     Resumes the last evolution of the genetic algorithm.
        /// </summary>
        public void Resume()
        {
            lock (_lock)
            {
                _stopRequested = false;
            }

            if (Population.NumberOfGenerations == 0)
                throw new InvalidOperationException(
                    "You can't resume a GeneticAlgorithm algorithm wich is not started yet.");

            if (Population.NumberOfGenerations > 1)
                if (GePrModel.GetTermination().HasReached(this))
                    throw new InvalidOperationException(
                        $"Termination ({GePrModel.GetTermination()}) is already reached. Please, specify a new termination or extend the current one.");

            if (EndCurrentGeneration().Result)
                return;

            var terminationConditionReached = false;

            do
            {
                if (_stopRequested)
                {
                    _isStopped = true;
                    break;
                }

                var startDateTime = DateTime.Now;
                // Adjust mutation and crossoverprobability
                MutationProbability -= 1 / GePrModel.GetMaximumNumberOfGenerations();
                CrossoverProbability += 1 / GePrModel.GetMaximumNumberOfGenerations();
                terminationConditionReached = EvolveOneGeneration();
                TimeEvolving += DateTime.Now - startDateTime;
            } while (!terminationConditionReached);

            Log.A(new FinishedSolution());
            _isStopped = true;
        }

        /// <summary>
        ///     Evolve one generation.
        /// </summary>
        /// <returns>True if termination has been reached, otherwise false.</returns>
        private bool EvolveOneGeneration()
        {
            var oldGenerationGenomes = Population.CurrentGeneration.Genomes.ToList();
            var newGenerationGenomes = GePrModel.GetGenerationEvolver().EvolveGeneration(oldGenerationGenomes);
            Population.CreateNewGeneration(newGenerationGenomes);
            return EndCurrentGeneration().Result;
        }

        /// <summary>
        ///     Ends the current generation.
        /// </summary>
        /// <returns><c>true</c>, if current generation was ended, <c>false</c> otherwise.</returns>
        private async Task<bool> EndCurrentGeneration()
        {
            await EvaluateFitness();
            Population.EndCurrentGeneration();

            if (GenerationRan != null)
                GenerationRan(this, EventArgs.Empty);

            if (GePrModel.GetTermination().HasReached(this))
            {
                //State = GeneticAlgorithmState.TerminationReached;

                if (TerminationReached != null)
                    TerminationReached(this, EventArgs.Empty);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Evaluates the fitness of each genome in the current generation.
        /// </summary>
        private async Task EvaluateFitness()
        {
            try
            {
                var genomesWithoutFitness =
                    Population.CurrentGeneration.Genomes.Where(c => !c.Fitness.HasValue).ToList();


                if (GePrModel.GetUseParalell())
                {
                    List<Task<Solution>> tasks = new List<Task<Solution>>();

                    foreach (IGenome genome in genomesWithoutFitness)
                    {
                        Task<Solution> t = new Task<Solution>(g =>
                        {
                            IFitness fitness = (IFitness)Activator.CreateInstance(GePrModel.GetFitness());
                            return fitness.Evaluate((IGenome)g);
                        }, genome);

                        tasks.Add(t);
                        t.Start();
                    }

                    await Task.WhenAll(tasks.ToArray());

                    for (int i = 0; i < genomesWithoutFitness.Count; i++)
                    {
                        Solution s = tasks[i].Result;
                        genomesWithoutFitness[i].Fitness = s.Fitness;
                        Log.A(s);
                    }
                }
                else
                {
                    for (var i = 0; i < genomesWithoutFitness.Count; i++)
                    {
                        var currentGenome = genomesWithoutFitness[i];

                        try
                        {
                            IFitness fitness = (IFitness)Activator.CreateInstance(GePrModel.GetFitness());

                            Solution solution = fitness.Evaluate(currentGenome);
                            Log.A(solution);

                            currentGenome.Fitness = solution.Fitness;
                        }
                        catch (Exception ex)
                        {
                            Log.D(ex.Message);
                            throw new Exception($"Error executing Fitness.Evaluate for genome: {currentGenome}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.D(ex.Message);
                throw ex;
            }

            Population.CurrentGeneration.Genomes =
                Population.CurrentGeneration.Genomes.OrderByDescending(c => c.Fitness.Value).ToList();
        }

        #endregion //Methods
    }
}