using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DigitalRubyShared;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class DeployPanel : MonoBehaviour
{
    private void Awake()
    {
        CreateGestures();
    }
    private void OnEnable()
    {
        InitializePanel();
    }
    private void OnDisable()
    {
        if (FingersScript.Instance != null)
        {
            DeactivatePanel();
        }
    }
    [Required]
    public SelectedCardTracker SelectedCardTracker;
    [Required]
    public GraphicRaycaster GraphicRaycaster;
    [Required]
    public DeployManager DeployManager;


    private DeployData currentDeployData;
    private DeploymentGesture deploymentGesture;


    private void CreateGestures()
    {
        CreateDeploymentGesture();
    }
    private void CreateDeploymentGesture()
    {

        deploymentGesture = new DeploymentGesture(SelectedCardTracker, GraphicRaycaster);
        deploymentGesture.StateUpdated += DeployGestureCallback;
    }
    private void DeployGestureCallback(GestureRecognizer gesture)
    {
        if (true)
        {

        }

        if (gesture.State == GestureRecognizerState.Began)
        {
            currentDeployData = DeployManager.CreateDeployData(SelectedCardTracker.SelectedCard);
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            DeployManager.ProcessTouchPosition(new Vector2(gesture.FocusX, gesture.FocusY), ref currentDeployData);
        }
        else if (gesture.State == GestureRecognizerState.Ended)
        {
            DeployManager.EndGestureTouch(new Vector2(gesture.FocusX, gesture.FocusY), ref currentDeployData);
        }
    }

    private void InitializePanel()
    {
        ActivateGestures();
    }
    private void DeactivatePanel()
    {
        DeployManager.OnPanelDisable();
        DeactivateGestures();
    }

    private void ActivateGestures()
    {
        FingersScript.Instance.AddGesture(deploymentGesture);
    }
    private void DeactivateGestures()
    {
        FingersScript.Instance.RemoveGesture(deploymentGesture);
    }




}
