# python3
import sys

def build_trie(patterns):
    tree = dict()
    tree[0] = {}
    nodeNumber = 1
    for pattern in patterns:
    	currentNode = 0
    	for c in pattern:
    		if c not in tree[currentNode]:
    			tree[currentNode][c] = nodeNumber
    			tree[nodeNumber] = {}
    			nodeNumber += 1
    		currentNode = tree[currentNode][c]
    return tree


if __name__ == '__main__':
    patterns = sys.stdin.read().split()[1:]
    tree = build_trie(patterns)
    for node in tree:
        for c in tree[node]:
            print("{}->{}:{}".format(node, tree[node][c], c))