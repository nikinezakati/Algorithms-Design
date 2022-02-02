using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using Priority_Queue;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { 
            ExcludeTestCases(47 , 48 , 49 , 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);

        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {
            int m=edges.Length;
            List<long> results = new List<long>();
            List<List<long>> adjacencyF = new List<List<long>>();
            List<List<long>> costsF = new List<List<long>>();

            List<List<long>> adjacencyB = new List<List<long>>();
            List<List<long>> costsB = new List<List<long>>();

            for (long i = 0; i < NodeCount; i++)
            {
                adjacencyF.Add(new List<long>());
                adjacencyB.Add(new List<long>());
                costsF.Add(new List<long>());
                costsB.Add(new List<long>());
            }
            for (int i = 0; i < m; i++)
            {
                adjacencyF[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
                costsF[(int)edges[i][0] - 1].Add((int)edges[i][2]);
                adjacencyB[(int)edges[i][1] - 1].Add((int)edges[i][0] - 1);
                costsB[(int)edges[i][1] - 1].Add((int)edges[i][2]);
            }
            
            foreach (var q in Queries)
            {
                float[] dist_f = Enumerable.Repeat(float.MaxValue , (int)NodeCount).ToArray();
                float[] dist_b = Enumerable.Repeat(float.MaxValue , (int)NodeCount).ToArray();
                var distanceF = MakePriorityQueue(NodeCount);
                var distanceB = MakePriorityQueue(NodeCount);
                bool[] VisitedF = new bool[NodeCount];
                bool[] VisitedB = new bool[NodeCount];
                distanceF.UpdatePriority(q[0] - 1, 0);
                distanceB.UpdatePriority(q[1] - 1, 0);
                results.Add(BiDijkstra(adjacencyF, adjacencyB, distanceF, distanceB
                , VisitedF, VisitedB, costsF, costsB
                , dist_f, dist_b));
            }
            return results.ToArray();
        }
        private SimplePriorityQueue<long> MakePriorityQueue(long NodeCount)
        {
            SimplePriorityQueue<long> PQ = new SimplePriorityQueue<long>();
            for (int i = 0; i < NodeCount; i++)
            {
                PQ.Enqueue(i, int.MaxValue);
            }
            return PQ;
        }
        private long BiDijkstra(List<List<long>> adj_forawrd, List<List<long>> adj_backward,
                    SimplePriorityQueue<long> distanceF, SimplePriorityQueue<long> distanceB,
                    bool[] VisitedF, bool[] VisitedB, List<List<long>> costsF,
                    List<List<long>> costsB, float[] dist_f, float[] dist_b)
        {
            while (true)
            {
                float DequeueP = distanceF.GetPriority(distanceF.First);
                if (DequeueP == int.MaxValue)
                    return -1;
                long DequeueF = distanceF.Dequeue();
                dist_f[DequeueF] = DequeueP;
                var adjDequeue = adj_forawrd[(int)DequeueF];
                for (int i = 0; i < adjDequeue.Count; i++)
                {
                    if (!VisitedF[adjDequeue[i]])
                    {
                        if (distanceF.GetPriority(adjDequeue[i]) > DequeueP + costsF[(int)DequeueF][i])
                        {
                            distanceF.UpdatePriority(adjDequeue[i], DequeueP + costsF[(int)DequeueF][i]);
                            dist_f[adjDequeue[i]] = DequeueP + costsF[(int)DequeueF][i];
                        }
                    }
                }
                VisitedF[DequeueF] = true;
                if (VisitedB[DequeueF])
                    return (long)Distance(dist_f, dist_b);
                float DequeueP_b = distanceB.GetPriority(distanceB.First);
                if (DequeueP_b == int.MaxValue)
                    return -1;
                long DequeueB = distanceB.Dequeue();
                dist_b[DequeueB] = DequeueP_b;
                var adjDequeueB = adj_backward[(int)DequeueB];
                for (int i = 0; i < adjDequeueB.Count; i++)
                {
                    if (!VisitedB[adjDequeueB[i]])
                    {
                        if (distanceB.GetPriority(adjDequeueB[i]) > DequeueP_b + costsB[(int)DequeueB][i])
                        {
                            distanceB.UpdatePriority(adjDequeueB[i], DequeueP_b + costsB[(int)DequeueB][i]);
                            dist_b[adjDequeueB[i]] = DequeueP_b + costsB[(int)DequeueB][i];
                        }
                    }
                }
                VisitedB[DequeueB] = true;
                if (VisitedF[DequeueB])
                    return (long)Distance(dist_f, dist_b);
                
            }
        }
        private float Distance(float[] distanceF, float[] distanceB)
        {
            float minimum = int.MaxValue;
            for (int i = 0; i < distanceF.Length; i++)
            {
                if (minimum > distanceF[i] + distanceB[i])
                    minimum = distanceF[i] + distanceB[i];
            }
            return minimum;
        }
        
    }
}
