using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CardLayout : MonoBehaviour
{

    [Required]
    public Deck Deck;

    [ValidateInput("FullArray", defaultMessage:"All the array elements must have a value")]
    [SerializeField]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
    private RectTransform[] cardRects = new RectTransform[Deck.DECK_CARD_AMMOUT];

    [ReadOnly]
    public Vector2[] LocalStartingPositions = new Vector2[Deck.DECK_CARD_AMMOUT];

    public void Initialize(ObjectFactory factory)
    {
        InstantiateDeck(factory);
    }
    private void Start()
    {
        SetStartingPostions();
    }

    private void SetStartingPostions()
    {
        for (int i = 0; i < Deck.DECK_CARD_AMMOUT; i++)
        {
            float x = cardRects[i].localPosition.x;
            float y = cardRects[i].localPosition.y;

            Vector2 localStartingPos = new Vector2(x, y);

            LocalStartingPositions[i] = localStartingPos;
        }
    }
    private void InstantiateDeck(ObjectFactory factory)
    {
        for (int i = 0; i < Deck.DECK_CARD_AMMOUT; i++)
        {
            InstantiateCard(factory, cardRects[i], i);
        }
    }
    private void InstantiateCard(ObjectFactory factory, RectTransform transform, int i)
    {
        Sprite cardSprite = GetCardSprite(Deck[i], factory);

        if (CheckForCard(transform, out DeploymentCard card))
        {
            card.Initialize(Deck[i], i, cardSprite);
        }
        else
        {
            DeploymentCard newCard = transform.gameObject.AddComponent<DeploymentCard>();
            newCard.Initialize(Deck[i], i, cardSprite);
        }
    }
    private bool CheckForCard(RectTransform transform, out DeploymentCard card)
    {
        card = transform.GetComponent<DeploymentCard>();

        if (card != null)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void ClearStartingPosition()
    {
        for (int i = 0; i < Deck.DECK_CARD_AMMOUT; i++)
        {
            LocalStartingPositions[i] = new Vector2Int(0, 0);
        }
    }

    public void RestartCard(int cardIndex)
    {
        if (cardIndex > Deck.DECK_CARD_AMMOUT || cardIndex < 0)
        {
            Debug.LogError("Invalid Input");
            return;
        }
        RectTransform rectTransform = cardRects[cardIndex];

        ReturnToStartingPos(cardIndex, rectTransform);
        rectTransform.localScale = Vector3.one;
        rectTransform.gameObject.SetActive(true);
    }
    public void RestartAll()
    {
        for (int i = 0; i < cardRects.Length; i++)
        {
            RestartCard(i);
        }
    }
    private void ReturnToStartingPos(int cardIndex, RectTransform rectTransform)
    {
        //Vector3 startingPos = new Vector3(StartingPositions[cardIndex].x, StartingPositions[cardIndex].y, 0);

        rectTransform.localPosition = LocalStartingPositions[cardIndex];
    }
    private Sprite GetCardSprite(EntityID Id, ObjectFactory factory)
    {
        Sprite sprite = factory.SpriteDictionary[Id];
        if (sprite == null)
        {
            //designar sprite default
        }

        return sprite;
    }

    private bool FullArray(RectTransform[] collection)
    {
        foreach (var item in collection)
        {
            if (item == null)
            {
                return false;
            }
        }
        return true;
    }

}
