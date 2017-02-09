using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPSolver.Interfaces;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.DynamicProgramming
{
   class TspSolver_DynamicProgramming : ITspSolver
   {
        private int length;
        private List<Address> adresses;
        private Dictionary<Address, Dictionary<Address, double>> distanceMatrix;

        private List<Dote> allDotes;

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix, List<Address> addresses)
      {
            length = adresses.Count;
            this.adresses = adresses;
            this.distanceMatrix = distanceMatrix;
            findprecedence();
            builtGraph();
            return null;
      }

        private void findprecedence()
        {
            return;
            throw new NotImplementedException("not done jet");
        }

        private void builtGraph()
        {
            List<Dote> currentDotes = new List<Dote>();
            currentDotes.Add(new Dote());
            for (int i = 0; i <= length; i++)
            {
                currentDotes = makeNewStep(currentDotes);

            }
        }

        private List<Dote> makeNewStep(List<Dote> currentDotes)
        {
            throw new NotImplementedException();
        }
    }
}
