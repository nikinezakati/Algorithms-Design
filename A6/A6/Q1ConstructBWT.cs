using System;
using System.Collections.Generic;
using TestCommon;

namespace A6
{
    public class Q1ConstructBWT : Processor
    {
        public Q1ConstructBWT(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Construct the Burrows–Wheeler transform of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> BWT(Text) </returns>
        public string Solve(string text)
        {
            string result="";
            List<string> Matrix=new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                Matrix.Add(text.Substring(i)+text.Substring(0,i));
            }
            Matrix.Sort();
            for (int i = 0; i < text.Length; i++)
            {
                result+=Matrix[i][Matrix.Count-1];
            }
            return result;
        }
    }
}
