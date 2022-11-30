using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : MonoBehaviour
{
    List<Card> m_cards;
    // Start is called before the first frame update
    void Start()
    {
        m_cards = new List<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(Card card, bool isFaceUp)
    {
        card.transform.localPosition = new Vector2(0.0f, 0.0f);
        card.transform.localEulerAngles = new Vector3(card.transform.localEulerAngles.x, card.transform.localEulerAngles.y, -90.0f);
        card.transform.SetParent(transform, false);
        card.SetFaceUp(isFaceUp);
        m_cards.Add(card);
    }
}
