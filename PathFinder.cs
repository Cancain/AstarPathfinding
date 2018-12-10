using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    GridMap grid;
    public Vector3 destination;

    void Awake()
    {
        grid = GetComponent<GridMap>();
    }

    public void stepTowardsCell(Vector3 startPos, Vector3 targetPos)
    {
        FindPath(startPos, targetPos);
        
    }

    //Metod for finding a path between two world positions
    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Cell startNode = grid.NodeFromWorldPoint(startPos);
        Cell targetNode = grid.NodeFromWorldPoint(targetPos);

        //a list for keeping all cells under evalutation
        List<Cell> openSet = new List<Cell>();
        //a list of all evaluated cell
        HashSet<Cell> closedSet = new HashSet<Cell>();
        openSet.Add(startNode);

        //while there is still cells in the Open list
        while (openSet.Count > 0)
        {
            //sets the selected not first in the open list
            Cell node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                //sets the cell with the lowest F-cost to current node
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            //if this passes the method has reached it's target
            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            //for each neigbour to the current node
            foreach (Cell neighbour in grid.GetNeighbours(node))
            {
                //skips the neibour if it's not walkable or is already in the closed set
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                //gets the new distance from the neigbour to the current node
                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);

                //if the new cost is lower then the old or is already in the open list
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //sets the new gCost and distance
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);

                    //sets the current node to the neigbours parent
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    //adds the path the findPath-method found to a list in the gridMap class
    void RetracePath(Cell startNode, Cell endNode)
    {
        List<Cell> path = new List<Cell>();
        Cell currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;

        try
        {
            destination = path[0].worldPosition;
        }
        catch (System.Exception)
        {

            
        }

    }

    //gets the distance from two cells
    int GetDistance(Cell cellA, Cell cellB)
    {
        //sets the horizontal and vertical distance with the absolute valute between them
        int dstX = Mathf.Abs(cellA.gridX - cellB.gridX);
        int dstY = Mathf.Abs(cellA.gridY - cellB.gridY);

        //Calculates the total distance
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}