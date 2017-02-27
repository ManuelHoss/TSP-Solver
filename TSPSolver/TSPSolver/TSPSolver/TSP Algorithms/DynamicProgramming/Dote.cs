using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.DynamicProgramming
{
    class Node
    {   
        private Node NodeBefore=null;
        private Address address;
        private int step;
       
        private double duration;
        private List<Address> notUsedAddresses;
        

        public Node()
        {
            duration = double.MaxValue;
        }

        public Node(Address location, List<Address> notUsedAddresses)
        {
            this.address = location;
            this.notUsedAddresses = notUsedAddresses;
        }



        public Node(Node current, double duration,int step, Address notUsed, List<Address> adresses)
        {
            this.NodeBefore = current;
            this.address = notUsed;
            setNotUsedAdresses(adresses);
            this.address = notUsed;       
            this.step = step;
            
        }
        
        private void setNotUsedAdresses(List<Address> adresses)
        {
            notUsedAddresses = new List<Address>();
            foreach(Address a in NodeBefore.getNotUsedAddresses())
            {
                notUsedAddresses.Add(a);
            }
            //notUsedAddresses = NodeBefore.getNotUsedAddresses();
            notUsedAddresses.Remove(address);
        }
        public void removeNotUsedAdress(Address toRemove)
        {
            notUsedAddresses.Remove(toRemove);
        }

        internal Node getNodeBefore()
        {
            return NodeBefore;
        }

        
        internal void refreshDatas(Node current,double newDuration)
        {
            if(newDuration < duration)
            {
                duration = newDuration;
                NodeBefore = current;
                
            }
        }

        internal double getDuration()
        {
            return duration;
        }



        internal Address getAddress()
        {
            return address;
        }

        
        internal List<Address> getNotUsedAddresses()
        {
            return notUsedAddresses;
        }

        internal bool isAddress(Address notUsed)
        {
            return address.Id == notUsed.Id;
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Node p = obj as Node;
            if ((System.Object)p == null)
            {
                return false;
            }

            if (p.getAddress().Equals(this.address) && p.step == this.step)
            {
                foreach(Address a in p.getNotUsedAddresses())
                {
                    if (!notUsedAddresses.Contains(a))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
