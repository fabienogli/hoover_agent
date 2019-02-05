#!/usr/bin/python
import numpy

def h(a,b,c):
    alpha = 0.1
    beta = 0.2
    gamma = 10.0
    dustSnorted = alpha*(1-a)
    jewelPicked = beta*(1-b)
    jewelSnorted = gamma*c
    result = dustSnorted + jewelPicked + jewelSnorted
    return result

def main():
    ref = h(1,1, 0)
    if ref != 0.0:
        print "your ref is bad, it should be 0, it's:", ref
        return 1
    for a in numpy.arange(0, 1, 0.01):
        for b in numpy.arange(0, 1, 0.01):
            for c in numpy.arange(0, 1, 0.01):
                shitty_heuristic = permut_h(a,b,c, 0, 0) 
                if shitty_heuristic:
                    print "your heuristic sucks"
                    return 1
    print "your heuristic is kinda working but you still suck"


def reject_h(result):
    return result < 0

def permut_h(a,b,c, result, count): 
    return reject_h(h(a,b,c)) or reject_h(h(b, a,c)) or reject_h(h(c, b, a)) or reject_h(h(c, a, b)) or reject_h(h(a,c,b)) or reject_h(h(b,c, a))


if  __name__ == '__main__':
    main()
