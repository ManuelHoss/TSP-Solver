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

        private List<Node> allDotes;
        private Node backInDepot;
        private Address depotAddress;
        private Dictionary<Address, Dictionary<Address, double>> durationMatrix;

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix, Dictionary<Address, Dictionary<Address, double>> durationMatrix, List<Address> addresses, Address depotAddress)
        {
            length = adresses.Count;
            this.depotAddress = depotAddress;
            this.adresses = adresses;
            this.distanceMatrix = distanceMatrix;
            this.durationMatrix = durationMatrix;
            findprecedence();
            builtGraph();
            return findShortesRoute();
            //return null;
        }

        private Route findShortesRoute()
        {
            Route r =new Route();
            double duration = 0;
            Node n = backInDepot;
            while ((n=n.getNodeBefore()) != null)
            {
                r.Addresses.Add(n.currentAddress());
            }
            return r;
            
        }


        //TODO
        private void findprecedence()
        {
            return;
            
        }
        //sollte fertig sein
        private void builtGraph()
        {
            List<Node> currentDotes = new List<Node>();
            currentDotes.Add(new Node(depotAddress, adresses));
            for (int i = 0; i < length; i++)
            {
                foreach (Node d in currentDotes)
                {
                    allDotes.Add(d);
                }
                currentDotes = makeNewStep(currentDotes, i);
                findPossiblePrecedence();
            }
            //zurück zum depot knoten fehlt noch
            foreach (Node d in currentDotes)
            {
                allDotes.Add(d);
            }

            lastStepBackToDepot(currentDotes);
        }
        //sollte fertig sein
        private void lastStepBackToDepot(List<Node> currentDotes)
        {
            Node depotDote = new Node();
            double min = int.MaxValue;
            Node shortestpath=null;

            foreach(Node current in currentDotes)
            {
                if (current.getDuration() + getDuration(current.currentAddress(), depotAddress) < min)
                {
                    min = current.getDuration() + getDuration(current.currentAddress(), depotAddress);
                    shortestpath = current;
                }
            }
            depotDote.refreshDatas(shortestpath,min);
            //set doteBackInDepot in the end.
            backInDepot = depotDote;
            
        }
        //ready
        /**
         * 
         */
        private double getDuration(Address firstAddress, Address secondAddress)
        {
            return durationMatrix[firstAddress][secondAddress];
            
        }
        //TODO
        private void findPossiblePrecedence()
        {
            
        }
        //ready
        private List<Node> makeNewStep(List<Node> currentDotes, int step)
        {
            List<Node> newStep = new List<Node>();
            foreach (Node current in currentDotes)
            {

                foreach (Address notUsed in current.getNotUsedAddresses())
                {
                    if (isPossibleOrder(current.currentAddress(), notUsed))
                    {
                        Node doteInChange = null;

                        if ((doteInChange = findDote(notUsed, newStep)) != null)
                        {
                            doteInChange.refreshDatas(current, getDuration(current.currentAddress(), doteInChange.currentAddress()));
                        }
                        else
                        {
                            double duration = getDuration(current.currentAddress(), notUsed);
                            newStep.Add(new Node(current,duration, step,notUsed,adresses));
                        }
                    }
                }

            }
            return newStep;
        }
        //ready
        private Node findDote(Address notUsed, List<Node> newStep)
        {

            foreach (Node d in newStep)
            {
                if (d.isAddress(notUsed))
                {
                    return d;
                }
            }
            return null;
        }
        //TODO
        private bool isPossibleOrder(Address current, Address notUsed)
        {
            return true;
        }
    }
}
