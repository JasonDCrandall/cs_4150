/*
 * Problem Set 5: This assignment finds the optimal placement of a monster in a maze to prevent
 * a player from getting the most treasure. This is done via a modification of "Whatever First Search"
 *
 * By: Jason Crandall u0726408
*/


using System;
using System.Collections.Generic;

namespace assignment_5
{
    class Assignment5
    {
        static char [,] maze;
        static int [,] visited;
        static int[] start = new int[2];
        static Stack<int> bag = new Stack<int>();
        static int lowestTreasure;
        static void Main(string[] args)
        {
            int rows;
            int columns;
            string input = Console.ReadLine();
            rows = int.Parse(input.Split(' ')[0]);
            columns = int.Parse(input.Split(' ')[1]);
            buildMaze(rows, columns);
            lowestTreasure = traverseMaze(rows, columns);
            int[] bestCoordinates = findOptimalPlacement(rows, columns);
            Console.WriteLine(bestCoordinates[0] + " " + bestCoordinates[1]);
            Console.WriteLine(lowestTreasure);
        }

        // Initial construction of the maze
        static void buildMaze(int rows, int columns)
        {
            maze = new char[rows, columns];
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
                    maze[i,j] = input[j];
                }
            }
        }
        
        // This function stores the coordinates of the optimal placement of the monster
        static int[] findOptimalPlacement(int rows, int columns) {
            int[] bestCoordinates = new int[2] {-1, -1};
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    if (!tooCloseToPlayer(i,j) && maze[i,j] == '.') {
                        maze[i,j] = 'm';
                        int treasure = traverseMaze(rows, columns);
                        maze[i,j] = '.';
                        if (treasure < lowestTreasure) {
                            lowestTreasure = treasure;
                            bestCoordinates[0] = i;
                            bestCoordinates[1] = j;
                        }
                    }
                }
            }
            return bestCoordinates;
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

                    // check if found treasure is already more than the most optimal
                    if (char.IsDigit(maze[row,col])) {
                        totalTreasure += maze[row,col] - 48;
                        if (totalTreasure > lowestTreasure)
                        {
                            bag.Clear();
                           return totalTreasure;
                        }
                    }
                    //mark v as visited
                    visited[row,col] = 1;
                    //push v's neighbors onto the bag
                    if(!monsterNearby(row, col)) {
                        if (row - 1 >= 0 && maze[row - 1,col] != '#') {
                            bag.Push(row - 1);
                            bag.Push(col);
                        }
                        if (row + 1 < rows && maze[row + 1,col] != '#') {
                            bag.Push(row + 1);
                            bag.Push(col);
                        }
                        if (col - 1 >= 0 && maze[row,col - 1] != '#') {
                            bag.Push(row);
                            bag.Push(col - 1);
                        }
                        if (col + 1 < columns && maze[row,col + 1] != '#') {
                            bag.Push(row);
                            bag.Push(col + 1);
                        }
                    }
                }
            }
            return totalTreasure;
        }


        // Helper function for checking if a monster is in adjacent cells
        static bool monsterNearby(int row, int col) {
            if (row - 1 >= 0 && maze[row - 1,col] == 'm') {
                return true;
            }
            if (row + 1 < maze.GetLength(0) && maze[row + 1,col] == 'm') {
                return true;
            }
            if (col - 1 >= 0 && maze[row,col - 1] == 'm') {
                return true;
            }
            if (col + 1 < maze.GetLength(1) && maze[row,col + 1] == 'm') {
                return true;
            }
            return false;
        }


        // Helper function for checking if the monster is too close to the player for
        // placement purposes
        static bool tooCloseToPlayer(int row, int column) {
            if (row == start[0] && column == start[1]) {
                return true;
            }
            if (row - 1 == start[0] && column == start[1]) {
                return true;
            }
            if (row + 1 == start[0] && column == start[1]) {
                return true;
            }
            if (row == start[0] && column - 1 == start[1]) {
                return true;
            }
            if (row == start[0] && column + 1 == start[1]) {
                return true;
            }
            return false;
        }
    }
}
