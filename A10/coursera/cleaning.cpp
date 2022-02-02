#include <iostream>
#include <vector>
#include <cstdio>

using std::cin;
using std::vector;
using std::ios;

struct Graph {

    Graph(int n, int m)
        : clauses_num(0)
        , vertices_num(n)
        , matrix(n, vector<bool>(n, false))
        , data(n, vector<int>(n))
    {
        for (int i = 0, cnt = 0; i < vertices_num; ++i) {
            for (int j = 0; j < vertices_num; ++j) {
                data[i][j] = ++cnt;
            }
        }
    }

    void SAT(const int max_clauses_num) 
    {
        clauses_stream.reserve(max_clauses_num * 3);

        each_vertext_in_path();
        each_vertext_in_path_only_once();
        at_most_one_in_path();
        exactly_one_in_position();
        only_adj_in_path();

        printf("%d %d \n%s", clauses_num, vertices_num * vertices_num, clauses_stream.c_str());
    }

    void each_vertext_in_path() 
    {
        for (int i = 0; i < vertices_num; ++i, ++clauses_num) {
            for (int j = 0; j < vertices_num; ++j) {
                clauses_stream += std::to_string(data[i][j]) + " ";
            }
            clauses_stream += "0\n";
        }
    }

    void each_vertext_in_path_only_once() 
    {
        for (int i = 0; i < vertices_num; ++i) {
            for (int j = 0; j < vertices_num; ++j) {
                for (int k = i + 1; k < vertices_num; ++k, ++clauses_num) {
                    clauses_stream += std::to_string(-data[i][j]) + " " + std::to_string(-data[k][j]) + " 0\n";
                }
            }
        }
    }

    void at_most_one_in_path() 
    {
        for (int i = 0; i < vertices_num; ++i, ++clauses_num) {
            for (int j = 0; j < vertices_num; ++j) {
                clauses_stream += std::to_string(data[j][i]) + " ";
            }
            clauses_stream += "0\n";
        }
    }

    void exactly_one_in_position() 
    {
        for (int i = 0; i < vertices_num; ++i) {
            for (int j = 0; j < vertices_num; ++j) {
                for (int k = j + 1; k < vertices_num; ++k, ++clauses_num) {
                    clauses_stream += std::to_string(-data[i][j]) + " " + std::to_string(-data[i][k]) + " 0\n";
                }
            }
        }
    }

    void only_adj_in_path() 
    {
        for (int i = 0; i < vertices_num; ++i) {
            for (int j = 0; j < vertices_num; ++j) {
                if (!matrix[i][j] && j != i) {
                    for (int k = 0; k < vertices_num - 1; ++k, ++clauses_num) {
                        clauses_stream += std::to_string(-data[i][k]) + " " + std::to_string(-data[j][k + 1]) + " 0\n";
                    }
                }
            }
        }
    }

    unsigned int clauses_num;
    const unsigned int vertices_num;
    vector<vector<bool> > matrix;
    vector<vector<int> > data;
    std::string clauses_stream;
};

int main()
{
    ios::sync_with_stdio(false);

    int n, m;
    cin >> n >> m;

    Graph converter(n, m);

    for (int k = 0; k < m; ++k) {
        int i, j;
        cin >> i >> j;
        converter.matrix[i - 1][j - 1] = true;
        converter.matrix[j - 1][i - 1] = true;
    }

    converter.SAT(120000);

    return 0;
}