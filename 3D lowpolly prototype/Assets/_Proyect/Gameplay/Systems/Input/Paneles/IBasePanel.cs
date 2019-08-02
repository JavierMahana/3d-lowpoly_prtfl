using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasePanel 
{
    //los paneles se encargan de crear y manejar gestures que son "unicas" en ese panel.
    void InitializePanel();
    void DeactivatePanel();
}
