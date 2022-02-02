using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q2DetectingAnomalies:Processor
    {
        public Q2DetectingAnomalies(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long[] dist;

        public long Solve(long nodeCount, long[][] edges)
        {
            int m=edges.Length;
            List<long>[] adjacency=new List<long>[nodeCount];
            List<long>[] costs=new List<long>[nodeCount];
            for (int j = 0; j < nodeCount; j++)
            {
                adjacency[j]=new List<long>();
                costs[j]=new List<long>();
            }
            for (int i = 0; i < m; i++)
            {
                long x=edges[i][0];
                long y=edges[i][1];
                long c=edges[i][2];
                adjacency[x-1].Add(y-1);
                costs[x-1].Add(c);
            }
            return NegativeCycle(adjacency,costs);
        }

        private long NegativeCycle(List<long>[] adjacency, List<long>[] costs)
        {
            long[] dist = new long[adjacency.Length];
            for (int i = 0;i < dist.Length;i++) 
            {
                dist[i] = int.MaxValue;
            }
            dist[0] = 0;
            for (int i = 0; i < adjacency.Length; i++)
            {
                for (int j = 0; j < adjacency.Length; j++)
                {
                    for (int k = 0; k < adjacency[j].Count; k++)
                    {
                        long u=adjacency[j][k];
                        if(dist[j]!=long.MaxValue && dist[u]>dist[j]+costs[j][k])
                        {
                            dist[u]=dist[j]+costs[j][k];
                            if(i==adjacency.Length-1)
                                return 1;
                        }
                    }
                }
            }
            return 0;
        }
    }
}

