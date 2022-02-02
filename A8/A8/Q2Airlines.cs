using System;
using System.Collections.Generic;
using TestCommon;

namespace A8
{
    public class Q2Airlines : Processor
    {
        public Q2Airlines(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long, long[][], long[]>)Solve);

        public virtual long[] Solve(long flightCount, long crewCount, long[][] info)
        {

            Graph graph = new Graph(flightCount + crewCount + 2);

            for(int u=0; u < flightCount; u++)
        	    graph.AddEdge(0, u+1, 1);

            for(int v=0; v < crewCount; v++)
        	    graph.AddEdge(flightCount + 1 + v, flightCount + crewCount + 1, 1);

            for (int i = 0; i < flightCount; ++i)
            {
                for (int j = 0; j < crewCount; ++j)
                {
                    long edge = info[i][j];
                    if(edge == 1)
                    {
                        graph.AddEdge(i + 1, flightCount + j + 1, 1);
                    }
                    
                }    
            }
            long[] matching = FindMatching(graph, 0, graph.size()-1,flightCount);
            var result=new long[flightCount];
            for (int i = 0; i < matching.Length; ++i) 
            {
                if (matching[i] == -1) 
                    result[i]=-1;
                else 
                    result[i]=matching[i] + 1;
            }
            return result;

        }
        public Edge[] matchdes;
        private long[] FindMatching(Graph graph, int src, int des,long flightCount)
        {
            long[] matching = new long[flightCount];
            Array.Fill(matching, -1);
            
                
                while(BFS(graph, src ,des))
                {
                    int flow = 1;
                    
                    for(long v=des; v!=src; v = matchdes[v].other((int)v))
                        matchdes[v].AddResidualFlow((int)v,flow);
                }
                foreach(Edge edge in graph.getEdges())
                {
                    if(edge.flow > 0 && edge.des != des && edge.src != src)
                    {	
                        matching[edge.src-1] = edge.des - flightCount - 1;
                    }
                }
            
            return matching;
        }

        private bool BFS(Graph graph, int src, int des)
        {
            matchdes = new Edge[graph.size()];
    
            var Visited = new bool[graph.size()];
            
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(src);
            Visited[src] = true;
            while(queue.Count!=0)
            {
                int v = queue.Dequeue();
                foreach(Edge edge in graph.adj(v))
                {
                    long u = edge.other(v);	
                    if(edge.ResidualCapacity((int)u) > 0 && !Visited[u])
                    {
                        matchdes[u] = edge;
                        Visited[u] = true;
                        queue.Enqueue((int)u);
                    }
                }
            }
            return Visited[des];
        }
    public class Graph 
    {
        private List<Edge>[] graph;
        private List<Edge> edges = new List<Edge>();

		public Graph(long n) 
        {
            this.graph = new List<Edge>[n];
            for (int i = 0; i < n; ++i)
                this.graph[i] = new List<Edge>();
        }

        public void AddEdge(long src, long des, int capacity) 
        {
            Edge edge = new Edge(src, des, capacity);
            graph[src].Add(edge);
            graph[des].Add(edge);
            edges.Add(edge);
        }
        public List<Edge> adj(int v)
        {
        	return graph[v];
        }

        public int size() 
        {
            return graph.Length;
        }
        public List<Edge> getEdges()
        {
        	return edges;
        }
    
    }
    public class Edge 
    {
        public long src;
        public long des;
        public int capacity, flow;

        public Edge(long src, long des, int capacity) 
        {
            this.src = src;
            this.des = des;
            this.capacity = capacity;
            this.flow = 0;
        }
        public long other(int vertex)
        {
        	if(vertex == src)
        		return des;
        	else
        		return src;
        }
        
        public int ResidualCapacity(int vertex)
        {
        	if(vertex == src)
        		return flow;
        	else 
        		return capacity-flow;
        }
        
        public void AddResidualFlow(int vertex, int deltaFlow)
        {
        	if(vertex == src)
        		flow -= deltaFlow;
        	else
        		flow += deltaFlow;
        }
        
        public int Residual()
        {
        	return capacity-flow;
        }
        
        
    }

    }
    
}
