#include <iostream>
#include <limits>
#include <vector>
#include <queue>

using std::vector;
using std::queue;
using std::pair;
using std::priority_queue;

void shortest_paths(vector<vector<int> > &adjancency, vector<vector<int> > &cost, int s, vector<long long> &distance, vector<int> &reachable, vector<int> &shortest) {
  reachable[s] = 1;
  distance[s] = 0;
  queue<int> q;
  for (int i = 0; i < adjancency.size(); i++) {
    for (int u = 0; u < adjancency.size(); u++){
      for (int k = 0; k < adjancency[u].size(); k++) {
        int v = adjancency[u][k];
        if (distance[u]!= std::numeric_limits<long long>::max() && distance[v] > distance[u] + cost[u][k]) {
          distance[v] = distance[u] + cost[u][k];
          reachable[v] = 1;
          if (i == adjancency.size() - 1) {
            q.push(v);
          }	
        }
      }
    }
  }
  
  vector<int> visited(adjancency.size());
  while (!q.empty()){
    int u = q.front();
	q.pop();
	visited[u] = 1;
	shortest[u] = 0;
	for (int i = 0; i < adjancency[u].size(); i++) {
	  int v = adjancency[u][i];
      if (!visited[v]) {
        q.push(v);
        visited[v] = 1;
        shortest[v] = 0;
	  }
	}
  }
  distance[s] = 0;
}

int main() {
  int n, m, s;
  std::cin >> n >> m;
  vector<vector<int> > adjancency(n, vector<int>());
  vector<vector<int> > cost(n, vector<int>());
  for (int i = 0; i < m; i++) {
    int x, y, w;
    std::cin >> x >> y >> w;
    adjancency[x - 1].push_back(y - 1);
    cost[x - 1].push_back(w);
  }
  std::cin >> s;
  s--;
  vector<long long> distance(n, std::numeric_limits<long long>::max());
  vector<int> reachable(n, 0);
  vector<int> shortest(n, 1);
  shortest_paths(adjancency, cost, s, distance, reachable, shortest);
  for (int i = 0; i < n; i++) {
    if (!reachable[i]) {
      std::cout << "*\n";
    } else if (!shortest[i]) {
      std::cout << "-\n";
    } else {
      std::cout << distance[i] << "\n";
    }
  }
}