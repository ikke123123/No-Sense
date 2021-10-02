using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GridKeeper maintains locations that are occupied by pieces.
/// </summary>
public struct GridKeeper
{
    public GridKeeper(int width, int height)
    {
        this.width = width;
        this.height = height;

        //Generate size of new byte array to store values. The total size needs to be doubled in order to account for what team's pieces are stored.
        storage = new byte[Mathf.CeilToInt(width * height * dataSize / 8f)];

        maxPosition = width * height;

        whitePieces = new Vector2Int[0];
        blackPieces = new Vector2Int[0];
    }

    public int Hash => AddAllToHash(storage);

    public int Width => width;

    public int Height => height;

    public int MaxPosition => maxPosition;

    public byte[] storage;
    //To easily maintain contact with pieces.
    public Vector2Int[] whitePieces;
    public Vector2Int[] blackPieces;

    private int width, height, maxPosition;

    public const int dataSize = 4;

    //Typical starter grid:
    //----------------------------------------
    // 1000 1000 1000 1000    
    // 1000 1000 1000 1000    
    // 1000 1000 1000 1000
    // 0000 0000 0000 0000
    // 0000 0000 0000 0000 <
    // 1100 1100 1100 1100 
    // 1100 1100 1100 1100
    // 1100 1100 1100 1100     
    //           ^
    //----------------------------------------
    //I'm beginning to think I should have saved this as a string, like they do with chess.

    /// <summary>
    /// Moves pawn, also automatically upgrades it if it reaches the other side.
    /// </summary>
    /// <param name="fromPosition"></param>
    /// <param name="toPosition"></param>
    /// <returns></returns>
    public bool Move(Vector2Int fromPosition, Vector2Int toPosition)
    {
        if (!CheckValidPosition(fromPosition) || !CheckValidPosition(toPosition))
            return false;

        bool isOccupied, isSpecial, toReturn = false;
        Team team;
        GetData(fromPosition, out isOccupied, out team, out isSpecial);

        if (!isOccupied)
            return false;

        if (!isSpecial)
        {
            //Checks if the item should be upgraded to a special one.
            if ((team == Team.White && toPosition.y == 0) || (team == Team.Black && toPosition.y == height))
            {
                isSpecial = toReturn = true;
            }
        }

        //Sets the data of the from position back to false.
        SetData(fromPosition, team, false, false);

        //Overwrites the data of the toPosition.
        SetData(toPosition, team, true, isSpecial);

        return toReturn;
    }

    /// <summary>
    /// Returns true when it is a valid position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckValidPosition(Vector2Int pos) => pos.x > -1 && pos.x <= width && pos.y > -1 && pos.y <= height;

    public void GetData(Vector2Int position, out bool isOccupied, out Team team, out bool isSpecial)
    {
        if (position.x < 0 || position.x > width || position.y < 0 || position.y > height)
        {
            Debug.LogError("Invalid input was: " + position + " should be between " + 0 + " and " + MaxPosition);
            isOccupied = isSpecial = false;
            team = Team.White;
            return;
        }

        int possessedByteIndex, colorByteIndex, possessedIndex, colorIndex, specialByteIndex, specialIndex;
        GetIndexOfPos(position, out possessedByteIndex, out colorByteIndex, out specialByteIndex, out possessedIndex, out colorIndex, out specialIndex);

        isOccupied = GetBitData(ref storage, possessedByteIndex, possessedIndex);
        team = GetBitData(ref storage, colorByteIndex, colorIndex) ? Team.White : Team.Black;
        isSpecial = GetBitData(ref storage, specialByteIndex, specialIndex);
    }

    public void SetData(Vector2Int position, Team team, bool isOccupied, bool isSpecial)
    {
        if (position.x < 0 || position.x > width || position.y < 0 || position.y > height)
        {
            Debug.LogError("Invalid input was: " + position + " should be between " + 0 + " and " + MaxPosition);
            return;
        }

        int possessedByteIndex, colorByteIndex, possessedIndex, colorIndex, specialByteIndex, specialIndex;
        GetIndexOfPos(position, out possessedByteIndex, out colorByteIndex, out specialByteIndex, out possessedIndex, out colorIndex, out specialIndex);

        SetBitData(ref storage, possessedByteIndex, possessedIndex, isOccupied);
        SetBitData(ref storage, colorByteIndex, colorIndex, team == Team.White);
        SetBitData(ref storage, specialByteIndex, specialIndex, isSpecial);
    }

    private int GetStandardIndex(Vector2Int position)
    {
        return (position.x + position.y * width) * dataSize;
    }

    private void GetIndexOfPos(Vector2Int position, out int possessedByteIndex, out int colorByteIndex, out int specialByteIndex, out int possessedIndex, out int colorIndex, out int specialIndex)
    {
        //Gets absolute location of possessedIndex.
        possessedIndex = GetStandardIndex(position);
        possessedByteIndex = possessedIndex / 8;
        colorIndex = possessedIndex + 1; //Because color index has an offset of 1
        specialIndex = possessedIndex + 2; //Special index has an offset of 2
        possessedIndex %= 8;

        colorByteIndex = colorIndex / 8;
        colorIndex %= 8;

        specialByteIndex = specialIndex / 8;
        specialIndex %= 8;
    }

    private static bool GetBitData(ref byte[] bytes, int byteIndex, int bitIndex)
    {
        return GetBit(ref bytes[byteIndex], bitIndex);
    }

    private static void SetBitData(ref byte[] bytes, int byteIndex, int bitIndex, bool value)
    {
        SetBit(ref bytes[byteIndex], bitIndex, value);
    }

    private static int AddAllToHash(byte[] array)
    {
        int hash = 0;
        foreach (byte bytes in array)
            hash += bytes;
        return hash;
    }

    private static void SetBit(ref byte _byte, int bitIndex, bool value)
    {
        if (value)
        {
            //Set to one
            _byte |= (byte)(1 << bitIndex);
        }
        else _byte &= (byte)~(1 << bitIndex); //Set to zero
    }

    private static bool GetBit(ref byte _byte, int bitIndex)
    {
        return (_byte & (1 << bitIndex - 1)) != 0; //Get value in boolean form.
    }
}
