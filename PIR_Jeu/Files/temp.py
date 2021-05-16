import matplotlib.pyplot as plt
import numpy as np

def somme_list(List):
    compteur = 0
    for i in List:
        compteur += i
    return (compteur/len(List))

f = open("Score.txt", "r");

scores = []

for line in f:
    if (line != ''):
        print(line)
        scores.append(float(line.replace(',', '.')))

count = 0
List_rolling = [0, 0, 0, 0, 0]
new_scores = []
for value in scores:
    List_rolling[count] = value
    count = (count+1) % 5
    new_scores.append(somme_list(List_rolling))

""" 
*-------------------------------------------------------*
"""

plt.figure()

plt.plot(np.linspace(1, len(scores), len(scores)), scores)
plt.plot(np.linspace(1, len(scores), len(scores)), new_scores)
plt.xlabel('Trial number', fontsize=30)
plt.ylabel('Score', fontsize=30)
plt.title('Scores across the learning process', fontsize=30)
plt.show()