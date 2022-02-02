# python3
import queue

class Edge:

    def __init__(self, u, v, capacity):
        self.u = u
        self.v = v
        self.capacity = capacity
        self.flow = 0


class Graph:
    def __init__(self, n):
        self.edges = []
        self.graph = [[] for _ in range(n)]

    def add_edge(self, src, des, capacity):
        forward_edge = Edge(src, des, capacity)
        backward_edge = Edge(des, src, 0)
        self.graph[src].append(len(self.edges))
        self.edges.append(forward_edge)
        self.graph[des].append(len(self.edges))
        self.edges.append(backward_edge)

    def size(self):
        return len(self.graph)

    def get_adjs(self, src):
        return self.graph[src]

    def get_edge(self, adj):
        return self.edges[adj]

    def add_flow(self, adj, flow):
        self.edges[adj].flow += flow
        self.edges[adj ^ 1].flow -= flow
        self.edges[adj].capacity -= flow
        self.edges[adj ^ 1].capacity += flow


def max_flow(graph, src, des):
    flow = 0
    while True:
        has_path, path, X = bfs(graph, src, des)
        if not has_path:
            return flow
        for adj in path:
            graph.add_flow(adj, X)
        flow += X
    return flow

def bfs(graph, src, des):
    X = float('inf')
    has_path = False
    n = graph.size()
    dist = [float('inf')]*n
    path = []
    parent = [(None, None)]*n
    q = queue.Queue()
    dist[src] = 0
    q.put(src)
    while not q.empty():
        currFromNode = q.get()
        for adj in graph.get_adjs(currFromNode):
            currEdge = graph.get_edge(adj)
            if float('inf') == dist[currEdge.v] and currEdge.capacity > 0:
                dist[currEdge.v] = dist[currFromNode] + 1
                parent[currEdge.v] = (currFromNode, adj)
                q.put(currEdge.v)
                if currEdge.v == des:
                    while True:
                        path.insert(0, adj)
                        currX = graph.get_edge(adj).capacity
                        X = min(currX, X)
                        if currFromNode == src:
                            break
                        currFromNode, adj = parent[currFromNode]
                    has_path = True
                    return has_path, path, X
    return has_path, path, X

if __name__ == '__main__':
    vertex_count, edge_count = map(int, input().split())
    graph = Graph(vertex_count)
    for _ in range(edge_count):
        u, v, capacity = map(int, input().split())
        graph.add_edge(u - 1, v - 1, capacity)
    print(max_flow(graph, 0, graph.size() - 1))