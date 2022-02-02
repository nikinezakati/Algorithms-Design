using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using static A4.Q1BuildingRoads;
using Priority_Queue;
using System.Diagnostics.CodeAnalysis;

namespace A4
{
    public class Q2Clustering : Processor
    {
        public Q2Clustering(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, double>)Solve);
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
            public long size;
            public long count;
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
        public double Solve(long pointCount, long[][] points, long clusterCount)
        {
            List<Edge> edges = new List<Edge>();
            int[] x = new int[pointCount];
            int[] y = new int[pointCount];

            for (int i = 0; i < pointCount; i++) 
            {
                x[i] = (int)points[i][0];
                y[i] = (int)points[i][1];
            }
            for (int i = 0; i < pointCount; i++)
            {
                for (int j = i+1; j < pointCount; j++)
                {
                    edges.Add(new Edge(i,j,Distance(x[i],y[i],x[j],y[j])));
                }
            }
            return Clustering(edges,pointCount,clusterCount);
        }

        private double Clustering(List<Edge> edges,long pointCount,long clusterCount)
        {
            double dist=0;
            DisjointSet disjoint=new DisjointSet(pointCount);
            List<Edge> resultMST = new List<Edge>();
            edges.Sort((e1,e2)=>e1.length.CompareTo(e2.length));
            for (int i = 0; i < edges.Count; i++)
            {
                long u=edges[i].u;
                long v=edges[i].v;
                if(disjoint.Find(u)!=disjoint.Find(v))
                {
                    if(disjoint.count==clusterCount)
                    {
                        dist=edges[i].length;
                        break;
                    }
                    resultMST.Add(edges[i]);
                    disjoint.Merge(u,v);
                }
            }
            return Math.Round(dist,6);
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
