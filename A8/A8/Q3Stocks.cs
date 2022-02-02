using System;
using System.Collections.Generic;
using TestCommon;

namespace A8
{
    public class Q3Stocks : Processor
    {
        public Q3Stocks(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long stockCount, long pointCount, long[][] matrix)
        {
            
            Graph graph= BuildGraph(stockCount,pointCount,matrix);
            MaxFlow(graph,0,graph.Size()-1);
            return MinCharts(graph, stockCount);
        }

        private long MinCharts(Graph graph, long stockCount)
        {
            long minPath=0;

            for (int i = 1; i <= stockCount; ++i) 
            {
                foreach(var adj in graph.GetAdj(i)) 
                {
                    Edge e = graph.GetEdge(adj);
                    if (e.flow > 0) 
                    {
                        ++minPath;
                        break;
                    }
                }
            }

            return stockCount - minPath;
        }

        private void MaxFlow(Graph graph, int s, long t)
        {
            long flow = 0;

            long[] prev=new long[graph.Size()];

            do 
            {
                BFS(graph, s, t, prev);

                if (prev[t] != -1) 
                {
                    long minFlow = int.MaxValue;

                    
                    for (long u = prev[t]; u != -1; u = prev[graph.GetEdge(u).from]) 
                        minFlow = Math.Min(minFlow, graph.GetEdge(u).capacity - graph.GetEdge(u).flow);
                    

                    for (long u = prev[t]; u != -1; u = prev[graph.GetEdge(u).from]) 
                        graph.AddFlow(u, minFlow);
                    

                    flow += minFlow;
                }

            } while (prev[t] != -1);
        }

        private void BFS(Graph graph, int s, long t, long[] prev)
        {
            Queue<long> q=new Queue<long>();
            q.Enqueue(s);

            Array.Fill(prev, -1);

            while (q.Count!=0) 
            {

                long curr = q.Peek();
                q.Dequeue();

                foreach(var adj in graph.GetAdj(curr)) 
                {

                    Edge e = graph.GetEdge(adj);

                    if (prev[e.to] == -1 && e.capacity > e.flow && e.to != s) 
                    {
                        prev[e.to] = adj;
                        q.Enqueue(e.to);
                    }
                }
            }
        }

        private Graph BuildGraph(long stockCount, long pointCount,long[][] matrix)
        {
            Graph graph=new Graph(stockCount * 2 + 2);

            
            for (int i = 0; i < stockCount; ++i) 
                graph.AddEdge(0, i + 1, 1);
            

            for (int i = 0; i < stockCount; ++i) 
            {
                int currStock = i;

                for (int j = 0; j < stockCount; ++j) 
                {
                    if (j == currStock) 
                        continue;
                    

                    bool each_less=true;

                    for (int k = 0; k < pointCount; ++k) 
                    {
                        if (matrix[i][k] >= matrix[j][k]) 
                        {
                            each_less = false;
                            break;
                        }
                    }

                    if (each_less) 
                        graph.AddEdge(i + 1, (int)stockCount + j + 1, 1);
                    
                }
            }

            
            for (long i = stockCount + 1; i <= stockCount * 2; ++i) 
                graph.AddEdge((int)i, (int)stockCount * 2 + 1, 1);
            

            return graph;
        }
        public class Edge
        {
            public long from,to,capacity,flow;
            public Edge(long f,long t,long c, long fl)
            {
                this.from=f;
                this.to=t;
                this.capacity=c;
                this.flow=fl;
            }
        }
        public class Graph
        {
            
            public List<Edge> edges;
            private List<long>[] graph;
            public Graph(long n)
            {
                edges=new List<Edge>();
                graph=new List<long>[n];
                for (int i = 0; i < n; i++)
                {
                    graph[i]=new List<long>();
                }
            }
            public void AddEdge(int from, int to, int capacity)
            {
                Edge forwardEdge = new Edge(from, to, capacity, 0);
                Edge backwardEdge = new Edge(to, from, 0, 0 );
                graph[from].Add(edges.Count);
                edges.Add(forwardEdge);
                graph[to].Add(edges.Count);
                edges.Add(backwardEdge);
            }
            public long Size()
            {
                return graph.Length;
            }

            public List<long> GetAdj(long from)
            {
                return graph[from];
            }

            public Edge GetEdge(long num) 
            {
                return edges[(int)num];
            }
            public void AddFlow(long num, long flow)
            {
                edges[(int)num].flow += flow;
                edges[(int)num ^ 1].flow -= flow;
            }
        }
    }
}
