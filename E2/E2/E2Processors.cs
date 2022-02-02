using System;
using System.Linq;

namespace E2
{
    // Exam Processors. Do not change this code.

    class E2Processors
    {
        public static string Q0Processor(string inStr, Func<int, int, int, long[,,], long[,,], string[]> Solve)
        {
            int M, N, P;
            var lines = inStr.Split('\n');
            int.TryParse(lines.First().Split(' ')[0], out M);
            int.TryParse(lines.First().Split(' ')[1], out N);
            int.TryParse(lines.First().Split(' ')[2], out P);
            long[,,] data1 = new long[M, N, P];
            long[,,] data2 = new long[M, N, P];

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    string[] line = lines[i * N + j + 1].Split(' ');

                    for (int k = 0; k < P; k++)
                    {
                        long.TryParse(line[k], out data1[i, j, k]);
                    }
                }
            }

            for (int i = M; i < 2 * M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    string[] line = lines[i * N + j + 1].Split(' ');

                    for (int k = 0; k < P; k++)
                    {
                        long.TryParse(line[k], out data2[i - M, j, k]);
                    }
                }
            }

            return string.Join("\n", Solve(M, N, P, data1, data2));
        }

        public static string Q1Processor(string inStr, Func<char[][], string[], string[]> Solve)
        {
            var lines = inStr.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var words = lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int n = lines.Length - 1;
            var board = new char[n][];
            for (int i = 0; i < n; i++)
            {
                board[i] = lines[i + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(d => d[0]).ToArray();
            }
            return string.Join(" ", Solve(board, words));
        }

        public static string Q2Processor(string inStr, Func<long[], long[], long[][], bool> solve)
        {
            var lines = inStr.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int n = int.Parse(lines[0]);
            long[][] G = new long[n][];
            long[] W = new long[n];
            long[] R = new long[n];
            for (int i = 0; i < n; i++)
            {
                var nextTeamRow = lines[i + 1];
                var tokens = nextTeamRow.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                W[i] = long.Parse(tokens[1]);
                G[i] = new long[n];
                for (int j = 0; j < n; j++)
                {
                    G[i][j] = long.Parse(tokens[j + 4]);
                    R[i] += G[i][j];
                }
            }
            return solve(W, R, G) ? "1" : "0";
        }
    }
}
