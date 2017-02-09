using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.DynamicProgramming
{
    class Dote
    {
        private Address address;
        private double distance;

        public Dote()
        {

        }
        public Dote(Address location)
        {
            this.address = location;
        }
        public Dote(int val, int[] currentAmount)
        {
            int n = 1+currentAmount.Length;
            int[] Amount =new int[n];
            int value = val;


        }

        internal List<Address> getNotUsedAddresses()
        {
            throw new NotImplementedException();
        }

        internal Address currentAddress()
        {
            return null;
            throw new NotImplementedException();
        }

        internal bool isAddress(Address notUsed)
        {
            return address.Equals(notUsed); //TODO write the equals 
            throw new NotImplementedException();
        }

        internal void refreshDatas(Dote current)
        {
            throw new NotImplementedException();
        }

        internal double getDistance()
        {
            return distance;
            throw new NotImplementedException();
        }
    }
}
