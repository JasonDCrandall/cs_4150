max = input()
max = int(max)
conn = input()
conn = int(conn)

i = 1
while (i <= conn):
    j = 1
    while (j <= max):
        print(str(j) + " " + str(i))
        j += 1
    i+=1