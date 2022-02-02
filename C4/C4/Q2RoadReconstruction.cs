using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace C4
{
    public class Q2RoadReconstruction : Processor
    {
        public Q2RoadReconstruction(string testDataName) : base(testDataName)
        {
        }

        public override Action<string, string> Verifier => RoadReconstructionVerifier.Verify;

        public override string Process(string inStr) {
            long count;
            long[][] data;
            TestTools.ParseGraph(inStr, out count, out data);
            return string.Join("\n", Solve(count, data).Select(edge => string.Join(" ", edge)));
        }

        // returns n different edges in the form of {u, v, weight}
        
        class DisjointSet
        {
            public long[] parents { get; set; }
            public DisjointSet(long count)
            {
                this.parents = new long[count];
                for (int i = 0; i < count; ++i) 
                    parents[i] = i;
            }
            public long FindSet(long v)
            {
                if (v == parents[v])
                    return v;
                return parents[v] = FindSet(parents[v]);
            }
            public void UnionSets(long s1, long s2)
            {
                s1 = FindSet(s1);
                s2 = FindSet(s2);
                if (s1 != s2)
                    parents[s2] = s1;
            }
        }
        
        public long[][] Solve(long n, long[][] distance)
        {
            long[][] result = new long[n][];
            DisjointSet disjoint = new DisjointSet(n);
            var notUsed = new List<(long u, long v, long weight)>();
            var adjancency = new List<(long v, long weight)>[n];
            var edges = new List<(long u, long v, long weight)>();
            bool found = false;
            
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    edges.Add((i, j, distance[i][j]));
                }
            }

            edges.Sort((e1, e2) => e1.Item3.CompareTo(e2.Item3));

            for (int i = 0; i < n; i++) 
                adjancency[i] = new List<(long v, long weight)>();

            foreach (var edge in edges)
            {
                if (disjoint.FindSet(edge.u) != disjoint.FindSet(edge.v))
                {
                    disjoint.UnionSets(edge.u, edge.v);
                    adjancency[edge.u].Add((edge.v, edge.weight));
                    adjancency[edge.v].Add((edge.u, edge.weight));
                }
                else
                    notUsed.Add(edge);
            }

            foreach (var edge in notUsed)
            {
                long weight = distance[edge.u][edge.v];

                if (BFS(adjancency, edge.u, edge.v) > weight)
                {
                    adjancency[edge.u].Add((edge.v, weight));
                    adjancency[edge.v].Add((edge.u, weight));
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                adjancency[notUsed[0].u].Add((notUsed[0].v, notUsed[0].weight));
                adjancency[notUsed[0].v].Add((notUsed[0].u, notUsed[0].weight));
            }
            
            long temp = 0;
            for (int i = 0; i < n; i++)
            {
                foreach (var neighbor in adjancency[i])
                {
                    if (neighbor.Item1 > i)
                    {
                        result[temp] = new long[] { neighbor.Item1 + 1, i + 1, neighbor.Item2 };
                        temp++;
                    }
                }
            }
            return result;
        }
        
        private static long BFS(List<(long v, long weight)>[] adjancency, long startnode, long endnode)
        {
            long[] distance = new long[adjancency.Length];

            for (int i = 0; i < adjancency.Length; ++i)
                distance[i] = long.MaxValue;

            Queue<long> queue = new Queue<long>();
            queue.Enqueue(startnode);
            distance[startnode] = 0;

            while (queue.Count != 0)
            {
                long current = queue.Dequeue();
                foreach (var neighbour in adjancency[current])
                {
                    if (distance[neighbour.v] == long.MaxValue)
                    {
                        queue.Enqueue(neighbour.v);
                        distance[neighbour.v] = distance[current] + neighbour.weight;
                    }
                    if (neighbour.Item1 == endnode)
                        return distance[neighbour.Item1];
                }
            }
            return -1;
        }    
    }
}
