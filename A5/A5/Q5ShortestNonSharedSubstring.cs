using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;


namespace A5
{
    public class Q5ShortestNonSharedSubstring : Processor
    {
        public Q5ShortestNonSharedSubstring(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, String>)Solve);
        
        private string Solve(string text1, string text2)
        {
            
            return null;

        }
        
    }
}
