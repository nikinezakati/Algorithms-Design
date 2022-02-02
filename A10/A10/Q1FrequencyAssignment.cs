using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q1FrequencyAssignment : Processor
    {
        public Q1FrequencyAssignment(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);


        public String[] Solve(int V, int E, long[,] matrix)
        {
            clauses=new List<string>();
            ExactlyOneOf(V);
            for (int i = 0; i < matrix.Length / 2; i++)
                AtMostOneOf(matrix[i, 0], matrix[i, 1]);  
            clauses.Insert(0, $"{clauses.Count} {V * 3 * 2}");      
            return clauses.ToArray();
        }

        private void AtMostOneOf(long v1, long v2)
        {
            clauses.Add($"-{(v1 - 1) * 3 + 1}  -{(v2 - 1) * 3 + 1} 0");
            clauses.Add($"-{(v1 - 1) * 3 + 2}  -{(v2 - 1) * 3 + 2} 0");
            clauses.Add($"-{(v1 - 1) * 3 + 3}  -{(v2 - 1) * 3 + 3} 0");
        }

        public List<string> clauses=new List<string>();
        
        private void ExactlyOneOf(int V)
        {
            for (int i = 0; i < V; i++)
            {
                string temp1 = $"{i * 3 + 1}";
                string temp2 = $"{i * 3 + 2}";
                string temp3 = $"{i * 3 + 3}";
                string temp = $"{temp1} {temp2} {temp3} 0";
                clauses.Add(temp);

            }
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

    }
}
