using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
[System.Serializable]
public class Map: ScriptableObject
{
    [SerializeField] private Row[] map;

    public int width => map.Length > 0 ? map[0].cells.Length : 0;
    public int height => map.Length;

    public CellType GetCellType(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError("Coordinates out of bounds: " + x + ", " + y);
            return CellType.Empty; // Return a default value or handle as needed
        }
        return map[y].cells[x];
    }

    public UnitType GetUnitType(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError("Coordinates out of bounds: " + x + ", " + y);
            return UnitType.Base; // Return a default value or handle as needed
        }
        return map[y].units[x];
    }

    public int GetPlayer(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError("Coordinates out of bounds: " + x + ", " + y);
            return -1; // Return a default value or handle as needed
        }
        return map[y].player[x];
    }

}



[System.Serializable]
class Row
{
    public CellType[] cells;
    public UnitType[] units;
    
    [Range(-1,1)]
    public int[] player; // 0 = player1, 1 = player2, -1 = empty
}