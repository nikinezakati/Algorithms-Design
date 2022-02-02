using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q3PatternMatchingSuffixArray : Processor
    {
        public Q3PatternMatchingSuffixArray(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(40,50);
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, string[], long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, long n, string[] patterns)
        {
            var results=new List<long>();
            text+='$';
            long[] order=new long[text.Length];
            CountSortChar(text,order);

            long[] SortClass=new long[text.Length];
            ComputeClass(text,order,SortClass);

            for (int l = 1; l <= text.Length; l*=2)
            {
                order=DoubleSort(text,l,order,SortClass);
                SortClass=UpdateClass(order,SortClass,l);
            }
            string bwt=BWT(text,order);
            Dictionary<char, int> chars = new Dictionary<char, int>();
            chars['$']=0;
            chars['A']=0;
            chars['C']=0;
            chars['G']=0;
            chars['T']=0;
            Dictionary<char,long[]> BeforeCount=new Dictionary<char, long[]>();
            BeforeCount['$']=new long[bwt.Length+1];
            BeforeCount['A']=new long[bwt.Length+1];
            BeforeCount['C']=new long[bwt.Length+1];
            BeforeCount['G']=new long[bwt.Length+1];
            BeforeCount['T']=new long[bwt.Length+1];
            PreProcessBWT(bwt,chars,BeforeCount);

            var Occurs=new bool[text.Length];
            Array.Fill(Occurs,false);
            for (int i = 0; i < n; ++i) 
            {
                string pattern = patterns[i];
                long[] occurrences = FindOccurrences(pattern, text, order, bwt, chars, BeforeCount);
                for (int j = 0; j < occurrences.Length; ++j) 
                    Occurs[occurrences[j]] = true;
            }
            for (int i = 0; i < Occurs.Length; ++i) 
            {
                if (Occurs[i]) 
                    results.Add(i);
            }
            if(results.Count==0)
                results.Add(-1);
            return results.ToArray();    
        }

        private long[] FindOccurrences(string pattern, string text, long[] order, string bwt, Dictionary<char, int> chars, Dictionary<char, long[]> beforeCount)
        {
            List<long> result=new List<long>();
            string patternCopy = pattern;
            long top = 0;
            long bottom = bwt.Length - 1;
            while (top <= bottom) {
                if (patternCopy.Length!=0) 
                {
                    char symbol = patternCopy[patternCopy.Length-1];
                    patternCopy=patternCopy.Substring(0,patternCopy.Length-1);
                    if (beforeCount[symbol][bottom + 1] > beforeCount[symbol][top]) 
                    {
                        top = chars[symbol] + beforeCount[symbol][top];
                        bottom = chars[symbol]+ beforeCount[symbol][bottom + 1] - 1;
                    } else {
                        return result.ToArray();
                    }
                } 
                else 
                {
                    for (long i = top; i <= bottom; i++) 
                        result.Add(order[i]);
                
                    return result.ToArray();
                }
            }

            return result.ToArray();
        }

        private void PreProcessBWT(string bwt, Dictionary<char, int> chars, Dictionary<char, long[]> beforeCount)
        {
            for (int i = 0; i < bwt.Length; i++)
            {
                chars[bwt[i]]+=1;
                beforeCount[bwt[i]][i+1]+=1;
                foreach (var it in beforeCount.Values)
                {
                    if(i<bwt.Length)
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

        private string BWT(string text, long[] order)
        {
            string result = "";
            for (int i = 0; i < order.Length; i++) 
                result += text[(int)(order[i] - 1 + text.Length) % text.Length];
            
            return result;
        }

        private long[] UpdateClass(long[] order, long[] sortClass, int l)
        {
            long[] newSortClass=new long[order.Length];
            int count = 0;
            newSortClass[order[0]] = count;
            for (int i = 1; i < order.Length; i++) 
            {
                long cur = order[i];
                long cur_mid = (order[i] + l) % order.Length;
                long prev = order[i - 1];
                long prev_mid = (order[i - 1] + l) % order.Length;
                if (sortClass[cur]!= sortClass[prev] || sortClass[cur_mid] != sortClass[prev_mid]) 
                    newSortClass[order[i]] = ++count;
                else 
                    newSortClass[order[i]] = count;
                
            }
            
            return newSortClass;
        }
        private long[] DoubleSort(string text, int l, long[] order, long[] sortClass)
        {
            long[] NewOrder=new long[text.Length];
            long[] sortTemp=new long[text.Length];

            for (int i = 0; i < order.Length; i++) 
                sortTemp[sortClass[i]]++;
    
            for (int i = 1; i < sortTemp.Length; i++) 
                sortTemp[i] += sortTemp[i - 1];
            
            for (int i = order.Length - 1; i >= 0; i--) 
            {
                long start = (order[i] - l + order.Length) % order.Length;
                NewOrder[--sortTemp[sortClass[start]]] = start;
            }
            
            return NewOrder;
        }
        private void ComputeClass(string text, long[] order, long[] SortClass)
        {
            int count = 0;
            SortClass[order[0]] = count;
            for (int i = 1; i < order.Length; i++) 
            {
                if (text[(int)order[i]] == text[(int)order[i - 1]])
                    SortClass[order[i]] = count;
                else
                    SortClass[order[i]] = ++count;
            }
        }
        private void CountSortChar(string text, long[] order)
        {
            long[] sortTemp=new long[6];
            for (int i = 0; i < text.Length; i++) 
                sortTemp[CharToIndex(text[i])]++;

            for (int i = 1; i < sortTemp.Length; i++) 
                sortTemp[i] += sortTemp[i - 1];
            
            for (int i = text.Length - 1; i >= 0; i--) 
                order[--sortTemp[CharToIndex(text[i])]] = i;
            
        }
        private int CharToIndex(char v)
        {
            switch (v) 
            {
                case '$':
                    return 0;
                case 'A':
                    return 1;
                case 'C':
                    return 2;
                case 'G':
                    return 3;
                case 'T':
                    return 4;
                default:
                    return -1;
            }
        }
    }
}
