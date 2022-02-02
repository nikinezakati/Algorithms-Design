using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using Priority_Queue;

namespace A3
{
    public class Q1MinCost : Processor
    {
        public Q1MinCost(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);


        public long Solve(long nodeCount, long[][] edges, long startNode, long endNode)
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
            long s=startNode-1;
            long e=endNode-1;
            return Distance(adjacency,costs,s,e);
        }

        private long Distance(List<long>[] adjacency, List<long>[] costs, long s, long e)
        {
            long[] dist = new long[adjacency.Length];
            for (int i = 0;i < dist.Length;i++) 
            {
                dist[i] = long.MaxValue;
            }
            dist[s] = 0;
            SimplePriorityQueue<Node,Node> queue = new SimplePriorityQueue<Node,Node>();
            queue.Enqueue(new Node(s, dist[s]),new Node(s, dist[s]));
            while(queue.Count>0)
            {
                Node u = queue.Dequeue();
                long uIndex = u.index;
                for (int i = 0; i < adjacency[uIndex].Count; i++)
                {
                    long v=adjacency[uIndex][i];
                    if(dist[v] > dist[uIndex] + costs[uIndex][i]) 
                    {
	                    dist[v] = dist[uIndex] + costs[uIndex][i]; 
                        queue.Enqueue(new Node(v,dist[v]),new Node(v,dist[v]));
                    }    
                }
            }
            if(dist[e] == long.MaxValue)
                return -1;
            return dist[e];
        }
    }
    public class Node:IComparable<Node> 
    {
		public long index;
		public long distance;
		
		public Node(long index, long distance) 
        {
            this.index = index;
            this.distance = distance;
        }
		
    	public int CompareTo(Node o) 
        {
			if (this.distance > o.distance) 
                return 1;
			else if (this.distance < o.distance) 
                return -1;
			else 
                return 0;
		}
	}
}
