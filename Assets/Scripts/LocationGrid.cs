using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LocationGrid
{
    public LocationGrid(int width, int height)
    {
        if (width == 0 || height == 0)
            Debug.LogError("The grid is too small.");

        locations = new Location[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = Location.GetXOffset(y); x < width; x += 2)
            {
                locations[x, y] = new Location(new Vector2Int(x, y), null);
            }
        }
    }

    public Location[,] locations;
}

public class Location
{
    public Location(Vector2Int gridLocation, Piece piece)
    {
        this.gridLocation = gridLocation;
        this.piece = piece;
    }

    public void LeaveLocation() => piece = null;

    public void OccupyLocation(Piece input) => piece = input;

    //Put all position dependent variables in here
    public Vector2Int gridLocation;
    public Piece piece;

    public bool IsOccupied => piece != null;

    public Vector3 Position => new Vector3
    {
        x = LocationKeeper.origin.x + gridLocation.x * LocationKeeper.tileScale,
        y = LocationKeeper.origin.y,
        z = LocationKeeper.origin.z + gridLocation.y * LocationKeeper.tileScale
    };

    //Because the board only works with half the tiles, the offset per column is calculated here.
    public static int GetXOffset(int y)
    {
        return (y + 1) % 2;
    }
}
