using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        Node sNode = new Node(startPos, null, (int)Vector2.Distance(startPos,startPos),(int)Vector2.Distance(startPos,endPos));
        Node eNode = new Node(endPos, null, (int)Vector2.Distance(startPos, endPos), (int)Vector2.Distance(endPos, endPos));

        List<Node> nonVisitedNodes = new List<Node>();
        List<Node> VisitedNodes = new List<Node>();

        nonVisitedNodes.Add(sNode);
        Node currentNode = null;

        while (nonVisitedNodes.Count > 0)
        {
            currentNode = nonVisitedNodes[0];

            for (int i = 0; i < nonVisitedNodes.Count; i++)
            {
                if (nonVisitedNodes[i].FScore < currentNode.FScore || nonVisitedNodes[i].FScore == currentNode.FScore)
                {
                    if (nonVisitedNodes[i].HScore < currentNode.HScore)
                    {
                        currentNode = nonVisitedNodes[i];
                    }
                }
            }
            nonVisitedNodes.Remove(currentNode);
            VisitedNodes.Add(currentNode);

            foreach (Node neighbor in GetNeighbourNodes(currentNode, grid))
            {
                // Calculate the new cost to move to the neighbour
                int newMovementCostToNeighbour = (int)currentNode.GScore + (int)Vector2.Distance(currentNode.position, neighbor.position); //GetDistanceBetweenNodes(currentNode, neighbor);
                
                if (newMovementCostToNeighbour < neighbor.GScore || !CheckForNode(neighbor, nonVisitedNodes))
                {
                    neighbor.GScore = newMovementCostToNeighbour;
                    neighbor.HScore = Vector2.Distance(neighbor.position, eNode.position);
                    neighbor.parent = currentNode;

                    // Check if it is in the openNodes
                    if (!CheckForNode(neighbor, nonVisitedNodes))
                    {
                        nonVisitedNodes.Add(neighbor);
                    }
                }
            }

            if (currentNode.position == eNode.position)
            {
                break;
            }
        }

        List<Vector2Int> path = new List<Vector2Int>();

        while (currentNode.position != sNode.position)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
            if (currentNode == null) break;
        }
        // Set path right way
        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbourNodes(Node n, Cell[,] grid)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 || x == -1 && y == 1 || x == 1 && y == 1 || x == -1 && y == -1 || x == 1 && y == -1)
                {
                    continue;
                }

                int checkX = n.position.x + x;
                int checkY = n.position.y + y;

                if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
                {
                    if (x == 0)
                    {
                        if (y == 1)
                        {
                            if ((grid[checkX, checkY].walls & Wall.DOWN) == 0) 
                            {
                                neighbours.Add(new Node(new Vector2Int(checkX, checkY), n, 0, 0));
                            }
                        }
                        else if (y == -1)
                        {
                            if ((grid[checkX, checkY].walls & Wall.UP) == 0) 
                            {
                                neighbours.Add(new Node(new Vector2Int(checkX, checkY), n, 0, 0));
                            }
                        }
                    }
                    else if (y == 0)
                    {
                        if (x == -1)
                        {
                            if ((grid[checkX, checkY].walls & Wall.RIGHT) == 0) 
                            {
                                neighbours.Add(new Node(new Vector2Int(checkX, checkY), n, 0, 0));
                            }
                        }
                        else if (x == 1)
                        {
                            if ((grid[checkX, checkY].walls & Wall.LEFT) == 0)
                            {
                                neighbours.Add(new Node(new Vector2Int(checkX, checkY), n, 0, 0));
                            }
                        }
                    }
                }
            }
        }
        return neighbours;
    }
    private bool CheckForNode(Node node, List<Node> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (node.position == list[i].position) 
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }

    
}
