using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace E1
{
    class RoadReconstructionVerifier
    {
        public static void Verify(
            string inFileName,
            string strResult)
        {
            var lines = File.ReadAllLines(inFileName);
            long count = long.Parse(lines[0]);
            long[][] tui32 = lines.Skip(1).Select(line => line.Split(" ").Select(x => long.Parse(x)).ToArray()).ToArray();
            var resultLines = strResult.Split("\n");
            long[][] e1s = resultLines.Select(line => line.Split(" ").Select(x => long.Parse(x)).ToArray()).ToArray();
            if (e1s.Length != count)
            {
                throw new Exception("Invalid number of edges: " + "Expected=" + count.ToString() + " Actual=" + e1s.Length);
            }
            for (int i = 0; i < count; i++)
            {
                if (e1s[i].Length != 3)
                {
                    throw new Exception("Invalid edge: " + string.Join(" ", e1s[i]));
                }
                long u = e1s[i][0];
                long v = e1s[i][1];
                if (u < 1 || u > count || v < 1 || v > count)
                {
                    throw new Exception("Invalid edge: " + string.Join(" ", e1s[i]));
                }
            }
            RoadReconstructionVerifierHelper helper = new RoadReconstructionVerifierHelper(e1s, tui32);
            helper.Verify();
        }
    }
    class RoadReconstructionVerifierHelper
    {
        long[][] e1s;
        long[][] tui32;
        long nn;
        public RoadReconstructionVerifierHelper(long[][] e1s, long[][] tui32)
        {
            this.e1s = e1s;
            this.nn = tui32.Length;
            this.tui32 = tui32;
        }

        public class Bar
        {
            long value;
            public long drs;
            public List<Bar> alkj;
            public List<long> w92;
            public bool vsw;
            public Bar(long value)
            {
                this.value = value;
                drs = long.MaxValue;

                vsw = false;

                alkj = new List<Bar>();

                w92 = new List<long>();
            }
        }
        public Bar[] nx;
        public void foo1(long nn, long[][] e1s)
        {
            nx = new Bar[nn + 1];
            for (int i = 1; i <= nn; i++)
            {
                nx[i] = new Bar(i);
            }
            foreach (var e1 in e1s)
            {
                nx[e1[0]].alkj.Add(nx[e1[1]]); nx[e1[1]].alkj.Add(nx[e1[0]]);  nx[e1[0]].w92.Add(e1[2]);  nx[e1[1]].w92.Add(e1[2]);
            }
        }

        public void init(long nn, Bar oiewpriz, Bar X2w)
        {
            for (int i = 1; i <= nn; i++)
            {
                nx[i].vsw = false;

                nx[i].drs = (nx[i] == oiewpriz) ? 0 : int.MaxValue;
            }
        }
        public long ff(Bar oiewpriz, Bar X2w)
        {
            SortedSet<Bar> Q = new SortedSet<Bar>(new Ro());
            Q.Add(oiewpriz);
            oiewpriz.vsw = true;
            while (Q.Count > 0)
            {
                Bar u = Q.First();
                if (u == X2w) return u.drs;
                u.vsw = true;
                Q.Remove(u);
                for (int i = 0; i < u.alkj.Count; i++)
                {
                    Bar v = u.alkj[i];
                    if (u.drs + u.w92[i] < v.drs)
                    {
                        Q.Remove(v);
                        v.drs = u.drs + u.w92[i];
                        Q.Add(v);
                    }
                }
            }
            return -1;
        }

        public class Ro : IComparer<Bar>
        {
            public int Compare(Bar x, Bar y)
            {
                if (x.drs == y.drs)return x.GetHashCode().CompareTo(y.GetHashCode());
                return x.drs.CompareTo(y.drs);
            }
        }

        public void Verify()
        {
            List<long> res = new List<long>();
            foo1(nn, e1s);
            long[][] tui3 = new long[nn + 1][];
            for (int i = 0; i < nn; i++)
            {
                tui3[i] = new long[nn + 1];
            }
            for (int i = 1; i <= nn; i++)
            {
                tui3[i - 1][i - 1] = 0;
                for (int j = i + 1; j <= nn; j++)
                {
                    init(nn, nx[i], nx[j]);
                    tui3[i - 1][j - 1] = ff(nx[i], nx[j]);
                    tui3[j - 1][i - 1] = tui3[i - 1][j - 1];
                }
            }

            for (int i = 0; i < nn; i++)
            {
                for (int j = 0; j < nn; j++)
                {
                    if (tui3[i][j] != this.tui32[i][j])
                    {
                        throw new Exception("Path for " + (i + 1).ToString() + " to " + (j + 1).ToString() + " was not correct. Expected=" + tui32[i][j] + " Actual=" + tui3[i][j]);
                    }
                }
            }
        }

    }
}
