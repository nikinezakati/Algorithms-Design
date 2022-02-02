# python3
import sys


def count_accour( pattern, bwt, inits, before_count ):
    top, bottom = 0, len(bwt) - 1

    while top <= bottom:
        if pattern:
            symbol = pattern[-1]
            pattern = pattern[:-1]
            if symbol in bwt[top:bottom + 1]:
                first_occurence = inits[symbol]
                top = first_occurence + before_count[symbol][top]
                bottom = first_occurence + before_count[symbol][bottom + 1] - 1
            else:
                return 0
        else:
            return bottom - top + 1
def process_bwt( bwt ):
    sorted_bwt = ''.join(sorted(bwt))
    char_set = set(sorted_bwt)
    inits = {x: sorted_bwt.find(x) for x in char_set}

    occ_count_before = {}

    for x in char_set:
        lst = [0]
        counter = 0
        for i in range(len(bwt)):
            if bwt[i] == x:
                counter += 1
            lst.append(counter)
        occ_count_before[x] = lst

    return inits, occ_count_before            

if __name__ == '__main__':
    bwt = sys.stdin.readline().strip()
    pattern_count = int(sys.stdin.readline().strip())
    patterns = sys.stdin.readline().strip().split()

    inits, before_count = process_bwt(bwt)
    result = []
    for pattern in patterns:
        result.append(count_accour(pattern, bwt, inits, before_count))
    print(' '.join(map(str, result)))
