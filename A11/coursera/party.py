#python3

import sys
import threading


sys.setrecursionlimit(10**6) 
threading.stack_size(2**26)  


class Vertex:
    def __init__(self, weight):
        self.children = []
        self.weight = weight

def Read_Tree():
    size = int(input())
    tree = [Vertex(weight) for weight in map(int, input().split())]
    for i in range(1, size):
        a, b = list(map(int, input().split()))
        tree[a - 1].children.append(b - 1)
        tree[b - 1].children.append(a - 1)
    return tree

def dfs(tree, vertex, parent, D):
    
    if -1 == D[vertex]:
        if 1 == len(tree[vertex].children) and 0 != vertex:
            D[vertex] = tree[vertex].weight
        else:
            m1 = tree[vertex].weight
            for u in tree[vertex].children:
                if u != parent:
                    for w in tree[u].children:
                        if w != vertex:
                            m1 += dfs(tree, w, u, D)
            m0 = 0
            for u in tree[vertex].children:
                if u != parent:
                    m0 += dfs(tree, u, vertex, D)
            D[vertex] = max(m1, m0)
    return D[vertex]

def MaxWeightIndependentTreeSubset(tree):
    size = len(tree)
    if size == 0:
        return 0
    D = [-1] * size
    d = dfs(tree, 0, -1, D)
    return d


def main():
    tree = Read_Tree()
    weight = MaxWeightIndependentTreeSubset(tree)
    print(weight)


threading.Thread(target=main).start()
