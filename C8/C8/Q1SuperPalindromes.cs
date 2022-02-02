using System;
using TestCommon;

namespace C8
{
    public class Q1SuperPalindromes : Processor
    {
        public Q1SuperPalindromes(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr)
        {
            var lines = inStr.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var num = lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            long n = long.Parse(num[0]);
            long m = long.Parse(num[1]);
            string s = lines[1];
            return Solve(n, m, s).ToString();
        }

        public long Solve(long n, long m, string s)
        {
            throw new NotImplementedException();
        }
    }
}
