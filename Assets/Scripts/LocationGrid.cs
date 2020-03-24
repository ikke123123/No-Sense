using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationGrid
{
    public Location[,] locations;

    public void GenerateGrid(int width, int depth, float scale = 2)
    {
        if (width == 0 || depth == 0) return;
        locations = new Location[width, depth];
        bool falseLocation = true;
        Vector3 basePos = new Vector3(-0.5f * width * scale + 0.5f * scale, 1.25f, -0.5f * depth * scale + 0.5f * scale);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                locations[i, j] = new Location
                {
                    validLocation = falseLocation,
                    position = basePos + new Vector3(i * scale, 0, j * scale),
                    gridLocation = new Vector2Int(i, j)
                };
                if (j < width - 1)
                {
                    if (falseLocation) falseLocation = false; else falseLocation = true;
                }
            }
        }
    }
}

public class Location
{
    public void LeaveLocation()
    {
        isOccupied = false;
        piece = null;
    }

    public void OccupyLocation(Piece input)
    {
        isOccupied = true;
        piece = input;
    }

    //Put all position dependent variables in here
    public Vector3 position;
    public Vector2Int gridLocation;
    public bool validLocation = false;
    public bool isOccupied = false;
    public Piece piece = null;
}
