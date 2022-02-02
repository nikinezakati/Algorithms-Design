using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q2AddExitToMaze : Processor
    {
        public Q2AddExitToMaze(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            long result=0;
            int m=edges.Length;
            List<long>[] adjacency=new List<long>[nodeCount];
            for (int j = 0; j < nodeCount; j++)
            {
                adjacency[j]=new List<long>();
            }
            bool[] Visited=new bool[nodeCount];
            for (int a = 0; a < nodeCount; a++)
            {
                Visited[a]=false;
            }
            for (int i = 0; i < m; i++)
            {
                long x=edges[i][0];
                long y=edges[i][1];
                adjacency[x-1].Add(y-1);
                adjacency[y-1].Add(x-1);
            }
            for (long q = 0; q < nodeCount; q++)
            {
                if (!Visited[q])
                {
                    Explore(adjacency, q, Visited);
                    result++;
                }
            }
            return result;
        }

        private void Explore(List<long>[] adjacency, long x, bool[] visited)
        {
            Stack<long> s=new Stack<long>();
            s.Push(x);
            while(s.Count!=0)
            {
                long q=s.Pop();
                visited[q]=true;
                for (int i = 0; i < adjacency[q].Count; i++)
                {
                    if(!visited[adjacency[q][i]])
                        s.Push(adjacency[q][i]);
                }
            }
        }
    }
}
