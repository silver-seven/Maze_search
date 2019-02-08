using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_AI
{
    class Node
    {
        public int h;
        public int w;
        public bool isEnd;
        public bool isStart;
        public bool isBlocked;
        public bool isLabeled;
        public int label;
        public int total_cost;
        public int est_cost;
        public int fn_cost;     //f(n) for A* search

        public Node() 
        {
            this.h = 0;
            this.w = 0;
            this.isEnd = false;
            this.isStart = false;
            this.isBlocked = false;
            this.isLabeled = false;
            this.label = -1;
            this.total_cost = 0;
            this.est_cost = 0;
            this.fn_cost = 0;
        }

        public Node(Node nodeCopy)  
        {
            this.h = nodeCopy.h;
            this.w = nodeCopy.w;
            this.isEnd = nodeCopy.isEnd;
            this.isStart = nodeCopy.isStart;
            this.isBlocked = nodeCopy.isBlocked;
            this.isLabeled = nodeCopy.isLabeled;
            this.label = nodeCopy.label;
            this.total_cost = nodeCopy.total_cost;
            this.est_cost = nodeCopy.est_cost;
            this.fn_cost = nodeCopy.fn_cost;
        }
    }
}
