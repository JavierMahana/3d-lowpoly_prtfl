using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour
{

    public RectTransform SelectionBoxTransform;
    [HideInInspector]
    public bool isSelecting;
    private GameObject selectionBoxObject;




    public void StartSelectionBox()
    {
        selectionBoxObject.SetActive(true);
        isSelecting = true;
    }

    public void DrawSelectionBox(Vector3 screenPointA, Vector3 screenPointB)
    {
        Vector3 min = Vector3.Min(screenPointA, screenPointB);
        Vector3 max = Vector3.Max(screenPointA, screenPointB);
        SelectionBoxTransform.anchoredPosition = min;
        SelectionBoxTransform.sizeDelta = (max - min);
    }
    public void EndSelectionBox()
    {
        selectionBoxObject.SetActive(false);
        isSelecting = false;
    }
    private void Start()
    {
        if (SelectionBoxTransform == null)
        {
            SelectionBoxTransform = GetComponentInChildren<RectTransform>();
            if (SelectionBoxTransform == null)
            {
                Debug.LogError("The selection box transform must be set in the inspector or it must be a children of this gameObject");
            }
        }
        SelectionBoxTransform.anchorMin = Vector2.zero;
        SelectionBoxTransform.anchorMax = Vector2.zero;
        SelectionBoxTransform.pivot = Vector2.zero;

        selectionBoxObject = SelectionBoxTransform.gameObject;
        selectionBoxObject.SetActive(false);
    }
}
