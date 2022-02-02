using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace A10
{
    public class Q2CleaningApartment : Processor
    {
        public Q2CleaningApartment(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public String[] Solve(int V, int E, long[,] matrix)
        {
            Graph graph=new Graph(V, E);
            for (int k = 0; k < E; ++k) 
            {
                long i =matrix[k,0];
                long j =matrix[k,1];
                graph.matrix[i - 1][(int)j - 1] = true;
                graph.matrix[j - 1][(int)i - 1] = true;
            }
            graph.SAT(120000);
            return graph.clauses.ToArray();
        }
    }
    public class Graph 
    {
        int clausesNum;
        int verticesNum;
        public bool[][] matrix;
        int[][] data;
        string clauses_stream;
        public Graph(int n, int m)
        {
            this.clausesNum=0;
            this.verticesNum=n;
            this.matrix=new bool[n][];
            this.data=new int[n][];
            for (int i = 0, cnt = 0; i < verticesNum; ++i) 
            {
                matrix[i]=new bool[n];
                Array.Fill(matrix[i],false);
                data[i]=new int[n];
                for (int j = 0; j < verticesNum; ++j) 
                {
                    data[i][j] = ++cnt;
                }
            }
        }
        public List<string> clauses=new List<string>();
        internal void SAT(int max_clausesNum)
        {
            VertexInPath();
            VertexInPathOnlyOnce();
            AtMostOneInPath();
            ExactlyOneInPosition();
            OnlyAdjInPath();
            string temp=$"{clausesNum} {verticesNum * verticesNum}";
            clauses.Add(clauses_stream);
            clauses.Insert(0,temp);      
            
        }
        void VertexInPath() 
        {
            for (int i = 0; i < verticesNum; ++i, ++clausesNum) 
            {
                for (int j = 0; j < verticesNum; ++j) 
                {
                    clauses_stream += $"{data[i][j]} ";
                }
                clauses_stream += "0";
                clauses.Add(clauses_stream);
            }
        }
        void VertexInPathOnlyOnce() 
        {
            for (int i = 0; i < verticesNum; ++i) 
            {
                for (int j = 0; j < verticesNum; ++j) 
                {
                    for (int k = i + 1; k < verticesNum; ++k, ++clausesNum) 
                    {
                        clauses_stream += $"{-data[i][j]} {-data[k][j]} 0";
                        clauses.Add(clauses_stream);
                    }
                }
            }
        }
        void AtMostOneInPath() 
        {
            for (int i = 0; i < verticesNum; ++i, ++clausesNum) {
                for (int j = 0; j < verticesNum; ++j) {
                    clauses_stream += $"{data[j][i]} ";
                }
                clauses_stream += "0";
                clauses.Add(clauses_stream);
            }
        }
        void ExactlyOneInPosition() 
        {
            for (int i = 0; i < verticesNum; ++i) 
            {
                for (int j = 0; j < verticesNum; ++j) 
                {
                    for (int k = j + 1; k < verticesNum; ++k, ++clausesNum) 
                    {
                        clauses_stream += $"{-data[i][j]} {-data[i][k]} 0";
                        clauses.Add(clauses_stream);
                    }
                }
            }
        }
        void OnlyAdjInPath() 
        {
            for (int i = 0; i < verticesNum; ++i) 
            {
                for (int j = 0; j < verticesNum; ++j) 
                {
                    if (!matrix[i][j] && j != i) 
                    {
                        for (int k = 0; k < verticesNum - 1; ++k, ++clausesNum) 
                        {
                            clauses_stream += $"{-data[i][k]} {-data[j][k + 1]} 0";
                            clauses.Add(clauses_stream);
                        }
                    }
                }
            }
        }
    }    
}
