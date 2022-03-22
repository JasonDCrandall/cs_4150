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

        static void printSolution()
        {
            foreach (string node in sortedNodes)
            {
                Console.WriteLine(node);
            }
        }

        //static void buildGraphs() {
        //    int player1Connections = int.Parse(Console.ReadLine());
        //    player1Nodes = new Dictionary<string, HashSet<string>>();
        //    for (int i = 0; i < player1Connections; i++) {
        //        string[] input = Console.ReadLine().Split(' ');
        //        string node1 = input[0];
        //        string node2 = input[1];
        //        if (!player1Nodes.ContainsKey(node1)) {
        //            player1Nodes.Add(node1, new HashSet<string>());
        //        }
        //        if (!player1Nodes.ContainsKey(node2))
        //        {
        //            player1Nodes.Add(node2, new HashSet<string>());
        //        }
        //        player1Nodes[node1].Add(node2);
        //    }

        //    int player2Connections = int.Parse(Console.ReadLine());
        //    player2Nodes = new Dictionary<string, HashSet<string>>();
        //    for (int i = 0; i < player2Connections; i++) {
        //        string[] input = Console.ReadLine().Split(' ');
        //        string node1 = input[0];
        //        string node2 = input[1];
        //        if (!player2Nodes.ContainsKey(node1)) {
        //            player2Nodes.Add(node1, new HashSet<string>());
        //        }
        //        if (!player2Nodes.ContainsKey(node2))
        //        {
        //            player2Nodes.Add(node2, new HashSet<string>());
        //        }
        //        player2Nodes[node1].Add(node2);
        //    }
        //    coopNodes = new string[int.Parse(Console.ReadLine())];
        //    for (int i = 0; i < coopNodes.Length; i++) {
        //        coopNodes[i] = Console.ReadLine();
        //    }
        //}

        //static string[] topologicalSort(Dictionary<string, HashSet<string>> nodes, Dictionary<string, int> marked, Dictionary<string, int> status)
        //{
        //    string[] sorted = new string[nodes.Count];
        //    int clock = nodes.Count;
        //    foreach(string key in nodes.Keys)
        //    {
        //        if (status[key] == 0)
        //        {
        //            clock = topSortDFS(key, clock, ref nodes, ref marked, ref status, ref sorted);
        //        }
        //    }

        //    return sorted;
        //}

        //static int topSortDFS(string node, int clock, ref Dictionary<string, HashSet<string>> nodes, ref Dictionary<string, int> marked, ref Dictionary<string, int> status, ref string[] sorted)
        //{
        //    status[node] = -1;
        //    foreach (string edge in nodes[node])
        //    {
        //        if (status[edge] == 0)
        //        {
        //            clock = topSortDFS(edge, clock, ref nodes, ref marked, ref status, ref sorted);
        //        }
        //        else if (status[edge] == -1)
        //        {
        //            Console.WriteLine("failed gracefully");
        //        }
        //    }
        //    status[node] = 1;
        //    sorted[clock-1] = node;
        //    clock--;
        //    return clock;
        //}
    }
}
