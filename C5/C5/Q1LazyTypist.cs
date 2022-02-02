using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace C5
{
    public class Q1LazyTypist : Processor
    {
        public Q1LazyTypist(string testDataName) : base(testDataName) { }

        public override string Process(string inStr)
        {
            var lines = inStr.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return Solve(long.Parse(lines[0]), lines.Skip(1).ToArray()).ToString();
        }
        public class Node
        {
            public List<Edge> Edges= new List<Edge>();
            public long Position;
            
            public Node(long p)
            {
                this.Position = p;
            }
        }
        public class Edge
        {
            
            public Node u;
            public Node v;
            public char Value;
            public Edge(char val, Node u_a, Node v_a)
            {
                this.Value = val;
                this.u = u_a;
                this.v = v_a;
            }
        }

        public long Solve(long n, string[] words)
        {
            Node root = new Node(0);
            List<Node> trie = new List<Node>();
            trie.Add(root);
            
            return BFSModified(n,words,root,trie);
        }

        private long BFSModified(long n,string[] words, Node root, List<Node> trie)
        {
            long result=0;
            long Depth = -1;
            long Edgescount = 0;

            foreach (var word in words)
            {
                Node CurrentNode = root;
                if (word.Length > Depth)
                    Depth = word.Length;

                foreach (var w in word)
                {
                    bool Found = false;
                    Node NewNode = new Node(Edgescount);
                    Edge NewEdge = new Edge(w, CurrentNode, NewNode);

                    foreach (var Visited in CurrentNode.Edges)
                    {
                        if (NewEdge.Value == Visited.Value)
                        {
                            Found = true;
                            NewNode = Visited.v;
                        }
                    }
                    if (!Found)
                    {
                        trie.Add(NewNode);
                        CurrentNode.Edges.Add(NewEdge);
                        Edgescount+=1;
                    }
                    CurrentNode = NewNode;
                }
            }
            result=Edgescount * 2 + n - Depth;
            return result;
        }
    }
}
