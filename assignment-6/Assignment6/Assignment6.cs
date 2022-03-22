using System;
using System.Collections.Generic;

namespace Assignment6
{
    class Assignment6
    {
        static Dictionary<string, HashSet<string>> player1Nodes;
        static Dictionary<string, HashSet<string>> player2Nodes;

        static Dictionary<string, int> player1MarkedNodes;
        static Dictionary<string, int> player2MarkedNodes;

        static Dictionary<string, string> player1NodeState;
        static Dictionary<string, string> player2NodeState;

        static string[] coopNodes;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            buildGraphs();


        }

        static void buildGraphs() {
            int player1Connections = int.Parse(Console.ReadLine());
            player1Nodes = new Dictionary<string, HashSet<string>>();
            player1MarkedNodes = new Dictionary<string, int>();
            player1NodeState = new Dictionary<string, string>();
            for (int i = 0; i < player1Connections; i++) {
                string[] input = Console.ReadLine().Split(' ');
                string node1 = input[0];
                string node2 = input[1];
                if (!player1Nodes.ContainsKey(node1)) {
                    player1Nodes.Add(node1, new HashSet<string>());
                }
                player1Nodes[node1].Add(node2);
            }

            int player2Connections = int.Parse(Console.ReadLine());
            player2Nodes = new Dictionary<string, HashSet<string>>();
            player2MarkedNodes = new Dictionary<string, int>();
            player2NodeState = new Dictionary<string, string>();
            for (int i = 0; i < player2Connections; i++) {
                string[] input = Console.ReadLine().Split(' ');
                string node1 = input[0];
                string node2 = input[1];
                if (!player2Nodes.ContainsKey(node1)) {
                    player2Nodes.Add(node1, new HashSet<string>());
                }
                player2Nodes[node1].Add(node2);
            }
            coopNodes = new string[int.Parse(Console.ReadLine())];
            for (int i = 0; i < coopNodes.Length; i++) {
                coopNodes[i] = Console.ReadLine();
            }
        }
    }
}
