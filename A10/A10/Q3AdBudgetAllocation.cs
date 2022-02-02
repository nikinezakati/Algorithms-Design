using System;
using System.Collections;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q3AdBudgetAllocation : Processor
    {
        public Q3AdBudgetAllocation(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public string[] Solve(long eqCount, long varCount, long[][] A, long[] b)
        {
            Equation e=new Equation(eqCount, varCount);
            e.A=A;
            e.b=b;
            return e.SAT();
        }
    }
    public class Equation 
    {
        public long[][] A;
        public long[] b;
        public string clause;
        public BitArray matrix=new BitArray(3);
        public Equation(long n, long m)
        {
            A=new long[n][];
            for (int i = 0; i < n; i++)
            {
                A[i]=new long[m];
            }
            b=new long[n];
        }
        public List<string> clauses=new List<string>();
        internal string[] SAT()
        {
            int cnt = 0;
            for(int i = 0; i < A.Length; ++i) 
            {
                var row = A[i];
                int n =0;
                for (int j = 0; j < row.Length; j++)
                {
                    if(row[j]!= 0)
                        n+=1;
                } 
                for(int j = 0, s = (int)Math.Pow(2, n); j < s; ++j) 
                {
                    matrix = new BitArray(new int[] { j });

                    long sum = 0, comb = 0;
                    foreach(var x in row) 
                    {
                        if(x != 0 && matrix[(int)comb++]) 
                            sum += x;
                    }
                    if (sum > b[i]) {
                        bool is_clause = false;
                        comb=0;
                        for(int k = 0; k < row.Length; ++k) 
                        {
                            if(row[k] != 0) {
                                clause += matrix[(int)comb] ? ((-(k+1)).ToString() + ' ') : ((k+1).ToString() + ' ');
                                ++comb;
                                is_clause = true;
                            }
                        }
                        if(is_clause) 
                        {
                            clause += "0";
                            clauses.Add(clause);
                            clause="";
                            ++cnt;
                        }
                    }
                }    
            }
            if(cnt == 0) 
            {
                cnt++;
                clause += "1 -1 0";
                clauses.Add(clause);
                clause="";
            }
            var temp=$"{cnt} {A[0].Length} ";
            clauses.Insert(0,temp);      
            return clauses.ToArray();
        }
    }    
}
