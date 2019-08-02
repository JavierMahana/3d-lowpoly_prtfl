using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class FloatReference 
{
    [HorizontalGroup("A")]
    public bool UseConstant = true;
    
    public float ConstantValue;

    public FloatVariable Variable;

    public float Value
    {
        get
        {
            return UseConstant ? ConstantValue : Variable.Value;
        }
    }
}
