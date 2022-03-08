// Programming Assignment 3: This program will find the optimal number of islands to place a store on, such
// that for every island, there will be access to a store through connections.
//
// By: Jason Crandall u0726408

using System;
using System.Collections.Generic;

namespace assignment_3
{
    class Assignment_3
    {

        static long[] island_bitmap;
        static long solution = 0;
        static long best_count = 0;
        static long best_solution = 0;
        static long max_val = 0;
        static long ideal_count = 0;

        /// <summary>
        /// Main method that builds a bitmap of islands, recursively searches for an optimized
        /// island layout, then displays the number of islands requiring stores, as well as
        /// which islands they are
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            buildBitmap();
            if (best_count != 1) {
                ideal_count = 2;
                findOptimalIslands(0,0,1,0);
            }
            List<long> islands_with_stores = parseSolution(best_solution);
            Console.WriteLine(islands_with_stores.Count);
            Console.WriteLine(string.Join(" ", islands_with_stores));
        }


        /// <summary>
        /// buildBitmap reads in a max value and connection number from the
        /// command line, then proceeds to loop through the number of connections
        /// gradually creating a bitmap of each island and its connections. The
        /// index of the array represents the island, the position of 1's at its
        /// value represent the valid connections
        /// </summary>
        static void buildBitmap() {
            string init_input = Console.ReadLine();
            max_val = Int64.Parse(init_input.Split(' ')[0]);
            best_count = max_val + 1;
            long num_connections = Int64.Parse(init_input.Split(' ')[1]);
            solution = (long)(Math.Pow(2, max_val)) - 1;
            island_bitmap = new long[max_val + 1];

            // initialize bitmap with all 0's
            for (long i = 0; i < island_bitmap.Length; i++)
                island_bitmap[i] = 0;

            ideal_count = max_val - num_connections;
            ideal_count = (ideal_count <= 0) ? 1 : ideal_count;

            // Set each connection to 1 at proper index
            for (long i = 0; i < num_connections; i++) {
                string[] connection = Console.ReadLine().Split(' ');
                long island_1 = Int64.Parse(connection[0]);
                long island_2 = Int64.Parse(connection[1]);
                island_bitmap[island_1] |= ((long)1 << (int)island_2 - 1);
                island_bitmap[island_2] |= ((long)1 << (int)island_1 - 1);
            }

            // Set the value of the index's location to 1
            for (long i = 1; i <= max_val; i++) {
                island_bitmap[i] |= ((long)1 << (int)i - 1);
                if (island_bitmap[i] == solution) {
                    best_count = 1;
                    best_solution = i;
                    break;
                }
            }
        }


        /// <summary>
        /// Recursively finds the optimal placement and count of islands
        /// </summary>
        /// <param name="partial"></param>
        /// <param name="sum"></param>
        /// <param name="island_index"></param>
        /// <param name="island_count"></param>
        static void findOptimalIslands(long partial, long sum, long island_index, long island_count) {
            long bit_index = ((long)1 << (int)(island_index - 1));

            if (best_count == ideal_count)
                return;
            if (sum == solution) {
                best_count = island_count;
                best_solution = partial;
                return;
            }
            if (island_index > max_val)
                return;
            if (island_count >= best_count-1)
                return ;

            // Will add island to the partial solution if not already accounted for
            if((sum | island_bitmap[island_index]) != sum)
                findOptimalIslands(partial | bit_index, sum | island_bitmap[island_index], island_index + 1, island_count + 1);

            // Will skip including the island if the island is not isolated and the best count hasn't been found
            if (island_bitmap[island_index] != bit_index && island_count < best_count - 1)
                findOptimalIslands(partial, sum, island_index + 1, island_count);

            return;
        }


        /// <summary>
        /// Retrievees the positions of 1's in the in the binary version of the solution
        /// representing the number and location of the islands
        /// </summary>
        /// <param name="bitSolution"></param>
        /// <returns></returns>
        static List<long> parseSolution(long bitSolution) {
            List<long> sol = new List<long>();
            for (long i = 1; i <= max_val; i++) {
                if ((bitSolution & ((long)1 << (int)i - 1)) != 0) {
                    sol.Add(i);
                }
            }
            return sol;
        }
    }
}
