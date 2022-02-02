using System;
using System.Collections.Generic;
using TestCommon;

namespace A1
{
    public class Q1MazeExit : Processor
    {
        public Q1MazeExit(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);

        public long Solve(long nodeCount, long[][] edges, long StartNode, long EndNode)
        {
            int m=edges.Length;
            List<long>[] adjacency=new List<long>[nodeCount];
            for (int j = 0; j < nodeCount; j++)
            {
                adjacency[j]=new List<long>();
            }
            bool[] Visited=new bool[nodeCount];
            for (int i = 0; i < m; i++)
            {
                long x=edges[i][0];
                long y=edges[i][1];
                adjacency[x-1].Add(y-1);
                adjacency[y-1].Add(x-1);
            }
            return Explore(adjacency,StartNode-1,EndNode-1,Visited);
        }

        private long Explore(List<long>[] adjacency, long x, long y, bool[] Visited)
        {
            if (x == y)
                return 1;
            Visited[x] = true;
            for(int i = 0; i < adjacency[x].Count; i++) 
            {
                if (!Visited[adjacency[x][i]]) 
                {
                    if(Explore(adjacency, adjacency[x][i], y, Visited)==1)
                        return 1;
                }
            }
            return 0;
        }
    }
}
