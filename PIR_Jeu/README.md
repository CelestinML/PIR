# PIR : GAME AGENT, EVOLUTIONNARY APPROACH

***
A quick guide to use, test, and improve our open source C# agent.

## TABLE OF CONTENT 
1. [Code structure](#code-structure)
2. [How to change the network parameters](#network-parameters)
3. [How to change the evolutionnary method parameters](#evolutionnary-parameters)
4. [Plot the results](#plot)
5. [How to load a network from .txt file](#load-network)

***

## Code structure

As C# is an object-oriented language, we separate the code into differents classes.

*Kevin.cs* contains the all the neural network logic. This class allow to create a neural network with a given shape or to load it from a txt file. This class also support network weight and biases mutation (method Mutate), toString, and obviously feedforward algorithm to compute the outputs from given inputs. This class DOES NOT handle input collection from the game, as we wanted to separate the neural network itself and all the game mechanic. Kevin is the pilot of the space ship.

*MoveWithAI.cs* link the neural network and the game. Update_input methods get the x position of the space ship and the y position of the closest asteroïds. As the asteroïds spawn on 6 distinct fixed column, 6 inputs of the neural network represent 6 columns. UpdateAI method allows to bind a new neural network to a space ship, as we have to update it each generations. The FeedForward method from the neural network is called in the FixedUpdate to compute the next move.

*UpdateGeneration.cs* handle the evolutionnary logic. This class allows to create a new generation of space ships, and parametrize it. For each generation, *nbChildren* are created, *nbBest* are selected to be mutated *nbChildrenPerBest* times. *nbRandom* allows to include totally randomly parametrized neural networks in the genration. Every generation, the scores and the best neural networks are stored in two txt files. *Scores.txt* allows to visualize the fitness evolution (See [Plot the results](#plot)) with the python program. *BestChildren.txt* allows to store the best children parameters, to eventually load them for a new execution, instead of starting from randoms neural networks parameters.

***

## How to change the network parameters

*Kevin.cs* has differents constructors :

```cs
// Create a new neural network with 7 inputs neurons, 2 hidden layers of 8 neurons, 6 outputs neurons.
// The wieghts and biases are randomly initialized.
// The mutation intensity and proba are to their default value : 0.1f.
public Kevin()

// Create a new neural network with 7 inputs neurons, 2 hidden layers of 8 neurons, 3 outputs neurons.
// The wieghts and biases are initialized from pWeights and pBiases.
// The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
public Kevin(float[][][] pWeights, float[][] pBiases, float pMutationProba, float pMutationIntensity)

// Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons.
// The wieghts and biases are randomly initialized.
// The mutation intensity and proba are to their default value : 0.1f.
public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize)

// Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons
// The wieghts and biases are initialized from pFlatWeights and pFlatBiases.
// The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize, float[] pFlatWeights, float[] pFlatBiases, float pMutationProba, float pMutationIntensity)

// Create a new neural network with inputsSize inputs neurons, 2 hidden layers of hiddenLayerSize neurons, outputsSize outputs neurons
// The wieghts and biases are initialized from pWeights and pBiases.
// The mutation intensity and proba are set to pMutationProba and pMutationIntensity.
public Kevin(int inputsSize, int hiddenLayerSize, int outputsSize, float[][][] pWeights, float[][] pBiases, float pMutationProba, float pMutationIntensity)
```

The mutation intensity define the range of mutation for a weight or bias that mutate, the mutation probability defines how likely a parameter (weight or bias) will mutate.

Load the neural network form flat parameters is a usefull constructor, as the weights and biases are written flat in *BestChildren.txt*, and stored in uni-dimensionnal array when loaded from this txt file (See [How to load a network from .txt file](#load-network)).

***

## How to change the evolutionnary method parameters

The evolutionnary method logic is handled in *UpdateGeneration.cs*.

!!! TO TODO !!!

***

## Plot the results

As the scores are stored in *Scores.txt*, it's easy to plot the score evolution with python. We provide a program *plot.py* to plot the max/min/average score evolution.   

***

## How to load a network from .txt file

// TODO