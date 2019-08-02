using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PreviewModelDrawer : MonoBehaviour
{
    [SerializeField]
    [Required]
    private GameObject snapVisualsPrefab;

    [SerializeField]
    [Required]
    private SnapVisualMaterials materials;

    [ReadOnly]
    public DeploymentPreview[] PreviewObjects = new DeploymentPreview[Deck.DECK_CARD_AMMOUT];
    private int[] objectSizes = new int[Deck.DECK_CARD_AMMOUT];
    private DeployGrid deployGrid;

    public void Initialize(DeployGrid deployGrid, ObjectFactory factory, Deck deck)
    {
        this.deployGrid = deployGrid;
        PopulatePreviewObjectsArray(factory, deck);
        PopulateSizesArray(deck);
    }
    public void HideAll()
    {
        foreach (DeploymentPreview preview in PreviewObjects)
        {
            preview.gameObject.SetActive(false);
        }
    }

    /// <param name="centerCellOrVertexIndex">returns min value if invalid index</param>
    public void MovePreviewToScreenSpacePoint(Vector2 screenPoint, int index, out Vector2Int centerCellOrVertexIndex)
    {
        if (index < 0 || index > Deck.DECK_CARD_AMMOUT)
        {
            Debug.LogError($"invalid index input {index}");
            centerCellOrVertexIndex = new Vector2Int(int.MinValue, int.MinValue);
            return;
        }
        int size = objectSizes[index];
        bool even = (size % 2) == 0;

        Vector3 point = GridProcesor.ScreenPositionToGridPlaneInWorldPostion(deployGrid, screenPoint, CameraUtility.Instance.MainCamera);
        DeploymentPreview previewObj = PreviewObjects[index];

        
        if (even)
        {
            VertexData vData = GridProcesor.WorldPositionToClosestVertex(deployGrid.GridData, point);
            Vector3 newPos = GridProcesor.VertexIndexToWorldPosition(deployGrid.GridData, vData.XIndex, vData.ZIndex);
            previewObj.transform.position = newPos;

            centerCellOrVertexIndex = new Vector2Int(vData.XIndex, vData.ZIndex);
        }
        else
        {
            CellData cData = GridProcesor.WorldPositionToClosestCell(deployGrid.GridData, point);
            Vector3 newPos = GridProcesor.CellIndexToWorldPosition(deployGrid.GridData, cData.XIndex, cData.ZIndex);
            previewObj.transform.position = newPos;

            centerCellOrVertexIndex = new Vector2Int(cData.XIndex, cData.ZIndex);
        }
        
    }
    public void CheckValidityAndSetVisualsAccordingly(int index, Vector2Int referenceCellIndices)
    {
        if (index < 0 || index > Deck.DECK_CARD_AMMOUT)
        {
            Debug.LogError($"invalid index input {index}");
            return;
        }
        DeploymentPreview preview = PreviewObjects[index];
        int size = objectSizes[index];

        bool valid = GridProcesor.EntityGridPlacementCheck(deployGrid.GridData, size, referenceCellIndices);

        if (valid)
        {
            preview.SetValid(materials);
        }
        else
        {
            preview.SetInvalid(materials);
        }
    }


    private void PopulateSizesArray(Deck deck)
    {
        for (int i = 0; i < Deck.DECK_CARD_AMMOUT; i++)
        {
            objectSizes[i] = deck[i].Size;
        }
    }
    private void PopulatePreviewObjectsArray(ObjectFactory factory, Deck deck)
    {
        for (int i = 0; i < Deck.DECK_CARD_AMMOUT; i++)
        {
            DeploymentPreview p = factory.PlacementPreviewDictionary[deck[i]];

            if (p != null)
            {
                DeploymentPreview previewInstantiated = Instantiate(p, transform);
                PreviewObjects[i] = previewInstantiated;


                CreateAndSetPreviewSnapVisuals(previewInstantiated.transform, ref previewInstantiated, deck[i].Size);

                previewInstantiated.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError($"no value in the preview dictionary for the key: {deck[i].ToString()}");
                continue;
            }
            
        }
    }

    private void CreateAndSetPreviewSnapVisuals(Transform parent, ref DeploymentPreview deployPreview, int size)
    {
        bool even = (size % 2 == 0);
        if (even)
            InitializeEvenPreviewSnapVisuals(parent, ref deployPreview, size);

        else
            InitializeOddPreviewSnapVisuals(parent, ref deployPreview, size);
    }
    private void InitializeEvenPreviewSnapVisuals(Transform parent, ref DeploymentPreview deployPreview, int size)
    {
        int m = - ((size/2) - 1);
        float xPos;
        float zPos;
        float cellRadious = deployGrid.GridData.CellRadious;
        Vector3 localScale = new Vector3(cellRadious, cellRadious, cellRadious) * 2;
        Vector3 localPos;

        for (int z = m; z < (size + m); z++)
        {
            for (int x = m; x < (size + m); x++)
            {
                xPos = (x * 2 * cellRadious) - cellRadious;
                zPos = (z * 2 * cellRadious) - cellRadious;

                localPos = new Vector3(xPos, deployGrid.GridData.Origin.y, zPos);

                GameObject newObj = Instantiate(snapVisualsPrefab.gameObject, parent);
                newObj.transform.localPosition = localPos;
                newObj.transform.localScale = localScale;

                AddVisualToTheDeployPreview(deployPreview, newObj);
            }
        }
    }
    private void InitializeOddPreviewSnapVisuals(Transform parent, ref DeploymentPreview deployPreview, int size)
    {
        int m = - ((size - 1)/ 2);
        float xPos;
        float zPos;
        float cellRadious = deployGrid.GridData.CellRadious;
        Vector3 localScale = new Vector3(cellRadious, cellRadious, cellRadious) * 2;
        Vector3 localPos;

        for (int z = m; z < (size + m); z++)
        {
            for (int x = m; x < (size + m); x++)
            {
                xPos = x * 2 * cellRadious;
                zPos = z * 2 * cellRadious;
                localPos = new Vector3(xPos, deployGrid.GridData.Origin.y, zPos);
                GameObject newObj = Instantiate(snapVisualsPrefab.gameObject, parent);
                newObj.transform.localPosition = localPos;
                newObj.transform.localScale = localScale;

                AddVisualToTheDeployPreview(deployPreview, newObj);
            }
        }
    }

    private void AddVisualToTheDeployPreview(DeploymentPreview deployPreview, GameObject newObj)
    {
        MeshRenderer renderer = newObj.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = newObj.AddComponent<MeshRenderer>();
        }
        deployPreview.SnapVisuals.Add(renderer);
    }
}


