using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    int[,] layout = new int[,] { 
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1 },

    };
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject parent;
    private int width = 3;
    private int height = 3;




    void Start()
    {
        width = layout.GetLength(1);  // number of rows
        height = layout.GetLength(0); // number of columns
        Debug.Log("width " + width + " height " + height);
        GameObject temp;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (layout[i,j] == 1)
                {
                    Debug.Log("grass on "+ i+ " "+ j);
                    temp = Instantiate(grassPrefab, new Vector3(j, i, 0), Quaternion.identity);
                } else
                {
                    Debug.Log("dirt on " + i + " " + j);
                    temp = Instantiate(dirtPrefab, new Vector3(j, i, 0), Quaternion.identity);
                }
                temp.transform.SetParent(parent.transform);
            }
        }

        var center = new Vector2((float)height / 2 - 0.5f, (float)width / 2 - 0.5f);

        Camera.main.transform.position = new Vector3(center.x, center.y, -5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
