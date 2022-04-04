import java.util.*;

@SuppressWarnings("unchecked")
public class Assignment7 {
    public static void main(String[] args) throws Exception {
        Scanner scan = new Scanner(System.in);
        String input = scan.nextLine().strip();
        String[] inputArray = input.split(" ");
        scan.close();
        int seed = Integer.parseInt(inputArray[0]);
        int vertexCount = Integer.parseInt(inputArray[1]);
        int minWeight = Integer.parseInt(inputArray[2]);
        int maxWeight = Integer.parseInt(inputArray[3]);
        int connectivity = Integer.parseInt(inputArray[4]);
        String algorithm = inputArray[5];
        int startVertex = 0;
        int[][] weights = generateWeights(seed, vertexCount, minWeight, maxWeight, connectivity);
        ArrayList<Edge> edges = new ArrayList<Edge>();

        if (algorithm.equals("Jarnik")) {
            startVertex = Integer.parseInt(inputArray[6]);
            edges = jarnik(weights, startVertex);
        } else if (algorithm.equals("Kruskal")) {
            edges = kruskal(weights);
        }
        else if (algorithm.equals("Boruvka")) {
            edges = boruvka(weights);
        }
        System.out.println(totalWeight(edges));
        System.out.println(edges.size());
        for (Edge edge : edges) {
            System.out.println(edge.from + " " + edge.to);
        }
    }

    public static class Edge implements Comparable {
        public int from;
        public int to;
        public int weight;

        public Edge(int from, int to, int weight) {
            this.from = from;
            this.to = to;
            this.weight = weight;
        }

        public int compareTo(Object o) {
            Edge other = (Edge) o;
            if (this.weight < other.weight) {
                return -1;
            } else if (this.weight > other.weight) {
                return 1;
            } else if (Math.min(this.from, this.to) < Math.min(other.from, other.to)) {
                return -1;
            } else if (Math.min(this.from, this.to) > Math.min(other.from, other.to)) {
                return 1;
            } else if (Math.max(this.from, this.to) < Math.max(other.from, other.to)) {
                return -1;
            } else if (Math.max(this.from, this.to) > Math.max(other.from, other.to)) {
                return 1;
            } else {
                return 0;
            }
        }

        public boolean equals(Object o) {
            return compareTo(o) == 0;
        }
    }

    public static int[][] generateWeights (int seed, int vertexCount, int minWeight, int maxWeight, int connectivity)  // Non-zero seed, cap vertices at 100, weights at 10000 
	{
		int[][] weights = new int[vertexCount][vertexCount];
		
		for (int pass = 0; pass < connectivity; pass++)
		{
			List<Integer> connected = new ArrayList<Integer>();
			List<Integer> unused    = new ArrayList<Integer>();
			
			connected.add(0);
			for (int vertex = 1; vertex < vertexCount; vertex++)
				unused.add(vertex);
			
			while (unused.size() > 0)
			{
				seed = (((seed ^ (seed >> 3)) >> 12) & 0xffff) | ((seed & 0x7fff) << 16);
				int weight = seed % (maxWeight-minWeight+1) + minWeight;
				
				seed = (((seed ^ (seed >> 3)) >> 12) & 0xffff) | ((seed & 0x7fff) << 16);
				Integer fromVertex = connected.get(seed % connected.size());
				
				seed = (((seed ^ (seed >> 3)) >> 12) & 0xffff) | ((seed & 0x7fff) << 16);
				Integer toVertex   = unused.get(seed % unused.size());
				
				weights[fromVertex][toVertex] = weight;
				weights[toVertex][fromVertex] = weight;  // Undirected
				
				connected.add(toVertex);
				unused.remove(toVertex);  // Note -- overloaded, remove element Integer, not position int
			}			
		}
		
		return weights;
	}

    public static ArrayList<Edge> jarnik(int[][] weights, int startVertex) {
        ArrayList<Edge> edges = new ArrayList<Edge>();
        int[] unmarked = new int[weights.length];
        for(int i = 0; i < weights.length; i++) {
            unmarked[i] = 1;
        }
        int unmarkedCount = weights.length;
        HashMap<Integer, ArrayList<Edge>> allEdges = new HashMap<Integer, ArrayList<Edge>>();
        for (int i = 0; i < weights.length; i++) {
            allEdges.put(i, new ArrayList<Edge>());
            for (int j = 0; j < weights.length; j++) {
                if (weights[i][j] != 0) {
                    allEdges.get(i).add(new Edge(i, j, weights[i][j]));
                }
            }
        }

        PriorityQueue<Edge> pq = new PriorityQueue<Edge>();
        unmarked[startVertex] = 0;
        unmarkedCount--;
        for (Edge edge : allEdges.get(startVertex)) {
            pq.add(edge);
        }
        while (unmarkedCount > 0) {
            Edge edge = pq.poll();
            if (unmarked[edge.from] ==1) {
                unmarked[edge.from] = 0;
                unmarkedCount--;
                edges.add(edge);
                for (Edge e : allEdges.get(edge.from)) {
                    pq.add(e);
                }
            }
            if (unmarked[edge.to] == 1) {
                unmarked[edge.to] = 0;
                unmarkedCount--;
                edges.add(edge);
                for (Edge e : allEdges.get(edge.to)) {
                    pq.add(e);
                }
            }
        }
        return edges;
    }

    public static ArrayList<Edge> kruskal(int[][] weights) {
        ArrayList<Edge> edges = new ArrayList<Edge>();
        ArrayList<Edge> sortedEdges = new ArrayList<Edge>();
        for (int i = 0; i < weights.length; i++) {
            for (int j = 0; j < weights.length; j++) {
                if (weights[i][j] != 0) {
                    sortedEdges.add(new Edge(i, j, weights[i][j]));
                }
            }
        }
        Collections.sort(sortedEdges);
        HashMap<Integer, Set<Integer>> vertexMap = new HashMap<Integer, Set<Integer>>();
        for (int i = 0; i < weights.length; i++) {
            vertexMap.put(i, new HashSet<Integer>());
            vertexMap.get(i).add(i);
        }
        for (Edge edge : sortedEdges) {
            if (!vertexMap.get(edge.from).equals(vertexMap.get(edge.to))) {
                edges.add(edge);
                Set<Integer> union = new HashSet<Integer>();
                union.addAll(vertexMap.get(edge.from));
                union.addAll(vertexMap.get(edge.to));
                for (int i : union) {
                    vertexMap.put(i, union);
                }
            }
        }
        return edges;
    }

    public static int[][] marked;
    public static ArrayList<Edge> boruvka(int[][] weights) {
        ArrayList<Edge> edges = new ArrayList<Edge>();
        marked = new int[weights.length][weights.length];
        int[] component = new int[weights.length];
        int count = countAndLabel(weights, edges, component);
        while (count > 1) {
            addAllSafeEdges(weights, edges, count, component);
            count = countAndLabel(weights, edges, component);
        }
        return edges;
    }

    public static int countAndLabel(int[][] weights, ArrayList<Edge> edges, int[] component) {
        int count = 0;
        int[] unmarked = new int[weights.length];
        for (int i = 0; i < weights.length; i++) {
            unmarked[i] = 1;
        }
        for (int i = 0; i < weights.length; i++) {
            if (unmarked[i] == 1) {
                count++;
                labelOne(weights, edges, i, count, unmarked, component);
            }
        }
        return count;
    }

    public static void labelOne(int[][] weights, ArrayList<Edge> edges, int vertex, int count, int[] unmarked, int[] component) {
        Stack<Integer> bag = new Stack<Integer>();
        bag.push(vertex);
        while (bag.size() > 0) {
            int v = bag.pop();
            if (unmarked[v] == 1) {
                unmarked[v] = 0;
                component[v] = count - 1;
                for (Edge edge : edges) {
                    if (edge.from == v) {
                        bag.push(edge.to);
                    } else if (edge.to == v) {
                        bag.push(edge.from);
                    }
                }
            }
        }
    }

    public static void addAllSafeEdges(int[][] weights, ArrayList<Edge> edges, int count, int[] component) {
        Edge[] safe = new Edge[count];
        ArrayList<Edge> allEdges = new ArrayList<Edge>();
        for (int i = 0; i < weights.length; i++) {
            for (int j = 0; j < weights.length; j++) {
                if (weights[i][j] != 0) {
                    allEdges.add(new Edge(i, j, weights[i][j]));
                    allEdges.add(new Edge(j, i, weights[i][j]));
                }
            }
        }
        for (Edge edge : allEdges) {
            int u = edge.from;
            int v = edge.to;
            if (component[u] != component[v]) {
                if (safe[component[u]] == null || edge.compareTo(safe[component[u]]) < 0) {
                    safe[component[u]] = edge;
                }
                if (safe[component[v]] == null || edge.compareTo(safe[component[v]]) < 0) {
                    safe[component[v]] = edge;
                }
            }
        }
        for (int i = 0; i < count; i++) {
            if (marked[safe[i].from][safe[i].to] == 0) {
                marked[safe[i].from][safe[i].to] = 1;
                marked[safe[i].to][safe[i].from] = 1;
                edges.add(safe[i]);
            }
        }
    }

    public static int totalWeight(ArrayList<Edge> edges) {
        int totalWeight = 0;
        for (Edge edge : edges) {
            totalWeight += edge.weight;
        }
        return totalWeight;
    }
}
