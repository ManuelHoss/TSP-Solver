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
        private Node doteBefore=null;
        private Address address;
        private int step;
        private double distance; //wird nicht verwendet
        private double duration;
        private List<Address> notUsedAddresses;
        

        public Node()
        {

        }

        public Node(Address location, List<Address> notUsedAddresses)
        {
            this.address = location;
            this.notUsedAddresses = notUsedAddresses;
        }



        public Node(Node current, double duration,int step, Address notUsed, List<Address> adresses)
        {
            this.doteBefore = current;
            this.address = notUsed;
            setNotUsedAdresses(adresses);
            this.address = notUsed;       
            this.step = step;
        }
        
        private void setNotUsedAdresses(List<Address> adresses)
        {
            notUsedAddresses = doteBefore.getNotUsedAddresses();
            notUsedAddresses.Remove(address);
        }

        internal Node getNodeBefore()
        {
            return doteBefore;
        }

        
        internal void refreshDatas(Node current,double newDuration)
        {
            if(newDuration < duration)
            {
                duration = newDuration;
                doteBefore = current;
            }
        }

        internal double getDuration()
        {
            return duration;
        }

        internal double getDistance()
        {
            return distance;
        }

        internal Address currentAddress()
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
    }
}
