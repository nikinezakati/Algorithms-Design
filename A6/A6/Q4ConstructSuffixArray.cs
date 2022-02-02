using System;
using TestCommon;

namespace A6
{
    public class Q4ConstructSuffixArray : Processor
    {
        public Q4ConstructSuffixArray(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        /// <summary>
        /// Construct the suffix array of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> SuffixArray(Text), that is, the list of starting positions
        /// (0-based) of sorted suffixes separated by spaces </returns>
        public long[] Solve(string text)
        {
            long[] result=new long[text.Length];
            (string,int)[] SuffixArray=new (string, int)[text.Length];

            for (int i = 0; i < text.Length; i++)
                SuffixArray[i]=(text.Substring(i),i);

            Array.Sort(SuffixArray);

            for (int i = 0; i < SuffixArray.Length; i++)
                result[i]=SuffixArray[i].Item2;

            return result;    
        }
    }
}
