using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DeploymentPreview : MonoBehaviour
{
    public HashSet<MeshRenderer> SnapVisuals = new HashSet<MeshRenderer>();


    public void SetValid(SnapVisualMaterials materials)
    {
        foreach (MeshRenderer renderer in SnapVisuals)
        {
            renderer.material = materials.Valid;
        }
    }

    public void SetInvalid(SnapVisualMaterials materials)
    {
        foreach (MeshRenderer renderer in SnapVisuals)
        {
            renderer.material = materials.Invalid;
        }
    }

}
