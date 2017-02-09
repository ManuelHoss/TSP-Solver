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
        private Dote backInDepot;
        private Address depotAddress;

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix, Dictionary<Address, Dictionary<Address, double>> durationMatrix, List<Address> addresses, Address depotAddress)
        {
            length = adresses.Count;
            this.depotAddress = depotAddress;
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
                foreach (Dote d in currentDotes)
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

            lastStepBackToDepot(currentDotes);
        }

        private void lastStepBackToDepot(List<Dote> currentDotes)
        {
            Dote depotDote = new Dote();
            double min = int.MaxValue;
            Dote shortestpath=null;

            foreach(Dote current in currentDotes)
            {
                if (current.getDistance() + getDistance(current.currentAddress(), depotAddress) < min)
                {
                    min = current.getDistance() + getDistance(current.currentAddress(), depotAddress);
                    shortestpath = current;
                }
            }
            depotDote.refreshDatas(shortestpath);
            //set doteBackInDepot in the end.
            throw new NotImplementedException();
        }

        private double getDistance(Address firstAddress, Address secondAddress)
        {
            return distanceMatrix[firstAddress][secondAddress];
            
            throw new NotImplementedException();
        }

        private void findPossiblePrecedence()
        {
            throw new NotImplementedException();
        }

        private List<Dote> makeNewStep(List<Dote> currentDotes)
        {
            List<Dote> newStep = new List<Dote>();
            foreach (Dote current in currentDotes)
            {

                foreach (Address notUsed in current.getNotUsedAddresses())
                {
                    if (isPossibleOrder(current.currentAddress(), notUsed))
                    {
                        Dote doteInChange = null;

                        if ((doteInChange = findDote(notUsed, newStep)) != null)
                        {
                            doteInChange.refreshDatas(current);
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

        private Dote findDote(Address notUsed, List<Dote> newStep)
        {

            foreach (Dote d in newStep)
            {
                if (d.isAddress(notUsed))
                {
                    return d;
                }
            }
            return null;
        }

        private bool isPossibleOrder(Address current, Address notUsed)
        {
            return true;
        }
    }
}
