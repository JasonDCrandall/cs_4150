using System;
using System.Collections.Generic;

namespace Assignment6
{
    class Assignment6
    {
        // So that you remember when you start working on this tomorrow: 
        // Make 1 graph with the coop nodes as the only shared ones between them
        // That way you don't really have to do any extra work
        static Dictionary<string, HashSet<string>> player1Nodes;
        static Dictionary<string, HashSet<string>> player2Nodes;

        static Dictionary<string, int> player1MarkedNodes;
        static Dictionary<string, int> player2MarkedNodes;

        // Node States: 0 = new, 1 = finished, -1 = in progress
        static Dictionary<string, int> player1NodeState;
        static Dictionary<string, int> player2NodeState;

        static string[] coopNodes;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            buildGraphs();
            string[] sortedPlayer1 = topologicalSort(player1Nodes, player1MarkedNodes, player1NodeState);
            string[] sortedPlayer2 = topologicalSort(player2Nodes, player2MarkedNodes, player2NodeState);
            findSolution(sortedPlayer1, sortedPlayer2);
        }

        static void buildGraphs() {
            int player1Connections = int.Parse(Console.ReadLine());
            player1Nodes = new Dictionary<string, HashSet<string>>();
            player1MarkedNodes = new Dictionary<string, int>();
            player1NodeState = new Dictionary<string, int>();
            for (int i = 0; i < player1Connections; i++) {
                string[] input = Console.ReadLine().Split(' ');
                string node1 = input[0];
                string node2 = input[1];
                if (!player1Nodes.ContainsKey(node1)) {
                    player1Nodes.Add(node1, new HashSet<string>());
                    player1MarkedNodes.Add(node1, 0);
                    player1NodeState.Add(node1, 0);
                }
                if (!player1Nodes.ContainsKey(node2))
                {
                    player1Nodes.Add(node2, new HashSet<string>());
                    player1MarkedNodes.Add(node2, 0);
                    player1NodeState.Add(node2, 0);
                }
                player1Nodes[node1].Add(node2);
            }

            int player2Connections = int.Parse(Console.ReadLine());
            player2Nodes = new Dictionary<string, HashSet<string>>();
            player2MarkedNodes = new Dictionary<string, int>();
            player2NodeState = new Dictionary<string, int>();
            for (int i = 0; i < player2Connections; i++) {
                string[] input = Console.ReadLine().Split(' ');
                string node1 = input[0];
                string node2 = input[1];
                if (!player2Nodes.ContainsKey(node1)) {
                    player2Nodes.Add(node1, new HashSet<string>());
                    player2MarkedNodes.Add(node1, 0);
                    player2NodeState.Add(node1, 0);
                }
                if (!player2Nodes.ContainsKey(node2))
                {
                    player2Nodes.Add(node2, new HashSet<string>());
                    player2MarkedNodes.Add(node2, 0);
                    player2NodeState.Add(node2, 0);
                }
                player2Nodes[node1].Add(node2);
            }
            coopNodes = new string[int.Parse(Console.ReadLine())];
            for (int i = 0; i < coopNodes.Length; i++) {
                coopNodes[i] = Console.ReadLine();
            }
        }

        static string[] topologicalSort(Dictionary<string, HashSet<string>> nodes, Dictionary<string, int> marked, Dictionary<string, int> status)
        {
            string[] sorted = new string[nodes.Count];
            int clock = nodes.Count;
            foreach(string key in nodes.Keys)
            {
                if (status[key] == 0)
                {
                    clock = topSortDFS(key, clock, ref nodes, ref marked, ref status, ref sorted);
                }
            }

            return sorted;
        }

        static int topSortDFS(string node, int clock, ref Dictionary<string, HashSet<string>> nodes, ref Dictionary<string, int> marked, ref Dictionary<string, int> status, ref string[] sorted)
        {
            status[node] = -1;
            foreach (string edge in nodes[node])
            {
                if (status[edge] == 0)
                {
                    clock = topSortDFS(edge, clock, ref nodes, ref marked, ref status, ref sorted);
                }
                else if (status[edge] == -1)
                {
                    Console.WriteLine("failed gracefully");
                }
            }
            status[node] = 1;
            sorted[clock-1] = node;
            clock--;
            return clock;
        }

        static void findSolution(string[] player1, string[] player2)
        {

        }
    }
}
