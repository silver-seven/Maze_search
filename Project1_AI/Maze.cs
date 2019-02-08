using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project1_AI
{
    class Maze
    {
        public Node[,] maze_input;
        public Node[,] maze_bck;
        public int width;
        public int height;
        public int label_count;
        public string file_name;
        public Node start;
        public Node end;

        public Maze(string input_file)
        {
            this.width = 11;
            this.height = 8;
            this.label_count = 1;
            this.file_name = input_file;

            int h_counter = 0;
            foreach (string line in File.ReadLines(input_file))     //count number of lines in file
            {
                string[] temp_line = line.Split(',');
                this.width = temp_line.Length;
                h_counter++;  
            }
            this.height = h_counter;

        
            this.maze_input = new Node[this.height, this.width];
            h_counter = 0;
            foreach (string line in File.ReadLines(input_file))     //read test file
            {
                string[] temp_line = line.Split(',');
                for (int i = 0; i < this.width; i++)
                {
                    Node temp_node = new Node();
                    temp_node.h = h_counter;
                    temp_node.w = i;
                    switch (temp_line[i])
                    {
                        case "[e]":
                            temp_node.isEnd = true;
                            temp_node.label = -3;
                            this.end = new Node(temp_node);
                            break;
                        case "[x]":
                            temp_node.isBlocked = true;
                            temp_node.label = -2;
                            break;
                        case "[s]":
                            temp_node.isStart = true;
                            temp_node.label = 0;
                            temp_node.isLabeled = true;
                            this.start = new Node(temp_node);
                            break;
                        default:
                            temp_node.label = -1;
                            break;
                    }
 
                    maze_input[h_counter, i] = temp_node;
                }
                h_counter++;
            }
            this.maze_bck = maze_input;
        }
        public void printUnlabeledMaze()
        {
            for (int i = 0; i < this.height; i++)
            {
                Console.WriteLine("\n");
                for (int j = 0; j < this.width; j++)
                {
                    Node temp = maze_bck[i, j];
                    if (temp.isBlocked)
                    {
                        Console.Write("[x] ");
                    }
                    else if (temp.isEnd)
                    {
                        Console.Write("[e] ");
                    }
                    else if (temp.isStart)
                    {
                        Console.Write("[s] ");
                    }
                    else
                    {
                        Console.Write("[]  ");
                    }
                }
            }
            Console.WriteLine("\n");
            //Console.WriteLine("\n" + this.start.w + "," + this.start.h);
        }
        public void printLabeledMaze(string label)
        {

            for (int i = 0; i < this.height; i++)
            {
                Console.WriteLine("\n");
                for (int j = 0; j < this.width; j++)
                {
                    Node temp = maze_input[i, j];
                    if (temp.isBlocked)
                    {
                        Console.Write("## ");
                    }
                    else if (temp.isEnd)
                    {
                        Console.Write(temp.label + " ");
                    }
                    else if (temp.isStart)
                    {
                        Console.Write("00 ");
                    }
                    else
                    {
                        if (temp.isLabeled)
                        {
                            if (temp.label > 9)
                            {
                                Console.Write(temp.label + " ");
                            }
                            else
                            {
                                Console.Write("0" + temp.label + " ");
                            }
                        }
                        else
                        {
                            Console.Write("[] ");
                        }
                    }
                }
            }
            Console.WriteLine("\n" + label);
            Console.WriteLine("");
        }
        public bool updateSurroundNodes(Node center, ref Node[] nodeOut)
        {
            /*Checks param center for adjacent nodes
                updates label, total cost, estimated cost, and estimated+total cost
                returns true if end is found*/
            bool isEndFound = false;

            //check adjacent nodes
            //west
            int new_pos = center.w - 1;
            if (new_pos >= 0 && !isEndFound)           //make sure it is in maze boundaries
            {
                if (maze_input[center.h, new_pos].isEnd)    //if end is found update
                {
                    isEndFound = true;
                }
                if (!maze_input[center.h, new_pos].isBlocked && !maze_input[center.h, new_pos].isStart && !maze_input[center.h, new_pos].isLabeled)     //only unlabeled
                {
                    nodeOut[0] = maze_input[center.h, new_pos];
                    //Console.WriteLine("W " + nodeOut[0].h + "," + nodeOut[0].w + "," + center.isBlocked);
                }
            }

            //north
            new_pos = center.h - 1;
            if (new_pos >= 0 && !isEndFound)
            {
                if (maze_input[new_pos, center.w].isEnd)
                {
                    isEndFound = true;
                }
                if (!maze_input[new_pos, center.w].isBlocked && !maze_input[new_pos, center.w].isStart && !maze_input[new_pos, center.w].isLabeled)
                {
                    nodeOut[1] = maze_input[new_pos, center.w];
                  //  Console.WriteLine("N " + nodeOut[1].h + "," + nodeOut[1].w);
                }
            }

            //east
            new_pos = center.w + 1;
            if (new_pos < this.width && !isEndFound)
            {
                if (maze_input[center.h, new_pos].isEnd)
                {
                    isEndFound = true;
                }

                if (!maze_input[center.h, new_pos].isBlocked && !maze_input[center.h, new_pos].isStart && !maze_input[center.h, new_pos].isLabeled)
                {
                    nodeOut[2] = maze_input[center.h, new_pos];
                 //   Console.WriteLine("E " + nodeOut[2].h + "," + nodeOut[2].w);
                }
            }

            //south
            new_pos = center.h + 1;
            if (new_pos < this.height && !isEndFound)
            {
                if (maze_input[new_pos, center.w].isEnd)
                {
                    isEndFound = true;
                }

                if (!maze_input[new_pos, center.w].isBlocked && !maze_input[new_pos, center.w].isStart && !maze_input[new_pos, center.w].isLabeled)
                {
                    nodeOut[3] = maze_input[new_pos, center.w];
                  //  Console.WriteLine("S " + nodeOut[3].h + "," + nodeOut[3].w);
                }
            }

            //update main maze labels & cost
            for (int i = 0; i < 4; i++)
            {
                if (nodeOut[i] != null)
                {
                    int _h = nodeOut[i].h;
                    int _w = nodeOut[i].w;
               //     Console.WriteLine(i + " " + _h + ",," + _w + " ");
                    maze_input[_h, _w].label = label_count;     //update label
                    maze_input[_h, _w].isLabeled = true;
                    label_count++;
                //    Console.WriteLine(label_count);
                    int temp_cost = 0;                          //determine windy cost
                    switch (i)
                    {
                        case 1:
                            temp_cost = 1;
                            break;
                        case 3:
                            temp_cost = 3;
                            break;
                        default:
                            temp_cost = 2;
                            break;
                    }
                    int c = i;
                    int tot_c = center.total_cost + temp_cost;// add windy cost to total
                    maze_input[_h, _w].total_cost = tot_c;  
                    int est_c = Math.Abs(this.end.w - _w)*2 + Math.Abs(_h - this.end.h)*1;//calculate distance from end
                    maze_input[_h, _w].est_cost = est_c;
                    int fn_c = tot_c + est_c; //calculate f(n) cost
                    maze_input[_h, _w].fn_cost = fn_c;
                    nodeOut[i] = new Node(maze_input[_h, _w]);  //update nodeOut based on maze

                }
           //     Console.WriteLine(i + " " + nodeOut[i]);
            }
            if (isEndFound)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void reset()
        {
            this.label_count = 1;

            this.maze_input = new Node[this.height, this.width];
            int h_counter = 0;
            foreach (string line in File.ReadLines(file_name))     //read test file
            {
                string[] temp_line = line.Split(',');
                for (int i = 0; i < this.width; i++)
                {
                    Node temp_node = new Node();
                    temp_node.h = h_counter;
                    temp_node.w = i;
                    switch (temp_line[i])
                    {
                        case "[e]":
                            temp_node.isEnd = true;
                            temp_node.label = -3;
                            this.end = new Node(temp_node);
                            break;
                        case "[x]":
                            temp_node.isBlocked = true;
                            temp_node.label = -2;
                            break;
                        case "[s]":
                            temp_node.isStart = true;
                            temp_node.label = 0;
                            temp_node.isLabeled = true;
                            this.start = new Node(temp_node);
                            break;
                        default:
                            temp_node.label = -1;
                            break;
                    }

                    maze_input[h_counter, i] = temp_node;
                }
                h_counter++;
            }
            this.maze_bck = maze_input;
        }
    
        public void getSolvedBFS()
        {
            Queue<Node> frontier = new Queue<Node>();
            frontier.Enqueue(this.start);
            
            while(frontier.Count > 0)
            {
                Node[] sur_nodes = new Node[4];
                Node temp_node = new Node(frontier.Dequeue());
               // Console.WriteLine("count" + frontier.Count);
                bool endFound = updateSurroundNodes(temp_node, ref sur_nodes);  //update surrounding nodes
                if(endFound)
                {
                    break;
                }
                else
                {
                    for(int i = 0; i < 4; i++)
                    {
                        if(sur_nodes[i] != null)
                        {
                            frontier.Enqueue(maze_input[sur_nodes[i].h, sur_nodes[i].w]);
                        }
                        else
                        {

                        }
                    }
                }
            }
            
        }
        public void getSolvedUCS()
        {
            List<Node> frontier = new List<Node>();
            frontier.Add(this.start);

            while (frontier.Count > 0)
            {
                Node[] sur_nodes = new Node[4];
                Node temp_node = new Node(frontier[0]);
                frontier.RemoveAt(0);
                //Console.WriteLine("count" + frontier.Count);
                bool endFound = updateSurroundNodes(temp_node, ref sur_nodes);
                if (endFound)  //exit found
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (sur_nodes[i] != null)
                        {
                            frontier.Add(sur_nodes[i]);
                            //makeshift priority queue using List
                            frontier.Sort((x, y) => x.label.CompareTo(y.label));                //sort label
                            frontier.Sort((x, y) =>  x.total_cost.CompareTo(y.total_cost));     //sort descending


                        }
                    }
                 /*   Console.WriteLine("sort");
                    foreach (var item in frontier)
                    {

                        Console.WriteLine(item.total_cost + ": " + item.label);
                    }*/
                }
            }

        }
        public void getSolvedDFS()
        {
            Stack<Node> frontier = new Stack<Node>();
            frontier.Push(this.start);

            while (frontier.Count > 0)
            {
                Node[] sur_nodes = new Node[4];
                Node temp_node = new Node(frontier.Pop());
                // Console.WriteLine("count" + frontier.Count);
                bool endFound = updateSurroundNodes(temp_node, ref sur_nodes);
                if (endFound)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (sur_nodes[i] != null)
                        {
                            frontier.Push(maze_input[sur_nodes[i].h, sur_nodes[i].w]);
                        }
                        else
                        {

                        }
                    }
                }
            }

        }
        public void getSolvedGreedy()
        {
            List<Node> frontier = new List<Node>();
            frontier.Add(this.start);

            while (frontier.Count > 0)
            {
                Node[] sur_nodes = new Node[4];
                Node temp_node = new Node(frontier[0]);
                frontier.RemoveAt(0);
                //Console.WriteLine("count" + frontier.Count);
                bool endFound = updateSurroundNodes(temp_node, ref sur_nodes);
                if (endFound)  //exit found
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (sur_nodes[i] != null)
                        {
                            frontier.Add(sur_nodes[i]);
                            //makeshift priority queue using List
                            frontier.Sort((x, y) => x.label.CompareTo(y.label));                //sort label
                            frontier.Sort((x, y) => x.est_cost.CompareTo(y.est_cost));     //sort descending


                        }
                    }
               /*     Console.WriteLine("sort");
                    foreach (var item in frontier)
                    {
                        Console.WriteLine("l: " + item.label + " w:" + item.w + " h:" + item.h);
                        Console.WriteLine("t: " + item.total_cost + " e:" + item.est_cost + " f:" + item.fn_cost);
                    }*/
                }
            }

        }
        public void getSolvedA_star()
        {
            List<Node> frontier = new List<Node>();
            frontier.Add(this.start);

            while (frontier.Count > 0)
            {
                Node[] sur_nodes = new Node[4];
                Node temp_node = new Node(frontier[0]);
                frontier.RemoveAt(0);
                //Console.WriteLine("count" + frontier.Count);
                bool endFound = updateSurroundNodes(temp_node, ref sur_nodes);
                if (endFound)  //exit found
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (sur_nodes[i] != null)
                        {
                            frontier.Add(sur_nodes[i]);
                            //makeshift priority queue using List
                            frontier.Sort((x, y) => x.label.CompareTo(y.label));                //sort label descending
                            frontier.Sort((x, y) => x.fn_cost.CompareTo(y.fn_cost));     //sort fn cost descending


                        }
                    }
                /*    Console.WriteLine("sort");
                    foreach (var item in frontier)
                    {

                        Console.WriteLine("l: " + item.label + " w:" + item.w + " h:" + item.h);
                        Console.WriteLine("t: " + item.total_cost + " e:" + item.est_cost + " f:" + item.fn_cost);
                    }*/
                }
            }

        }

    }
}
