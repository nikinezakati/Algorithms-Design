using System;
using System.Collections.Generic;
using TestCommon;

namespace A2
{
    public class Q1ShortestPath : Processor
    {
        public Q1ShortestPath(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long[][], long, long, long>)Solve);
        
        public long Solve(long NodeCount, long[][] edges, 
                          long StartNode,  long EndNode)
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
            long S=StartNode-1;
            long E=EndNode-1;
            return ShortestPath(adjacency,S,E);
        }

        private long ShortestPath(List<long>[] adjacency, long StartNode, long EndNode)
        {
            if(StartNode==EndNode)
                return 0;

            long[]distance=new long[adjacency.Length];

            for (int i = 0; i < adjacency.Length; i++)
            {
                distance[i]=long.MaxValue;
            }
            distance[StartNode]=0;
            Queue<long> nodes=new Queue<long>();
            nodes.Enqueue(StartNode);
            
            while(nodes.Count>0)
            {
                long u=nodes.Dequeue();

                for (int i = 0; i < adjacency[u].Count; i++)
                {
                    long v=adjacency[u][i];
                    if(distance[v]==long.MaxValue)
                    {
                        nodes.Enqueue(v);
                        distance[v]=distance[u]+1;
                    }
                }
            }
            if(distance[EndNode] != long.MaxValue)
                return distance[EndNode];

            return -1;    
        }
    }
}
