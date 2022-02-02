# python3
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

    def add_edge(self, from_, to, capacity):
        forward_edge = Edge(from_, to, capacity)
        backward_edge = Edge(to, from_, 0)
        self.graph[from_].append(len(self.edges))
        self.edges.append(forward_edge)
        self.graph[to].append(len(self.edges))
        self.edges.append(backward_edge)

    def size(self):
        return len(self.graph)

    def get_ids(self, from_):
        return self.graph[from_]

    def get_edge(self, id):
        return self.edges[id]

    def add_flow(self, id, flow):
        self.edges[id].flow += flow
        self.edges[id ^ 1].flow -= flow



def get_path(graph, from_,path):
    q=[]
    visited=[False]*graph.size()
    q.append(from_)
    visited[from_]=True
    i=0
    while q:
        u=q.pop(0)
        for p in graph.get_ids(u):
            e=graph.get_edge(p)
            if not (visited[e.v]) and e.flow<e.capacity:
                q.append(e.v)
                visited[e.v]=True
                path[e.v]=u
    return visited[graph.size()-1]

def find_edge(graph, from_, to):
    i=0
    for e in graph.edges:
        if (e.u==from_ and e.v==to) and e.capacity>e.flow:
            return (e.capacity-e.flow,i)
        i+=1
    
def max_flow(graph, from_, to,match,n):
    flow = 0
    parentl=[-1]*graph.size()
    while get_path(graph,from_,parentl):
        s=to
        minflow=10000
        path=[]
        while s!=from_:
            minflow=min(minflow,find_edge(graph,parentl[s],s)[0])
            path.append(find_edge(graph,parentl[s],s)[1])
            s=parentl[s]
        for p in path:
            graph.add_flow(p,minflow)
    for e in graph.edges:
        if e.flow==1 and e.u!=0 and e.v!=to:
            match[e.u-1]=e.v-n-1
    return flow
class MaxMatching:
    def read_data(self):
        n, m = map(int, input().split())
        adj_matrix = [list(map(int, input().split())) for i in range(n)]
        return adj_matrix

    def write_response(self, matching):
        line = [str(-1 if x == -1 else x + 1) for x in matching]
        print(' '.join(line))
                    
    def read_dataf(self,adj_matrix):
        n = len(adj_matrix)
        m = len(adj_matrix[0])
        matching = [-1] * n
        busy_right = [False] * m
        nodes=n+m+2
        edgel=[]
        for i in range(2,n+2):
            ed=(1,i,1)
            edgel.append(ed)
            j=0
            while j<m:
                if adj_matrix[i-2][j]==1:
                    ed=(i,n+2+j,1)
                    edgel.append(ed)
                ed=(n+2+j,nodes,1)
                edgel.append(ed)
                j+=1
        edgel=list(set(edgel))
        vertex_count, edge_count = nodes,len(edgel)
        graph = Graph(vertex_count)
        i=0
        for _ in range(edge_count):
            u, v, capacity = edgel[_]
            graph.add_edge(u - 1, v - 1, capacity)
        return graph
    def find_matching(self, adj_matrix):
        n = len(adj_matrix)
        m = len(adj_matrix[0])
        matching = [-1] * n
        busy_right = [False] * m

        graph=self.read_dataf(adj_matrix)
        max_flow(graph,0,n+m+1,matching,n)
        return matching

    def solve(self):
        adj_matrix = self.read_data()
        matching = self.find_matching(adj_matrix)
        self.write_response(matching)

if __name__ == '__main__':
    max_matching = MaxMatching()
    max_matching.solve()