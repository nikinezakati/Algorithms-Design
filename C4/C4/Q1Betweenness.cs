using System;
using System.Collections.Generic;
using TestCommon;
using System.Linq;

namespace C4
{
    public class Q1Betweenness : Processor
    {
        public Q1Betweenness(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(1, 1);
            //this.ExcludeTestCaseRangeInclusive(3, 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);

        public static Dictionary<long,List<long>> Important= new Dictionary<long, List<long>>();
        
        public long[] Solve(long NodeCount, long[][] edges)
        {
            long[] results=new long[NodeCount];
            Dictionary<long,long> count= new Dictionary<long,long>();

            int m=edges.Length;
            List<long>[] adjacency=new List<long>[NodeCount];
            for (int j = 0; j < NodeCount; j++)
            {
                adjacency[j]=new List<long>();
                Important[j]=new List<long>();
            }
            bool[] Visited=new bool[NodeCount];
            for (int a = 0; a < NodeCount; a++)
            {
                Visited[a]=false;
            }
            for (int i = 0; i < m; i++)
            {
                long x=edges[i][0];
                long y=edges[i][1];
                adjacency[x-1].Add(y-1);
            }
            for (int i = 0; i < adjacency.Length; i++)
            {
                adjacency[i]= adjacency[i].OrderBy((a)=> -a).ToList();
            }
            for (long q = 0; q < NodeCount; q++)
            {
                Visited=new bool[NodeCount];
                Array.Fill(Visited,false);

                    var parent=BFS(adjacency, q, Visited,NodeCount);
                    for(int i=0; i<parent.Length; i++)
                    {
                        var path=GetPath(parent,i);
                        for (int j = 1; j < path.Count-1; j++)
                        {
                            results[path[j]]++;
                        }
                    }
                    
            }
            return results;
        }

        private List<long> GetPath(long[] parent, long p)
        {
            List<long> path= new List<long>();
            
            path.Add(p);
            while(parent[p]!= -1)
            {
                path.Add(parent[p]);
                p=parent[p];
            }
            path.Reverse();
            return path;
        }

        private long[] BFS(List<long>[] adjacency, long x, bool[] visited,long NodeCount)
        {
            visited[x]=true;
            long[] parent=new long[NodeCount];
            for (int i = 0; i < NodeCount; i++)
            {
                parent[i]=-1;
            }
            Queue<long> s=new Queue<long>();
            s.Enqueue(x);
            while(s.Count!=0)
            {
                long q=s.Dequeue();
                for (int i = 0; i < adjacency[q].Count; i++)
                {
                    if(!visited[adjacency[q][i]])
                    {
                        visited[adjacency[q][i]]=true;
                        s.Enqueue(adjacency[q][i]);
                        parent[adjacency[q][i]]=q;
                    }
                        
                }
            }
            return parent;
        }
        
    }
}
