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


        private List<Node> allNodes;
        private Node backInDepot;
        private Address depotAddress;
        private Dictionary<Address, Dictionary<Address, double>> durationMatrix;

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> durationMatrix, List<Address> addresses, Address depotAddress)
        {
            length = addresses.Count;
            this.depotAddress = depotAddress;
            this.adresses = addresses;
            this.durationMatrix = durationMatrix;
            allNodes = new List<Node>();
            adresses.Remove(depotAddress); //entfernung des depotknoten
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
            r.Addresses.Add(n.getAddress());
            while ((n=n.getNodeBefore()) != null)
            {
                r.Addresses.Add(n.getAddress());
            }
            r.Duration = backInDepot.getDuration();
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
            List<Node> currentNodes = new List<Node>();
            currentNodes.Add(new Node(depotAddress, adresses));

            for (int i = 0; i < length-1; i++)
            {
                foreach (Node d in currentNodes)
                {
                    allNodes.Add(d);
                }

                currentNodes = makeNewStep(currentNodes, i); //rückgabe wert passt anscheinde nicht
                findPossiblePrecedence();
            }
            //zurück zum depot knoten fehlt noch
            foreach (Node d in currentNodes)
            {
                allNodes.Add(d);
            }

            lastStepBackToDepot(currentNodes);
        }


        //sollte fertig sein
        private void lastStepBackToDepot(List<Node> currentNodes)
        {
            Node depotNode = new Node();
            double min = double.MaxValue;
            Node shortestpath=null;

            foreach(Node current in currentNodes)
            {
                if (current.getDuration() + getDuration(current.getAddress(), depotAddress) < min)
                {
                    min = current.getDuration() + getDuration(current.getAddress(), depotAddress);
                    shortestpath = current;
                }
            }
            depotNode.refreshDatas(shortestpath,min );
            //set NodeBackInDepot in the end.
            backInDepot = depotNode;
        }

        private double getDuration(Address firstAddress, Address secondAddress)
        {
            return durationMatrix[firstAddress][secondAddress];
        }
 


        //TODO
        private void findPossiblePrecedence()
        {
            
        }


        //ready
        private List<Node> makeNewStep(List<Node> currentNodes, int step)
        {
            List<Node> newStep = new List<Node>();
            foreach (Node current in currentNodes)
            {
                foreach (Address notUsed in current.getNotUsedAddresses())
                {
                    if (isPossibleOrder(current.getAddress(), notUsed))
                    {
                        double duration = getDuration(current.getAddress(), notUsed);
                        Node newNode =new Node(current, duration, step, notUsed, adresses);
                        Node  NodeInChange=null;
                        if ((NodeInChange=findNode(newNode,newStep))==null) {
                            newStep.Add(newNode);
                        }
                        else
                        {
                            NodeInChange.refreshDatas(current, getDuration(current.getAddress(), NodeInChange.getAddress()));
                        }
                    }    
                }

            }
            return newStep;
        }


        //TODO
        private Node findNode(Node newNode, List<Node> newStep)
        {
            foreach (Node d in newStep)
            {
                if (d.Equals(newNode))
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
