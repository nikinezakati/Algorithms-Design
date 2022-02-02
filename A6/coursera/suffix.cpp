#include <algorithm>
#include <iostream>
#include <string>
#include <vector>
#include <algorithm>
#include <utility>

using std::cin;
using std::cout;
using std::endl;
using std::make_pair;
using std::pair;
using std::string;
using std::vector;


int main() {
  string text;
  cin >> text;
  vector<int> result(text.size());
  vector<pair<string, int> > suffix(text.size());
  for (int i = 0; i < text.size(); i++) {
    suffix[i] = make_pair(text.substr(i), i);
  }
  sort(suffix.begin(), suffix.end());
  for (int i = 0; i < suffix.size(); i++) {
    result[i] = suffix[i].second;
  }
  for (int i = 0; i < result.size(); ++i) {
    cout << result[i] << ' ';
  }
  cout << endl;
  return 0;
}
