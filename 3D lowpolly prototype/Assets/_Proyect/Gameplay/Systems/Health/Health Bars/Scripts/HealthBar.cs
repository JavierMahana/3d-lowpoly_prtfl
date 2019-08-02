using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage = null;
    [SerializeField]
    private float updateTimeSeconds = 0;
    [SerializeField]
    private float verticalOffSet = 0;

    private Health health; 


    private void LateUpdate()
    {
        if (health != null)
        {
            transform.position = CameraUtility.Instance.MainCamera.WorldToScreenPoint(health.transform.position + Vector3.up * verticalOffSet);
        }
    }

    public void SetHealth(Health health)
    {
        this.health = health;
        health.OnHealthChanged += HandleHealthChange;
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnHealthChanged -= HandleHealthChange;
        }
        health = null;
    }

    private void HandleHealthChange(float percent, Health health)
    {
        StartCoroutine(ChangeHealthSmoothly(percent));
    }

    private IEnumerator ChangeHealthSmoothly(float percent)
    {
        float preCahngePercent = foregroundImage.fillAmount;
        float elapsed = 0;
        float filledAmount;
        
        while (elapsed <= updateTimeSeconds) 
        {
            filledAmount = updateTimeSeconds == 0 ? 0 : elapsed / updateTimeSeconds;
            foregroundImage.fillAmount = Mathf.Lerp(preCahngePercent, percent, filledAmount);
            elapsed += Time.deltaTime;

            yield return null;
        }
        foregroundImage.fillAmount = percent;
    }


}
