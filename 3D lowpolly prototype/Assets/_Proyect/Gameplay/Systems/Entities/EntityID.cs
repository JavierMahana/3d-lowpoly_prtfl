using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Entity/ID")]
public class EntityID : ScriptableObject
{
    [ValidateInput("ValidateSize")]
    public int Size = 1;
    public int Cost = 10;
    public int maxHealth;
    [TextArea]
    public string Descrition = "";
    private bool ValidateSize(int size)
    {
        if (size < 1)
        {
            return false;
        }
        return true;
    }
}
