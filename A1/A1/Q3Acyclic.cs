using System;
using System.Collections.Generic;
using TestCommon;

namespace A1
{
    public class Q3Acyclic : Processor
    {
        public Q3Acyclic(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            int m=edges.Length;
            List<long>[] adjacency=new List<long>[nodeCount];
            for (int j = 0; j < nodeCount; j++)
            {
                adjacency[j]=new List<long>();
            }
            int[] Visited=new int[nodeCount];
            int[] Recurse=new int[nodeCount];
            for (int v = 0; v < nodeCount; v++)
            {
                Visited[v]=0;
                Recurse[v]=0;
            }
            for (int i = 0; i < m; i++)
            {
                long x=edges[i][0];
                long y=edges[i][1];
                adjacency[x-1].Add(y-1);
            }
            for (int q = 0; q < nodeCount; q++)
            {
                if (Visited[q]==0) 
                {
                    if(DFS(adjacency, q, Visited, Recurse)==1) 
                        return 1;
                }
            }
            return 0;
        }

        private int DFS(List<long>[] adjacency, long x, int[] visited, int[] recurse)
        {
            visited[x] = 1;
            recurse[x] = 1;
            for (int i = 0; i < adjacency[x].Count; i++) 
            {
                if (visited[adjacency[x][i]]==0 && DFS(adjacency, adjacency[x][i], visited, recurse)==1)
                    return 1;
                else if(recurse[adjacency[x][i]]==1)
                    return 1;
            }
            recurse[x] = 0;
            return 0;
        }
    }
}