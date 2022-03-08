# Problem Set 2: The following program is designed to find the global minimun of a 
# cyclical graph. It does this by starting requestin the first and last data members,
# and their mid point. It finds the lowest point and then proceeds to cut the set in half,
# repeating this process until the lowest is found.
#
# By: Jason Crandall u0727408

from math import floor

saved_data = {}
size = 0

def main():
    # Initial Setup
    global size
    size = int(input())
    start_index = 0
    if size == 0:
        return

    # Save the first and last data members
    # Determine the lowest value
    saveData(start_index)
    first = saved_data[0]
    last_index = 0 if (size-1) <= 0 else (size-1)
    saveData(last_index)
    last = saved_data[last_index]
    if first <= last:
            boundry = last_index
            lowest = start_index
    else:
        boundry = start_index
        lowest = last_index

    # Find the midpoint
    mid = floor((boundry + lowest) / 2)
    mid_index = 0 if mid <= 0 else mid%size
    saveData(mid_index)

    # Find and print the global minimum
    global_min = getGlobalMin(boundry, lowest, mid_index)
    print("minimum " + str(global_min))


def getGlobalMin(boundry, lowest, mid):
    global size
    b_val = saved_data[boundry]
    l_val = saved_data[lowest]
    m_val = saved_data[mid]

    # If the midpoint is the lowest, or boundry, return it
    if (m_val == l_val) or (m_val == b_val):
        min = lowest if l_val <= b_val else boundry
        return min

    # If the midpoint is higher than the lowest, set it as the boundry
    elif (m_val > l_val):
        boundry = mid

    # If the midpoint is lower than the lowest, find the direction of the slope
    # to determine the new boundry
    elif (m_val < l_val):
        towards_lowest = False

        # Determine if the downward slope is moving towards the existing lowest
        next_index = 0 if (mid+1) <= 0 else (mid+1)%size
        saveData(next_index)
        prev_index = 0 if (mid-1) <= 0 else (mid-1)%size
        saveData(prev_index)
        next_val = saved_data[next_index]
        prev_val = saved_data[prev_index]
        if (lowest < mid and prev_val < m_val) or (lowest > mid and next_val < m_val):
            towards_lowest = True


        # If the slope is moving towards the lowest, set it as the new boundry
        if towards_lowest:
            boundry = lowest
            lowest = mid
        # If the slope is moving away from the lowest, keep the previous boundry
        else:
            lowest = mid

    mid = floor((boundry + lowest) / 2)
    mid_index = 0 if mid <= 0 else mid%size
    saveData(mid_index)
    return getGlobalMin(boundry, lowest, mid_index)

# Method to query the array and save the value in the global dictionary
def saveData(index):
    try:
        saved_data[index]
    except KeyError:
        print("query "+str(index))
        saved_data[index] = int(input())



if "__main__" == __name__:
    main()