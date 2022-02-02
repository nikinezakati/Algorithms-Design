using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace A6
{
    public class Q2ReconstructStringFromBWT : Processor
    {
        public Q2ReconstructStringFromBWT(string testDataName) 
        : base(testDataName) {}

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Reconstruct a string from its Burrows–Wheeler transform
        /// </summary>
        /// <param name="bwt"> A string Transform with a single “$” sign </param>
        /// <returns> The string Text such that BWT(Text) = Transform.
        /// (There exists a unique such string.) </returns>
        public string Solve(string bwt)
        {
            StringBuilder result = new StringBuilder();
            List<(char,int)> matrix = new List<(char,int)>();
            for (int i = 0; i < bwt.Length; i++) 
            {
                matrix.Add((bwt[i],i));
            }
            matrix.Sort();
            var current = matrix[0];
            for (int i = 0; i < bwt.Length; i++) 
            {
                current=matrix[current.Item2];
                result.Append(current.Item1);
            }
            
            return result.ToString();  
        }
    }
}
