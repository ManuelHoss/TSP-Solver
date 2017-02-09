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
        private Address location;
        public Dote()
        {

        }
        public Dote(Address location)
        {
            this.location = location;
        }
        public Dote(int val, int[] currentAmount)
        {
            int n = 1+currentAmount.Length;
            int[] Amount =new int[n];
            int value = val;


        }

        internal List<Dote> getNotUsedDotes()
        {
            throw new NotImplementedException();
        }
    }
}
