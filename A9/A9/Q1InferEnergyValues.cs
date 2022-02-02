using System;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public Q1InferEnergyValues(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            var result=new double[MATRIX_SIZE];
            size=MATRIX_SIZE;
            double[,] a = new double[MATRIX_SIZE,MATRIX_SIZE];
            double[] b = new double[MATRIX_SIZE];
            for (int row = 0; row < MATRIX_SIZE; ++row) 
            {
                for (int column = 0; column < MATRIX_SIZE; ++column)
                    a[row,column] = matrix[row,column];
                b[row] = matrix[row,MATRIX_SIZE];
            }
            bool[] usedColumns = new bool[MATRIX_SIZE];
            bool[] usedRows = new bool[MATRIX_SIZE];
            for (long step = MATRIX_SIZE; step >0 ; step--) 
            {
                Position pivitElement = SelectPivotElement(a, usedRows, usedColumns);
                SwapLines(a, b, usedRows, pivitElement);
                ProcessPivotElement(a, b, pivitElement);
                MarkPivotElementUsed(pivitElement, usedRows, usedColumns);
            }
            BackSubstitution(a,b);
            for (int i = 0; i < b.Length; i++)
            {
                int num=(int)b[i];
                var frac=b[i]-num;
                if(frac!=0)
                {
                    if(Math.Abs(frac)<0.25)
                        result[i]=num;
                    else if(Math.Abs(frac)>0.75)
                    {
                        if(b[i]>0)
                            result[i]=num+1;
                        if(b[i]<0)
                            result[i]=num-1;    
                    }  
                    else if(0.25<Math.Abs(frac) && Math.Abs(frac)<0.75)
                    {
                        if(b[i]>0)
                            result[i]=num+0.5;
                        if(b[i]<0)
                            result[i]=num-0.5; 
                    }
                }
                else
                    result[i]=b[i];        
            }
            return result;
        }

        private void BackSubstitution(double[,] a, double[] b)
        {
            for(long i=size-1; i > 0; i--)
            {
                double v = b[i];
                for(int j=0; j != i; j++)
                {
                    b[j] -= a[j,i] * v;
                    a[j,i] = 0;
                }
            }
        }

        private void MarkPivotElementUsed(Position pivitElement, bool[] usedRows, bool[] usedColumns)
        {
            usedRows[pivitElement.row] = true;
            usedColumns[pivitElement.column] = true;
        }

        private void ProcessPivotElement(double[,] a, double[] b, Position pivitElement)
        {
            double mult = 0.0;
        
            scalePivot(a, b, pivitElement);
            
            for(int i=pivitElement.row + 1; i< size; i++)
            {
                mult = a[i,pivitElement.column];
                for(int j= pivitElement.column; j < size; j++)
                    a[i,j] -= (a[pivitElement.row,j] * mult);
                b[i] -= (b[pivitElement.row] * mult);
            }
        }

        private void scalePivot(double[,] a, double[] b, Position pivitElement)
        {
            double div = a[pivitElement.row,pivitElement.column];
    	
            for(int i=pivitElement.column; i<size; i++)
                a[pivitElement.row,i] /= div ;
            b[pivitElement.row] /= div;
        }

        private void SwapLines(double[,] a, double[] b, bool[] usedRows, Position pivitElement)
        {

            for (int column = 0; column < size; ++column) 
            {
                double tempa = a[pivitElement.column,column];
                a[pivitElement.column,column] = a[pivitElement.row,column];
                a[pivitElement.row,column] = tempa;
            }

            double tempb = b[pivitElement.column];
            b[pivitElement.column] = b[pivitElement.row];
            b[pivitElement.row] = tempb;

            bool tempu = usedRows[pivitElement.column];
            usedRows[pivitElement.column] = usedRows[pivitElement.row];
            usedRows[pivitElement.row] = tempu;

            pivitElement.row = pivitElement.column;
        }

        public long size;

        private Position SelectPivotElement(double[,] a, bool[] usedRows, bool[] usedColumns)
        {
            Position pivot = new Position(0,0);
            while(usedRows[pivot.row])
                ++pivot.row;
            while(usedColumns[pivot.column])
                ++pivot.column;
            double max = 0;
            for(int from = pivot.row; from < size; from++)
            {
                if(Math.Abs(a[from,pivot.column]) > Math.Abs(max))
                {
                    max = a[from,pivot.column];
                    pivot.row = from;
                }
            }
            return pivot;
        }

        public class Position 
        {
            public int column;
            public int row;
            public Position(int column, int row) 
            {
                this.column = column;
                this.row = row;
            }
        }
    }
}
