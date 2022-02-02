import sys
import math


class Node:
    def __init__(self, a, b, c):
        self.x = a
        self.y = b
        self.parent = c
        self.rank = 0

class Edge:
    def __init__(self, a, b, c):
        self.u = a
        self.v = b
        self.weight = c

def make_set(i, nodes, x, y):
    nodes.append(Node(x[i], y[i], i))

def weight(x1, y1, x2, y2):
  return math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))

def find(i, nodes):
  if (i != nodes[i].parent) :
        nodes[i].parent = find(nodes[i].parent, nodes)
  return nodes[i].parent

def union(u, v, nodes):
    r1 = find(u, nodes)
    r2 = find(v, nodes)
    if (r1 != r2):
        if (nodes[r1].rank > nodes[r2].rank):
            nodes[r2].parent = r1
        else:
            nodes[r1].parent = r2
            if (nodes[r1].rank == nodes[r2].rank):
                nodes[r2].rank += 1

def mst(x, y):
    result = 0.
    n = len(x)
    nodes = []
    for i in range(n):
       make_set(i, nodes, x, y)
    edges = []
    for i in range(n):
        for j in range(i+1, n):
            edges.append(Edge(i, j, weight(x[i], y[i], x[j], y[j])))
    edges = sorted(edges, key=lambda edge: edge.weight)
    for edge in edges:
        if find(edge.u, nodes) != find(edge.v, nodes):
            result += edge.weight
            union(edge.u, edge.v, nodes)
    return result


if __name__ == '__main__':
    input = sys.stdin.read()
    temp = list(map(int, input.split()))
    n = temp[0]
    x = temp[1::2]
    y = temp[2::2]
    print("{0:.9f}".format(mst(x, y)))