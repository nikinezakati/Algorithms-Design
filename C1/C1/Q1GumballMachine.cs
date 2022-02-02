using System;
using System.Collections.Generic;
using TestCommon;

namespace C1
{
    public class Node
    {
        public long value;
        public long distance;
        public Node(long v, long d)
        {
            this.value=v;
            this.distance=d;
        }
        public long getValue()
        {
            return value;
        }
        public void setDistance(long d)
        {
            this.distance=d;
        }
        public long getDistance()
        {
            return this.distance;
        }

    }
    
    public class Q1GumballMachine : Processor
    {
        
        public Q1GumballMachine(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long>)Solve);

        
        public long Solve(long x, long y)
        {
            Queue<Node> nodes= new Queue<Node>();
            HashSet<long> visited=new HashSet<long>();
            Node root=new Node(x,0);
            nodes.Enqueue(root);
            while(nodes.Count!=0)
            {
                Node current=nodes.Dequeue();
                visited.Add(current.value);
                if(current.value==y)
                    return current.distance;
                if(!visited.Contains(current.value-1))
                    nodes.Enqueue(new Node(current.value-1,current.distance+1));
                if(current.value<y && !visited.Contains(current.value*2))
                    nodes.Enqueue(new Node(current.value*2,current.distance+1));    
            }
            return -1;
        }
    }
}
