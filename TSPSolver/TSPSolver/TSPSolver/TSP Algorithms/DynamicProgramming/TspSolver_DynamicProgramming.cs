using System;
using System.Collections.Generic;
using TSPSolver.Interfaces;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.DynamicProgramming
{
    class Tsp_Solver_BeanchSearch : ITspSolver
    {
        int beanRange;

        public void setBeanRange(int range)
        {
            this.beanRange = range;
        }

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> adjacencyMatrix, List<Address> addresses, Address depotAddress)
        {
            TspSolver_DynamicProgramming solver = new TspSolver_DynamicProgramming();
            solver.useBeanSearch(beanRange);
            return solver.CalculateShortestRoute(adjacencyMatrix, addresses, depotAddress);
        }

    }

    class TspSolver_DynamicProgramming : ITspSolver
    {
        private int length;
        private List<Address> adresses;
        private Node backInDepot;
        private Address depotAddress;
        private Dictionary<Address, Dictionary<Address, double>> durationMatrix;
        private bool beanSearchIsTrue =false;
        private int beanRange;

        public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> durationMatrix, List<Address> addresses, Address depotAddress)
        {
            length = addresses.Count;
            this.depotAddress = depotAddress;
            this.adresses = addresses;
            this.durationMatrix = durationMatrix;
            adresses.Remove(depotAddress); 
            findprecedence();
            builtGraph();
            return findShortesRoute();
            
        }

        private Route findShortesRoute()
        {
            Route r =new Route();
            
            Node n = backInDepot;
            r.Addresses.Add(n.getAddress());
            while ((n=n.getNodeBefore()) != null)
            {
                r.Addresses.Add(n.getAddress());
            }
            r.Duration = backInDepot.getDuration();
            return r;
        }

        public void useBeanSearch(int range)
        {
            this.beanRange = range;
            this.beanSearchIsTrue = true;
        }

        //TODO here are the prototype for timewindows
        private void findprecedence()
        {
            return;
            
        }


        
        private void builtGraph()
        {
            List<Node> currentNodes = new List<Node>();
            currentNodes.Add(new Node(depotAddress, adresses));

            for (int i = 0; i < length-1; i++)
            {
                currentNodes = makeNewStep(currentNodes, i); 
                currentNodes = findPossiblePrecedence(currentNodes);
                if (beanSearchIsTrue)
                {
                    currentNodes = useBeanSearch(currentNodes);

                }
            }
            lastStepBackToDepot(currentNodes);
        }

        //this is only possible if you use the Beansearch
        private List<Node> useBeanSearch(List<Node> currentNodes)
        {
            List<Node> tmp = new List<Node>();
            currentNodes.Sort((x, y) => x.getDuration().CompareTo(y.getDuration()));
            for(int i = 0; i < beanRange; i++)
            {
                tmp.Add(currentNodes[i]);
            }
            return tmp;
        }

        private List<Node> findPossiblePrecedence(List<Node> currentNodes)
        {
            List<Node> tmp = new List<Node>();
            foreach(Node temp in currentNodes)
            {
                tmp.Add(temp);
            }
            foreach(Node current in currentNodes)
            {
                
                foreach (Address t in current.getNotUsedAddresses()) {
                    if (!isPossibleOrder(current.getAddress(), t)){
                        tmp.Remove(current);
                        break;
                    }
                }
            }
            return tmp;
            
        }

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
            
            backInDepot = depotNode;
        }

        private double getDuration(Address firstAddress, Address secondAddress)
        {
            return durationMatrix[firstAddress][secondAddress];
        }


        
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


        //TODO here are the protoype for timewindows
        private bool isPossibleOrder(Address current, Address notUsed)
        {
            return false;
        }
    }

}
