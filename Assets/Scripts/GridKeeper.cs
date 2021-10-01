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
    }

    public int Hash => AddAllToHash(storage);

    public int Width => width;

    public int Height => height;

    public int MaxPosition => maxPosition;

    public byte[] storage;

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

    public void GetData(int position, out bool isOccupied, out bool whiteTeam, out bool isSpecial)
    {
        if (position < 0 || position > maxPosition)
        {
            Debug.LogError("Invalid input was: " + position + " should be between " + 0 + " and " + MaxPosition);
            isOccupied = isSpecial = whiteTeam = false;
            return;
        }

        int possessedByteIndex, colorByteIndex, possessedIndex, colorIndex, specialByteIndex, specialIndex;
        GetIndexOfPos(position, out possessedByteIndex, out colorByteIndex, out specialByteIndex, out possessedIndex, out colorIndex, out specialIndex);

        isOccupied = GetBitData(ref storage, possessedByteIndex, possessedIndex);
        whiteTeam = GetBitData(ref storage, colorByteIndex, colorIndex);
        isSpecial = GetBitData(ref storage, specialByteIndex, specialIndex);
    }

    public void SetData(int position, Team team, bool isOccupied, bool isSpecial)
    {
        if (position < 0 || position > maxPosition)
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

    private int GetStandardIndex(int position)
    {
        return position * dataSize;
    }

    private void GetIndexOfPos(int position, out int possessedByteIndex, out int colorByteIndex, out int specialByteIndex, out int possessedIndex, out int colorIndex, out int specialIndex)
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
