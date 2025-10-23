using UnityEngine;

public class LeftCardDropArea : MonoBehaviour, ICardDropArea
{
    public void OnCardDropped(Card card)
    {
        // Move the card to the designated drop position
        card.transform.position = transform.position;
        Debug.Log("Card dropped in here!");
    }
}