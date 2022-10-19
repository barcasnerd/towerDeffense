using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref: https://www.youtube.com/watch?v=alU04hvz6L4&t=1065s

public class PathManager : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public static PathManager Instance;
    private List<Cell> openList;
    private List<Cell> closedList;
    public Vector2Int powerUnitLocation;
    private Grid grid;

    private void Awake()
    {
        Instance = this;
    }


    public List<Cell> FindPath(Grid grid, int startx, int starty)
    {
        //Debug.Log("Find path from " + startx + " " + starty);
        return FindPath(grid, startx, starty, powerUnitLocation.x, powerUnitLocation.y);
    }

    public List<Cell> FindPath(Grid grid, int startx, int starty, int endx, int endy)
    {
        this.grid = grid;
        Cell startCell = grid.GetGridObject(startx, starty);
        Cell endCell = grid.GetGridObject(endx, endy);

        openList = new List<Cell> { startCell };
        closedList = new List<Cell>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {

                Cell pathNode = grid.GetGridObject(x, y);
                if (pathNode.isWalkable)
                    pathNode.SetColor(Color.blue);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.pastCell = null;
            }
        }

        startCell.gCost = 0;
        startCell.hCost = CalculateDistanceCost(startCell, endCell);
        startCell.CalculateFCost();

        while (openList.Count > 0)
        {
            Cell currentNode = GetLowestFCostNode(openList);
            if (currentNode == endCell)
            {
                // Reached final node
                //Debug.Log("Reach the end");
                return CalculatePath(endCell);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Cell neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.pastCell = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endCell);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
                //PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentNode, openList, closedList);
            }
        }
        //Debug.Log("Did not reach the end");
        return null;
    }

    private List<Cell> CalculatePath(Cell endCell)
    {
        List<Cell> path = new List<Cell>();
        path.Add(endCell);
        Cell currentNode = endCell;
        while (currentNode.pastCell != null)
        {
            path.Add(currentNode.pastCell);
            currentNode = currentNode.pastCell;
        }
        path.Reverse();

        foreach (Cell c in path)
        {
            c.SetColor(Color.green);
            //Debug.Log(c.ToString());
        }
        return path;
    }

    private List<Cell> GetNeighbourList(Cell currentNode)
    {
        List<Cell> neighbourList = new List<Cell>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public Cell GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private int CalculateDistanceCost(Cell a, Cell b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Cell GetLowestFCostNode(List<Cell> pathNodeList)
    {
        Cell lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }


}