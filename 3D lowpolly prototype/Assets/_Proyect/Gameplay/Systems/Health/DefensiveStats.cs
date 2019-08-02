using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Entity/Entity/Defensive Stats")]
public class DefensiveStats : ScriptableObject
{
    public int Defence = 0;

    public void TakeDamage(ref int entityHealth, int attackStrength)
    {
        int damageDeatl = attackStrength - Defence;
        if (damageDeatl <= 0)
        {
            damageDeatl = 1;
        }
        entityHealth -= damageDeatl;
    }
}
