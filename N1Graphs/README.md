# Instruções para realizar o trabalho

Primeiro precisamos ter uma estrutura de como o código irá funcionar, provavelmente será desta maneira:

1.  Initialize the open list
2.  Initialize the closed list
    put the starting node on the open
    list (you can leave its f at zero)

3.  while the open list is not empty

    1. find the node with the least f on
       the open list, call it "q"

    2. pop q off the open list

    3. generate q's 8 successors and set their
       parents to q

    4. for each successor

        1. if successor is the goal, stop search
           successor.g = q.g + distance between
           successor and q
           successor.h = distance from goal to
           successor (This can be done using many
           ways, we will discuss three heuristics-
           Manhattan, Diagonal and Euclidean
           Heuristics)

            `successor.f = successor.g + successor.h`

        2. if a node with the same position as
           successor is in the OPEN list which has a
           lower f than successor, skip this successor

        3. if a node with the same position as
           successor is in the CLOSED list which has
           a lower f than successor, skip this successor
           otherwise, add the node to the open list

    5. push q on the closed list
