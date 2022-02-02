using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q4OrderOfCourse: Processor
    {
        public Q4OrderOfCourse(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);

        public long[] Solve(long nodeCount, long[][] edges)
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
            long[]temp=TopologicalOrder(adjacency);
            long[] result=new long[temp.Length];
            for (int q = 0; q < result.Length; q++)
            {
                result[q]=temp[temp.Length-q-1]+1;
            }
            return result;
        }

        private long[] TopologicalOrder(List<long>[] adjacency)
        {
            int[] Used=new int[adjacency.Length];
            List<long> Order=new List<long>();
            for (int i = 0; i < adjacency.Length; i++)
            {
                if(Used[i]==0)
                    DFS(adjacency,Used,Order,i);
            }
            return Order.ToArray();
        }

        private void DFS(List<long>[] adjacency, int[] used, List<long> order, long x)
        {
            used[x]=1;
            for (int i = 0; i < adjacency[x].Count; i++)
            {
                if(used[adjacency[x][i]]==0)
                    DFS(adjacency,used,order,adjacency[x][i]);
            }
            order.Add(x);
        }
    }
}
