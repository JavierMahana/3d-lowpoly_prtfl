using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class VisualizerDetailedGroupSelection : MonoBehaviour
{
    [SerializeField]
    [Required]
    private Image image;
    [SerializeField]
    [Required]
    private TextMeshProUGUI maxHealthText;
    [SerializeField]
    [Required]
    private TextMeshProUGUI currentHealthText;


    private Color imageColor = new Color(1, 1, 1);

    public void SetVisualizer(Entity entity, EntityID id, ObjectFactory factory)
    {
        currentHealthText.text = entity.Health.CurrentHealth.ToString();
        maxHealthText.text = id.maxHealth.ToString();

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
