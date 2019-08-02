using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SelectionVisual : MonoBehaviour
{
    [SerializeField]
    private bool autoScale = true;
    [ShowIf("autoScale")]
    [Required]
    [SerializeField]
    private Transform modelRoot = null;
    [ShowIf("autoScale")]
    [SerializeField]
    private bool squareShape = true;
    private Entity entity;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        entity.OnSelect += OnSelect;
        entity.OnDeselect += OnDeselect;

        if (autoScale)
        {
            Debug.Assert(modelRoot != null, "The model root must be assigned if you want to auto scale the selection circle");

            SetSize();
        }
        

        this.gameObject.SetActive(false);
    }

    public void SetSize()
    {
        MeshRenderer[] mr = modelRoot.GetComponentsInChildren<MeshRenderer>();
        Debug.Assert(mr != null, "missing renderer in children");
        Bounds completeBounds = mr[0].bounds;

        for (int i = 1; i < mr.Length; i++)
        {
            completeBounds.Encapsulate(mr[i].bounds);
        }

        if (squareShape)
        {
            float scaleValue = Mathf.Max(completeBounds.size.x, completeBounds.size.z);
            transform.localScale = new Vector3(scaleValue + SELECTION_CIRCLE_OFFSET, scaleValue + SELECTION_CIRCLE_OFFSET, 1);
        }
        else
        {
            // se modifica el valor de la y, ya que esta rotado en 90 grados
            transform.localScale = new Vector3(completeBounds.size.x + SELECTION_CIRCLE_OFFSET, completeBounds.size.z + SELECTION_CIRCLE_OFFSET, 1);
        }
        
    }

    public void OnSelect()
    {
        this.gameObject.SetActive(true);
    }
    public void OnDeselect()
    {
        this.gameObject.SetActive(false);
    }

    private const float SELECTION_CIRCLE_OFFSET = 0.5f;
}
