using System;
using System.Collections.Generic;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            
            var pre=new long[edgeCount];
            
            var flow=new long[edgeCount];

            long [,] capacity=new long[edgeCount,edgeCount];

            if(edgeCount==0)
                return 0;
            if(edgeCount==1)
                return edges[0][2];    
            for (int i = 0; i < edgeCount; i++)
            {
                if(edges[i][0]==edges[i][1])
                    continue;
                capacity[edges[i][0],edges[i][1]]+=edges[i][2];    
            }
            return MaxFlow(1,nodeCount,capacity,pre,flow);
        }

        private long MaxFlow(int src, long des, long[,] capacity, long[] pre, long[] flow)
        {
            long m=des;
            long increasement= 0;
            long sumflow = 0;
            while((increasement=BFS(src,des,flow,capacity,pre,m))!=-1)
            {
                long k = des;         
                while(k!=src)
                {
                    long last = pre[k];
                    capacity[last,k] -= increasement; 
                    capacity[k,last] += increasement; 
                    k = last;
                }
                sumflow += increasement;
            }
            return sumflow;
        }

        private long BFS(int src, long des, long[] flow, long[,] capacity, long[] pre,long m)
        {
            
            while(q.Count>0)      
                q.Dequeue();

            for(int i=1;i<m+1;++i)
            {
                pre[i]=-1;
            }
            pre[src]=0;
            flow[src]= int.MaxValue;
            q.Enqueue(src);
            while(q.Count>0)
            {
                long index = q.Peek();
                q.Dequeue();
                if(index == des)          
                    break;
                for(int i=1;i<m+1;++i)
                {
                    if(i!=src && capacity[index,i]>0 && pre[i]==-1)
                    {
                        pre[i] = index; 
                        flow[i] = Math.Min(capacity[index,i],flow[index]);   
                        q.Enqueue(i);
                    }
                }
            }
            if(pre[des]==-1)      
                return -1;
            else
                return flow[des];
        }

        public Queue<long> q=new Queue<long>();
        
    }
}
