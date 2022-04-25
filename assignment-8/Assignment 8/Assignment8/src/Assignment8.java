/*
 * Problem Set 8: This program determines the shortest path between a start position
 * and an end position prioritizing right turns, then straight throughs, then left turns.
 * 
 * By: Jaosn Crandall u0726408
 */

import java.util.*;

public class Assignment8 {
    public static List<HashMap<Integer,Integer>> route;
    public static int[] distances;
    public static int[] predecessors;
    public static int segments;
    public static int intersections;
    public static int infinity = Integer.MAX_VALUE;
    public static void main(String[] args) throws Exception {
        // Initial setup
        Scanner scan = new Scanner(System.in);
        String lengthSpec = scan.nextLine().strip();
        String[] lengthSpecArray = lengthSpec.split(" ");

        segments = Integer.parseInt(lengthSpecArray[0]);
        route = new ArrayList<HashMap<Integer,Integer>>(segments);
        distances = new int[segments];
        predecessors = new int[segments];

        // Initialize the graph
        for (int i = 0; i < segments; i++) {
            route.add(new HashMap<Integer, Integer>());
        }
        intersections = Integer.parseInt(lengthSpecArray[1]);

        // Populate the graph prioritizing right turns
        String se = scan.nextLine().strip();
        String[] seArray = se.split(" ");
        int start = Integer.parseInt(seArray[0]);
        int end = Integer.parseInt(seArray[1]);

        for (int i = 0; i < intersections; i++) {
            String road = scan.nextLine().strip();
            String[] roadArray = road.split(" ");

            int from = Integer.parseInt(roadArray[0]);
            int to = Integer.parseInt(roadArray[2]);
            String dir = roadArray[1];
            int weight = getDirectionalWeight(dir);

            if (!route.get(from).containsKey(to)) {
                route.get(from).put(to, weight);
            } else if (weight < route.get(from).get(to)) {
                route.get(from).remove(to);
                route.get(from).put(to, weight);
            }
        }
        scan.close();

        // Implementation of Dijkstra's algorithm
        initSSP(start);
        dijkstra(start);
        printSolution(start, end);
    }

    /// <summary>
    /// Initializes the distances and predecessors arrays as outlined in the textbook
    /// </summary>
    public static void initSSP(int s) {
        distances[s] = 0;
        predecessors[s] = -1;
        for (int i = 0; i < segments; i++) {
            if (i != s) {
                distances[i] = infinity;
                predecessors[i] = -1;
            }
        }
    }

    /// <summary>
    /// Dijkstra's algorithm as outlined in the textbook, but instead of Java's priority queue,
    /// I implemented a custom priority queue with a HashSet and a getMin method so that I could 
    /// still use primitive data structures over custom objects
    /// </summary>
    public static void dijkstra(int s) {
        HashSet<Integer> pq = new HashSet<Integer>();
        pq.add(s);
        while (!pq.isEmpty()) {
            int u = findSmallestVertex(pq);
            for (int v : route.get(u).keySet()) {
                int relaxDst = distances[u] + route.get(u).get(v);
                if (relaxDst < distances[v]) {
                    distances[v] = relaxDst;
                    predecessors[v] = u;
                    pq.add(v);
                }
            }
        }
    }

    /// <summary>
    /// Finds the smallest vertex in the custom "priority queue"
    /// </summary>
    public static int findSmallestVertex(HashSet<Integer> pq) {
        int min = infinity;
        int minIndex = -1;
        for (int i = 0; i < distances.length; i++) {
            if (pq.contains(i) && distances[i] < min) {
                min = distances[i];
                minIndex = i;
            }
        }
        pq.remove(minIndex);
        return minIndex;
    }

    /// <summary>
    /// Helper method that applies a numerical weight based on the direction. 
    /// Right turns are weighted 1, straight turns are weighted equal the number of segments,
    /// and left turns are weighted equal to the number of segments squared, thereby guarenteeing stric priority
    /// </summary>
    public static int getDirectionalWeight(String dir) {
        switch(dir) {
            case "right":
                return 1;
            case "left":
                return segments*segments;
            case "straight":
                return segments;
            default:
                return 0;
        }
    }

    /// <summary>
    /// Prints the solution to the shortest path using the predecessors of the end vertex
    /// </summary>
    public static void printSolution(int start, int end) {
        ArrayList<String> solution = new ArrayList<String>();
        int v = end;
        while (v != start) {
            int sum1 = distances[v];
            v = predecessors[v];
            int sum2 = distances[v];
            solution.add(getDirectionFromNumber(sum1 - sum2));
        }
        StringBuffer sb = new StringBuffer();
        sb.append(solution.size());
        Collections.reverse(solution);
        for (int j = 0; j < solution.size(); j++) {
            sb.append(" " + solution.get(j));
        }
        System.out.println(sb.toString());
    }

    /// <summary>
    /// Helper method that converts a number to a direction string
    /// </summary>
    public static String getDirectionFromNumber(int num) {
        if (num == 1) {
            return "right";
        } else if (num == segments*segments) {
            return "left";
        } else if (num == segments) {
            return "straight";
        } else {
            return "";
        }
    }
}
