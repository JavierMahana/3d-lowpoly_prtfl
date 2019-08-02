using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerUnitManager : MonoBehaviour
{
    private void Awake()
    {
        Unit.UnitCreated += AddUnitIfFriendly;
        Unit.UnitDestroyed += RemoveUnit;
    }

    public HashSet<Unit> PlayerUnits = new HashSet<Unit>();
    public Team PlayerTeam;

    public void AddUnitIfFriendly(Unit entity)
    {
        if (entity.Team == PlayerTeam)
        {
            PlayerUnits.Add(entity);
        }
    }
    public void RemoveUnit(Unit unit)
    {
        if (PlayerUnits.Contains(unit))
        {
            PlayerUnits.Remove(unit);
        }
    }
}
