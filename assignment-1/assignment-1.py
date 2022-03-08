# Programming Assignment 1: Takes in a group of "words" and outputs the number
#                           of groups of anagrams.
# By: Jason Crandall u0726408
# Date: 01/15/2022

def main():
    initial_in = input()
    initial_in = initial_in.split(" ")
    count = int(initial_in[0])
    anagrams = []
    all_words = []
    for i in range(count):
        raw_word = input()
        word = {}

        # Build word in the form of a dictionary
        for letter in raw_word:
            if letter in word:
                word[letter] += 1
            else:
                word[letter] = 1

        # Add word to the list of words if it is not already in the list
        if all_words.count(word) == 0:
            all_words.append(word)
        else:
            # Add word to the anagrams list if it has previously been found
            if anagrams.count(word) == 0:
                anagrams.append(word)
    print(len(anagrams))



if __name__ == '__main__':
    main()