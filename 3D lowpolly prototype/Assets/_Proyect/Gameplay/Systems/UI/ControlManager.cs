using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Doozy.Engine;

public class ControlManager : MonoBehaviour
{
    private string EmptySelectionEvent = "EmptySelection";
    private string SingleSelectionEvent = "SingleSelection";
    private string DetailedSelectionSelectionEvent = "DetailedGroupSelection";
    private string GroupSelectionEvent = "GroupSelection";

    private string selOther = "SelOther";

    private string sel03 = "Sel3";
    private string sel04 = "Sel4";
    private string sel06 = "Sel6";
    private string sel07 = "Sel7";
    private string sel08 = "Sel8";
    private string sel09 = "Sel9";
    private string sel10 = "Sel10";

    [Required]
    [SerializeField]
    [FoldoutGroup("Visualizers")]
    private VisualizerSingleSelection singleSelectionVisualizer;
    [Required]
    [SerializeField]
    [ListDrawerSettings(HideAddButton = true)]
    [ValidateInput("FullArray")]
    [FoldoutGroup("Visualizers")]
    private VisualizerDetailedGroupSelection[] detailedGroupSelectionVisualizers = new VisualizerDetailedGroupSelection[4];
    [Required]
    [SerializeField]
    [ListDrawerSettings(HideAddButton = true)]
    [ValidateInput("FullArray")]
    [FoldoutGroup("Visualizers")]
    private VisualizerGroupSelection[] groupSelectionVisualizers = new VisualizerGroupSelection[SelectionManager.MAX_SELECTED];

    [Required]
    [SerializeField]
    private SelectionManager selectionManager;
    [Required]
    [SerializeField]
    private ObjectFactory factory;
    private List<Entity> currentEntitiesBeingDisplayed = new List<Entity>();


    // called when a selected entity changes health
    private void SelectedEntityHealthChanged(float currentHealthPercent, Health health)
    {
        int count = currentEntitiesBeingDisplayed.Count;
        if (count == 0)
        {
            Debug.Log("wowzers");
            return;
        }

        int entityIndex = int.MinValue;

        if (! selectionManager.Selected.Contains(health.HealthsEntity))
        {
            Debug.LogError("yo!!!! look that this entity is not selected and somehow he activated this");
            return;
        }
        entityIndex = selectionManager.Selected.IndexOf(health.HealthsEntity);


        if (count == 1)
        {
            singleSelectionVisualizer.UpdateVisualizerColor(currentHealthPercent);
        }
        else if (count <= 4)
        {
            detailedGroupSelectionVisualizers[entityIndex].UpdateVisualizerColor(currentHealthPercent);
        }
        else if (count <= 10)
        {
            groupSelectionVisualizers[entityIndex].UpdateVisualizerColor(currentHealthPercent);
        }
    }
    // called when the selection changes
    private void SelectionChangedCallBack()
    {
        StopListeningToDisplayedEntities();
        StartListeningToSelectedEntities();

        SendDoozyEvents();

        SetVisualizers();
    }


    private void SetVisualizers()
    {
        int count = currentEntitiesBeingDisplayed.Count;
        if (count == 0)
        {
            Debug.Log("wowzers");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Entity entity = currentEntitiesBeingDisplayed[i];

            if (entity == null)
            {
                continue;
            }

            if (count == 1)
            {
                singleSelectionVisualizer.SetVisualizer(entity, entity.Id, factory);
            }
            else if (count <= 4)
            {
                detailedGroupSelectionVisualizers[i].SetVisualizer(entity, entity.Id, factory);
            }
            else if (count <= 10)
            {
                groupSelectionVisualizers[i].SetVisualizer(entity, entity.Id, factory);
            }
        }

    }

    private void Start()
    {
        selectionManager.SelectionChanged += SelectionChangedCallBack;
    }


    private void SendDoozyEvents()
    {
        int selectedCount = selectionManager.Selected.Count;
        if (selectedCount > 0)
        {
            if (selectedCount == 1)
            {
                GameEventMessage.SendEvent(SingleSelectionEvent);
            }
            else if (selectedCount <= 4)
            {
                GameEventMessage.SendEvent(DetailedSelectionSelectionEvent);

                if (selectedCount == 3)
                    GameEventMessage.SendEvent(sel03);
                else if (selectedCount == 4)
                    GameEventMessage.SendEvent(sel04);
                else
                    GameEventMessage.SendEvent(selOther);

            }
            else if (selectedCount > 4)
            {
                GameEventMessage.SendEvent(GroupSelectionEvent);

                if (selectedCount == 6)
                    GameEventMessage.SendEvent(sel06);
                else if (selectedCount == 7)
                    GameEventMessage.SendEvent(sel07);
                else if (selectedCount == 8)
                    GameEventMessage.SendEvent(sel08);
                else if (selectedCount == 9)
                    GameEventMessage.SendEvent(sel09);
                else if (selectedCount == 10)
                    GameEventMessage.SendEvent(sel10);
                else
                    GameEventMessage.SendEvent(selOther);
            }
        }
        else
        {
            GameEventMessage.SendEvent(EmptySelectionEvent);
        }
    }
    private void StartListeningToSelectedEntities()
    {
        foreach (Entity entity in selectionManager.Selected)
        {
            if (entity == null)
                continue;
            entity.Health.OnHealthChanged += SelectedEntityHealthChanged;
        }
        currentEntitiesBeingDisplayed.AddRange(selectionManager.Selected);
    }
    private void StopListeningToDisplayedEntities()
    {
        foreach (Entity entity in currentEntitiesBeingDisplayed)
        {
            if (entity == null)
                continue;

            entity.Health.OnHealthChanged -= SelectedEntityHealthChanged;
        }
        currentEntitiesBeingDisplayed.Clear();
    }
    private bool FullArray(VisualizerGroupSelection[] collection)
    {
        foreach (var item in collection)
        {
            if (item == null)
            {
                return false;
            }
        }
        return true;
    }
    private bool FullArray(VisualizerDetailedGroupSelection[] collection)
    {
        foreach (var item in collection)
        {
            if (item == null)
            {
                return false;
            }
        }
        return true;
    }
}
