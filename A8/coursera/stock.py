# python3
import queue

class Graph:
    def read_data( self ):
        n, k = map(int, input().split())
        matrix = [list(map(int, input().split())) for i in range(n)]
        return matrix
    def min_charts( self, matrix ):
        n = len(matrix)
        k = len(matrix[0])
        adj = [[0] * n for _ in range(n)]
        for i in range(n):
            for j in range(n):
                if all([x < y for x, y in zip(matrix[i], matrix[j])]):
                    adj[i][j] = 1
        matching = [-1] * n
        busy_Right = [False] * n

        def bfs():
            visited = set()
            q = queue.Queue()
            q.put((1, None))
            visited.add((1, None))
            path = []
            parent = dict()
            while not q.empty():
                curr = q.get()
                if 1 == curr[0]: 
                    for i in range(n):
                        if -1 == matching[i]:
                            visited.add((2, i))
                            parent[(2, i)] = curr
                            q.put((2, i))
                elif 2 == curr[0]:  
                    i = curr[1]
                    for j in range(n):
                        if 1 == adj[i][j] and j != matching[i] and not (3, j) in visited:
                            visited.add((3, j))
                            parent[(3, j)] = curr
                            q.put((3, j))
                elif 3 == curr[0]:
                    j = curr[1]
                    if not busy_Right[j]:
                        prev = curr
                        curr = (4, j)
                        while True:
                            path.insert(0, (prev, curr))
                            if 1 == prev[0]:
                                break
                            curr = prev
                            prev = parent[curr]
                        for e in path:
                            if 2 == e[0][0]:
                                matching[e[0][1]] = e[1][1]
                            elif 3 == e[0][0] and 4 == e[1][0]:
                                busy_Right[e[1][1]] = True
                        return True  
                    else:
                        for i in range(n):
                            if j == matching[i] and not (2, i) in visited:
                                visited.add((2, i))
                                parent[(2, i)] = curr
                                q.put((2, i))
            return False 

        while bfs():
            continue
        return len([0 for i in matching if -1 == i])

    def solve( self ):
        matrix = self.read_data()
        result = self.min_charts(matrix)
        print(result)


if __name__ == '__main__':
    Graph = Graph()
    Graph.solve()
