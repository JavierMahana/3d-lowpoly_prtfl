using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DeployGrid : MonoBehaviour
{
    [HideInInspector]
    public GridData GridData;
    [Required]
    public Transform OriginPositionPlaceholder;
    [SerializeField]
    [Required]
    private FloatReference gridCellRadious;
    [SerializeField]
    private int amountOfCellX, amountOfCellY;
    [SerializeField]
    private LayerMask terrainLayer;
    [SerializeField]
    private LayerMask obstacleLayer;

    private void Start()
    {
        Bake();//__________________________________ ojo con esto. Quizas hacer un SO para que no tenga que correr en runtime.
    }

    [Button(ButtonSizes.Large)]
    private void Bake()
    {
        if (OriginPositionPlaceholder == null || gridCellRadious.Value == 0 || amountOfCellX == 0 || amountOfCellY == 0)
        {
            return;
        }
        GridData = GridProcesor.CreateGridData(OriginPositionPlaceholder.position, gridCellRadious.Value, amountOfCellX, amountOfCellY, terrainLayer, obstacleLayer);
    }
    [Button(ButtonSizes.Small)]
    private void Clear() { GridData = null; }



    //-------------------Gizmos--------------------------|
    [SerializeField]
    private bool showGizmos;
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if (GridData != null)
            {
                for (int z = 0; z < GridData.ZSize; z++)
                {
                    for (int x = 0; x < GridData.XSize; x++)
                    {
                        DrawDepoyGridQuad(x, z);
                    }
                }
            }
        }
    }

    private void DrawDepoyGridQuad(int x, int y)
    {
        
        bool walkable = GridData[x, y];
        if (walkable)
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.red;

        Vector3 pos = GridProcesor.CellIndexToWorldPosition(GridData, x, y);
        Gizmos.DrawSphere(pos, gridCellRadious.Value);
    }
}
