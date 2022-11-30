using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float m_spacing = 8.0f;

    List<Card> m_cards = new List<Card>();

    public void AddCard(Card card, bool isFaceUp)
    {
        card.transform.SetParent(transform, false);
        card.transform.localPosition = new Vector3(m_spacing * m_cards.Count, 0.0f, 0.0f);
        card.SetFaceUp(isFaceUp);
        m_cards.Add(card);
    }

    public int Score()
    {
        int score = 0;
        int numAces = 0;

        // TODO calculate the score
        foreach (Card card in m_cards)
        {
            if (card.m_rank == 1)
            {
                //ace
                numAces++;
            }

            score += Card.s_value[card.m_rank];
        }

        //aces
        for (int i = 0; i < numAces; i++)
        {
            if (score + 10 <= 21)
            {
                score += 10;
            }
        }


        return score;
    }

    public int NumCards()
    {
        return m_cards.Count;
    }

    public void RevealAll()
    {
        foreach (Card card in m_cards)
        {
            card.SetFaceUp(true);
        }
    }

    public void Clear()
    {
        foreach (Card card in m_cards)
        {
            Destroy(card.gameObject);
        }
        m_cards.Clear();
    }
}
