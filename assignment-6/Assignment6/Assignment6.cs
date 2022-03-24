/*
 * Problem Set 6: This program provides a valid ordering of quests for two players
 *                using topological sort
 * 
 * By: Jason Crandall u0726408
 */

using System;
using System.Collections.Generic;

namespace Assignment6
{
    class Assignment6
    {
        static Dictionary<string, HashSet<string>> nodesFrom;
        static Dictionary<string, HashSet<string>> nodesInto;
        // Node States: 0 = new, 1 = finished, -1 = in progress
        static Dictionary<string, int> nodeStatus;

        static string[] sortedNodes;
        static bool solvable = true;

        static void Main(string[] args)
        {
            buildGraphComponents();
            updateCoopDependencies();
            topologicalSort();
            if (solvable)
                printSolution();
            else
                Console.WriteLine("Unsolvable");
        }

        /// <summary>
        /// This method initializes the graph structure with two separate components, one for 
        /// player one, the other for player two
        /// </summary>
        static void buildGraphComponents()
        {
            nodesFrom = new Dictionary<string, HashSet<string>>();
            nodesInto = new Dictionary<string, HashSet<string>>();
            nodeStatus = new Dictionary<string, int>();
            int playerConnections;
            int player = 1;
            while (player <=2)
            {
                playerConnections = int.Parse(Console.ReadLine());
                for (int i = 0; i < playerConnections; i++)
                {
                    string[] input = Console.ReadLine().Split(' ');
                    string node1 = input[0] + "-" + player;
                    string node2 = input[1] + "-" + player;

                    if (!nodesFrom.ContainsKey(node1))
                    {
                        nodesFrom.Add(node1, new HashSet<string>());
                        nodeStatus.Add(node1, 0);
                    }
                    if (!nodesInto.ContainsKey(node1))
                    {
                        nodesInto.Add(node1, new HashSet<string>());
                    }
                    if (!nodesFrom.ContainsKey(node2))
                    {
                        nodesFrom.Add(node2, new HashSet<string>());
                        nodeStatus.Add(node2, 0);
                    }
                    if (!nodesInto.ContainsKey(node2))
                    {
                        nodesInto.Add(node2, new HashSet<string>());
                    }

                    nodesFrom[node1].Add(node2);
                    nodesInto[node2].Add(node1);
                }

                player++;
            }
        }

        /// <summary>
        /// This method gathers the list of coop nodes, then modifies the 
        /// two exisiting graph components to ensure that they point to the 
        /// same node
        /// </summary>
        static void updateCoopDependencies()
        {
            int coopDependencies = int.Parse(Console.ReadLine());
            for (int i = 0; i < coopDependencies; i++)
            {
                string coopNode = Console.ReadLine();
                HashSet<string> intoCoop = new HashSet<string>();
                HashSet<string> fromCoop = new HashSet<string>();

                int player = 1;
                while (player <= 2)
                {
                    string playerNode = coopNode + "-" + player;
                    if (nodesInto.ContainsKey(playerNode))
                    {
                        foreach (string node in nodesInto[playerNode])
                        {
                            intoCoop.Add(node);
                        }
                        nodesInto.Remove(playerNode);
                    }
                    if (nodesFrom.ContainsKey(playerNode))
                    {
                        foreach (string node in nodesFrom[playerNode])
                        {
                            fromCoop.Add(node);
                        }
                        nodesFrom.Remove(playerNode);
                    }
                    foreach (string node in intoCoop)
                    {
                        nodesFrom[node].Remove(playerNode);
                        nodesFrom[node].Add(coopNode);
                    }
                    foreach (string node in fromCoop)
                    {
                        nodesInto[node].Remove(playerNode);
                        nodesInto[node].Add(coopNode);
                    }
                    nodeStatus.Remove(playerNode);
                    player++;
                }
                nodesInto.Add(coopNode, intoCoop);
                nodesFrom.Add(coopNode, fromCoop);
                nodeStatus.Add(coopNode, 0);
            }
        }

        /// <summary>
        /// This is the parent method that initializes the recursive topological sort
        /// </summary>
        static void topologicalSort()
        {
            sortedNodes = new string[nodesFrom.Count];
            int clock = nodesFrom.Count;
            foreach (string node in nodesFrom.Keys)
            {
                if (nodeStatus[node] == 0)
                {
                    clock = topSortDFS(node, clock);
                }
            }
        }

        /// <summary>
        /// Recursive topological sort method that positions the node in the 
        /// sorted array and returns the new clock status
        /// </summary>
        /// <param name="node"></param>
        /// <param name="clock"></param>
        /// <returns></returns>
        static int topSortDFS(string node, int clock)
        {
            nodeStatus[node] = -1;
            foreach (string edge in nodesFrom[node])
            {
                if (nodeStatus[edge] == 0)
                {
                    clock = topSortDFS(edge, clock);
                }
                else if (nodeStatus[edge] == -1)
                {
                    solvable = false;
                }
            }
            nodeStatus[node] = 1;
            sortedNodes[clock - 1] = node;
            clock--;
            return clock;
        }

        /// <summary>
        /// Method for desplaying the solution
        /// </summary>
        static void printSolution()
        {
            foreach (string node in sortedNodes)
            {
                Console.WriteLine(node);
            }
        }
    }
}
