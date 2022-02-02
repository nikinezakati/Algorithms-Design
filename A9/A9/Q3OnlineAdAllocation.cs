using System;
using TestCommon;

namespace A9
{
    public class Q3OnlineAdAllocation : Processor
    {

        public Q3OnlineAdAllocation(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int c, int v, double[,] matrix1)
        {
            // Comment the line below and write your code here
            throw new NotImplementedException();
        }
    }
}
