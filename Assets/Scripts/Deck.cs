using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public GameObject m_cardPrefab;

    List<Card> m_cards;

    // Start is called before the first frame update
    void Start()
    {
        m_cards = new List<Card>();
        // fill the deck with cards
        for (int suit = 0; suit < 4; ++suit)
        {
            for (int rank = 1; rank <= 13; ++rank)
            {
                CreateCard((Card.Suit)suit, rank);
            }
        }
#if false
        {   // don't forget the jokers
            CreateCard(Card.Suit.none, 0);
            CreateCard(Card.Suit.none, 1);
        }
#endif
        
        Shuffle();
    }

    GameObject CreateCard(Card.Suit suit, int rank)
    {
        GameObject cardObject = Instantiate(m_cardPrefab, transform);
        Card card = cardObject.GetComponent<Card>();
        card.Initialize(suit, rank);
        m_cards.Add(card);
        cardObject.SetActive(false);
        return cardObject;
    }

    void Shuffle()
    {
        Debug.Log("Starting Shuffle");

        for (int n = m_cards.Count - 1; n > 0; n--)
        {
            int index = Random.Range(0, n);

            //swap index card and last card in deck
            Card temp = m_cards[n];
            m_cards[n] = m_cards[index];
            m_cards[index] = temp;
        }


        // TODO shuffle the deck
    }

    public Card GetCard()
    {
        Card card = null;
        // TODO take the next card if there is one
        if (m_cards.Count <= 0)
        {
            return null;
        } 
        else
        {
            card = m_cards[m_cards.Count -1];
            m_cards.RemoveAt(m_cards.Count - 1);
            card.gameObject.SetActive(true);
        }
        
        if (m_cards.Count <= 0)
        {   // if we're out of cards, gray out the deck image
            Image image = GetComponent<Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
        }
        return card;
    }

    public void Reset()
    {
        foreach (Card card in m_cards)
        {
            Destroy(card.gameObject);
        }
        Image image = GetComponent<Image>();
        image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Start();
    }
}
