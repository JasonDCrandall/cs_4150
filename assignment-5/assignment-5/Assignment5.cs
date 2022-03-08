using System;
using System.Collections.Generic;

namespace assignment_5
{
    class Assignment5
    {

        // For my sanity: The order of pushing onto the stack is row, col
        // The order of popping from the stack is col, row
        static string [,] maze;
        static int [,] visited;
        static int[] start = new int[2];
        static Stack<int> bag = new Stack<int>();
        static void Main(string[] args)
        {
            int rows;
            int columns;
            string input = Console.ReadLine();
            rows = int.Parse(input.Split(' ')[0]);
            columns = int.Parse(input.Split(' ')[1]);
            buildMaze(rows, columns);
            int treasure = traverseMaze(rows, columns);
            printMaze(rows, columns);
            Console.WriteLine(treasure);
        }

        static void buildMaze(int rows, int columns)
        {
            maze = new string[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                string input = Console.ReadLine();
                for (int j = 0; j < columns; j++)
                {
                    string value = input[j].ToString();
                    if (value == "p")
                    {
                        start[0] = i;
                        start[1] = j;
                    }
                    maze[i,j] = input[j].ToString();
                }
            }
        }

        // This function marks all the reachable cells and acts as the WFS algorithm
        // and returns the total treasure that can be reached
        static int traverseMaze(int rows, int columns) {
            int totalTreasure = 0;
            visited = new int[rows, columns];
            bag.Push(start[0]);
            bag.Push(start[1]);
            while (bag.Count != 0) {
                //take v from bag
                int col = bag.Pop();
                int row = bag.Pop();
                //if v is unmarked
                if (visited[row,col] == 0) {
                    try {
                        totalTreasure += int.Parse(maze[row,col]);
                    }
                    catch (Exception e) {
                        //do nothing
                    }
                    //mark v as visited
                    visited[row,col] = 1;
                    //push v's neighbors onto the bag
                    if(!monsterNearby(row, col)) {
                        if (row - 1 >= 0 && maze[row - 1,col] != "#") {
                            bag.Push(row - 1);
                            bag.Push(col);
                        }
                        if (row + 1 < rows && maze[row + 1,col] != "#") {
                            bag.Push(row + 1);
                            bag.Push(col);
                        }
                        if (col - 1 >= 0 && maze[row,col - 1] != "#") {
                            bag.Push(row);
                            bag.Push(col - 1);
                        }
                        if (col + 1 < columns && maze[row,col + 1] != "#") {
                            bag.Push(row);
                            bag.Push(col + 1);
                        }
                    }
                }
            }
            return totalTreasure;
        }

        static bool monsterNearby(int row, int col) {
            if (row - 1 >= 0 && maze[row - 1,col] == "m") {
                return true;
            }
            if (row + 1 < maze.GetLength(0) && maze[row + 1,col] == "m") {
                return true;
            }
            if (col - 1 >= 0 && maze[row,col - 1] == "m") {
                return true;
            }
            if (col + 1 < maze.GetLength(1) && maze[row,col + 1] == "m") {
                return true;
            }
            return false;
        }

        static void findOptimalPlacement() {

        }

        static void printMaze(int rows, int columns) {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(visited[i,j]);
                }
                Console.WriteLine();
            }
        }
    }
}
