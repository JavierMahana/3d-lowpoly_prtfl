using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

public static class GridProcesor
{
    private const float RAY_LENGTH = 30;
    private const float RAY_VERTICAL_HEIGTH = 10;


    /// <param name="referenceCellIndices">the center cell if odd, and the center down left if even</param>
    public static bool EntityGridPlacementCheck(GridData gridData, int size , Vector2Int referenceCellIndices)
    {
        if (size <= 0)
        {
            Debug.LogError("invalid entity size");
        }

        int xIndex = referenceCellIndices.x;
        int zIndex = referenceCellIndices.y;
        bool even = (size % 2 == 0);

        if (even)
            return EvenSizedEntityCheck(gridData, size, xIndex, zIndex);
        else
            return OddSizedEntityCheck(gridData, size, xIndex, zIndex);
        
    }
    /// <param name="referenceCellIndices">the center cell if odd, and the center down left if even</param>
    public static bool EntityGridPlacementCheck(GridData gridData, int size, Vector2Int referenceCellIndices, out HashSet<CellData> cellsData)
    {
        if (size <= 0)
        {
            Debug.LogError("invalid entity size");
        }

        int xIndex = referenceCellIndices.x;
        int zIndex = referenceCellIndices.y;
        bool even = (size % 2 == 0);

        if (even)
            return EvenSizedEntityCheck(gridData, size, xIndex, zIndex, out cellsData);
        else
            return OddSizedEntityCheck(gridData, size, xIndex, zIndex, out cellsData);

    }




    public static GridData CreateGridData(Vector3 origin, float cellRadious, int xSize, int zSize, int terrainLayer, int obstacleLayer, float rayHeightOrigin = RAY_VERTICAL_HEIGTH, float rayLenght = RAY_LENGTH)
    {
        bool[,] gridCellData = new bool[xSize, zSize];
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                Vector3 positionToCheck = CellIndexToWorldPosition(origin, cellRadious, x, z);

                if (PositionRaycheck(positionToCheck, terrainLayer, obstacleLayer, rayHeightOrigin, rayLenght))
                    gridCellData[x, z] = PositionCheckFourCorners(positionToCheck, cellRadious, terrainLayer, obstacleLayer, rayHeightOrigin, rayLenght);

                else
                    gridCellData[x, z] = false;

            }
        }

        GridData gridData = new GridData(gridCellData, origin, cellRadious);
        return gridData;

    }

    public static Vector2Int GetReferenceIndicesFromWorldPosition(GridData gridData, Vector3 worldPos, int size)
    {
        bool even = (size % 2) == 0;
        Vector2Int referenceIndices;

        if (even)
        {
            VertexData data = WorldPositionToClosestVertex(gridData, worldPos);
            referenceIndices = new Vector2Int(data.XIndex, data.ZIndex);
        }
        else
        {
            CellData data = WorldPositionToClosestCell(gridData, worldPos);
            referenceIndices = new Vector2Int(data.XIndex, data.ZIndex);
        }
        return referenceIndices;
    }


    public static Vector3 CellIndexToWorldPosition(Vector3 origin, float cellRadious, int x, int z)
    {
        Vector3 worldPosition = new Vector3
            (
            origin.x + (cellRadious + (2 * cellRadious * x)),
            origin.y,
            origin.z + (cellRadious + (2 * cellRadious * z))
            );
        return worldPosition;
    }
    public static Vector3 CellIndexToWorldPosition(GridData gridData, int x, int z)
    {
        Vector3 worldPosition = new Vector3
            (
            gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x)),
            gridData.Origin.y,
            gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z))
            );
        return worldPosition;
    }

    public static Vector3 VertexIndexToWorldPosition(GridData gridData, int x, int z)
    {
        Vector3 worldPosition = new Vector3(
            gridData.Origin.x + (2 * gridData.CellRadious * (x + 1)),
            gridData.Origin.y,
            gridData.Origin.z + (2 * gridData.CellRadious * (z + 1))
            );

        return worldPosition;
    }
    public static Vector3 VertexIndexToWorldPosition(Vector3 origin, float cellRadious, int x, int z)
    {
        Vector3 worldPosition = new Vector3(
            origin.x + (2 * cellRadious * x),
            origin.y,
            origin.z + (2 * cellRadious * z)
            );
        return worldPosition;
    }

    public static CellData WorldPositionToClosestCell(GridData gridData, Vector3 worldPosition)
    {
        Vector2Int indices = GetCellIndicesInBounds(gridData, worldPosition);


        int xIndex = indices.x;
        int zIndex = indices.y;

        if (xIndex == int.MinValue || zIndex == int.MinValue)
        {
            Debug.Log("returning out of bound cell");
            DirectionRelativeToOrigin relativePosition = GetDirectionRelativeToOriginFromPoint(gridData.Origin, worldPosition);
            Vector2Int newIndices = GetCellIndexOffBounds(gridData, worldPosition, relativePosition);

            return new CellData(false, newIndices.x, newIndices.y);
        }

        return new CellData(gridData[xIndex, zIndex], xIndex, zIndex);
    }
    public static VertexData WorldPositionToClosestVertex(GridData gridData, Vector3 worldPosition)
    {
        //los indices de las vertices es igual al indice de su celda de abajo a la izquierda.
        Vector2Int indices = GetVertexIndicesInBounds(gridData, worldPosition);

        int xIndex = indices.x;
        int zIndex = indices.y;
        
        if (xIndex == int.MinValue || zIndex == int.MinValue)
        {
            Debug.Log("returning out of bound vertex");
            DirectionRelativeToOrigin relativePosition = GetDirectionRelativeToOriginFromPoint(gridData.Origin, worldPosition);
            Vector2Int index = GetVertexIndexOffBounds(gridData, worldPosition, relativePosition);

            return new VertexData(false, index.x, index.y);
        }

        return new VertexData(gridData[xIndex, zIndex], xIndex, zIndex);
    }
    public static Vector3 ScreenPositionToGridPlaneInWorldPostion(DeployGrid deployGrid, Vector2 screenPos, Camera camera)
    {
        float gridHeight = deployGrid.OriginPositionPlaceholder.position.y;


        Ray ray = camera.ScreenPointToRay(screenPos);
        Vector3 dir = ray.direction;
        Vector3 origin = ray.origin;

        float directionMult = (gridHeight - origin.y) / dir.y;
        if (directionMult < 0)
        {
            Debug.LogError("opsite direction");
        }

        float x = origin.x + dir.x * directionMult;
        float z = origin.z + dir.z * directionMult;

        return new Vector3(x, gridHeight, z);
    }




    private static bool PositionRaycheck(Vector3 position, int terrainLayer, int obstacleLayer, float rayHeightOrigin, float rayLenght)
    {
        Ray ray = new Ray(new Vector3(position.x, rayHeightOrigin, position.z), Vector3.down);
        if (Physics.Raycast(ray, rayLenght, terrainLayer))
        {
            if (Physics.Raycast(ray, rayLenght, obstacleLayer))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    private static bool PositionCheckFourCorners(Vector3 center, float cellRadious, int terrainLayer, int obstacleLayer, float rayHeightOrigin, float rayLenght)
    {
        Vector3 topLeft = new Vector3(center.x - cellRadious, 0, center.z + cellRadious);
        Vector3 topRigth = new Vector3(center.x + cellRadious, 0, center.z + cellRadious);
        Vector3 downLeft = new Vector3(center.x - cellRadious, 0, center.z - cellRadious);
        Vector3 downRight = new Vector3(center.x + cellRadious, 0, center.z - cellRadious);

        if (!PositionRaycheck(topLeft, terrainLayer, obstacleLayer, rayHeightOrigin, rayLenght))
        {
            return false;
        }
        if (!PositionRaycheck(topRigth, terrainLayer, obstacleLayer, rayHeightOrigin, rayLenght))
        {
            return false;
        }
        if (!PositionRaycheck(downLeft, terrainLayer, obstacleLayer, rayHeightOrigin, rayLenght))
        {
            return false;
        }
        if (!PositionRaycheck(downRight, terrainLayer, obstacleLayer, rayHeightOrigin, rayLenght))
        {
            return false;
        }
        return true;
    }

    private static DirectionRelativeToOrigin GetDirectionRelativeToOriginFromPoint(Vector3 origin, Vector3 point)
    {
        float xOffset = point.x - origin.x;
        float zOffset = point.z - origin.z;

        if (xOffset < 0 && zOffset < 0)
            return DirectionRelativeToOrigin.DOWN_LEFT;

        else if (0 <= xOffset && zOffset < 0)
            return DirectionRelativeToOrigin.DOWN_RIGHT;

        else if (xOffset < 0 && 0 <= zOffset)
            return DirectionRelativeToOrigin.UP_LEFT;

        else //(0 <= xOffset && 0 <= zOffset)
            return DirectionRelativeToOrigin.UP_RIGHT;

    }

    /// <returns>returns min value if is out of bounds</returns>
    private static Vector2Int GetCellIndicesInBounds(GridData gridData, Vector3 worldPosition)
    {
        int xIndex = int.MinValue;
        int zIndex = int.MinValue;
        for (int x = 0; x < gridData.XSize; x++)
        {
            float temp = gridData.Origin.x + gridData.CellRadious + (gridData.CellRadious * 2 * x);
            if (Mathf.Abs(temp - worldPosition.x) <= gridData.CellRadious)
            {
                xIndex = x;
                break;
            }
        }
        for (int z = 0; z < gridData.ZSize; z++)
        {
            float temp = gridData.Origin.z + gridData.CellRadious + (gridData.CellRadious * 2 * z);
            if (Mathf.Abs(temp - worldPosition.z) <= gridData.CellRadious)
            {
                zIndex = z;
                break;
            }
        }
        return new Vector2Int(xIndex, zIndex);
    }
    /// <returns>returns min value if is out of bounds</returns>
    private static Vector2Int GetVertexIndicesInBounds(GridData gridData, Vector3 worldPosition)
    {
        int xIndex = int.MinValue;
        int zIndex = int.MinValue;
        for (int x = -1; x < gridData.XSize; x++)
        {
            float temp = gridData.Origin.x + (gridData.CellRadious * 2 * (x + 1));
            if (Mathf.Abs(temp - worldPosition.x) <= gridData.CellRadious)
            {
                xIndex = x;
                break;
            }
        }
        for (int z = -1; z < gridData.ZSize; z++)
        {
            float temp = gridData.Origin.z + (gridData.CellRadious * 2 * (z + 1));
            if (Mathf.Abs(temp - worldPosition.z) <= gridData.CellRadious)
            {
                zIndex = z;
                break;
            }
        }
        return new Vector2Int(xIndex, zIndex);
    }

    private static Vector2Int GetVertexIndexOffBounds(GridData gridData, Vector3 worldPosition, DirectionRelativeToOrigin currentCase)
    {
        int xIndex = int.MinValue;
        int zIndex = int.MinValue;
        switch (currentCase)
        {
            case DirectionRelativeToOrigin.DOWN_LEFT:
                for (int z = -1; int.MinValue < z; z--) //down
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = -1; int.MinValue < x; x--) //left
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;


            case DirectionRelativeToOrigin.DOWN_RIGHT:
                for (int z = -1; int.MinValue < z; z--) //down
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = -1; x < int.MaxValue; x++) //rigth
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;


            case DirectionRelativeToOrigin.UP_LEFT:
                for (int z = -1; z < int.MaxValue; z++) //up
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = -1; int.MinValue < x; x--) //left
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;


            case DirectionRelativeToOrigin.UP_RIGHT:
                for (int z = -1; z < int.MaxValue; z++) //up
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = -1; x < int.MaxValue; x++) //rigth
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;
        }
        return new Vector2Int(xIndex, zIndex);
    }
    private static Vector2Int GetCellIndexOffBounds(GridData gridData, Vector3 worldPosition, DirectionRelativeToOrigin currentCase)
    {
        int xIndex = int.MinValue;
        int zIndex = int.MinValue;
        switch (currentCase)
        {
            case DirectionRelativeToOrigin.DOWN_LEFT:
                for (int z = 0; int.MinValue < z; z--) //down
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = 0; int.MinValue < x; x--) //left
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;


            case DirectionRelativeToOrigin.DOWN_RIGHT:
                for (int z = 0; int.MinValue < z; z--) //down
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = 0; x < int.MaxValue; x++) //rigth
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;


            case DirectionRelativeToOrigin.UP_LEFT:
                for (int z = 0; z < int.MaxValue; z++) //up
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = 0; int.MinValue < x; x--) //left
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;


            case DirectionRelativeToOrigin.UP_RIGHT:
                for (int z = 0; z < int.MaxValue; z++) //up
                {
                    float zTemp = gridData.Origin.z + (gridData.CellRadious + (2 * gridData.CellRadious * z));
                    if (Mathf.Abs(zTemp - worldPosition.z) <= gridData.CellRadious)
                    {
                        zIndex = z;
                        break;
                    }
                }
                for (int x = 0; x < int.MaxValue; x++) //rigth
                {
                    float xTemp = gridData.Origin.x + (gridData.CellRadious + (2 * gridData.CellRadious * x));
                    if (Mathf.Abs(xTemp - worldPosition.x) <= gridData.CellRadious)
                    {
                        xIndex = x;
                        break;
                    }
                }
                break;
        }
        return new Vector2Int(xIndex, zIndex);
    }

    private static bool OddSizedEntityCheck(GridData gridData, int size, int xCenterIndex, int zCenterIndex)
    {
        int d = (size - 1)/2;

        int xDownLeftCornerIndex = xCenterIndex - d;
        int zDownLeftCornerIndex = zCenterIndex - d;

        for (int z = zDownLeftCornerIndex; z < zDownLeftCornerIndex + size; z++)
        {
            for (int x = xDownLeftCornerIndex; x < xDownLeftCornerIndex + size; x++)
            {
                if (!gridData[x, z])
                {
                    return false;
                }
            }
        }
        return true;
    }
    private static bool EvenSizedEntityCheck(GridData gridData, int size, int xCenterIndex, int zCenterIndex)
    {
        int d = (size / 2) - 1;

        int xDownLeftCornerIndex = xCenterIndex - d;
        int zDownLeftCornerIndex = zCenterIndex - d;

        for (int z = zDownLeftCornerIndex; z < zDownLeftCornerIndex + size; z++)
        {
            for (int x = xDownLeftCornerIndex; x < xDownLeftCornerIndex + size; x++)
            {
                if (!gridData[x, z])
                {
                    return false;
                }
            }
        }
        return true;
    }
    private static bool OddSizedEntityCheck(GridData gridData, int size, int xCenterIndex, int zCenterIndex, out HashSet<CellData> cells)
    {
        int d = (size - 1) / 2;

        int xDownLeftCornerIndex = xCenterIndex - d;
        int zDownLeftCornerIndex = zCenterIndex - d;

        bool returnValue = true;
        cells = new HashSet<CellData>();

        for (int z = zDownLeftCornerIndex; z < zDownLeftCornerIndex + size; z++)
        {
            for (int x = xDownLeftCornerIndex; x < xDownLeftCornerIndex + size; x++)
            {

                CellData cData = new CellData(gridData[x, z], x, z);
                cells.Add(cData);
                if (!gridData[x, z])
                    returnValue = false;
            }
        }
        return returnValue;
    }
    private static bool EvenSizedEntityCheck(GridData gridData, int size, int xCenterIndex, int zCenterIndex, out HashSet<CellData> cells)
    {
        int d = (size / 2) - 1;

        int xDownLeftCornerIndex = xCenterIndex - d;
        int zDownLeftCornerIndex = zCenterIndex - d;

        bool returnValue = true;
        cells = new HashSet<CellData>();

        for (int z = zDownLeftCornerIndex; z < zDownLeftCornerIndex + size; z++)
        {
            for (int x = xDownLeftCornerIndex; x < xDownLeftCornerIndex + size; x++)
            {
                CellData cData = new CellData(gridData[x, z], x, z);
                cells.Add(cData);

                if (!gridData[x, z])
                    returnValue = false;
            }
        }
        return returnValue;
    }
    private enum DirectionRelativeToOrigin
    {
        DOWN_LEFT,
        DOWN_RIGHT,
        UP_LEFT,
        UP_RIGHT
    }
}
