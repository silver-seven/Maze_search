using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_AI
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze("maze_map.csv");
            maze.printUnlabeledMaze();

            maze.getSolvedBFS();
            maze.printLabeledMaze("BFS");

            maze.reset();
            maze.getSolvedUCS();
            maze.printLabeledMaze("UCS");

            maze.reset();
            maze.getSolvedDFS();
            maze.printLabeledMaze("DFS");

            maze.reset();
            maze.getSolvedGreedy();
            maze.printLabeledMaze("Greedy Search");

            maze.reset();
            maze.getSolvedA_star();
            maze.printLabeledMaze("A* Search");

            Console.Read();
        }
    }
}
