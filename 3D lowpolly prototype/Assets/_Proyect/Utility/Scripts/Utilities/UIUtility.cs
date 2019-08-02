using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIUtility 
{
    /// <summary>
    /// Convert rect transform to screen space
    /// </summary>
    /// <param name="transform">Rect transform</param>
    /// <returns>Screen space rect</returns>
    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        float x = transform.position.x - (size.x * 0.5f);
        float y = transform.position.y - (size.y * 0.5f);
        return new Rect(x, y, size.x, size.y);
    }

    public static float GetTopCoordinateOfRectTransformInViewportSpace(RectTransform rectTransform, Canvas canvas)
    {
        return CameraUtility.Instance.MainCamera.ScreenToViewportPoint(new Vector2(0, GetTopCoordinateOfRectTransformInScreenSpace(rectTransform, canvas))).y; ;
    }
    private static float GetTopCoordinateOfRectTransformInScreenSpace(RectTransform rectTransform, Canvas canvas)
    {
        Rect rectTransformRect = RectTransformUtility.PixelAdjustRect(rectTransform, canvas);
        return rectTransformRect.height;
    }
}
