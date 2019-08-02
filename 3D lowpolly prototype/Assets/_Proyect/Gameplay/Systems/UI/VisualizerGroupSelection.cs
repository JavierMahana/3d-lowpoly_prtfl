using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class VisualizerGroupSelection : MonoBehaviour
{
    [SerializeField]
    [Required]
    private Image image;

    private Color imageColor = new Color(1, 1, 1);
    public void SetVisualizer(Entity entity, EntityID id, ObjectFactory factory)
    {
        image.sprite = factory.SpriteDictionary[id];

        float bg = (float)entity.Health.CurrentHealth / (float)id.maxHealth;
        image.color = new Color(1, bg, bg);
    }

    public void UpdateVisualizerColor(float healthPercent)
    {
        imageColor.b = healthPercent;
        imageColor.g = healthPercent;

        image.color = imageColor;
    }
}
