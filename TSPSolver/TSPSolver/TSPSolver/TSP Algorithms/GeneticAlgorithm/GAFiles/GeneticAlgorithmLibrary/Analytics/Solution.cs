using System;

namespace PortableGeneticAlgorithm.Analytics
{
    public class FinishedSolution : Solution
    {
    }
    /// <summary>
    ///     Represents informations about a found solution, useed for exchang within the programm
    /// </summary>
    /*[Serializable]*/
    public abstract class Solution
    {
        public string Id { get; }
        public double Fitness { get; }
        public double EvaluationTime { get; }
        public int Generation { get; }

        /// <summary>
        /// Creates a dummy solution!
        /// </summary>
        protected Solution()
        {
            Id = Guid.NewGuid().ToString();
            Fitness = double.NaN;
            EvaluationTime = double.NaN;
            Generation = int.MinValue;
        }

        protected Solution(double evaluationTime, int generation, double fitness) : this()
        {

            EvaluationTime = evaluationTime;
            Generation = generation;
            Fitness = fitness;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Solution)
                return Id.Equals(((Solution)obj).Id);
            return false;
        }

        /// <summary>
        ///     Returns a hashcode for the solution
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}