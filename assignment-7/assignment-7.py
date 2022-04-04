from queue import PriorityQueue


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
        if other != None:
            return self.weight == other.weight and self.from_v == other.from_v and self.to_v == other.to_v
        return False

    def printEdge(self):
        print("{} {}".format(self.from_v, self.to_v))


def jarnikAlgorithm(weights:list, start_vertex:int):
    unmarked = [1]*len(weights)
    unmarkedCount = len(weights)
    allEdges = {}
    for v in range(len(weights)):
        allEdges[v] = []
        for j in range(len(weights[v])):
            if weights[v][j] != 0:
                allEdges[v].append(Edge(v, j, weights[v][j]))
    edges = []
    pq = PriorityQueue()
    unmarked[start_vertex] = 0
    unmarkedCount -= 1
    for i in range(len(allEdges[start_vertex])):
        pq.put(allEdges[start_vertex][i])
    while unmarkedCount > 0:
        edge = pq.get()
        if unmarked[edge.from_v] == 1:
            edges.append(edge)
            unmarked[edge.from_v] = 0
            unmarkedCount -= 1
            for i in range(len(allEdges[edge.from_v])):
                pq.put(allEdges[edge.from_v][i])
        if unmarked[edge.to_v] == 1:
            edges.append(edge)
            unmarked[edge.to_v] = 0
            unmarkedCount -= 1
            for i in range(len(allEdges[edge.to_v])):
                pq.put(allEdges[edge.to_v][i])
    return edges

def kruskalAlgorithm(weights:list):
    edges = []
    sortedEdges = []
    for i in range(len(weights)):
        for j in range(len(weights[i])):
            if weights[i][j] != 0:
                sortedEdges.append(Edge(i, j, weights[i][j]))
    sortedEdges.sort()
    print('after sort')
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

marked = []
def boruvkaAlgorithm(weights:list):
    global marked
    marked = [False]*len(weights)
    for v in range(len(weights)):
        marked[v] = [False]*len(weights)
    edges = []
    component = [0]*len(weights)
    count = countAndLabel(weights, edges, component)
    while count > 1:
        addAllSafeEdges(weights, edges, count, component)
        count = countAndLabel(weights, edges, component)
    return edges

def countAndLabel(weights:list, edges:list, component:list):
    count = 0
    unmarked = [1]*len(weights)
    for v in range(len(weights)):
        if unmarked[v] == 1:
            count += 1
            labelOne(weights, edges, v, count, unmarked, component)
    return count

def labelOne(weights:list, edges:list, v:int, count:int, unmarked:list, component:list):
    bag = []
    bag.append(v)
    while bag:
        v = bag.pop(0)
        #[0 1 2 3 5 6 7 8 9]
        if unmarked[v] == 1:
            unmarked[v] = 0
            component[v] = count-1
            for w in range(len(edges)):
                if edges[w].from_v == v:
                    bag.append(edges[w].to_v)
                elif edges[w].to_v == v:
                    bag.append(edges[w].from_v)
                # if weights[v][w] != 0:
                #     bag.append(w)


def addAllSafeEdges(weights:list, edges:list, count:int, component:list):
    safe = [None for _ in range(count)]
    allEdges = []
    for i in range(len(weights)):
        for j in range(len(weights[i])):
            if weights[i][j] != 0:
                allEdges.append(Edge(i, j, weights[i][j]))
                allEdges.append(Edge(j, i, weights[i][j]))
    for edge in allEdges:
        u = edge.from_v
        v = edge.to_v
        if component[u] != component[v]:
            if safe[component[u]] == None or edge.__lt__(safe[component[u]]):
                safe[component[u]] = edge
            if safe[component[v]] == None or edge.__lt__(safe[component[v]]):
                safe[component[v]] = edge
    for i in range(count):
        if not marked[safe[i].from_v][safe[i].to_v]:
            edges.append(safe[i])
            marked[safe[i].from_v][safe[i].to_v] = True
            marked[safe[i].to_v][safe[i].from_v] = True



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