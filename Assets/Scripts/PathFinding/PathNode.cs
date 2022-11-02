using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    private Grid<PathNode> grid;
    private int x, y, value;
    public int gCost, hCost, fCost;
    public bool isWalkable;
    public PathNode cameFromNode;

    public PathNode(Grid<PathNode> grid, int x, int y, int value)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.value = value;
        isWalkable = true;
    }

    public int GetValue()
    {
        return value;
    }

    public void SetValue(int value)
    {
        this.value = value;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public void CalculatefCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public bool UnitCheck(int x, int y, TowerSmall unit)
    {
        if (unit.GetX() == x && unit.GetY() == y)
        {
            return true;
        }
        return false;
    }

    public bool UnitCheck(int x, int y, TowerHeavy unit)
    {
        if (unit.GetX() == x && unit.GetY() == y)
        {
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
