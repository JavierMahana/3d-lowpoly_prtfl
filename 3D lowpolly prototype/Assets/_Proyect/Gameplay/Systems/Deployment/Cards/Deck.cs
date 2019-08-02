using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName ="Deployment/Deck")]
public class Deck : ScriptableObject
{
    public const int DECK_CARD_AMMOUT = 4;

    public EntityID this[int i]
    {
        get
        {
            if (i < 0 || i >= DECK_CARD_AMMOUT)
            {
                Debug.LogError("invalid index input");
                return null;
            }
            else
            {
                return cardsId[i];
            }
        }
    }

    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
    [SerializeField]
    private EntityID[] cardsId = new EntityID[DECK_CARD_AMMOUT];
}
