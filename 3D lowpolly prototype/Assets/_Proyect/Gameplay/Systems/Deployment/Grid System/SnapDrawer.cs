using System.Collections;
using Sirenix.OdinInspector;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class SnapDrawer : MonoBehaviour
{
    public Transform ParentOfIndicators;

    public GameObject valid_VisualIndicatorPrefab;
    public GameObject invalid_VisualIndicatorPrefab;


    private bool showingValidVisualIndicator;
    private Vector3 currentLowerLeftCell = Vector3.negativeInfinity;
    private List<GameObject> activeObjects = new List<GameObject>();


    public void ManageVisuals(bool show, bool valid, int size, Vector3 lowerLeftCellPosition, float cellRadious)
    {
        if (!show)
        {
            if (activeObjects != null)
            {
                DespawnVisuals();
            }
            return;
        }

        if (showingValidVisualIndicator == valid)
        {
            if (currentLowerLeftCell != lowerLeftCellPosition)
                MoveVisuals(lowerLeftCellPosition);
        }
        else
        {
            DespawnVisuals();
            SpawnVisuals(valid, size, lowerLeftCellPosition, cellRadious);
        }
    }



    private void SpawnVisuals(bool valid, int size, Vector3 lowerLeftCellPosition, float cellRadious)
    {

        if (valid)
        {
            SpawnPrefabInGrid(valid_VisualIndicatorPrefab, size, lowerLeftCellPosition, cellRadious);
        }
        else
        {
            SpawnPrefabInGrid(invalid_VisualIndicatorPrefab, size, lowerLeftCellPosition, cellRadious);
        }
    }
    private void MoveVisuals(Vector3 newLowerLeftCellPosition)
    {
        ParentOfIndicators.position = newLowerLeftCellPosition;
    }
    private void DespawnVisuals()
    {
        foreach (GameObject obj in activeObjects)
        {
            LeanPool.Despawn(obj);
        }
        activeObjects.Clear();
    }
    private void SpawnPrefabInGrid(GameObject prefab, int size, Vector3 lowerLeftCellPosition, float cellRadious)
    {
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject g =
                    LeanPool.Spawn(
                    prefab,
                    new Vector3((x * cellRadious * 2), 0, (z * 2 * cellRadious)),
                    Quaternion.Euler(90, 0, 0),
                    ParentOfIndicators
                    );
                activeObjects.Add(g);
            }
        }
        ParentOfIndicators.position = lowerLeftCellPosition;
    }
}
