using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q2CunstructSuffixArray : Processor
    {
        public Q2CunstructSuffixArray(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        protected virtual long[] Solve(string text)
        {
            long[] order=new long[text.Length];
            CountSortChar(text,order);

            long[] SortClass=new long[text.Length];
            ComputeClass(text,order,SortClass);

            for (int l = 1; l <= text.Length; l*=2)
            {
                order=DoubleSort(text,l,order,SortClass);
                SortClass=UpdateClass(order,SortClass,l);
            }
            return order;
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
