using System;
using System.Collections.Generic;
using TestCommon;

namespace A6
{
    public class Q3MatchingAgainCompressedString : Processor
    {
        public Q3MatchingAgainCompressedString(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        /// <summary>
        /// Implement BetterBWMatching algorithm
        /// </summary>
        /// <param name="text"> A string BWT(Text) </param>
        /// <param name="n"> Number of patterns </param>
        /// <param name="patterns"> Collection of n strings Patterns </param>
        /// <returns> A list of integers, where the i-th integer corresponds
        /// to the number of substring matches of the i-th member of Patterns
        /// in Text. </returns>
        public long[] Solve(string text, long n, String[] patterns)
        {
            long[] result=new long[n];
            Dictionary<char,int> chars=new Dictionary<char, int>();
            chars['$']=0;
            chars['A']=0;
            chars['C']=0;
            chars['G']=0;
            chars['T']=0;
            Dictionary<char,int[]> beforCount=new Dictionary<char, int[]>();
            foreach (var key in chars.Keys)
            {
                beforCount[key]=new int[text.Length+1];
            }
            PreProcessBWT(text,chars,beforCount);
            for (int i = 0; i < n; i++)
            {
                result[i]=CountAccurrences(patterns[i],text,chars,beforCount);
            }
            return result;
        }

        private long CountAccurrences(string pattern, string text, Dictionary<char, int> chars, Dictionary<char, int[]> beforCount)
        {
            string patternCopy = pattern;
            int top = 0;
            int bottom = text.Length - 1;
            while (top <= bottom) 
            {
                if (patternCopy.Length!=0) 
                {
                    char symbol = patternCopy[patternCopy.Length-1];
                    patternCopy=patternCopy.Substring(0,patternCopy.Length-1);
                    if (beforCount[symbol][bottom + 1] > beforCount[symbol][top])
                    {
                        top = chars[symbol] + beforCount[symbol][top];
                        bottom = chars[symbol]+ beforCount[symbol][bottom + 1] - 1;
                    } 
                    else
                        return 0;
                    
                } 
                else 
                    return bottom - top + 1;
            }

            return 0;
        }

        private void PreProcessBWT(string text, Dictionary<char, int> chars, Dictionary<char, int[]> beforCount)
        {
            for (int i = 0; i < text.Length; i++)
            {
                chars[text[i]]+=1;
                beforCount[text[i]][i+1]+=1;
                foreach (var it in beforCount.Values)
                {
                    if(i<text.Length)
                        it[i+1]+=it[i];
                }
            }
            int cf=0;
            List<char> keys = new List<char>(chars.Keys);
            foreach (var key in keys)
            {
                if(chars[key]==0)
                {
                    chars[key]=-1;
                }
                else 
                {
                    int temp=chars[key];
                    chars[key]=cf;
                    cf+=temp;
                }   
            }
        }
    }
}
