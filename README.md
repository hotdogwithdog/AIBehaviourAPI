# AIBehaviourAPI
BehaviourAPI do in C# for create Finite State Machines, Behaviour Trees and Utility Systems   

The code is just like that because the API is mean to just added like a git submodule or even just download and grab all the files into the folder of the API   

# Notes
All the code has the same structure for the namespace that is the same of the folder ones, so all the files of the API are below AIBheaviourAPI.
There will be one branch with demos do in Unity, if your project is not in Unity just see the files on Git and they are really selft explainatory, also is really simple to just expand the API with more logic, or for example in the US, or the BT just add more types of nodes, the code is simple just take in care when you look at it, that is really based on the use of the C# delegates. Also in the FSM you can mix Mealy and Moore in the way that you want, and even create a node that his action is the Update of other BehaviourEngine (FSM, BT, or US).   

In the future i will add the possibility of do that also in a BT node and in a US action. (right now you can do it is just create a normal one and have their action to be the update of the child BehaviourEngine, see the NodeHFSM for reference)
