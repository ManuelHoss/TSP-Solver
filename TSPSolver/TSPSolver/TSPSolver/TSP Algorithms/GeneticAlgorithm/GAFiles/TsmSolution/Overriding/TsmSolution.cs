using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableGeneticAlgorithm.Analytics;

namespace PortableTsmSolution.Overriding
{
    public class TsmSolution : Solution
    {
        public TsmSolution() : base()
        {

        }

        public TsmSolution(double evaluationTime, int generation, double fitness) : base(evaluationTime, generation, fitness)
        {
        }
    }
}