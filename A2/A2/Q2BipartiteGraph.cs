using System;
using System.Collections.Generic;
using TestCommon;

namespace A2
{
    public class Q2BipartiteGraph : Processor
    {
        public Q2BipartiteGraph(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long NodeCount, long[][] edges)
        {
            int m=edges.Length;
            List<long>[] adjacency=new List<long>[NodeCount];
            for (int j = 0; j < NodeCount; j++)
            {
                adjacency[j]=new List<long>();
            }
            for (int i = 0; i < m; i++)
            {
                long x=edges[i][0];
                long y=edges[i][1];
                adjacency[x-1].Add(y-1);
                adjacency[y-1].Add(x-1);
            }
            return Bipartite(adjacency);
        }

        private long Bipartite(List<long>[] adjacency)
        {
            long[] color=new long[adjacency.Length];
            for (int i = 0; i < adjacency.Length; i++)
            {
                color[i]=-1;
            }

            color[0]=1;
            Queue<long> q=new Queue<long>();
            q.Enqueue(0);

            while(q.Count>0)
            {
                long u=q.Dequeue();
                for (int j = 0; j < adjacency[u].Count; j++)
                {
                    long v=adjacency[u][j];
                    if(color[v]==-1)
                    {
                        color[v]=1-color[u];
                        q.Enqueue(v);
                    }
                    else if(color[v]==color[u])
                        return 0;
                }
            }
            return 1;
        }
    }
}
