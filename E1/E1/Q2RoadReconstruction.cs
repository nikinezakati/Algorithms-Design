using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace E1
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
        public class Node
        {
            public long parent;
            public long rank;
            public Node(long p,long r)
            {
                this.parent=p;
                this.rank=r;
            }
        }    
        class DisjointSet
        {
            public Node[] set;
            long size;
            long count;
            public DisjointSet(long s)
            {
                this.count=s;
                this.size=s;
                set=new Node[size];
                for (long i = 0; i < size; i++)
                {
                    set[i]=new Node(i,0);
                }
            }
            public long Find(long i)
            {
                while(i!=set[i].parent)
                {
                    i=set[i].parent;
                }
                return i;
            }
            public void Merge(long i,long j)
            {
                long i_id=Find(i);
                long j_id=Find(j);
                if(i_id==j_id)
                    return;
                else if(set[i_id].rank<set[j_id].rank)
                {
                    count=count -1;
                    set[i_id].parent=j_id;
                }    
                else
                {
                    count=count -1;
                    set[j_id].parent = i_id;
                    if(set[i_id].rank==set[j_id].rank)
                        set[i_id].rank=set[i_id].rank+1;
                }
            }
        }
        public class Edge :IComparable<Edge> 
        {
            public long u;
            public long v;
            public double length;
            
            public Edge(long a, long b, double w) 
            {
                u = a;
                v = b;
                length = w;
            }

            public int CompareTo( Edge other)
            {
                if (this.length > other.length) 
                    return 1;
                else if (this.length < other.length) 
                    return -1;
                else 
                    return 0;
            }
        }
        public long[][] Solve(long n, long[][] distance)
        {
            List<Edge> edges = new List<Edge>();
            long[] x = new long[n];
            long[] y = new long[n];
            long[][] result=new long[n][];

            for (long i = 0; i < n; i++) 
            {
                x[i] = distance[i][0];
                y[i] = distance[i][1];
                result[i]=new long[n];
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    edges.Add(new Edge(i,j,Distance(x[i],y[i],x[j],y[j])));
                }
            }
            var mst= KruskalMST(n,edges);
            edges.Sort();
            foreach (var edge in edges)
            {
                if(!mst.Contains(edge))
                {
                    mst.Add(edge);
                    break;
                }
            }
            for (int i = 0; i < mst.Count; i++)
            {
                long[] temp=new long [3];
                temp[0]=mst[i].u;
                temp[1]=mst[i].v;
                temp[2]=(long)mst[i].length;
                result[i]=temp;
            }
            return result;
        }
        private List<Edge> KruskalMST(long n, List<Edge> edges)
        {
            double mindist=0;
            DisjointSet disjoint=new DisjointSet(n);
            List<Edge> resultMST = new List<Edge>();
            edges.Sort((e1,e2)=>e1.length.CompareTo(e2.length));
            for (int i = 0; i < edges.Count; i++)
            {
                long u=edges[i].u;
                long v=edges[i].v;
                if(disjoint.Find(u)!=disjoint.Find(v))
                {
                    resultMST.Add(edges[i]);
                    disjoint.Merge(u,v);
                }
            }
            
            for (int i = 0; i < resultMST.Count; i++)
            {
                mindist+=resultMST[i].length;
            }
            return resultMST;
        }

        private double Distance(long v1, long v2, long v3, long v4)
        {
            double x1=(double)v1;
            double y1=(double)v2;
            double x2=(double)v3;
            double y2=(double)v4;
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
    }
}
