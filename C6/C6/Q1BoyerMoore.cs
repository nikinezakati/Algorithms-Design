using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace C6
{
    public class Q1BoyerMoore : Processor
    {
        public Q1BoyerMoore(string testDataName) : base(testDataName)
        {
			this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");
        
        protected virtual long[] Solve(string text, string pattern)
        {
            var result=new List<long>();
            int[] badChar = new int[256];
            int tL = text.Length;
            int pL = pattern.Length;
        
            BadCharH(pattern, pL, badChar);
        
            int shift = 0; 
            while(shift <= (tL - pL))
            {
                int index = pL-1;
        
                while(index >= 0 && pattern[index] == text[shift+index])
                    index-=1;
        
                if (index==-1)
                {
                    result.Add(shift);

                    if(shift+pL < tL)
                        shift+=pL-badChar[text[shift+pL]];
                    else
                        shift+=1;
        
                }
                else
                    shift += Math.Max(1, index - badChar[text[shift+index]]);
            }
            if(result.Count==0)
                result.Add(-1);
            return result.ToArray();
        }
        private void BadCharH(string s, int size ,int[] badChar)
        {
            
            Array.Fill(badChar,-1);
    
            for (int i = 0; i < size; i++)
                badChar[(int) s[i]] = i;
        }
    }
}
