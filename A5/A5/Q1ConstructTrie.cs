using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q1ConstructTrie : Processor
    {
        public Q1ConstructTrie(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, String[], String[]>) Solve);

        public string[] Solve(long n, string[] patterns)
        {
            List<string> pattern=new List<string>();
            List<SortedDictionary<char,long>> trie = new List<SortedDictionary<char, long>>();
            for (int i = 0; i < n; i++)
            {
                pattern.Add(patterns[i]);
            }
            trie=BuildTrie(pattern);
            List<string> result = new List<string>();
            for (int i = 0; i < trie.Count; i++)
            {
                SortedDictionary<char,long> temp= trie[i];
                if(temp.Count!=0)
                {
                    foreach (var key in temp.Keys)
                    {
                        result.Add($"{i}->{temp[key]}:{key}");
                    }
                }
            }
            return result.ToArray();
        }

        private List<SortedDictionary<char,long>> BuildTrie(List<string> pattern)
        {
            List<SortedDictionary<char,long>> trie = new List<SortedDictionary<char, long>>();
            SortedDictionary<char,long> root = new SortedDictionary<char, long>();
            trie.Add(root);
            var nodeNum=1;
            foreach (var p in pattern)
            {
                long current=0;
                foreach (var c in p.ToCharArray())
                {
                    if(!trie[(int)current].ContainsKey(c))
                    {
                        trie[(int)current][c]=nodeNum;
                        trie.Add(new SortedDictionary<char, long>());
                        nodeNum=nodeNum+1;
                    }
                    current=trie[(int)current][c];
                }
            }
            return trie;
        }
    }
}
