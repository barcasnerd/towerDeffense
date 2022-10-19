using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridGenerator : MonoBehaviour
{
/*
    int[,] layout = new int[,] {
    { 0,0,    0,  0,  0,  0 ,  0,  0},
    { 1,1,    1,  0,  0,  0 ,  1,  1},
    { 0, 0,    1,  1,  1,  1 ,  1,  0},
    { 0, 1,    1,  1,  0,  0 ,  0,  0},
    { 1, 0,    0,  1,  0,  0 ,  0,  0},
    { 1, 0,    0,  1,  1,  1 ,  1,  0},
    { 1, 0,    0,  0,  0,  0 ,  1,  1},};
*/

    int[,] layout = new int[,] {
    { 1,1,    1,  1,  1,  1 ,  1,  1},
    { 1,0,    0,  0,  0,  0 ,  0,  1},
    { 1,0,    0,  0,  0,  0 ,  0,  1},
    { 1,0,    0,  0,  0,  0 ,  0,  1},
    { 1,0,    0,  0,  0,  0 ,  0,  1},
    { 1, 1,   1,  1,  1,  1 ,  1,  1},};
    public Tilemap dirtTilemap;
    public Tilemap grassTilemap;
    public Tile initialPrefab;
    public Tile grassPrefab;
    public Tile dirtPrefab;
    private int width;
    private int height;

    void Start()
    {
        width = layout.GetLength(1);  // number of rows
        height = layout.GetLength(0); // number of columns

        Debug.Log("width " + width + " height " + height);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Debug.Log("check on " + i + " " + j);
                
                    if (layout[i,j] == 0)
                    {
                        Debug.Log("grass on "+ i+ " "+ j);
                        //Instantiate(grassPrefab, new Vector3(j, i, 0), Quaternion.identity);
                        grassTilemap.SetTile(new Vector3Int(i, j, 0), grassPrefab);
                    } else
                    {
                        Debug.Log("dirt on " + i + " " + j);
                        //Instantiate(dirtPrefab, new Vector3(j, i, 0), Quaternion.identity);
                        dirtTilemap.SetTile(new Vector3Int(i, j, 0), dirtPrefab);
                    }
                
            }
        }
    }

}
