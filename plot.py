# -*- coding: utf-8 -*-
"""
Created on Mon May  3 20:59:39 2021

@author: loica
"""
import matplotlib.pyplot as plt

maximum = 7900
number_of_versions = 3

scoresvx = []
lifesx = []

for i in range(number_of_versions):
    file = open("scoresv" + str(i) + ".txt", 'r')
    lines = file.readlines()
    score_list = []
    life_list = []
    for line in lines:
        score, life = line.split()
        if int(life) == 0 and int(score) > 1500:
            pass
            # these are wrong data, don't consider
        score_list.append(score)
        life_list.append(int(life))
    score_list = list(map(int,score_list))
    
    score_list = score_list[:maximum]
    life_list = life_list[:maximum]
    
    scoresvx.append(score_list)
    lifesx.append(life_list)
    
    

fig, axs = plt.subplots(number_of_versions)
fig.suptitle('A different future for different Billies')

colors = ["red", "blue", "green"]

for i in range(number_of_versions):
    axs[i].hist(scoresvx[i], bins=40, color=colors[i], edgecolor='black')
    axs[i].title.set_text("Billie v" + str(i))
    axs[i].axvline(1500, color='red', linestyle='dashed', linewidth=1.5)
    axs[i].axvline(400, color='black', linestyle='dashed', linewidth=1)
    axs[i].axvline(800, color='black', linestyle='dashed', linewidth=1)
    axs[i].set_xlim([0,2200])
    axs[i].set_ylim([0,900])
    axs[i].annotate("   Min : " + str(min(scoresvx[i])), xy=(0.85, 0.725), xycoords=axs[i].transAxes, fontsize=8)
    axs[i].annotate("  Max : " + str(max(scoresvx[i])), xy=(0.85, 0.5), xycoords=axs[i].transAxes, fontsize=8)
    axs[i].annotate("Mean : " + str(int(sum(scoresvx[i])/len(scoresvx[i]))), xy=(0.85, 0.3), xycoords=axs[i].transAxes, fontsize=8)


fig.text(0.5, 0.005, 'Score', ha="center")
fig.text(0.005, 0.5, 'Number of dead Billies', va='center', rotation='vertical')

fig.tight_layout()
fig.savefig('plot.pdf')
    
for i in range(number_of_versions):
    score = len(list(filter(lambda x : x > 1500, scoresvx[i])))
    print("Score " + str(i) + " : " + str(score))
    print("number :" + str(len(scoresvx[i])))
