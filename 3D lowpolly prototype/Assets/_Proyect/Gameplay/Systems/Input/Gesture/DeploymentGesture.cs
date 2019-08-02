using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;


public class DeploymentGesture : GestureRecognizer
{
    private SelectedCardTracker selectedCardTracker;
    private GraphicRaycaster graphicRaycaster;
    public DeploymentGesture(SelectedCardTracker cardManager, GraphicRaycaster graphicRaycaster)
    {
        this.graphicRaycaster = graphicRaycaster;
        selectedCardTracker = cardManager;
    }


    protected override void TouchesBegan(System.Collections.Generic.IEnumerable<GestureTouch> touches)
    {
        foreach (GestureTouch touch in touches)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = new Vector2(touch.X, touch.Y);

            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                Button b = result.gameObject.GetComponent<Button>();
                DeploymentCard d = result.gameObject.GetComponent<DeploymentCard>();
                if (b != null)
                {
                    SetState(GestureRecognizerState.Failed);
                    return;
                }
                else if (d != null)
                {
                    selectedCardTracker.SelectedCard = d;
                    break;
                }
                
            }
        }

        if (selectedCardTracker.SelectedCard == null)
        {
            SetState(GestureRecognizerState.Failed);
        }
        else
        {
            CalculateFocus(CurrentTrackedTouches, true);
            SetState(GestureRecognizerState.Began);
        }


    }

    protected override void TouchesMoved()
    {
        if (State == GestureRecognizerState.Began || State == GestureRecognizerState.Executing)
        {
            CalculateFocus(CurrentTrackedTouches);
            SetState(GestureRecognizerState.Executing);
        }
    }

    protected override void TouchesEnded()
    {
        if (State == GestureRecognizerState.Executing)
        {
            CalculateFocus(CurrentTrackedTouches);
            SetState(GestureRecognizerState.Ended);
        }
    }
}


