# # Solve(partial, exploration state)
# #       if(non-solution)
# #           return(false)
# #       if(solution)
# #           return(true)
# #       solve(partial, exploration state)

# # Input: max val = N, number of connections = R
# # Solution: 2^(N) - 1

# # K_i long 1: 10100011 - 163
# # K_i long 2: 00100111 - 39
# # K_i long 3: 00011110 - 30
# # K_i long 4: 01011100 - 92

# # Partial: sum of K_i

# # Non Solution: if depth > N, if depth >= best_depth

# from operator import truediv
# from tkinter import N


# Solve(0 (sum), 1):
#     bool Solve(sum, i):
#         if sum > d:
#             return false
#         if i >= n:
#             return False
#         if sum == d:
#             return true
#         if (solve(sum + k[i], i + 1)):
#             return true
#         if (solve(sum, i + 1)):
#             return true
#         return false

# step 1: for N, initialize k
# step 2: build out each k (or the islands and routes)
# K = [bitmap of islands]

# best_count = N
# best_solution = 0
# best_solution = '00001111'
# best_count = 4
# best_count = 3
# best_count = 2
# best_solution = '00001011'
# solution = 2^(N) - 1

# solve(0, 0, 1, 0)




# def solve(partial, sum, k_index, island_count):
#     if (k_index > N):
#         return
#     if (sum == solution):
#         best_solution = partial
#         best_count = island_count
#         return
#     if (island_count >= best_count-1):
#         return 
#     solve(partial | k_index,sum | k[k_index], k_index + 1, island_count + 1)
#     solve(partial, sum, k_index + 1, islandCount)
#     return

    
# solve('00000001','10100011', 2, 1)
#     solve('00000011', '10100111', 3, 2)
#         solve('00000111', '10111111', 4, 3)
#             solve('00001111', '11111111', 5, 4)
#             !solve('00000111', '10111111', 5, 3)
#         !solve('00000011', '10100111', 4, 2)
#             solve('00001011', '11111111', 4, 3)
#     !solve('00000001','10100011', 3, 1)
#         solve('00000101', '10111111', 4, 2)
#         !solve('00000001','10100011', 4, 1)
#             solve('00001001', '11111111', 4, 2)



island_bitmap = []
solution = 0
best_count = 0
best_solution = 0
max_val = 0
ideal_count = 0

# Note to self: build an initial partial and while you're building the bitmap keep track of the island count for the solution.
# that way when you call the function for the first time you already have a solution in mind

def main():
    global ideal_count

    buildBitmap()
    if (best_count != 1):
        ideal_count = 2
        findOptimalIslands(0, 0, 1, 0)
    # findOptimalIslands(partial, partial, 1, min_island_count)
    islands_with_stores = parseSolution(best_solution)
    print(len(islands_with_stores))
    print(' '.join(str(x) for x in islands_with_stores))




def buildBitmap():
    global island_bitmap
    global solution
    global max_val
    global best_count
    global ideal_count
    global best_solution

    init_input = input()
    max_val = int(init_input.split(' ')[0])
    best_count = max_val + 1
    num_connections = int(init_input.split(' ')[1])
    solution = (2**max_val) - 1
    island_bitmap = [0] * (max_val + 1)
    ideal_count = max_val - num_connections
    ideal_count = 1 if ideal_count <= 0 else ideal_count

    i = 0
    while (i < num_connections):
        connection = input()
        left = int(connection.split(' ')[0])
        right = int(connection.split(' ')[1])
        island_bitmap[left] = island_bitmap[left] | (1 << right-1)
        island_bitmap[right] = island_bitmap[right] | (1 << left-1)
        i += 1

    j = 1
    while (j <= max_val):
        island_bitmap[j] = island_bitmap[j] | (1 << j-1)
        if (island_bitmap[j] == solution):
            best_count = 1
            best_solution = j
            break
        j += 1


def findOptimalIslands(partial, sum, island_index, island_count):
    print("partial: " + str(bin(partial)) + " sum: " + str(bin(sum)) + " island_index: " + str(island_index) + " island_count: " + str(island_count))
    global best_count
    global best_solution    
    global solution
    global ideal_count
    bit_index = (1 << island_index - 1)
    if (best_count == ideal_count):
        return
    if (sum == solution):
        best_count = island_count
        best_solution = partial
        return
    if (island_index > max_val):
        return
    if (island_count >= best_count-1):
        return 
    if((sum | island_bitmap[island_index]) != sum):
        findOptimalIslands(partial | bit_index, sum | island_bitmap[island_index], island_index + 1, island_count + 1)
    if (island_bitmap[island_index] != bit_index and island_count < best_count - 1):
        findOptimalIslands(partial, sum, island_index + 1, island_count)
    return
        
def parseSolution(bitSolution):
    solution = []
    i = 1
    while (i <= max_val):
        if (bitSolution & (1 << i-1)):
            solution.append(i)
        i += 1
    return solution



if __name__ == "__main__":
    main()