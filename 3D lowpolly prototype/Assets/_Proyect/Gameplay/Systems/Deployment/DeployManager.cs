using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CardLayout), typeof(PreviewModelDrawer))]
public class DeployManager : MonoBehaviour
{
    public bool debug;

    [SerializeField]
    [Required]
    private DeployGrid deployGrid;
    [Required]
    [SerializeField]
    private DeploymentHeightFilter heightFilter;
    [Required]
    [SerializeField]
    private CardLayout layout;
    [Required]
    [SerializeField]
    private PreviewModelDrawer previewDrawer;
    [Required]
    [SerializeField]
    private ObjectFactory objectFactory;

    private float reductionStartHeightVP;
    private float reductionEndHeightVP;
    private float touchHeightVP;



    private void OnValidate()
    {
        SetLayoutAndPreview();
    }
    private void Start()
    {
        InitializeServices();
        SetHeightVariables();
    }

    public DeployData CreateDeployData(DeploymentCard referenceCard)
    {
        DeployData deployData = new DeployData(referenceCard, previewDrawer.PreviewObjects[referenceCard.Index], referenceCard.Index);
        return deployData;
    }
    public void OnPanelDisable()
    {
        layout.RestartAll();
        previewDrawer.HideAll();

    }

    public void ProcessTouchPosition(Vector2 position, ref DeployData deployData)
    {
        touchHeightVP = CameraUtility.Instance.MainCamera.ScreenToViewportPoint(position).y;

        if (touchHeightVP < reductionStartHeightVP)
            ShowCardAt(position, ref deployData);

        else if (touchHeightVP < reductionEndHeightVP)
            ShowCardAtWithReduction(position, deployData);

        else
            ShowModelAt(position, ref deployData);
    }
    public void EndGestureTouch(Vector2 position, ref DeployData deployData)
    {
        touchHeightVP = CameraUtility.Instance.MainCamera.ScreenToViewportPoint(position).y;
        if (touchHeightVP > reductionEndHeightVP)
        {
            int size = deployData.DeploymentCard.Id.Size;
            HashSet<CellData> cellsToFill;
            Vector3 pos = GridProcesor.ScreenPositionToGridPlaneInWorldPostion(deployGrid, position, CameraUtility.Instance.MainCamera);
            Vector2Int referenceIndices = GridProcesor.GetReferenceIndicesFromWorldPosition(deployGrid.GridData, pos, size);

            if (GridProcesor.EntityGridPlacementCheck(deployGrid.GridData, size, referenceIndices, out cellsToFill))
            {
                Vector3 snapedPosition;
                if ((size % 2) == 0)
                    snapedPosition = GridProcesor.VertexIndexToWorldPosition(deployGrid.GridData, referenceIndices.x, referenceIndices.y);
                else
                    snapedPosition = GridProcesor.CellIndexToWorldPosition(deployGrid.GridData, referenceIndices.x, referenceIndices.y);

                Entity newEntity = objectFactory.InstantiateEntity(deployData.DeploymentCard.Id, snapedPosition);

                Structure structure = newEntity as Structure;
                if (structure != null)
                {
                    structure.PlaceStructure(cellsToFill, deployGrid.GridData);
                }
            }
        }

        ShowCardAndHideModel(ref deployData);
        layout.RestartCard(deployData.DeploymentCard.Index);
    }



    private void ShowCardAt(Vector2 position, ref DeployData deployData)
    {
        if (!deployData.ShowingCard)
        {
            ShowCardAndHideModel(ref deployData);
        }
        Transform cardTransform = deployData.DeploymentCard.transform;

        cardTransform.parent.SetAsLastSibling();
        cardTransform.localScale = Vector3.one;
        cardTransform.position = position;

    }
    private void ShowCardAtWithReduction(Vector2 position, DeployData deployData)
    {
        if (!deployData.ShowingCard)
        {
            ShowCardAndHideModel(ref deployData);
        }
        Transform cardTransform = deployData.DeploymentCard.transform;

        cardTransform.parent.SetAsLastSibling();
        cardTransform.position = position;

        float scaleFactor = Mathf.Max(1 - ((touchHeightVP - reductionStartHeightVP) / (reductionEndHeightVP - reductionStartHeightVP)), heightFilter.MaxReduction);

        cardTransform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
    private void ShowModelAt(Vector2 position, ref DeployData deployData)
    {
        if (deployData.ShowingCard || deployData.DeploymentCard.gameObject.activeInHierarchy || !deployData.Preview.gameObject.activeInHierarchy)
        {
            ShowModelAndHideCard(ref deployData);
        }
        previewDrawer.MovePreviewToScreenSpacePoint(position, deployData.Index, out Vector2Int referenceIndices);

        previewDrawer.CheckValidityAndSetVisualsAccordingly(deployData.Index, referenceIndices);
    }




    private void InitializeServices()
    {
        CheckFactoryVaiable();

        layout.Initialize(objectFactory);
        previewDrawer.Initialize(deployGrid, objectFactory, layout.Deck);
    }
    private void SetLayoutAndPreview()
    {
        if (layout == null)
        {
            layout = GetComponent<CardLayout>();
        }
        if (previewDrawer == null)
        {
            previewDrawer = GetComponent<PreviewModelDrawer>();
        }
    }
    private void CheckFactoryVaiable()
    {
        if (objectFactory == null)
        {
            if (debug)
            {
                Debug.Log("No factory set in the inspector... searching for one in the scene");
            }
            ObjectFactory factoryInScene = FindObjectOfType<ObjectFactory>();
            Debug.Assert(factoryInScene != null, "no factory in scene");
            objectFactory = factoryInScene;
        }
    }
    private void SetHeightVariables()
    {
        reductionStartHeightVP = heightFilter.ReductionStartHeightVP;
        reductionEndHeightVP = heightFilter.ReductionEndHeightVP;
    }

    private void ShowCardAndHideModel(ref DeployData deployData)
    {
        deployData.DeploymentCard.gameObject.SetActive(true);
        deployData.Preview.gameObject.SetActive(false);

        deployData.ShowingCard = true;
    }
    private void ShowModelAndHideCard(ref DeployData deployData)
    {
        deployData.DeploymentCard.gameObject.SetActive(false);
        deployData.Preview.gameObject.SetActive(true);

        deployData.ShowingCard = false;
    }






//    public DeployData CreateDeployData(DeploymentCard referenceCard)
//    {
//        DeployData deployData = new DeployData(referenceCard, preview.PreviewObjects[referenceCard.Index]);
//        return deployData;
//    }
//    private void ShowCardAndHideModel(ref DeployData deployData)
//    {
//        deployData.ShowingCard = true;

//        deployData.DeploymentCard.gameObject.SetActive(true);
//        deployData.Preview.gameObject.SetActive(false);
//    }
//    private void ShowModelAndHideCard(ref DeployData deployData)
//    {
//        deployData.ShowingCard = false;

//        deployData.DeploymentCard.gameObject.SetActive(false);
//        deployData.Preview.gameObject.SetActive(true);
//    }
}
