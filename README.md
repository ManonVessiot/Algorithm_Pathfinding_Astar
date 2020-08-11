# Algorithm_Pathfinding_Astar

We need
- A grid 
- Nodes composing the grid
- 1 starting node
- 1 ending node

Each node has 3 differents cost :
- g_cost = distance from starting node (change if a better path is found)
- h_cost = distance heuristic from end node (never change)
- f_cost = g_cost + h_cost

---
<font color="green">OPEN</font> // the set of nodes to be evaluated <br/>
<font color="red">CLOSED</font> // the set of nodes already evaluated <br/>
add the start node to <font color="green">OPEN</font>

loop (<font color="green">OPEN</font> not empty)<br/>
- <font color="cyan">current</font> = node in <font color="green">OPEN</font> with the lowest f_cost (or the lowest h_cost)
- remove <font color="cyan">current</font> from <font color="green">OPEN</font>
- add <font color="cyan">current</font> to <font color="red">CLOSED</font><br/>
    <br/>
- if <font color="cyan">current</font> is the target node // path has been found<br/>
    - return
    <br/>
- foreach <font color="orange">neighbour</font> of the <font color="cyan">current</font> node
    - if <font color="orange">neighbour</font> is not traversable OR <font color="orange">neighbour</font> is in <font color="red">CLOSED</font><br/>
        - skip to the next <font color="orange">neighbour</font>
    - if new path to <font color="orange">neighbour</font> is shorter OR <font color="orange">neighbour</font> is not in <font color="green">OPEN</font><br/>
        - set f_cost of <font color="orange">neighbour</font>
        - set parent of <font color="orange">neighbour</font> to <font color="cyan">current</font>
        - if <font color="orange">neighbour</font> is not in <font color="green">OPEN</font>
            - add <font color="orange">neighbour</font> to <font color="green">OPEN</font>
---