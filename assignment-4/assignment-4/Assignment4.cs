/*
 * Assignment 4: This program uses dynamic programming to find the optimal path of completion
 * for developing a "processor". It takes in several possible routes, as well as the cost 
 * of switching between them.
 *
 * Author: Jason Crandall, u0726408
*/

using System;

namespace assignment_4
{
    class Assignment4
    {
        static void Main(string[] args)
        {
            // Get initial data sizes
            string[] size = Console.ReadLine().Split(' ');
            int rows = int.Parse(size[0]);
            int cols = int.Parse(size[1]);

            // initialize storage structures
            string[,] dataSolution = new string[rows, cols+1];
            int[,] timeSolution = new int[rows, cols+1];
            int[,] times = new int[rows, cols];
            int[] switchCost = new int[cols];

            // Populate the times array
            for(int i = 0; i < rows; i++)
            {
                string[] stringRow = Console.ReadLine().Split(' ');
                int[] row = Array.ConvertAll(stringRow, int.Parse);
                for(int j = 0; j < cols; j++)
                {
                    times[i,j] = row[j];
                }
            }

            // Populate the switchCost array
            string[] stringSwitchCost;
            if (cols > 1)
            {
                stringSwitchCost = Console.ReadLine().Split(' ');
                for (int i = 0; i < stringSwitchCost.Length; i++)
                {
                    switchCost[i] = int.Parse(stringSwitchCost[i]);
                }
            }
            switchCost[cols-1] = 0;

            // Dynamic Programming algorithm: work backwards from the last column,
            // storing the previous fastest time and check if swithing is more expesive,
            // or if staying on the current path is more optimal.
            int prevFastest = 0;
            for(int col = cols-1; col >= 0; col--)
            {
                int currentFastest = 0;
                for(int row = 0; row < rows; row++)
                {
                    if(timeSolution[row,col+1] > timeSolution[prevFastest, col+1] + switchCost[col])
                    {
                        timeSolution[row,col] = timeSolution[prevFastest,col+1] + switchCost[col] + times[row,col];
                        dataSolution[row,col] = "" + (row + 1) + " " + dataSolution[prevFastest,col + 1];
                    }
                    else
                    {
                        timeSolution[row,col] = timeSolution[row,col+1] + times[row,col];
                        dataSolution[row,col] = "" + (row + 1) + " " + dataSolution[row,col + 1];
                    }
                    if(timeSolution[row,col] < timeSolution[currentFastest,col])
                    {
                        currentFastest = row;
                    }
                }
                prevFastest = currentFastest;
            }

            // Find best solution in the first column
            int lowestIndex = 0;
            for (int i = 0; i < rows; i++)
            {
                if (timeSolution[i,0] < timeSolution[lowestIndex,0])
                {
                    lowestIndex = i;
                }
            }
            Console.WriteLine(timeSolution[lowestIndex,0]);
            Console.WriteLine(dataSolution[lowestIndex,0]);
        }
    }
}
