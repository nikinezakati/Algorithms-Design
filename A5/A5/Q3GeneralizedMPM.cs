using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q3GeneralizedMPM : Processor
    {
        public Q3GeneralizedMPM(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, string[] patterns)
        {
            List<long> result= new List<long>();
            var root=new Node();
            foreach (var pattern in patterns)
            {
                var currnode=root;
                for (int i = 0; i <pattern.Length; i++)
                {
                    if(!currnode.next.ContainsKey(pattern[i]))
                        currnode.next[pattern[i]]=new Node();
                    if(i==pattern.Length-1)
                        currnode.next[pattern[i]].leaf=true;
                    else
                        currnode=currnode.next[pattern[i]];        
                }
            }
            for (int i = 0; i < text.Length; i++)
            {
                var index=i;
                var currnode = root;
                while(index<text.Length)
                {
                    var c=text[index];
                    if(!currnode.next.ContainsKey(c))
                        break;
                    currnode=currnode.next[c];
                    if(currnode.leaf)
                    {
                        if(!result.Contains(i))
                            result.Add(i);
                        break;
                    }
                    index++;
                }
            }
            if (result.Count==0)
                result.Add(-1);
            result.Sort((x, y) => x.CompareTo(y));
            return result.ToArray();
        }
        public class Node
        {
            public Dictionary<char,Node> next;
            public bool leaf;
            public Node()
            {
                next= new Dictionary<char, Node>();
                this.leaf=false;
            }
        }
    }
}
