using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using Sirenix.OdinInspector;

public class ControlPanel : MonoBehaviour
{
    [Required]
    [SerializeField]
    private SelectionManager selectionManager;
    [Required]
    [SerializeField]
    private SelectionBox selectionBox;
    [Required]
    [SerializeField]
    private PlayerUnitManager playerUnitManager;
    [Required]
    [SerializeField]
    private RectTransform viewPort;
    [Required]
    [SerializeField]
    private Canvas viewPortCanvas;

    private TapGestureRecognizer tapResetGesture;
    private TapGestureRecognizer doubleTapGesture;
    private PanGestureRecognizer dragGesture;
    private TapGestureRecognizer tapOnEntityGesture;


    private bool tapOnEntity = false;
    private float viewPortViewPortHeigth = 0;

    private void Awake()
    {
        CreateGestures();

        viewPortViewPortHeigth = UIUtility.GetTopCoordinateOfRectTransformInViewportSpace(viewPort, viewPortCanvas);

        if (playerUnitManager == null)
        {
            playerUnitManager = FindObjectOfType<PlayerUnitManager>();
        }
        if (viewPort == null)
        {
            viewPort = GetComponentInChildren<RectTransform>();
        }
        if (viewPortCanvas == null)
        {
            if (viewPort != null)
            {
                viewPortCanvas = viewPort.GetComponent<Canvas>();
            }
        }
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
    private void CreateGestures()
    {
        CreateDoubleTapGesture();
        CreateTapResetGesture();
        CreateDragGesture();
        CreateTapOnEntityGesture();
    }


    private bool CheckHeight(Vector2 screenPos)
    {
        float vp = CameraUtility.Instance.MainCamera.ScreenToViewportPoint(screenPos).y;

        if (vp > viewPortViewPortHeigth)
        {
            return false;
        }
        return true;
    }
    private void CreateTapOnEntityGesture()
    {
        tapOnEntityGesture = new TapGestureRecognizer();
        tapOnEntityGesture.AllowSimultaneousExecutionWithAllGestures();
        tapOnEntityGesture.SendBeginState = true;
        tapOnEntityGesture.StateUpdated += TapOnEntityCallback;
    }
    private void TapOnEntityCallback(GestureRecognizer gesture)
    {
        if (CheckHeight(new Vector2(gesture.FocusX, gesture.FocusY)))
        {
            return;
        }

        if (gesture.State == GestureRecognizerState.Began)
        {
            tapOnEntity = false;
            if (RayUtility.EntityInScreenPoint(new Vector2(gesture.FocusX, gesture.FocusY)))
            {
                tapOnEntity = true;
            }
        }
        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (tapOnEntity)
            {
                doubleTapGesture.SetState(GestureRecognizerState.Failed);
                tapResetGesture.SetState(GestureRecognizerState.Failed);

                selectionManager.PointSelection(CameraUtility.Instance.MainCamera, new Vector3(gesture.FocusX, gesture.FocusY));
            }
            
        }
    }

    private void CreateTapResetGesture()
    {
        tapResetGesture = new TapGestureRecognizer();
        tapResetGesture.RequireGestureRecognizerToFail = doubleTapGesture;
        tapResetGesture.StateUpdated += TapResetCallback;
    }
    private void TapResetCallback(GestureRecognizer gesture)
    {
        if (CheckHeight(new Vector2(gesture.FocusX, gesture.FocusY)))
        {
            return;
        }

        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (tapOnEntity != true)
            {
                selectionManager.Reset(true);
            }
        }
    }

    private void CreateDoubleTapGesture()
    {
        doubleTapGesture = new TapGestureRecognizer();
        doubleTapGesture.NumberOfTapsRequired = 2;
        doubleTapGesture.ThresholdSecondsAfterRelease = 0.3f;
        doubleTapGesture.StateUpdated += DoubleTapCallback;
        
    }
    private void DoubleTapCallback(GestureRecognizer gesture)
    {
        if (CheckHeight(new Vector2(gesture.FocusX, gesture.FocusY)))
        {
            return;
        }

        if (gesture.State == GestureRecognizerState.Ended)
        {

            Ray ray = CameraUtility.Instance.MainCamera.ScreenPointToRay(new Vector2(gesture.FocusX, gesture.FocusY));
            if (Physics.Raycast(ray, out RaycastHit hit, SelectionManager.RAY_MAX_DISTANCE, 1<<11 ))
            {
                foreach (Unit unit in selectionManager.Selected)
                {
                    unit.PlayerMoveComand(hit.point, Unit.MacroState.ATTACK_MOVE); 
                }
            }
            
        }
    }


    private void CreateDragGesture()
    {
        dragGesture = new PanGestureRecognizer();
        
        dragGesture.StateUpdated += DragCallback;
    }
    private void DragCallback(GestureRecognizer gesture)
    {

        if (gesture.State == GestureRecognizerState.Began)
        {

            if (CheckHeight(new Vector2(gesture.FocusX, gesture.FocusY)))
            {
                gesture.SetState(GestureRecognizerState.Failed);
                return;
            }
        }

        if (gesture.State == GestureRecognizerState.Executing)
        {

            if (! selectionBox.isSelecting)
            {
                selectionBox.StartSelectionBox();
            }
            selectionBox.DrawSelectionBox(new Vector3(gesture.StartFocusX, gesture.StartFocusY), new Vector3(gesture.FocusX, gesture.FocusY));
        }
        if (gesture.State == GestureRecognizerState.Ended)
        {
            selectionBox.EndSelectionBox();
            selectionManager.AreaSelection(CameraUtility.Instance.MainCamera, new Vector2(gesture.StartFocusX, gesture.StartFocusY), new Vector2(gesture.FocusX, gesture.FocusY), playerUnitManager);
        }
    }

    private void InitializePanel()
    {

        FingersScript.Instance.AddGesture(tapResetGesture);
        FingersScript.Instance.AddGesture(doubleTapGesture);
        FingersScript.Instance.AddGesture(dragGesture);
        FingersScript.Instance.AddGesture(tapOnEntityGesture);
    }
    private void DeactivatePanel()
    {
        FingersScript.Instance.RemoveGesture(tapResetGesture);
        FingersScript.Instance.RemoveGesture(doubleTapGesture);
        FingersScript.Instance.RemoveGesture(dragGesture);
        FingersScript.Instance.RemoveGesture(tapOnEntityGesture);

        selectionManager.Reset();
    }
}
