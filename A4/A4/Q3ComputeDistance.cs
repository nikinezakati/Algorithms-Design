using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using Priority_Queue;
using System.Diagnostics.CodeAnalysis;
//using GeoCoordinatePortable;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public static readonly char[] IgnoreChars = new char[] { '\n', '\r', ' ' };
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        private static double[][] ReadTree(IEnumerable<string> lines)
        {
            return lines.Select(line => 
                line.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(n => double.Parse(n)).ToArray()
                            ).ToArray();
        }
        public override string Process(string inStr)
        {
            return Process(inStr, (Func<long, long, double[][], double[][], long,
                                    long[][], double[]>)Solve);
        }
        public static string Process(string inStr, Func<long, long, double[][]
                                  ,double[][], long, long[][], double[]> processor)
        {
           var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
           long[] count = lines.First().Split(IgnoreChars,
                                              StringSplitOptions.RemoveEmptyEntries)
                                        .Select(n => long.Parse(n))
                                        .ToArray();
            double[][] points = ReadTree(lines.Skip(1).Take((int)count[0]));
            double[][] edges = ReadTree(lines.Skip(1 + (int)count[0]).Take((int)count[1]));
            long queryCount = long.Parse(lines.Skip(1 + (int)count[0] + (int)count[1]) 
                                         .Take(1).FirstOrDefault());
            long[][] queries = ReadTree(lines.Skip(2 + (int)count[0] + (int)count[1]))
                                        .Select(x => x.Select(z => (long)z).ToArray())
                                        .ToArray();

            return string.Join("\n", processor(count[0], count[1], points, edges,
                                queryCount, queries));
        }

        public double[] Solve(long nodeCount,
                            long edgeCount,
                            double[][] points,
                            double[][] edges,
                            long queriesCount,
                            long[][] queries)
        {
            double[] result=new double[queriesCount];
            AStar CoordinatesDist = new AStar(nodeCount);
            CoordinatesDist.adj = new List<double>[nodeCount];
            CoordinatesDist.cost = new List<double>[nodeCount];
            for (int i = 0; i < nodeCount; i++) 
            {
                CoordinatesDist.adj[i] = new List<double>();
                CoordinatesDist.cost[i] = new List<double>();
                CoordinatesDist.x[i] = points[i][0];
                CoordinatesDist.y[i] = points[i][1];
            }
            for (int i = 0; i < edgeCount; i++) 
            {
                double x=edges[i][0];
                double y=edges[i][1];
                double c=edges[i][2];
                CoordinatesDist.adj[(long)x - 1].Add(y - 1);
                CoordinatesDist.cost[(long)x - 1].Add(c);
            }
            for (int i = 0; i < queriesCount; i++) 
            {
                long u=queries[i][0];
                long v=queries[i][1];
               result[i]=CoordinatesDist.Distance(u-1, v-1);
            }
            return result;
        }
        class Node : IComparable<Node>
        {
            public long cost;
            public double node;
          
            public Node(long c, double n)
            {
                this.cost = c;
                this.node = n;
            }

            public int CompareTo(Node other)
            {
                return cost < other.cost ? -1 : cost > other.cost ? 1 : 0;
            }
        }
                public class AStar 
        {
            public long n;
            public double[] x;
            public double[] y;
            public List<double>[] adj;
            public List<double>[] cost;
            public long[] distance;
            SimplePriorityQueue<Node,Node> queue = new SimplePriorityQueue<Node,Node>();
            public bool[] visited;
            public List<double> workset;
            public Dictionary<double, long> p=new Dictionary<double, long>();
            public long infinity = long.MaxValue;
            public AStar(long m) 
            {
                this.n = m;
                visited = new bool[n];
                x = new double[n];
                y = new double[n];
                Array.Fill(visited, false);
                workset = new List<double>();
                p = new Dictionary<double, long>();
                distance = new long[n];
                for (int i = 0; i < n; ++i) {
                    distance[i] = infinity;
                }
                queue = new SimplePriorityQueue<Node,Node>();
            }
            public void clear() 
            {
                foreach (long v in workset) 
                {
                    distance[v] = infinity;
                    visited[v] = false;
                }
                workset.Clear();
                queue.Clear();
                p.Clear();
            }
            public void Relax(double v, long dist, long measure) 
            {
                if (distance[(long)v] > dist) 
                {
                    distance[(long)v] = dist;
                    queue.Enqueue(new Node(distance[(long)v] + measure, v),new Node(distance[(long)v] + measure, v));
                    workset.Add(v);
                }
            }
            public long Potential(double s, long t) 
            {
                if (!p.ContainsKey(s)) 
                    p[(long)s]=(long) Math.Sqrt((x[(long)s]-x[t])*(x[(long)s]-x[t])+(y[(long)s]-y[t])*(y[(long)s]-y[t]));
                return p[s];
            }
            public double extractMin() 
            {
                Node n = queue.Dequeue();
                return n.node;
            }
            void Process(double u, long t, List<double>[] adj, List<double>[] cost) 
            {
                for (int i = 0; i < adj[(long)u].Count; ++i) 
                {
                    double v = adj[(long)u][i];
                    if (visited[(long)v] != true) 
                    {
                        long w = (long) cost[(long)u][i];
                        Relax(v, distance[(long)u] + w, Potential(v, t));
                    }
                }
            }
            public long Distance(long s, long t) 
            {
                clear();
                Relax(s, 0L, Potential(s, t));
                while (queue.Count!=0) 
                {
                    double v = extractMin();
                    if (v == t) 
                    {
                        if(distance[t] != infinity)
                            return distance[t];
                        else
                            return -1L;
                    }
                    if (visited[(long)v] != true) 
                    {
                        Process(v, t, adj, cost);
                        visited[(long)v] = true;
                    }
                }
                return -1L;
            }
        }
        
    }
    
}