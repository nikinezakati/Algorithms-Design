using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q3ExchangingMoney : Processor
    {
        public Q3ExchangingMoney(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, string[]>)Solve);

        public string[] Solve(long nodeCount, long[][] edges, long startNode)
        {
            List<string> results=new List<string>();
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
            long s=startNode-1;
            long[] distance = new long[nodeCount];
            int[] reachable = new int[nodeCount];
            int[] shortest = new int[nodeCount];
            for (int i = 0; i < nodeCount; i++) 
            {
                distance[i] = long.MaxValue;
                reachable[i] = 0;
                shortest[i] = 1;
            }
            ShortestPaths(adjacency, costs, s, distance, reachable, shortest);
            for (int i = 0; i < nodeCount; i++) 
            {
                if (reachable[i] == 0) 
                {
                    results.Add("*");
                } 
                else if (shortest[i] == 0) 
                {
                    results.Add("-");
                } 
                else 
                {
                    results.Add(distance[i].ToString());
                }
            }
            return results.ToArray();
        }

        private void ShortestPaths(List<long>[] adjacency, List<long>[] costs, long s, long[] distance, int[] reachable, int[] shortest)
        {
            distance[s] = 0;
            reachable[s] = 1;
            Queue <long> queue = new Queue<long>();
            for (int i = 0; i < adjacency.Length; i++) 
            {
                for (int u = 0; u < adjacency.Length; u++) 
                {
                    for (int k = 0; k < adjacency[u].Count; k++)
                    {
                        long v=adjacency[u][k];
                        if(distance[u]!=long.MaxValue && distance[v]>distance[u]+costs[u][k])
                        {
                            distance[v] = distance[u] + costs[u][k];
                            reachable[v] = 1;
                            if(i==adjacency.Length-1)
                                queue.Enqueue(v);
                        }
                    }
                }
            }
            int[] visited = new int[adjacency.Length];
            while (queue.Count>0) 
            {
                long u = queue.Dequeue();
                visited[u] = 1;
                shortest[u] = 0;
                for (int i = 0; i < adjacency[u].Count; i++)
                {
                    long v=adjacency[u][i];
                    if(visited[v]==0)
                    {
                        queue.Enqueue(v);
                        visited[v]=1;
                        shortest[v]=0;
                    }
                }
            }
            distance[s] = 0;
        }
    }
}
