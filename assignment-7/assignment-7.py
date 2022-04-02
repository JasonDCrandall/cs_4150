def main():
    rawInput = input()
    inputArray = rawInput.split(" ")
    seed = int(inputArray[0])
    vertexCount = int(inputArray[1])
    minWeight = int(inputArray[2])
    maxWeight = int(inputArray[3])
    connectivity = int(inputArray[4])
    algorithm = inputArray[5]
    startVertex = 0
    weights = generate_weights(seed, vertexCount, minWeight, maxWeight, connectivity)
    edges = []

    if algorithm == "Jarnik":
        startVertex = int(inputArray[6])
        edges = jarnikAlgorithm(weights, startVertex)

    elif algorithm == "Kruskal":
        edges = kruskalAlgorithm(weights)

    elif algorithm == "Boruvka":
        edges = boruvkaAlgorithm(weights)

    print(getTotalWeight(edges))
    print(len(edges))
    printEdges(edges)
    

# Student generated code curtesy of Piazza
def generate_weights(seed:int, vertex_count:int,
                    min_weight:int, max_weight:int,
                    connectivity:int):
    weights = [[0]*vertex_count for _ in range(vertex_count)]
    for _pass in range(connectivity):
        connected = [0]
        unused = [v for v in range(1, vertex_count)]
        while unused:
            seed = (((seed ^ (seed >> 3)) >> 12) & 0xffff) | ((seed & 0x7fff) << 16)
            weight = seed % (max_weight-min_weight+1) + min_weight

            seed = (((seed ^ (seed >> 3)) >> 12) & 0xffff) | ((seed & 0x7fff) << 16)
            from_v = connected[seed % len(connected)]

            seed = (((seed ^ (seed >> 3)) >> 12) & 0xffff) | ((seed & 0x7fff) << 16)
            to_v = unused[seed % len(unused)]

            weights[from_v][to_v] = weight
            weights[to_v][from_v] = weight

            connected.append(to_v)
            unused.remove(to_v)
    return weights

class Edge:
    def __init__(self, from_v:int, to_v:int, weight:int):
        self.from_v = from_v
        self.to_v = to_v
        self.weight = weight
    
    def __lt__(self, other):
        if self.weight < other.weight:
            return True
        elif self.weight > other.weight:
            return False
        elif min(self.from_v, self.to_v) < min(other.from_v, other.to_v):
            return True
        elif min(self.from_v, self.to_v) > min(other.from_v, other.to_v):
            return False
        elif max(self.from_v, self.to_v) < max(other.from_v, other.to_v):
            return True
        elif max(self.from_v, self.to_v) > max(other.from_v, other.to_v):
            return False

    def __gt__(self, other):
        return other.__lt__(self)

    def __eq__(self, other):
        return self.weight == other.weight and self.from_v == other.from_v and self.to_v == other.to_v

    def printEdge(self):
        print("{} {}".format(self.from_v, self.to_v))


def jarnikAlgorithm(weights:list, start_vertex:int):
    unmarked = [v for v in range(len(weights))]
    edges = []
    priorityQueue = []
    unmarked.remove(start_vertex)
    for v in range(len(weights[start_vertex])):
        if weights[start_vertex][v] != 0:
            priorityQueue.append(Edge(start_vertex, v, weights[start_vertex][v]))
    priorityQueue.sort()
    while unmarked:
        edge = priorityQueue.pop(0)
        if edge.from_v in unmarked:
            edges.append(edge)
            unmarked.remove(edge.from_v)
            for v in range(len(weights[edge.from_v])):
                if weights[edge.from_v][v] != 0:
                    priorityQueue.append(Edge(edge.from_v, v, weights[edge.from_v][v]))
            priorityQueue.sort()
        if edge.to_v in unmarked:
            edges.append(edge)
            unmarked.remove(edge.to_v)
            for v in range(len(weights[edge.to_v])):
                if weights[edge.to_v][v] != 0:
                    priorityQueue.append(Edge(edge.to_v, v, weights[edge.to_v][v]))
            priorityQueue.sort()
    return edges

def kruskalAlgorithm(weights:list):
    edges = []
    sortedEdges = []
    for i in range(len(weights)):
        for j in range(len(weights[i])):
            if weights[i][j] != 0:
                sortedEdges.append(Edge(i, j, weights[i][j]))
    sortedEdges.sort()
    vertexMap = {}
    for v in range(len(weights)):
        vertexMap[v] = {v}
    for i in range(len(sortedEdges)):
        edge = sortedEdges[i]
        if vertexMap[edge.from_v] != vertexMap[edge.to_v]:
            edges.append(edge)
            updatedComponent = vertexMap[edge.from_v].union(vertexMap[edge.to_v])
            for v in updatedComponent:
                vertexMap[v] = updatedComponent
    return edges

def boruvkaAlgorithm(weights:list):
    edges = []
    return edges


def printEdges(edges:list):
    for edge in edges:
        edge.printEdge()

def getTotalWeight(edges:list):
    totalWeight = 0
    for edge in edges:
        totalWeight += edge.weight
    return totalWeight

if __name__ == '__main__':
    main()