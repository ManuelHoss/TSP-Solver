using System;
using System.Collections.Generic;
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
      private Address depotAddress;

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix, List<Address> addresses, Address depotAddress)
      {
         length = adresses.Count;
         this.depotAddress=depotAddress;
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
         currentDotes.Add(new Dote(depotAddress));
         for (int i = 0; i < length; i++)
         {
            foreach(Dote d in currentDotes)
                {
                    allDotes.Add(d);
                }
            currentDotes = makeNewStep(currentDotes);
                findPossiblePrecedence();
         }
            //zurück zum depot knoten fehlt noch
            foreach (Dote d in currentDotes)
            {
                allDotes.Add(d);
            }
            lastStepBackToDepot(currentDotes, depotAddress);
        }

        private void lastStepBackToDepot(List<Dote> currentDotes, Address depotAddress)
        {
            throw new NotImplementedException();
        }

        private void findPossiblePrecedence()
        {
            throw new NotImplementedException();
        }

        private List<Dote> makeNewStep(List<Dote> currentDotes)
      {
            List<Dote> newStep = new List<Dote>();
        foreach(Dote current in currentDotes){
                foreach(Dote notUsed in current.getNotUsedDotes())
                {
                    if (isPossibleOrder(current,notUsed))
                    {
                        if (doteAlreadyExists(notUsed))
                        {

                        }
                        else
                        {
                            newStep.Add(new Dote());
                        }
                    }
                }

            }
            return newStep;
      }

        private bool doteAlreadyExists(Dote notUsed)
        {
            return false;
        }

        private bool isPossibleOrder(Dote current, Dote notUsed)
        {
            return true;
        }
    }
}
