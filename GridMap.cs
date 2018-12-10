using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    //Layermask for detecting objects in the Layer "unwalkable
    public LayerMask unwalkableMask;

    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Cell[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        createGrid();

    }
    //Creates the grid
    void createGrid()
    {
        grid = new Cell[gridSizeX, gridSizeY];

        //sets the lower left of the grid to a variable
        //gets the point of this object and goes down and left half the size of the grid
        Vector3 WorldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Sets the lower left point of the current cell
                //goes from the grids left lower corner and up diagonaly the size of the cell diameter + radious
                Vector3 worldpoint = WorldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

                //sets the cell to be unwalkable if anything object in the layer "unwalkable" collides with it
                bool walkable = !(Physics2D.OverlapBox(new Vector2(worldpoint.x, worldpoint.y), new Vector2(nodeDiameter, nodeDiameter), 0));
               
                //sets the outmost cells as unwalkable
                if (x.Equals(gridSizeX -1) || x.Equals(0) || y.Equals(gridSizeY -1) || y.Equals(0)) walkable = false;

                //puts the cell in it's correct place in the grid-array
                grid[x, y] = new Cell(walkable, worldpoint, x, y);
            }
        }
        grid[3, 5].walkable = false;
        grid[3, 6].walkable = false;

        grid[6, 5].walkable = false;
        grid[6, 6].walkable = false;

        grid[5, 3].walkable = false;
        grid[4, 3].walkable = false;







    }

    //gets all the neighbouring cells for the cell, even diagonaly
    public List<Cell> GetNeighbours(Cell cell)
    {
        List<Cell> neighbours = new List<Cell>();

        //for each cell next to the cell
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //if it's not the cell selected
                if (x == 0 && y == 0)
                {
                    continue;
                }

                // checks if the neighbouring square is in the grid
                int checkX = cell.gridX + x;
                int checkY = cell.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    //adds the square to the list
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    //gets the cell from the selected worldposition
    public Cell NodeFromWorldPoint(Vector3 worldPosition)
    {
        //Gets the percentage of the grid in a variable
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Cell> path;


    void OnDrawGizmos()
    {
        //Draws the outside of the grid 
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));


        if (grid != null)
            foreach (Cell n in grid)
            { 
                //Draws all walkable cells while, others red
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    //highlights the path calculated by the pathfinder
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                //draws all cells
                Gizmos.DrawCube(n.worldPosition, new Vector3(nodeRadius * 2 - 0.1f, nodeRadius * 2 - 0.1f, 0.1f));
            }
    }
}

