using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To get the main acces without doing a tag search
/// </summary>
public class CameraUtility : Singleton<CameraUtility>
{
    new private Camera camera;
    public Camera MainCamera
    {
        get
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            return camera;
        }   
    }
}
