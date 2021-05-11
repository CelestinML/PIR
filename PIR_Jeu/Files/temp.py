import matplotlib.pyplot as plt

f = open("Score.txt", "r");

def Max_or_Min(List, searchMax):
    if(searchMax):
        Min_Max = 0
    else:
        Min_Max = 99999
    for i in List:
        i = float(i.replace(',', '.'))
        if(searchMax and i > Min_Max):
            Min_Max = i
        elif(not(searchMax) and i < Min_Max):
            Min_Max = i
    return Min_Max

def somme_list(List):
    compteur = 0
    for i in List:
        compteur += i
    return (compteur/len(List))

def mean(List):
    tot = 0
    for i in List:
        tot += float(i.replace(',', '.'))
    return(tot/(len(List)))

def newMean(List):
    tot = 0
    for i in List:
        tot += i
    return(tot/(len(List)))


Y = []
List_Max = []
List_Min = []
List_Mean = []
List_tmp = []
compteur = 0

for lines in f:
    if(lines != "*----*\n"):
        List_tmp.append(lines) 
    else:
        Y.append(compteur)
        List_Mean.append(mean(List_tmp))
        List_Max.append(Max_or_Min(List_tmp, True))
        List_Min.append(Max_or_Min(List_tmp, False))
        List_tmp = []
        compteur += 1
        
""" 
*-------------------------------------------------------*
"""

count = 0
List_flottante = [0, 0, 0, 0, 0]
List_rollingMean = []
        
for value in List_Mean:
    List_flottante[count] = value
    count = (count+1) % 5
    List_rollingMean.append(somme_list(List_flottante))
    
count = 0
List_flottante = [0, 0, 0, 0, 0]
List_rollingMax = []

for valueMax in List_Max:
    List_flottante[count] = valueMax
    count = (count+1) % 5
    List_rollingMax.append(somme_list(List_flottante)) 
    
for i in range(5):
    List_rollingMax[i] = newMean(List_rollingMax)
    List_rollingMean[i] = newMean(List_rollingMean)
    
plt.figure()

plt.plot(Y, List_rollingMean, '-k', label = 'Mean Score')    
plt.plot(Y, List_rollingMax, '-r', label = 'Max Score')
plt.plot(Y, List_Min, '-b', label = 'Min Score')
plt.xlabel('Number of generations')
plt.ylabel('Score')
plt.legend()
plt.title('Score of generation per generations')
plt.show()