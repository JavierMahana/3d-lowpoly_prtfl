
using UnityEngine;

public class GridData
{
    public GridData(bool[,] gridCellData, Vector3 origin, float cellRadious)
    {
        this.gridCellData = gridCellData;
        Origin = origin;
        XSize = gridCellData.GetLength(0);
        ZSize = gridCellData.GetLength(1);
        CellRadious = cellRadious;
    }
    public bool this[int x, int z]
    {
        get
        {
            if (XSize <= x || ZSize <= z || x < 0 || z < 0)
                return false;

            else
                return gridCellData[x, z];
        }
        set
        {
            if (XSize <= x || ZSize <= z || x < 0 || z < 0)
                return;

            else
                gridCellData[x, z] = value;
        }
    }
    private bool[,] gridCellData;


    public Vector3 Origin;
    public int XSize, ZSize;
    public float CellRadious;


}

public class VertexData
{
    //las vertices en el origen comienzan con el indice -1
    public VertexData(bool downLeftCellState, int xCoordinate, int zCoordinate)
    {
        DownLeftCellState = downLeftCellState;
        XIndex = xCoordinate;
        ZIndex = zCoordinate;
    }

    public bool DownLeftCellState { get; }
    public int XIndex { get; }
    public int ZIndex { get; }
}
public class CellData
{
    public CellData(bool state, int xCoordinate, int zCoordinate)
    {
        State = state;
        XIndex = xCoordinate;
        ZIndex = zCoordinate;
    }

    public bool State { get; } 
    public int XIndex { get; }
    public int ZIndex { get; }
}
