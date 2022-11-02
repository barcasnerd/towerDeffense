using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<GridObject>
{

    #region ValueChangedEvent
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    #endregion

    private int width, height;
    public const int sortingOrderDefault = 5000;
    private float cellSize;
    private GridObject[,] gridArray;
    private bool showDebug;
    public bool _Debug = false;
    private TextMesh[,] debugTextArray;//Debug

    public Grid(int width, int height, float cellSize, Func<Grid<GridObject>, int, int, GridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new GridObject[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = createGridObject(this, i, j);
            }
        }
        #region DebugItems
        showDebug = true;
        if(showDebug == true)
        {
            if (_Debug)
            {
                debugTextArray = new TextMesh[width, height];

                for (int i = 0; i < gridArray.GetLength(0); i++)
                {
                    for (int j = 0; j < gridArray.GetLength(1); j++)
                    {
                        debugTextArray[i, j] = CreateWorldText(gridArray[i, j]?.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 1000f);
                        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 1000f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 1000f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 1000f);

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                };
            }
        }
        #endregion
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void SetGridObject(int x, int y, GridObject value)
    {
        if (x >= 0 && y >= 00 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }//Directly to the array

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }

    public void SetGridObject(Vector3 worldPosition, GridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }//In a specific posistion

    public GridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 00 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(GridObject) ;
        }
    }//Directly to the array

    public GridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }//In a specific posistion

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }
    #region Utils
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, 
                                           Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment 
                                           = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null)
        {
            color = Color.white;
        }
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, 
                                           TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    #endregion
}
