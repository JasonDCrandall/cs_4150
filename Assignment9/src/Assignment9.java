/*
 * Problem Set 9: This program returns max cut min flow information for a given graph.
 * 
 * By: Jason Crandall (u0726408)
*/


import java.util.*;

public class Assignment9 {
    // Vertex is essentially an int with paralell structures;
    public static int[][] capacity;
    public static int[][] flow;
    public static int[][] residual;
    public static int[] distances;
    public static int[] predecessors;
    public static int maxFlow;
    public static int infinity = Integer.MAX_VALUE;
    public static void main(String[] args) throws Exception {
        Scanner scan = new Scanner(System.in);
        String initParams = scan.nextLine().strip();
        String[] initParamsArr = initParams.split(" ");
        int n = Integer.parseInt(initParamsArr[0]);
        int s = Integer.parseInt(initParamsArr[1]);
        int t = Integer.parseInt(initParamsArr[2]);

        capacity = new int[n][n];
        flow = new int[n][n];
        residual = new int[n][n];

        for (int row = 0; row < n; row++) {
            String line = scan.nextLine().strip();
            String[] lineArr = line.split(" ");
            for (int col = 0; col < n; col++) {
                int val = Integer.parseInt(lineArr[col]);
                capacity[row][col] = val;
                residual[row][col] = val;
                flow[row][col] = 0;
            }
        }
        scan.close();


        findAugmentingPathFlows(n, s, t);
        System.out.println(maxFlow);
        getSaturatedTotal();
        ArrayList<Integer> vInS = getVerticesInS(n, s);
        for (int i = 0; i < vInS.size(); i++) {
            System.out.print(vInS.get(i) + " ");
        }
        System.out.println("");
        getFlowFromStoT(n, vInS);
        getCapacityFromTtoS(n, vInS);
    }

    // This method finds the total capacity from the set of vertices in S to the 
    // set of vertices in T.
    private static void getCapacityFromTtoS(int n, ArrayList<Integer> vInS) {
        int totalCapacity = 0;
        for (int row = 0; row < n; row++) {
            for (int col = 0; col < n; col++) {
                if (!vInS.contains(row) && vInS.contains(col)) {
                    totalCapacity += capacity[row][col];
                }
            }
        }
        System.out.println(totalCapacity);
    }

    // This method finds the flow from the set of vertices in S to the set of
    // vertices in T.
    private static void getFlowFromStoT(int n, ArrayList<Integer> vInS) {
        int totalFlow = 0;
        for (int row = 0; row < n; row++) {
            for (int col = 0; col < n; col++) {
                if (vInS.contains(row) && !vInS.contains(col)) {
                    totalFlow += flow[row][col];
                }
            }
        }
        System.out.println(totalFlow);
    }

    // This method uses depth first search to find all of the vertices that s 
    // can reach.
    private static ArrayList<Integer> getVerticesInS(int n, int s) {
        HashSet<Integer> verticesInS = new HashSet<>();
        Stack<Integer> bag = new Stack<>();
        int[] visited = new int[n];
        bag.push(s);
        while (!bag.isEmpty()) {
            int v = bag.pop();
            if (visited[v] == 0) {
                visited[v] = 1;
                verticesInS.add(v);
                for (int i = 0; i < n; i++) {
                    if (residual[v][i] > 0) {
                        bag.push(i);
                    }
                }
            }
        }
        ArrayList<Integer> verticesInSList = new ArrayList<>(verticesInS);
        Collections.sort(verticesInSList);
        return verticesInSList;
    }

    // This method uses the pre-existing dfs algorithm to see
    // if t is reachable from s.
    private static boolean canReachT(int n, int s, int t) {
        ArrayList<Integer> reachable = getVerticesInS(n, s);
        return reachable.contains(t);
    }

    // This method uses the final flow graph and the original capacity
    // graph to determine the number of saturated edges.
    private static void getSaturatedTotal() {
        int total = 0;
        for (int row = 0; row < capacity.length; row++) {
            for (int col = 0; col < capacity[row].length; col++) {
                if (capacity[row][col] != 0 && (flow[row][col] - flow[col][row]) == capacity[row][col]) {
                    total++;
                }
            }
        }

        System.out.println(total);
    }

    // This method uses looks for all of the path weights from s to t
    // and prints the findings
    private static void findAugmentingPathFlows(int n, int s, int t) {
        ArrayList<Integer> pathFlows = new ArrayList<>();
        while (canReachT(n, s, t)) {
            int pathFlow = findPath(n, s, t);
            if (pathFlow != 0) {
                pathFlows.add(pathFlow);
            }
        }
        for (int p : pathFlows) {
            System.out.print("" + p + " ");
        }
        System.out.println("");
    }

    // This method runs the bfs algorithm once to find the flow cap of a given 
    // path, and then proceeds to run the algorithm again with the cap in place
    private static int findPath(int n, int s, int t) {
        int maxPathFlow = getPathFLow(n, s, t, false, infinity);
        if (maxPathFlow == 0) {
            return 0;
        }
        return getPathFLow(n, s, t, true, maxPathFlow);
    }

    // This method uses a combination of Dijkstra's and Dynamic programming
    // to find the BEST augmenting path of a graph. It does this with a priority 
    // queue of vertices with a built in comparator and dijkstras for traversal,
    // and dynamic programming for determining the optimal predeccessor of a specific vertex.
    public static int getPathFLow(int n, int s, int t, boolean canModify, int max) {
        ArrayList<Vertex> vertices = initSearch(n, s);
        PriorityQueue<Vertex> pq = new PriorityQueue<Vertex>();
        pq.add(vertices.get(s));
        while(!pq.isEmpty()) {
            Vertex u = pq.poll();
            for (int v = 0; v < n; v++) {
                int relaxDst = u.distance + 1;
                if (residual[u.id][v] > 0 && relaxDst < vertices.get(v).distance) {
                    vertices.get(v).distance = u.distance + 1;
                    setPredecessor(vertices, v);
                    vertices.get(v).flow = Math.min(max, vertices.get(v).flow);
                    pq.add(vertices.get(v));
                }
            }
        }

        // Find the path flow and update the adjadcent flow and residual graphs accordingly
        int pathFlow = infinity;
        int curr = t;
        ArrayList<Integer> v = new ArrayList<>();
        while (curr != s) {
            v.add(curr);
            pathFlow = Math.min(pathFlow, residual[vertices.get(curr).predecessor][curr]);
            curr = vertices.get(curr).predecessor;
        }
        curr = t;
        while (curr != s) {
            if (canModify) {
                residual[vertices.get(curr).predecessor][curr] -= pathFlow;
                residual[curr][vertices.get(curr).predecessor] += pathFlow;
                flow[vertices.get(curr).predecessor][curr] += pathFlow;
            }
            curr = vertices.get(curr).predecessor;
        }
        // Update the max flow if the path isn't the first run-through
        if (canModify) {
            maxFlow += pathFlow;
        }
        v.add(s);
        Collections.reverse(v);
        return pathFlow;
    }


    // setPredecessor is a helper method that takes a vertex and looks at all of the
    // possible predecessors. Using the built-in comparator, it finds the best predecessor
    // and sets its flow as the possible minimum flow.
    private static void setPredecessor(ArrayList<Vertex> vertices, int v) {
        Vertex bestPredecessor = null;
        for (int row = 0; row < residual.length; row++) {
            if (residual[row][v] > 0) {
                Vertex copy = new Vertex(vertices.get(row));
                copy.flow = Math.min(copy.flow, residual[row][v]);
                if (bestPredecessor == null) {
                    bestPredecessor = copy;
                } else {
                    if (bestPredecessor.compareTo(copy) > 0) {
                        bestPredecessor = copy;
                    }
                }
            }
        }
        vertices.get(v).predecessor = bestPredecessor.id;
        vertices.get(v).flow = bestPredecessor.flow;
    }

    // This method initializes an array of vertices with an infinite distance
    // no flow, and no predecessor.
    private static ArrayList<Vertex> initSearch(int n, int s) {
        ArrayList<Vertex> vertices = new ArrayList<>();
        for (int i = 0; i < n; i++) {
            vertices.add(new Vertex(infinity, 0, i, -1));        
        }
        vertices.get(s).distance = 0;
        vertices.get(s).flow = infinity;
        return vertices;
    }

    // Vertex class that holds the distance, flow, and predecessor of a vertex.
    // Implements Comparable to be used in the priority queue based on the assignment
    // tie breaker rules
    public static class Vertex implements Comparable<Object> {
        public int distance;
        public int flow;
        public int id;
        public int predecessor;

        public Vertex(int distance, int flow, int id, int predecessor) {
            this.distance = distance;
            this.flow = flow;
            this.id = id;
            this.predecessor = predecessor;
        }

        public Vertex(Vertex v) {
            this.distance = v.distance;
            this.flow = v.flow;
            this.id = v.id;
            this.predecessor = v.predecessor;
        }

        public int compareTo(Object o) {
            Vertex v = (Vertex) o;
            if (this.distance < v.distance) {
                return -1;
            } else if (this.distance > v.distance) {
                return 1;
            } else if (this.flow > v.flow) {
                return -1;
            } else if (this.flow < v.flow) {
                return 1;
            } else if (this.predecessor < v.predecessor) {
                return -1;
            } else if (this.predecessor > v.predecessor) {
                return 1;
            } else {
                return 0;
            }
        }

        public boolean equals(Object o) {
            return this.compareTo(o) == 0;
        }

        public void printVertex() {
            System.out.println("distance: " + this.distance + " flow: " + this.flow + " id: " + this.id + " predecessor: " + this.predecessor);
        }
    }
}
