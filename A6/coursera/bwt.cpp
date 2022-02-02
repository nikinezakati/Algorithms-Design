#include <algorithm>
#include <iostream>
#include <string>
#include <vector>

using std::cin;
using std::cout;
using std::endl;
using std::string;
using std::vector;

string BWT(const string& s) 
{
  string result = "";
  vector<string> matrix(s.size());

  for (int i = 0; i < matrix.size(); i++) 
  	matrix[i] = s.substr(i) + s.substr(0, i);
  

  sort(matrix.begin(), matrix.end());

  for (int i = 0; i < matrix.size(); i++) 
  	result += matrix[i][matrix.size() - 1];
  

  return result;
}

int main() {
  string s;
  cin >> s;
  cout << BWT(s) << endl;
  return 0;
}
