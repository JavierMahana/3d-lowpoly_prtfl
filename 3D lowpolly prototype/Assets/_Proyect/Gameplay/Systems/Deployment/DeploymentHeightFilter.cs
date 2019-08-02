using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DeploymentHeightFilter : MonoBehaviour
{
    [Range(0, 1)]
    public float MaxReduction;

    [SerializeField][Required]
    private Canvas canvas;
    [SerializeField][Required]
    private RectTransform showCardAreaRect;
    [SerializeField][Required]
    private RectTransform showcardWithReductionRect;

    private float reductionStartHeightVP = float.MinValue;
    public float ReductionStartHeightVP
    {
        get
        {
            if (reductionStartHeightVP == float.MinValue)
            {
                reductionStartHeightVP = UIUtility.GetTopCoordinateOfRectTransformInViewportSpace(showCardAreaRect, canvas);
            }
            return reductionStartHeightVP;
        }
    }

    private float reductionEndHeightVP = float.MinValue;
    public float ReductionEndHeightVP
    {
        get
        {
            if (reductionEndHeightVP == float.MinValue)
            {
                reductionEndHeightVP = UIUtility.GetTopCoordinateOfRectTransformInViewportSpace(showcardWithReductionRect, canvas);
            }
            return reductionEndHeightVP;
        }
    }



}
