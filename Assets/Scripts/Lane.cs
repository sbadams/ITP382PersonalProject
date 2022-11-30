using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public float m_spacing = 8.0f;
    public GameObject CardPos1;
    public GameObject CardPos2;

    List<Card> m_cards = new List<Card>();

    public void AddCard(Card card, bool isFaceUp)
    {
        card.transform.SetParent(transform, false);
        if (m_cards.Count > 0)
        {
            card.transform.localPosition = CardPos2.transform.localPosition;
        }
        else
        {
            card.transform.localPosition = CardPos1.transform.localPosition;
        }
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
