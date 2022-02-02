#include <algorithm>
#include <iostream>
#include <string>
#include <vector>
#include <utility>

using std::cin;
using std::cout;
using std::endl;
using std::string;
using std::vector;
using std::pair;
using std::make_pair;

string bwt_inverse(const string& text) 
{
  string result = "";
  vector<pair<char, int> > first_col(text.size());

  for (int i = 0; i < text.size(); i++) 
  	first_col[i] = make_pair(text[i], i);
  
  sort(first_col.begin(), first_col.end());

  pair<char, int> ch = first_col[0];

  for (int i = 0; i < text.size(); i++) {
  	ch = first_col[ch.second];
  	result += ch.first;
  }

  return result;
}

int main() {
  string text;
  cin >> text;
  cout << bwt_inverse(text) << endl;
  return 0;
}