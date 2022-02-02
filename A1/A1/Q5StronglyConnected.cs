using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q5StronglyConnected: Processor
    {
        public Q5StronglyConnected(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            int m=edges.Length;
            List<long>[] adjacency=new List<long>[nodeCount];
            for (int j = 0; j < nodeCount; j++)
            {
                adjacency[j]=new List<long>();
            }
            for (int i = 0; i < m; i++)
            {
                long x=edges[i][0];
                long y=edges[i][1];
                adjacency[x-1].Add(y-1);
            }
            return StronglyConnected(adjacency);
        }

        private long StronglyConnected(List<long>[] adjacency)
        {
            int[] Visited=new int[adjacency.Length];
            long result=0;
            Stack<long> Reachable=new Stack<long>();
            for (int i = 0; i < adjacency.Length; i++)
            {
                if(Visited[i]==0)
                    DFS(adjacency,i,Visited,Reachable);
            }
            List<long>[] reverseadj=Reverse(adjacency);
            for (int i = 0; i < adjacency.Length; i++)
            {
                Visited[i]=0;
            }
            while(Reachable.Count!=0)
            {
                long x=Reachable.Peek();
                Reachable.Pop();
                if(Visited[x]==0)
                {
                    Stack<long> Component=new Stack<long>();
                    DFS(reverseadj,x,Visited,Component);
                    result++;
                }
            }
            return result;
        }

        private List<long>[] Reverse(List<long>[] adjacency)
        {
            List<long>[] reverseadj=new List<long>[adjacency.Length];
            for (int j = 0; j < adjacency.Length; j++)
            {
                reverseadj[j]=new List<long>();
            }
            for (int i = 0; i < adjacency.Length; i++)
            {
                for (int j = 0; j < adjacency[i].Count; j++)
                {
                    reverseadj[adjacency[i][j]].Add(i);
                }
            }
            return reverseadj;
        }

        private void DFS(List<long>[] adjacency, long x, int[] visited, Stack<long> reachable)
        {
            visited[x]=1;
            for (int i = 0; i < adjacency[x].Count; i++)
            {
                if(visited[adjacency[x][i]]==0)
                {
                    visited[adjacency[x][i]]=1;
                    DFS(adjacency,adjacency[x][i],visited,reachable);
                }
            }
            reachable.Push(x);
        }
    }
}
