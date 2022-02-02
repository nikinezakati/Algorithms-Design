using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace E2
{
    public class Q1Times : Processor
    {
        public Q1Times(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(41,41);
        }

        public override string Process(string inStr)
            => E2Processors.Q1Processor(inStr, Solve);

        public string[] Solve(char[][] board, string[] words)
        {
            List<string> result=new List<string>();
            var row_num=board.Length;
            var col_num=board[0].Length;
            foreach (var word in words)
            {
                if(WordExists(word,board,row_num,col_num))
                    result.Add(word);
            }
            result.Sort();
            return result.ToArray();
        }

        private bool WordExists(string word, char[][] board, int row, int col)
        {
            int l = word.Length;
            if (l > row * col)
                return false;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (board[i][j] == word[0])
                        if (FindMatch(board, word, i, j, row, col, 0))
                            return true;
                }
            }
            return false;
        }

        private bool FindMatch(char[][] board, string word, int x, int y, int row, int col, int level)
        {
            int l = word.Length;
            if (level == l)
                return true;
            if (x < 0 || y < 0 || x >= row || y >= col)
                return false;
            if (board[x][y] == word[level])
            {
                char curr = board[x][y];
                board[x][y] = '*';
                bool res = FindMatch(board, word, x - 1, y, row, col, level + 1) |
                            FindMatch(board, word, x + 1, y, row, col, level + 1) |
                            FindMatch(board, word, x, y - 1, row, col, level + 1) |
                            FindMatch(board, word, x, y + 1, row, col, level + 1);
                board[x][y] = curr;
                return res;
            }
            else
                return false;
        }
    }
}
