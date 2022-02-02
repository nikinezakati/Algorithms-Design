using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace C2
{
    public class Q1RoadsInHackerLand : Processor
    {
        public Q1RoadsInHackerLand(string testDataName) : base(testDataName) { }

        public override string Process(string inStr)
        {
            var lines = inStr.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string[] first = lines[0].TrimEnd().Split(' ');

            long n = long.Parse(first[0]);
            long m = long.Parse(first[1]);

            long[][] roads = lines.Skip(1).Select(line => line.Split(' ').Select(num => long.Parse(num)).ToArray()).ToArray();
            return Solve(n, roads);
        }

        public string Solve(long n, long[][] roads)
        {

            int m = roads.Length;
            Array.Sort(roads, (e1, e2) => e1[2].CompareTo(e2[2]));
            DisJointSetUnion disjointset = new DisJointSetUnion((int)n);
            List<Tuple<long, long>>[]  tree = Init<List<Tuple<long, long>>>(n);
            for (int i = 0; i < m; i++)
            {
                roads[i][0]--;
                roads[i][1]--;
                if (disjointset.FindSet((int)roads[i][0]) != disjointset.FindSet((int)roads[i][1]))
                {
                    tree[roads[i][0]].Add(Tuple.Create(roads[i][1], roads[i][2]));
                    tree[roads[i][1]].Add(Tuple.Create(roads[i][0], roads[i][2]));
                    disjointset.UnionSets((int)roads[i][0],(int) roads[i][1]);
                }
            }

            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
            Stack<Tuple<int, int>> stack2 = new Stack<Tuple<int, int>>();
            stack.Push(Tuple.Create(0, -1));
            while (stack.Count > 0)
            {
                var temp = stack.Pop();
                foreach (var t in tree[temp.Item1])
                    if (t.Item1 != temp.Item2)
                        stack.Push(Tuple.Create((int)t.Item1,(int) temp.Item1));
                stack2.Push(temp);
            }

            int[] count = new int[n];
            while (stack2.Count > 0)
            {
                var temp = stack2.Pop();
                count[temp.Item1] = 1;
                foreach (var t in tree[temp.Item1])
                    if (t.Item1 != temp.Item2)
                        count[temp.Item1] += count[t.Item1];
            }

            Stack<Tuple<int, int, int>> stack3 = new Stack<Tuple<int, int, int>>();
            var result = Enumerable.Repeat(0L, m).ToList();
            stack3.Push(Tuple.Create(0, -1, 0));
            while (stack3.Count > 0)
            {
                var temp = stack3.Pop();
                foreach (var t in tree[temp.Item1])
                    if (t.Item1 != temp.Item2)
                    {
                        result[(int)t.Item2] = 1L * count[t.Item1] * (count[temp.Item1] - count[t.Item1] + temp.Item3);
                        stack3.Push(Tuple.Create((int)t.Item1, (int)temp.Item1, (int)temp.Item3 + count[temp.Item1] - count[t.Item1]));
                    }
            }

            for (int i = 0; i < result.Count; i++)
            {
                long ex = result[i] / 2;
                result[i] %= 2;
                if (ex > 0)
                {
                    if (i == result.Count - 1)
                        result.Add(0);
                    result[i + 1] += ex;
                }
            }
            while (result[result.Count - 1] == 0)
                result.RemoveAt(result.Count - 1);
            result.Reverse();

            return string.Concat(result);
        }

        class DisJointSetUnion
        {
            private readonly int[] parent;

            public DisJointSetUnion(int n)
            {
                parent = Enumerable.Range(0,n).ToArray();
            }

            public int FindSet(int v)
            {
                if (v == parent[v])
                    return v;
                return parent[v] = FindSet(parent[v]);
            }

            public void UnionSets(int a, int b)
            {
                a = FindSet(a);
                b = FindSet(b);
                if (a != b)
                    parent[b] = a;
            }
        }
        private static T[] Init<T>(long size) where T : new() { var ret = new T[size]; for (int i = 0; i < size; i++) ret[i] = new T(); return ret; }
    }
}
