using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q4SuffixTree : Processor
    {
        public Q4SuffixTree(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String[]>)Solve);

        public string[] Solve(string text)
        {
            List<string> result = new List<string>();
            Node root = new Node();
            for (int i = 0; i < text.Length; i++)
            {
                UpdateTree(root,text,i);
            }
            SubStrings(root,text,result);
            return result.ToArray();
        }

    
        private void SubStrings(Node root, string text, List<string> result)
        {
            for(int i=0;i<root.next.Count;i++)
            {
                result.Add(text.Substring((int)root.edges[i].Item1,(int)root.edges[i].Item2));
                Node nextNode = root.next[i];
                SubStrings(nextNode,text,result);
            }
        }

        private void UpdateTree(Node root, string text, int index)
        {
            if(index >= text.Length)
                return;
            for (int i = 0; i < root.next.Count; i++)
            {
                long startPos = root.edges[i].Item1;
                long Commonstrlen = CommonStringLen(text, startPos, index, root.edges[i].Item2);
                if((Commonstrlen>0)&&(Commonstrlen<root.edges[i].Item2))
                {
                    Node oldNode = root.next[i];
                    Node newNode = new Node();
                    (long,long) oldEdge = (startPos+Commonstrlen, root.edges[i].Item2-Commonstrlen);
                    (long,long) newEdge = (index+Commonstrlen, text.Length-index-Commonstrlen);
                    Node middle = new Node();
                    middle.next.Add(oldNode);
                    middle.next.Add(newNode);
                    middle.edges.Add(oldEdge);
                    middle.edges.Add(newEdge);
                    root.next[i] = middle;
                    (long,long) temp=root.edges[i]; 
                    temp.Item2=Commonstrlen;
                    root.edges[i]=temp;
                    return;
                }
                else if(Commonstrlen==root.edges[i].Item2)
                {
                    Node nextNode1 = root.next[i];
                    UpdateTree(nextNode1, text, index + (int) Commonstrlen);
                    return;
                }
                else if((Commonstrlen==0)&&(i==root.next.Count-1))
                {
                    Node newNode = new Node();
                    root.next.Add(newNode);
                    (long,long) newEdge=(index,text.Length-index);
                    root.edges.Add(newEdge);
                    return;
                }
            }   
            Node nextNode = new Node();
            root.next.Add(nextNode);
            (long,long) stringIndex = (index, text.Length-index);
            root.edges.Add(stringIndex);
            return; 
        }

        private long CommonStringLen(string text, long startPos1, int startPos2, long len)
        {
            long charNUm = 0;
            while(text[(int)startPos1+(int)charNUm] == text[(int)startPos2 + (int)charNUm])
            {
                charNUm+=1;
                if((charNUm==len)||(startPos2+charNUm)==text.Length)
                    break;
           
            }
            return charNUm;
        }

        public class Node
        {
            public List<(long,long)> edges;
            public List<Node> next;
            public Node()
            {
                this.edges=new List<(long, long)>();
                this.next=new List<Node>();
            }

        }
    }
}
