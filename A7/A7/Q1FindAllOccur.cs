using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q1FindAllOccur : Processor
    {
        public Q1FindAllOccur(string testDataName) : base(testDataName)
        {
			this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, string pattern)
        {
            List<long> result = new List<long>();
            string p=pattern+'$'+text;
            var sFunc=new long[p.Length];
            ComputePrefix(p,sFunc);
            for (int i = pattern.Length + 1; i < p.Length; i++) 
            {
                if (sFunc[i] == pattern.Length) 
                    result.Add(i - 2 * pattern.Length);
            }
            if(result.Count==0)
                result.Add(-1);
            return result.ToArray();
        }

        private void ComputePrefix(string p, long[] sFunc)
        {
            long border = 0;
            for (int i = 1; i < p.Length; i++) 
            {
                while (border > 0 && p[i] != p[(int)border]) 
                    border = sFunc[border - 1];
                if (p[i] == p[(int)border]) 
                {
                    border++;
                    sFunc[i] = border;
                }
                if (border == 0) 
                    sFunc[i] = 0;
            }
        }
    }
}
