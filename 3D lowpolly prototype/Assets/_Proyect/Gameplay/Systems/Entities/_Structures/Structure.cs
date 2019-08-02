using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Structure : Entity
{
    private GridData currentGrid;
    private HashSet<CellData> ocupiedGridCells = new HashSet<CellData>();


    public void PlaceStructure(HashSet<CellData> gridCellsToOcupy, GridData gridData)
    {
        foreach (CellData cell in gridCellsToOcupy)
        {
            gridData[cell.XIndex, cell.ZIndex] = false;
        }

        currentGrid = gridData;
        ocupiedGridCells = gridCellsToOcupy;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (CellData cell in ocupiedGridCells)
        {
            currentGrid[cell.XIndex, cell.ZIndex] = true;
        }
        ocupiedGridCells.Clear();
    }
}
