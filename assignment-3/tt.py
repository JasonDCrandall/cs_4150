import random

board = [[0, 1, 2], [3, 4, 5], [6, 7, 8]]


def initialize_board(board):
    number = 1
    for i in range(3):
        for j in range(3):
            board[i][j] = number
            number += 1
    #Set starting computer move
    board[1][1] = "X"


def display_board(board):
    # The function accepts one parameter containing the board's current status
    # and prints it out to the console.
    print('+-------+-------+-------+')
    print('|       |       |       |')
    print('|  ', board[0][0], '  |  ', board[0][1], '  |  ', board[0][2], '  |')
    print('|       |       |       |')
    print('+-------+-------+-------+')
    print('|       |       |       |')
    print('|  ', board[1][0], '  |  ', board[1][1], '  |  ', board[1][2], '  |')
    print('|       |       |       |')
    print('+-------+-------+-------+')
    print('|       |       |       |')
    print('|  ', board[2][0], '  |  ', board[2][1], '  |  ', board[2][2], '  |')
    print('|       |       |       |')
    print('+-------+-------+-------+')


def enter_move(board):
    # The function accepts the board's current status, asks the user about their move, 
    # checks the input, and updates the board according to the user's decision.
    display_board(board)
    #do I want to do a check against the dynamically created list of remaining options? 
    move = input("Please enter move: ")
    
    for i in range(3):
        for j in range(3):
            if int(move) == board[i][j]:
                board[i][j] = "O"
                return True
    return False


def make_list_of_free_fields(board):
    # The function browses the board and builds a list of all the free squares; 
    # the list consists of tuples, while each tuple is a pair of row and column numbers.
    free_fields = []
    for i in range(3):
        for j in range(3):
            if board[i][j] != "X" and board[i][j] != "O":
                free_fields.append(board[i][j])
    
    return free_fields

def victory_for(board, sign):
    # The function analyzes the board's status in order to check if 
    # the player using 'O's or 'X's has won the game
    for i in range(3):
        if sign == board[i][0] and sign == board[i][1] and sign == board[i][2]:
            return True
    for j in range(3):
        if sign == board[0][j] and sign == board[1][j] and sign == board[2][j]:
            return True
    if sign == board[0][0] and sign == board[1][1] and sign == board[2][2]:
        return True
    if sign == board[0][2] and sign == board[1][1] and sign == board[2][0]:
        return True
    return False


def draw_move(board):
    # The function draws the computer's move and updates the board.
    
    #Set random function to return a random number from a list.
    free_fields = []
    free_fields = make_list_of_free_fields(board)
    print(free_fields)
    move = random.choice(free_fields)
    
    #Iterate through the board and set the position to "X"
    for i in range(3):
        for j in range(3):
            if move == board[i][j]:
                board[i][j] = "X"
                

initialize_board(board)    
turns = 1
is_winner = False
winner = ""
while turns < 9:
    if enter_move(board):
        turns+=1
        is_winner = victory_for(board,"O")
        if is_winner:
            winner = "O"
        else:
            draw_move(board)
            turns+=1
            is_winner = victory_for(board,"X")
            if is_winner:
                winner = "X"
    else:
        print("Please try again.")
    if is_winner:
        print("Winner is: ", winner)
        display_board(board)
        break
    print(turns)
if not is_winner:
    print("Draw")

    
    
    
    
    
    
    