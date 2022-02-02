using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C4
{
    class RoadReconstructionVerifier
    {
        public static void Verify(
            string inFileName,
            string strResult)
        {
            var lines = File.ReadAllLines(inFileName);
            long count = long.Parse(lines[0]);
            long[][] shortestPathMatrix = lines.Skip(1).Select(line => line.Split(" ").Select(x => long.Parse(x)).ToArray()).ToArray();

            var resultLines = strResult.Split("\n");

            long[][] edges = resultLines.Select(line => line.Split(" ").Select(x => long.Parse(x)).ToArray()).ToArray();

            if (edges.Length != count)
            {
                throw new Exception("Invalid number of edges: " + "Expected=" + count.ToString() + " Actual=" + edges.Length);
            }

            for (int i = 0; i < count; i++)
            {
                if (edges[i].Length != 3)
                {
                    throw new Exception("Invalid edge: " + string.Join(" ", edges[i]));
                }
                long u = edges[i][0];
                long v = edges[i][1];
                if (u < 1 || u > count || v < 1 || v > count)
                {
                    throw new Exception("Invalid edge: " + string.Join(" ", edges[i]));
                }
            }

            RoadReconstructionVerifierHelper helper = new RoadReconstructionVerifierHelper(edges, shortestPathMatrix);
            helper.Verify();
        }
    }
    class RoadReconstructionVerifierHelper
    {
        long[][] edges;
        long[][] shortestPathMatrix;
        long nodeCount;

        public RoadReconstructionVerifierHelper(long[][] edges, long[][] shortestPathMatrix)
        {
            this.edges = edges;
            this.nodeCount = shortestPathMatrix.Length;
            this.shortestPathMatrix = shortestPathMatrix;
        }

        public class Node
        {
            long value;
            public long distanceFromSource;

            public List<Node> adj;

            public List<long> weight;

            public bool visited;

            public Node(long value)
            {
                this.value = value;
                distanceFromSource = int.MaxValue;

                visited = false;

                adj = new List<Node>();

                weight = new List<long>();
            }
        }

        public Node[] nodes;
        public void initGraph(long nodeCount, long[][] edges)
        {
            nodes = new Node[nodeCount + 1];
            for (int i = 1; i <= nodeCount; i++)
            {
                nodes[i] = new Node(i);
            }
            foreach (var edge in edges)
            {
                nodes[edge[0]].adj.Add(nodes[edge[1]]);
                nodes[edge[1]].adj.Add(nodes[edge[0]]);
                nodes[edge[0]].weight.Add(edge[2]);
                nodes[edge[1]].weight.Add(edge[2]);
            }
        }

        public void init(long nodeCount, Node source)
        {
            for (int i = 1; i <= nodeCount; i++)
            {
                nodes[i].visited = false;

                nodes[i].distanceFromSource = (nodes[i] == source) ? 0 : int.MaxValue;
            }
        }

        public long findPath(Node source)
        {
            SortedSet<Node> Q = new SortedSet<Node>(new mycmp());

            Q.Add(source);

            source.visited = true;

            while (Q.Count > 0)
            {
                Node u = Q.First();

                u.visited = true;

                Q.Remove(u);

                for (int i = 0; i < u.adj.Count; i++)
                {
                    Node v = u.adj[i];
                    long offer = u.distanceFromSource + u.weight[i];
                    if (offer < v.distanceFromSource)
                    {
                        Q.Remove(v);
                        v.distanceFromSource = offer;
                        Q.Add(v);
                    }
                }
            }
            return -1;
        }

        public class mycmp : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                double xVal = x.distanceFromSource;
                double yVal = y.distanceFromSource;
                if (xVal == yVal)
                {
                    return x.GetHashCode().CompareTo(y.GetHashCode());
                }
                return xVal.CompareTo(yVal);
            }
        }

        public void Verify()
        {
            List<long> res = new List<long>();
            initGraph(nodeCount, edges);
            long[][] mat = new long[nodeCount + 1][];
            for (int i = 0; i < nodeCount; i++)
            {
                mat[i] = new long[nodeCount + 1];
            }
            for (int i = 1; i <= nodeCount; i++)
            {
                mat[i - 1][i - 1] = 0;
                init(nodeCount, nodes[i]);
                findPath(nodes[i]);
                for (int j = i + 1; j <= nodeCount; j++)
                {
                    mat[i - 1][j - 1] = nodes[j].distanceFromSource;
                    mat[j - 1][i - 1] = mat[i - 1][j - 1];
                }
            }

            for (int i = 0; i < nodeCount; i++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    if (mat[i][j] != this.shortestPathMatrix[i][j])
                    {
                        throw new Exception("Path for " + (i + 1).ToString() + " to " + (j + 1).ToString() + " was not correct. Expected=" + shortestPathMatrix[i][j] + " Actual=" + mat[i][j]);
                    }
                }
            }
        }

    }
}
