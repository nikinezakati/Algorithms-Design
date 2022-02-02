using System;
using System.Linq;
using TestCommon;

namespace C7
{
    public class Q1SuffixArrayHashing : Processor
    {
        const int MOD = 1_000_000_000 + 9;
        const int P = 31;
        long[] ppow;
        long[] h;
        string text;

        public Q1SuffixArrayHashing(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) => TestTools.Process(inStr, (Func<String, long[]>)Solve);

        public static long[] BuildHash(string text)
        {
            long n=text.Length;
            long[] result= new long[n];
            result[0]=text[0]-'a'+1;
            long ppow=1;
            for (int i = 1; i < n; i++)
            {
                ppow^=P;
                result[i]=(result[i-1]+(text[i]-'a'+1)^(ppow))%MOD;
            }
            return result;
        }
        private long[] powers(long n)
        {
            long[] result=new long[n];
            long ppow=1;
            result[0]=ppow;
            for (int i = 1; i < n; i++)
            {
                ppow*=P;
                ppow%=MOD;
                result[i]=ppow%MOD;
            }
            return result;
        }

        public long[] Solve(string text)
        {
            text+="$";
            this.h=BuildHash(text);
            ppow=powers(text.Length);
            int[] suffixes=Enumerable.Range(0,text.Length).ToArray();
            this.text=text;
            Array.Sort(suffixes,CompareSuffixes);
            return suffixes.Select(num=>(long)num).ToArray();
        }

        private int CompareSuffixes(int i, int j)
        {
            int minLength=Math.Min(text.Length-i,text.Length-j);
            if(text[i]!=text[j])
                return text[i].CompareTo(text[j]);

            if(text.Substring(i,minLength)==text.Substring(j,minLength))
                return j.CompareTo(i);

            int lo=1;
            int hi=minLength;

            while(hi-lo>1)
            {
                int mid=(lo+hi)/2;
                if(SubstringEqual(i,mid,j,mid))
                    lo=mid;
                else
                    hi=mid;    
            }
            int lcp=lo;
            return (text[i+lo]).CompareTo(text[j+lo]);        
        }

        private bool SubstringEqual(int i, int n, int j, int m)
        {
            long hashA= (h[j+n-1]-(j>0?h[i-1]:0)+MOD)%MOD;
            long hashB= (h[j+m-1]-(j>0?h[j-1]:0)+MOD)%MOD;

            if(i<j)
            {
                hashA*=ppow[j-i];
                hashA%=MOD;
            }
            else
            {
                hashB*=ppow[i-j];
                hashB%=MOD;
            }
            return hashA==hashB;
        }
    }
}
