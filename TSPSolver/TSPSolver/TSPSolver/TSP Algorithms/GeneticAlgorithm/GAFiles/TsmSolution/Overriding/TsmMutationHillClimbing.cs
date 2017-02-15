using System;
using System.Collections.Generic;
using System.Linq;
using PortableGeneticAlgorithm.Interfaces;

namespace PortableTsmSolution.Overriding
{
    public class TsmMutationHillClimbing : IMutation
    {
        public IGenome Mutate(IGenome genome, double mutationProbability)
        {
            throw new NotImplementedException();
        }
    }
}