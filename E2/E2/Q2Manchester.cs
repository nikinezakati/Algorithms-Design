using System;
using System.Collections.Generic;
using TestCommon;

namespace E2
{
    public class Q2Manchester : Processor
    {
        public Q2Manchester(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(4,66);
        }

        public override string Process(string inStr)
            => E2Processors.Q2Processor(inStr, Solve);

        /**
         * W[i] = Number of wins i-th team currently has.
         * R[i] = Total number of remaining games for i-th team.
         * G[i][j] = Number of upcoming games between teams i and j.
         * 
         * Should return whether the team can get first place.
         */
        public bool Solve(long[] W, long[] R, long[][] G)
        {
            var teamsCount=W.Length;
            if(teamsCount==1)
                return true;
            var matchCount=0;
            List<(int,int,long)> matches=new List<(int, int,long)>();
            for (int i = 0; i < G.Length; i++)
            {
                for (int j = 0; j < G[i].Length; j++)
                {
                    if(G[i][j]!=0 && i+1 != teamsCount && j+1 !=teamsCount)
                    {
                        matchCount++;
                        matches.Add((i+1,j+1,G[i][j]));
                    }
                }
            }
            Graph graph = new Graph(matchCount + teamsCount + 2);

            //edges from source to matches
            for(int u=0; u < matchCount; u++)
        	    graph.AddEdge(0, matches[u].Item1, matches[u].Item3);

            //edges from teams to sink
            for(int v=0; v < teamsCount; v++)
        	    graph.AddEdge(matchCount + 1 + v, matchCount + teamsCount + 1, 
                    W[teamsCount-1]+R[teamsCount-1]-W[v]);

    
            foreach(var match in matches)
            {
                graph.AddEdge(match.Item1, match.Item1, int.MaxValue);
                graph.AddEdge(match.Item1, match.Item2, int.MaxValue);
            }    
            var flow=MaxFlow(graph, 0, graph.size() - 1);
            foreach (var edge in graph.adj(0))
            {
                if(edge.flow==edge.capacity)
                    return true;
            }
            return false;
        }

        private object MaxFlow(Graph graph, int src, int des)
        {
            int maxflow = 0;
 		
            while(BFS(graph, src ,des))
                {
                    int pathFlow = int.MaxValue;
                    
                    for(int v = des; v != src; v = edgeTo[v].other(v))
                        pathFlow = (int) Math.Min(pathFlow,  edgeTo[v].residualCapacityTo(v));
                    
                    for(int v=des; v!=src; v = edgeTo[v].other(v))
                        edgeTo[v].addResidualFlowTo(v,pathFlow);
                  
                    maxflow += pathFlow;
                }
            return maxflow;
        }
        private bool BFS(Graph graph, int src, int des)
        {
          
            edgeTo = new Edge[graph.size()];
            
            marked = new bool[graph.size()];
            
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(src);
            marked[src] = true;
            while(queue.Count!=0)
            {
                int v = queue.Dequeue();
                foreach(Edge e in graph.adj(v))
                {
                    int w = e.other(v);	
                    if(e.residualCapacityTo(w) > 0 && !marked[w])
                    {
                        edgeTo[w] = e;
                        marked[w] = true;
                        queue.Enqueue(w);
                    }
                }
            }
            return marked[des];
        }
        public bool[] marked;
        public Edge[] edgeTo;
    }
    public class Graph 
    {
        private List<Edge>[] graph;
		public Graph(int n) 
        {
            this.graph = new List<Edge>[n];
            for (int i = 0; i < n; ++i)
                this.graph[i] = new List<Edge>();
        }

        public void AddEdge(int src, int des, long capacity) 
        {
            Edge forwardEdge = new Edge(src, des, capacity);
            graph[src].Add(forwardEdge);
            graph[des].Add(forwardEdge);
        }
        public List<Edge> adj(int v)
        {
        	return graph[v];
        }

        public int size() 
        {
            return graph.Length;
        }
    
    }
    public class Edge 
    {
        public int src, des, flow;
        public long capacity;

        public Edge(int from, int to, long capacity) 
        {
            this.src = from;
            this.des = to;
            this.capacity = capacity;
            this.flow = 0;
        }
       
        public int other(int vertex)
        {
        	if(vertex == src)
        		return des;
        	else
        		return src;
        }
        
        public long residualCapacityTo(int vertex)
        {
        	if(vertex == src)
        		return flow;
        	else 
        		return capacity-flow;
        }
       
        public void addResidualFlowTo(int vertex, int deltaFlow)
        {
        	if(vertex == src)
        		flow -= deltaFlow;
        	else
        		flow += deltaFlow;
        }
        
        public long residual()
        {
        	return capacity-flow;
        }
        
        
    }

}
