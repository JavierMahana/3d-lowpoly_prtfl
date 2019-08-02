using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DeploymentCard : MonoBehaviour
{
    public void Initialize(EntityID id, int index, Sprite sprite)
    {
        Id = id;
        Index = index;

        imageComponent = GetComponent<Image>();
        imageComponent.sprite = sprite;

    }

    private Image imageComponent;

    [HideInInspector]
    public EntityID Id;
    [HideInInspector]
    public int Index;


}
