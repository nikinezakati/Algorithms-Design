using System;
using System.Collections.Generic;
using TestCommon;

namespace A9
{
    public class Q2OptimalDiet : Processor
    {
        public Q2OptimalDiet(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(1, 1);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int N, int M, double[,] matrix1)
        {
            var result="";
            double[][] A = new double[N][];
            for (int i = 0; i < N; i++)
            {
                A[i]=new double[M];
                for (int j = 0; j < M; j++)
                    A[i][j] = matrix1[i,j];
            }  
            double[] b = new double[N];
            for (int i = 0; i < N; i++)
                b[i] = matrix1[i,M];

            double[] c = new double[M];
            for (int i = 0; i < M; i++)
                c[i] = matrix1[M+1,i]; 

            double[] ansx = new double[M];
        
            int sol = Diet(N, M, A, b, c, ansx);   
            if (sol == -1) {
                result+="No solution\n";
                return result;
            } 
            else if (sol == 0) 
            {
                result+="Bounded solution\n";
                for (int i = 0; i < M; i++) 
                    result+= ansx[i].ToString()+' ';
                
                return result;
            }
            else if (sol == 1) 
            {
                result+="Infinity\n";
                return result;
            }
            return result;
        }

        private int Diet(int constraintNum, int vars, double[][] constraints, double[] budgets, double[] pleasures, double[] x)
        {
            Double maxVal = Double.NaN;
        
            double[] solution = new double[vars];
            
            List<int[]> com = new SubArraysOfArray(vars, constraintNum + vars + 1).GetSubArrays();
            foreach (int[] combo in com) 
            {
                double[][] currForm = new double[vars][];
                for (int i = 0; i < vars; i++)
                {
                    currForm[i]=new double[vars];
                }
                double[] currBudget = new double[vars];
                CurrForm(constraints, budgets, currForm, currBudget, combo);
                double[] solutionCandidate;
                
                
                try {
                    solutionCandidate = RowReduce(currForm, currBudget);
                } 
                catch (Exception e) {
                    continue;
                }

                
                int[] nonUsedIndices = NotUsedIndices(constraintNum + vars + 1, combo);
                currForm = new double[nonUsedIndices.Length][];
                for (int i = 0; i < nonUsedIndices.Length; i++)
                {
                    currForm[i]=new double[vars];
                }
                currBudget = new double[nonUsedIndices.Length];
                CurrForm(constraints, budgets, currForm, currBudget, nonUsedIndices);
                
               
                if (VerifySolution(solutionCandidate, currForm, currBudget)) {
                    double maxValCandidate = MaxValCandidate(solutionCandidate, pleasures);
                    if (Double.IsNaN(maxVal) || maxValCandidate > maxVal) {
                        maxVal = maxValCandidate;
                        Array.Copy(solutionCandidate,solution, solutionCandidate.Length);
                    }
                }

            }

            int NoSolution = -1;
            int BoundedSolution = 0;
            int UnboundedSolution = 1;

            if (Double.IsNaN(maxVal)) {
                return NoSolution;
            }

            if (maxVal > 999999990.0) {
                return UnboundedSolution;
            }

            for (int i = 0; i < vars; i++) {
                x[i] = solution[i];
            }

            return BoundedSolution;
        }

        private double MaxValCandidate(double[] solutionCandidate, double[] pleasures)
        {
            double maxVal = 0;
            for (int i = 0; i < solutionCandidate.Length; i++) {
                maxVal += solutionCandidate[i] * pleasures[i];
            }

            return maxVal;
        }

        private bool VerifySolution(double[] solution, double[][] constraints, double[] budgets)
        {
            for (int row = 0; row < constraints.Length; row++) 
            {
                double LHS = 0;
                double RHS = budgets[row];

                for (int unknown = 0; unknown < solution.Length; unknown++) 
                    LHS += solution[unknown] * constraints[row][unknown];

                if (LHS > RHS) 
                    return false;
            }
            return true;
        }

        private int[] NotUsedIndices(int arraySize, int[] indices)
        {
            if (indices.Length >= arraySize)
                return new int[0];
            
            int[] a = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
                a[i] = i;
            
            
            HashSet<int> used = new HashSet<int>();
            foreach (int u in indices)
                used.Add(u);
            
            int counter = 0;
            int[] other = new int[a.Length - indices.Length];
            
            for (int i = 0; i < a.Length; i++)
                if (!used.Contains(a[i]))
                    other[counter++] = a[i];
            
            return other;
        }

        private double[] RowReduce(double[][] A, double[] b)
        {
            int len = A[0].Length;
            
            for (int p = 0; p < len; p++) 
            {
                int max = p;
                
                for (int i = p + 1; i < len; i++)
                    if (Math.Abs(A[i][p]) > Math.Abs(A[max][p]))
                        max = i;

                
                if (Math.Abs(A[max][p]) <= 0.000001) 
                {
                    throw new InvalidOperationException();
                }
                double[] temp = A[p];
                A[p] = A[max];
                A[max] = temp;
                
                double t = b[p];
                b[p] = b[max];
                b[max] = t;

                for (int i = p + 1; i < len; i++) 
                {
                    double alpha = A[i][p] / A[p][p];
                    b[i] -= alpha * b[p];
                    for (int j = p; j < len; j++) 
                        A[i][j] -= alpha * A[p][j];
                }
            }

            double[] solution = new double[len];
            for (int i = len - 1; i >= 0; i--) 
            {
                double sum = 0.0;
                for (int j = i + 1; j < len; j++)
                    sum += A[i][j] * solution[j];
                
                solution[i] = (b[i] - sum) / A[i][i];
            }

            return solution;
        }

        private void CurrForm(double[][] constraints, double[] budgets, double[][] currForm, double[] currFormBudget, int[] indices) 
        {
            for (int i = 0; i < indices.Length; i++) 
            {
                if (indices[i] < constraints.Length) 
                {
                    Array.Copy(constraints[indices[i]],currForm[i], constraints[i].Length);
                    currFormBudget[i] = budgets[indices[i]];
                } 
                else if (indices[i] == constraints.Length + constraints[0].Length) 
                {
                    Array.Fill(currForm[i], 1);
                    currFormBudget[i] = 1000000000.0;
                } 
                else 
                    currForm[i][indices[i] - constraints.Length] = -1;
            }
        }
        public class SubArraysOfArray 
        {
            private int subArraySize;
            private int arraySize;
            private List<int[]> subArrays;
            private int[] array;

            public SubArraysOfArray(int subArraySize, int arraySize) 
            {
                this.subArraySize = subArraySize;
                this.arraySize = arraySize;
                subArrays = new List<int[]>();
                if (subArraySize < 1) 
                    return;
                

                this.array = new int[arraySize];
                for (int i = 0; i < arraySize; i++) 
                    array[i] = i;
                

                for (int i = 0; i <= arraySize - subArraySize; i++) 
                {
                    int[] combo = new int[subArraySize];
                    combo[0] = array[i];
                    SetItem(i + 1, combo, 1);
                }
            }

            private void SetItem(int pos, int[] combo, int height) 
            {
                if (height >= this.subArraySize) 
                {
                    subArrays.Add(combo);
                    return;
                }
                for (int p = pos; p <= arraySize - subArraySize + height; p++) 
                {
                    int[] newcombo = new int[combo.Length];
                    Array.Copy(newcombo,combo,combo.Length);
                    newcombo[height] = array[p];
                    SetItem(p + 1, newcombo, height + 1);
                }
            }


            public List<int[]> GetSubArrays() 
            {
                return subArrays;
            }
        }

    }
}
